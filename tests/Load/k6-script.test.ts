import { GetApiMatchListParams, GetApiPlayerParams, PlayersAPIV1Client, UpdatePlayerDataRequest } from "./playersAPIV1.ts";
import { check, sleep } from 'k6';
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";
import {
  randomIntBetween,
  randomString,
} from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { SharedArray } from 'k6/data';

const globalPatch = "1";
const balancePatch = "1";
const gameKey = __ENV.SERVER_KEY
const adminKey = __ENV.ADMIN_KEY
const baseUrl = __ENV.BASE_URL || "http://host.docker.internal:8080";

const playersAPIV1Client = new PlayersAPIV1Client({ baseUrl });

// Shared state between VUs
let registeredDotaIds: number[] = [];
let registeredSteamIds: number[] = [];
let ids: number[] = [];

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
    ids: [] as number[]
  };
}

export default function () {
  let id,
  headers,
  params:GetApiPlayerParams,
  updatePlayerDataRequest:UpdatePlayerDataRequest;

  let dotaId = randomIntBetween(1, 99999);
  let steamId = randomIntBetween(1, 99999);

  while (registeredDotaIds.includes(dotaId)) dotaId++;
  while (registeredSteamIds.includes(steamId)) steamId++;

  headers = {
    "X-Dedicated-Server-Key": gameKey,
    "X-Dota-Id": dotaId,
    "X-Steam-Id": steamId,
    "X-Global-Patch-Version": globalPatch,
    "X-Balance-Patch-Version": balancePatch
  };

  const postApiPlayerRegisterResponseData = playersAPIV1Client.postApiPlayerRegister(headers);

  if (postApiPlayerRegisterResponseData.response.status !== 201) {
    return;
  }

  registeredDotaIds.push(dotaId);
  registeredSteamIds.push(steamId);

  sleep(1);

  const getApiPlayerMeResponseData = playersAPIV1Client.getApiPlayerMe(headers);

  ids.push(getApiPlayerMeResponseData.data.id!);

  params = {
    dotaId: dotaId
  };

  const getApiPlayerByDotaIdResponseData = playersAPIV1Client.getApiPlayer(params);

  params = {
    steamId: steamId
  };

  const getApiPlayerBySteamIdResponseData = playersAPIV1Client.getApiPlayer(params);

  params = {
    id: getApiPlayerMeResponseData.data.id
  };

  const getApiPlayerByIdResponseData = playersAPIV1Client.getApiPlayer(params);

  updatePlayerDataRequest = {
    isPublicForLadder: true,
    publicName: randomString(5),
  };

  const patchApiPlayerEditResponseData = playersAPIV1Client.patchApiPlayerEdit(
    updatePlayerDataRequest,
    headers,
  );

  headers = {
    "X-Dedicated-Server-Key": adminKey,
    "X-Dota-Id": dotaId,
    "X-Steam-Id": steamId,
    "X-Global-Patch-Version": globalPatch,
    "X-Balance-Patch-Version": balancePatch
  };

  const getApiPlayerAllResponseData = playersAPIV1Client.getApiPlayerAll(headers);

  const getApiMatchAllResponseData = playersAPIV1Client.getApiMatchAll(headers);

  headers = {
    "X-Dedicated-Server-Key": gameKey,
    "X-Dota-Id": dotaId,
    "X-Steam-Id": steamId,
    "X-Global-Patch-Version": globalPatch,
    "X-Balance-Patch-Version": balancePatch
  };

  const postApiMatchResponseData = playersAPIV1Client.postApiMatch(headers);

  if (postApiMatchResponseData.data.id) {
    const getApiMatchIdResponseData = playersAPIV1Client.getApiMatchId(postApiMatchResponseData.data.id);

    const getApiMatchResponseData = playersAPIV1Client.getApiMatch(headers);

    let getApiMatchListParamsa: GetApiMatchListParams = {
      dotaId: dotaId,
      steamId: steamId,
      id: getApiPlayerMeResponseData.data.id!,
      detailed: true,
      page: 1,
      size: 20
    }

    headers = {
      "X-Dedicated-Server-Key": adminKey,
      "X-Dota-Id": dotaId,
      "X-Steam-Id": steamId,
      "X-Global-Patch-Version": globalPatch,
      "X-Balance-Patch-Version": balancePatch
    };

    const getApiMatchListResponseData = playersAPIV1Client.getApiMatchList(
      getApiMatchListParamsa,
      headers
    );
  }
}
