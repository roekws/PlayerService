/**
 * Automatically generated by @grafana/openapi-to-k6: 0.3.2
 * Do not edit manually.
 * Players.API | v1
 * Service version: 1.0.0
 */
import { URL, URLSearchParams } from "https://jslib.k6.io/url/1.0.0/index.js";

import http from "k6/http";
import type { Params, Response } from "k6/http";

export interface BatchDeleteResult {
  deletedCount: number;
  notFoundIds: number[];
  deletedIds: number[];
}

export interface BuildingAbilityDto {
  id?: number;
  /** @nullable */
  name?: string | null;
  /** @nullable */
  level?: number | null;
}

export interface BuildingDto {
  id?: number;
  /** @nullable */
  name?: string | null;
  /** @nullable */
  level?: number | null;
  /** @nullable */
  experience?: number | null;
  /** @nullable */
  health?: number | null;
  /** @nullable */
  gridX?: number | null;
  /** @nullable */
  gridY?: number | null;
  /** @nullable */
  gridZ?: number | null;
  /** @nullable */
  rotation?: number | null;
  abilities?: BuildingAbilityDto[];
}

export interface ChangePlayerIdRequest {
  id: number;
  newDotaId: number;
  newSteamId: number;
}

export interface CharacterAbilityDto {
  id?: number;
  /** @nullable */
  name?: string | null;
  /** @nullable */
  level?: number | null;
}

/**
 * @nullable
 */
export type CharacterDto = {
  id?: number;
  /** @nullable */
  hero?: string | null;
  /** @nullable */
  level?: number | null;
  /** @nullable */
  experience?: number | null;
  /** @nullable */
  health?: number | null;
  /** @nullable */
  gold?: number | null;
  items?: CharacterItemDto[];
  abilities?: CharacterAbilityDto[];
} | null;

export interface CharacterItemDto {
  id?: number;
  /** @nullable */
  name?: string | null;
}

/**
 * @nullable
 */
export type CityDto = {
  id?: number;
  /** @nullable */
  level?: number | null;
  /** @nullable */
  health?: number | null;
  buildings?: BuildingDto[];
} | null;

export interface MatchBattleDto {
  id?: number;
  /** @nullable */
  matchId?: number | null;
  /** @nullable */
  enemyPlayerId?: number | null;
  /** @nullable */
  startTime?: string | null;
  /** @nullable */
  endTime?: string | null;
  /** @nullable */
  characterHealthChange?: number | null;
  /** @nullable */
  baseHealthChange?: number | null;
  /** @nullable */
  goldChange?: number | null;
  /** @nullable */
  experienceChange?: number | null;
  /** @nullable */
  number?: number | null;
  /** @nullable */
  enemyCharacterSnapshotJson?: string | null;
  /** @nullable */
  enemyCitySnapshotJson?: string | null;
  /** @nullable */
  playerCharacterSnapshotJson?: string | null;
  /** @nullable */
  playerCitySnapshotJson?: string | null;
  /** @nullable */
  state?: string | null;
}

export interface MatchDto {
  id?: number;
  /** @nullable */
  playerId?: number | null;
  /** @nullable */
  characterId?: number | null;
  /** @nullable */
  cityId?: number | null;
  /** @nullable */
  startTime?: string | null;
  /** @nullable */
  endTime?: string | null;
  /** @nullable */
  level?: number | null;
  /** @nullable */
  ratingChange?: number | null;
  /** @nullable */
  globalPatchVersion?: number | null;
  /** @nullable */
  state?: string | null;
  player?: PlayerDto;
  character?: CharacterDto;
  city?: CityDto;
  battles?: MatchBattleDto[];
}

export interface PaginatedListOfMatchDto {
  items?: MatchDto[];
  pageIndex?: number;
  pageSize?: number;
  totalCount?: number;
  hasNextPage?: boolean;
  hasPreviousPage?: boolean;
}

export interface PaginatedListOfPlayerDto {
  items?: PlayerDto2[];
  pageIndex?: number;
  pageSize?: number;
  totalCount?: number;
  hasNextPage?: boolean;
  hasPreviousPage?: boolean;
}

/**
 * @nullable
 */
