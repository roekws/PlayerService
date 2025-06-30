import { PlayersAPIV1Client, UpdatePlayerDataRequest } from "./playersAPIV1.ts";
import { check, group, sleep } from 'k6';
import {
  randomIntBetween,
  randomString,
} from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

const globalPatch = "1";
const balancePatch = "1";
const gameKey = __ENV.SERVER_KEY;
const adminKey = __ENV.ADMIN_KEY;
const baseUrl = __ENV.BASE_URL || "http://host.docker.internal:8080";

const playersAPIV1Client = new PlayersAPIV1Client({ baseUrl });

// Shared state between VUs
let registeredDotaIds: number[] = [];
let registeredSteamIds: number[] = [];

export const options = {
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<150'],
    'http_reqs{status:500}': ['count==0']
  },
};

export function setup() {
  return {
    registeredDotaIds: [] as number[],
    registeredSteamIds: [] as number[],
  };
}

export default function () {
  let id,
  headers,
  updatePlayerDataRequest:UpdatePlayerDataRequest;

  let dotaId = randomIntBetween(1, 99999);
  let steamId = randomIntBetween(1, 99999);

  while (registeredDotaIds.includes(dotaId)) dotaId++;
  while (registeredSteamIds.includes(steamId)) steamId++;

  group('Player register', function () {
    headers = {
      "X-Dedicated-Server-Key": gameKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    const register = playersAPIV1Client.postApiPlayerRegister(headers);

    check(register.response, {
      'Status is 201 or 400 for /register': (r) => r.status === 201 || r.status === 400,
    });

    if (register.response.status !== 201) {
      return;
    }

    registeredDotaIds.push(dotaId);
    registeredSteamIds.push(steamId);
    id = register.data.id;
    sleep(1);
  });


  group('Player get profile', function () {
    if (id === undefined) {
      return;
    }

    headers = {
      "X-Dedicated-Server-Key": gameKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    const getMe = playersAPIV1Client.getApiPlayerMe(headers);

    check(getMe.response, {
      'Status is 200 for get /me': (r) => r.status === 200,
    });

    const getById = playersAPIV1Client.getApiPlayer({ id: id });

    check(getById.response, {
      'Status is 200 for get by id': (r) => r.status === 200,
    });

    const getByDotaId = playersAPIV1Client.getApiPlayer({ dotaId: dotaId });

    check(getByDotaId.response, {
      'Status is 200 for get by dotaId': (r) => r.status === 200,
    });

    const getBySteamId = playersAPIV1Client.getApiPlayer({ steamId: steamId });

    check(getBySteamId.response, {
      'Status is 200 for get by steamId': (r) => r.status === 200,
    });
  });

  group('Player update profile', function () {
    headers = {
      "X-Dedicated-Server-Key": gameKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    let name = randomString(5)
    let publicProfile = true;

    updatePlayerDataRequest = {
      isPublicForLadder: publicProfile,
      publicName: name,
    };

    const updateProfile = playersAPIV1Client.patchApiPlayerEdit(
      updatePlayerDataRequest,
      headers,
    );

    check(updateProfile.response, {
      'Status is 200 for update profile': (r) => r.status === 200,
    });

    const getByDotaId = playersAPIV1Client.getApiPlayer({ dotaId: dotaId });

    check(getByDotaId, {
      'Status is 200 get updated player by dotaId': (body) => body.response.status === 200,
      'Name updated': (body) => body.data.publicName === name,
      'Privacy updated': (body) => body.data.isPublicForLadder === publicProfile,
    });
  });

  group('Match actions', function () {
    headers = {
      "X-Dedicated-Server-Key": gameKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    const createMatch = playersAPIV1Client.postApiMatch(headers);

    check(createMatch.response, {
      'Status is 201 or 400 for create match': (r) => r.status === 201 || r.status === 400,
    });

    if (createMatch.data.id) {
      const getMatch = playersAPIV1Client.getApiMatchId(createMatch.data.id);

      check(getMatch.response, {
        'Status is 200 for get match by id': (r) => r.status === 200,
      });

      const getActiveMatch = playersAPIV1Client.getApiMatch(headers);

      check(getActiveMatch.response, {
        'Status is 200 for get active match': (r) => r.status === 200,
      });
    }
  });

  group('Admin endpoints', function () {
    headers = {
      "X-Dedicated-Server-Key": adminKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    const getApiPlayerAllResponseData = playersAPIV1Client.getApiPlayerAll(
      headers,
      { page: 1, size: 20 }
    );

    check(getApiPlayerAllResponseData.response, {
      'Status is 200 for get all players': (r) => r.status === 200,
    });

    const getApiMatchAllResponseData = playersAPIV1Client.getApiMatchAll(headers);

    check(getApiMatchAllResponseData.response, {
      'Status is 200 for get all matches': (r) => r.status === 200,
    });

    if (id === undefined) {
      return;
    }

    let getApiMatchListParams = {
      dotaId: dotaId,
      steamId: steamId,
      id: id,
      detailed: true,
      page: 1,
      size: 20
    }

    const getPlayerMatches = playersAPIV1Client.getApiMatchList(
      getApiMatchListParams,
      headers
    );

    check(getPlayerMatches.response, {
      'Status is 200 for get player\'s matches': (r) => r.status === 200,
    });
  });
}
