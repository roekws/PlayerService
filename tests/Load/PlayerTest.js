import http from 'k6/http';
import { check, sleep } from 'k6';
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";
import {
  randomIntBetween,
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
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '10s', target: 100 },
        { duration: '10s', target: 175 },
        { duration: '10s', target: 250 },
        { duration: '10s', target: 325 },
        { duration: '10s', target: 400 },
      ],
      gracefulRampDown: '0s',
    },
  },
  thresholds: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<150'],
    'http_reqs{status:500}': ['count==0']
  },
};

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
      'Content-Type': 'application/json'
    },
  });

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

  check(res, {
    'authenticated player status is 200': (r) => r.status === 200,
  });
}

function getPlayerByDotaId() {
  if (registeredDotaIds.length === 0) return;

  const dotaId = registeredDotaIds[randomIntBetween(0, registeredDotaIds.length - 1)];
  const res = http.get(`${BASE_URL}/api/player/dota=${dotaId}`);

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

  check(res, {
    'player by dota_id status is 200 or 404': (r) => r.status === 200 || r.status === 404,
  });
}

function getPlayerBySteamId() {
  if (registeredSteamIds.length === 0) return;

  const steamId = registeredSteamIds[randomIntBetween(0, registeredSteamIds.length - 1)];
  const res = http.get(`${BASE_URL}/api/player/steam=${steamId}`);

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

  check(res, {
    'player by steam_id status is 200 or 404': (r) => r.status === 200 || r.status === 404,
  });
}

function getPlayerById() {
  const playerId = randomIntBetween(1, 1000);
  const res = http.get(`${BASE_URL}/api/player/${playerId}`);

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

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
        'Content-Type': 'application/json'
      },
    }
  );

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

  check(res, {
    'edit player status is 200': (r) => r.status === 200,
  });
}

function registerNewPlayer() {
  let dotaId = randomIntBetween(1, 99999);
  let steamId = randomIntBetween(1, 99999);

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

  if (res.error) {
    console.error(`[ERROR] ${res.error} | URL: ${res.request.url}`);
  }

  if (res.status === 201) {
    registeredDotaIds.push(dotaId);
    registeredSteamIds.push(steamId);
  }
}

export function handleSummary(data) {
  return {
    stdout: textSummary(data, { indent: " ", enableColors: true })
  };
}