export type PlayerDto = {
  id?: number;
  /** @nullable */
  dotaId?: string | null;
  /** @nullable */
  steamId?: string | null;
  /** @nullable */
  publicName?: string | null;
  isPublicForLadder?: boolean;
  rating?: number;
  /** @nullable */
  lastActivityTime?: string | null;
  /** @nullable */
  createdAt?: string | null;
  /** @nullable */
  currentMatchId?: string | null;
} | null;

export interface PlayerDto2 {
  id?: number;
  /** @nullable */
  dotaId?: string | null;
  /** @nullable */
  steamId?: string | null;
  /** @nullable */
  publicName?: string | null;
  isPublicForLadder?: boolean;
  rating?: number;
  /** @nullable */
  lastActivityTime?: string | null;
  /** @nullable */
  createdAt?: string | null;
  /** @nullable */
  currentMatchId?: string | null;
}

export interface ProblemDetails {
  /** @nullable */
  type?: string | null;
  /** @nullable */
  title?: string | null;
  /** @nullable */
  status?: number | null;
  /** @nullable */
  detail?: string | null;
  /** @nullable */
  instance?: string | null;
}

export interface UpdatePlayerDataRequest {
  /** @nullable */
  isPublicForLadder?: boolean | null;
  /** @nullable */
  publicName?: string | null;
}

export type GetApiMatchIdParams = {
  detailed?: boolean;
};

export type GetApiMatchHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type PostApiMatchHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type GetApiMatchListParams = {
  dotaId?: number;
  steamId?: number;
  id?: number;
  detailed?: boolean;
  page?: number;
  size?: number;
};

export type GetApiMatchAllParams = {
  detailed?: boolean;
  page?: number;
  size?: number;
};

export type GetApiMatchAllHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type GetApiPlayerParams = {
  id?: number;
  dotaId?: number;
  steamId?: number;
};

export type DeleteApiPlayerHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type GetApiPlayerAllParams = {
  page?: number;
  size?: number;
};

export type GetApiPlayerAllHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type PostApiPlayerRegisterHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type GetApiPlayerMeHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type PatchApiPlayerEditHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type PatchApiPlayerIdchangeHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

export type DeleteApiPlayerIdHeaders = {
  "X-Dedicated-Server-Key": string;
  "X-Dota-Id": unknown;
  "X-Steam-Id": unknown;
  "X-Global-Patch-Version": unknown;
};

/**
 * This is the base client to use for interacting with the API.
 */
export class PlayersAPIV1Client {
  private cleanBaseUrl: string;
  private commonRequestParameters: Params;

  constructor(clientOptions: {
    baseUrl: string;
    commonRequestParameters?: Params;
  }) {
    this.cleanBaseUrl = clientOptions.baseUrl.replace(/\/+$/, "");

    this.commonRequestParameters = clientOptions.commonRequestParameters || {};
  }

