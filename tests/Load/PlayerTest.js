import http from 'k6/http';
import { check, sleep } from 'k6';
import {
  randomIntBetween,
  randomItem,
  randomString,
} from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

const BASE_URL = 'http://host.docker.internal:8080';
const SERVER_KEY = __ENV.SERVER_KEY || 'x';
const TEST_VERSION = '1';

// Shared state between VUs
let registeredDotaIds = [];
let registeredSteamIds = [];

export const options = {
  scenarios: {
    player_scenario: {
      executor: 'per-vu-iterations',
      vus: 1000,
      maxDuration: '30s',
    },
  },
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<150'],
    'http_reqs{status:500}': ['count==0']
  },
};

export function setup() {
  for (let i = 0; i < 10; i++) {
    registerNewPlayer();
  }
  return { dotaIds: registeredDotaIds, steamIds: registeredSteamIds };
}

export default function () {
  registerNewPlayer();
  getAuthenticatedPlayer();
  getPlayerByDotaId();
  getPlayerBySteamId();
  getPlayerById();
  editPlayer();

  sleep(1);
}

function getAuthenticatedPlayer() {
  if (registeredDotaIds.length === 0) return;

  const playerIdx = randomIntBetween(0, registeredDotaIds.length - 1);
  const res = http.get(`${BASE_URL}/api/player`, {
    headers: {
      'X-Dedicated-Server-Key': SERVER_KEY,
      'X-Dota-Id': registeredDotaIds[playerIdx].toString(),
      'X-Steam-Id': registeredSteamIds[playerIdx].toString(),
      'X-Game-Client-Version': TEST_VERSION,
    },
  });

  check(res, {
    'authenticated player status is 200': (r) => r.status === 200,
  });
}

function getPlayerByDotaId() {
  if (registeredDotaIds.length === 0) return;

  const dotaId = registeredDotaIds[randomIntBetween(0, registeredDotaIds.length - 1)];
  const res = http.get(`${BASE_URL}/api/player/dota=${dotaId}`);

  check(res, {
    'player by dota_id status is 200 or 404': (r) => r.status === 200 || r.status === 404,
  });
}

function getPlayerBySteamId() {
  if (registeredSteamIds.length === 0) return;

  const steamId = registeredSteamIds[randomIntBetween(0, registeredSteamIds.length - 1)];
  const res = http.get(`${BASE_URL}/api/player/steam=${steamId}`);

  check(res, {
    'player by steam_id status is 200 or 404': (r) => r.status === 200 || r.status === 404,
  });
}

function getPlayerById() {
  const playerId = randomIntBetween(1, 1000);
  const res = http.get(`${BASE_URL}/api/player/${playerId}`);

  check(res, {
    'player by id status is 200 or 404': (r) => r.status === 200 || r.status === 404,
  });
}

function editPlayer() {
  if (registeredDotaIds.length === 0) return;

  const playerIdx = randomIntBetween(0, registeredDotaIds.length - 1);
  const name = randomString(5);

  const res = http.patch(`${BASE_URL}/api/player/edit`,
    JSON.stringify({
      isPublicForLadder: true,
      publicName: name  ,
    }),
    {
      headers: {
        'X-Dedicated-Server-Key': SERVER_KEY,
        'X-Dota-Id': registeredDotaIds[playerIdx].toString(),
        'X-Steam-Id': registeredSteamIds[playerIdx].toString(),
        'X-Game-Client-Version': TEST_VERSION,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    }
  );

  check(res, {
    'edit player status is 200': (r) => r.status === 200,
  });
}

function registerNewPlayer() {
  let dotaId = randomIntBetween(100000, 999999);
  let steamId = randomIntBetween(100000, 999999);

  while (registeredDotaIds.includes(dotaId)) dotaId++;
  while (registeredSteamIds.includes(steamId)) steamId++;

  const res = http.post(`${BASE_URL}/api/player/register`, null, {
    headers: {
      'X-Dedicated-Server-Key': SERVER_KEY,
      'X-Dota-Id': dotaId.toString(),
      'X-Steam-Id': steamId.toString(),
      'X-Game-Client-Version': TEST_VERSION,
    },
  });

  if (res.status === 201) {
    registeredDotaIds.push(dotaId);
    registeredSteamIds.push(steamId);
  }
}