  getApiMatchId(
    id: number,
    params?: GetApiMatchIdParams,
    requestParameters?: Params,
  ): {
    response: Response;
    data: MatchDto;
  } {
    const url = new URL(
      this.cleanBaseUrl +
        `/api/matches/${id}` +
        `?${new URLSearchParams(params).toString()}`,
    );
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiMatch(
    headers: GetApiMatchHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: void;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/matches/active`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  postApiMatch(
    headers: PostApiMatchHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: MatchDto;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/matches`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("POST", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiMatchList(
    params?: GetApiMatchListParams,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PaginatedListOfMatchDto;
  } {
    const url = new URL(
      this.cleanBaseUrl +
        `/api/matches/list` +
        `?${new URLSearchParams(params).toString()}`,
    );
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiMatchAll(
    headers: GetApiMatchAllHeaders,
    params?: GetApiMatchAllParams,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PaginatedListOfMatchDto;
  } {
    const url = new URL(
      this.cleanBaseUrl +
        `/api/matches/all` +
        `?${new URLSearchParams(params).toString()}`,
    );
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiPlayer(
    params?: GetApiPlayerParams,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PlayerDto2;
  } {
    const url = new URL(
      this.cleanBaseUrl +
        `/api/players` +
        `?${new URLSearchParams(params).toString()}`,
    );
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  deleteApiPlayer(
    deleteApiPlayerBody: number[],
    headers: DeleteApiPlayerHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: BatchDeleteResult;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request(
      "DELETE",
      url.toString(),
      JSON.stringify(deleteApiPlayerBody),
      {
        ...mergedRequestParameters,
        headers: {
          ...mergedRequestParameters?.headers,
          "Content-Type": "application/json",
          // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
          ...Object.fromEntries(
            Object.entries(headers || {}).map(([key, value]) => [
              key,
              String(value),
            ]),
          ),
        },
      },
    );
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiPlayerAll(
    headers: GetApiPlayerAllHeaders,
    params?: GetApiPlayerAllParams,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PaginatedListOfPlayerDto;
  } {
    const url = new URL(
      this.cleanBaseUrl +
        `/api/players/all` +
        `?${new URLSearchParams(params).toString()}`,
    );
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  postApiPlayerRegister(
    headers: PostApiPlayerRegisterHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PlayerDto2;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players/register`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("POST", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  getApiPlayerMe(
    headers: GetApiPlayerMeHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PlayerDto2;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players/me`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("GET", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  patchApiPlayerEdit(
    updatePlayerDataRequest: UpdatePlayerDataRequest,
    headers: PatchApiPlayerEditHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PlayerDto2;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players/edit`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request(
      "PATCH",
      url.toString(),
      JSON.stringify(updatePlayerDataRequest),
      {
        ...mergedRequestParameters,
        headers: {
          ...mergedRequestParameters?.headers,
          "Content-Type": "application/json",
          // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
          ...Object.fromEntries(
            Object.entries(headers || {}).map(([key, value]) => [
              key,
              String(value),
            ]),
          ),
        },
      },
    );
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  patchApiPlayerIdchange(
    changePlayerIdRequest: ChangePlayerIdRequest,
    headers: PatchApiPlayerIdchangeHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: PlayerDto2;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players/idchange`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request(
      "PATCH",
      url.toString(),
      JSON.stringify(changePlayerIdRequest),
      {
        ...mergedRequestParameters,
        headers: {
          ...mergedRequestParameters?.headers,
          "Content-Type": "application/json",
          // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
          ...Object.fromEntries(
            Object.entries(headers || {}).map(([key, value]) => [
              key,
              String(value),
            ]),
          ),
        },
      },
    );
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  deleteApiPlayerId(
    id: number,
    headers: DeleteApiPlayerIdHeaders,
    requestParameters?: Params,
  ): {
    response: Response;
    data: void;
  } {
    const url = new URL(this.cleanBaseUrl + `/api/players/${id}`);
    const mergedRequestParameters = this._mergeRequestParameters(
      requestParameters || {},
      this.commonRequestParameters,
    );
    const response = http.request("DELETE", url.toString(), undefined, {
      ...mergedRequestParameters,
      headers: {
        ...mergedRequestParameters?.headers,
        // In the schema, headers can be of any type like number but k6 accepts only strings as headers, hence converting all headers to string
        ...Object.fromEntries(
          Object.entries(headers || {}).map(([key, value]) => [
            key,
            String(value),
          ]),
        ),
      },
    });
    let data;

    try {
      data = response.json();
    } catch {
      data = response.body;
    }
    return {
      response,
      data,
    };
  }

  /**
   * Merges the provided request parameters with default parameters for the client.
   *
   * @param {Params} requestParameters - The parameters provided specifically for the request
   * @param {Params} commonRequestParameters - Common parameters for all requests
   * @returns {Params} - The merged parameters
   */
  private _mergeRequestParameters(
    requestParameters?: Params,
    commonRequestParameters?: Params,
  ): Params {
    return {
      ...commonRequestParameters, // Default to common parameters
      ...requestParameters, // Override with request-specific parameters
      headers: {
        ...(commonRequestParameters?.headers || {}), // Ensure headers are defined
        ...(requestParameters?.headers || {}),
      },
      cookies: {
        ...(commonRequestParameters?.cookies || {}), // Ensure cookies are defined
        ...(requestParameters?.cookies || {}),
      },
      tags: {
        ...(commonRequestParameters?.tags || {}), // Ensure tags are defined
        ...(requestParameters?.tags || {}),
      },
    };
  }
}
