/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./node_modules/@aspnet/signalr/dist/esm/AbortController.js":
/*!******************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/AbortController.js ***!
  \******************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AbortController": () => (/* binding */ AbortController)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Rough polyfill of https://developer.mozilla.org/en-US/docs/Web/API/AbortController
// We don't actually ever use the API being polyfilled, we always use the polyfill because
// it's a very new API right now.
// Not exported from index.
/** @private */
var AbortController = /** @class */ (function () {
    function AbortController() {
        this.isAborted = false;
    }
    AbortController.prototype.abort = function () {
        if (!this.isAborted) {
            this.isAborted = true;
            if (this.onabort) {
                this.onabort();
            }
        }
    };
    Object.defineProperty(AbortController.prototype, "signal", {
        get: function () {
            return this;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AbortController.prototype, "aborted", {
        get: function () {
            return this.isAborted;
        },
        enumerable: true,
        configurable: true
    });
    return AbortController;
}());

//# sourceMappingURL=AbortController.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/Errors.js":
/*!*********************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/Errors.js ***!
  \*********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HttpError": () => (/* binding */ HttpError),
/* harmony export */   "TimeoutError": () => (/* binding */ TimeoutError)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/** Error thrown when an HTTP request fails. */
var HttpError = /** @class */ (function (_super) {
    __extends(HttpError, _super);
    /** Constructs a new instance of {@link @aspnet/signalr.HttpError}.
     *
     * @param {string} errorMessage A descriptive error message.
     * @param {number} statusCode The HTTP status code represented by this error.
     */
    function HttpError(errorMessage, statusCode) {
        var _newTarget = this.constructor;
        var _this = this;
        var trueProto = _newTarget.prototype;
        _this = _super.call(this, errorMessage) || this;
        _this.statusCode = statusCode;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        _this.__proto__ = trueProto;
        return _this;
    }
    return HttpError;
}(Error));

/** Error thrown when a timeout elapses. */
var TimeoutError = /** @class */ (function (_super) {
    __extends(TimeoutError, _super);
    /** Constructs a new instance of {@link @aspnet/signalr.TimeoutError}.
     *
     * @param {string} errorMessage A descriptive error message.
     */
    function TimeoutError(errorMessage) {
        var _newTarget = this.constructor;
        if (errorMessage === void 0) { errorMessage = "A timeout occurred."; }
        var _this = this;
        var trueProto = _newTarget.prototype;
        _this = _super.call(this, errorMessage) || this;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        _this.__proto__ = trueProto;
        return _this;
    }
    return TimeoutError;
}(Error));

//# sourceMappingURL=Errors.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/HandshakeProtocol.js":
/*!********************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/HandshakeProtocol.js ***!
  \********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HandshakeProtocol": () => (/* binding */ HandshakeProtocol)
/* harmony export */ });
/* harmony import */ var _TextMessageFormat__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./TextMessageFormat */ "./node_modules/@aspnet/signalr/dist/esm/TextMessageFormat.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


/** @private */
var HandshakeProtocol = /** @class */ (function () {
    function HandshakeProtocol() {
    }
    // Handshake request is always JSON
    HandshakeProtocol.prototype.writeHandshakeRequest = function (handshakeRequest) {
        return _TextMessageFormat__WEBPACK_IMPORTED_MODULE_0__.TextMessageFormat.write(JSON.stringify(handshakeRequest));
    };
    HandshakeProtocol.prototype.parseHandshakeResponse = function (data) {
        var responseMessage;
        var messageData;
        var remainingData;
        if ((0,_Utils__WEBPACK_IMPORTED_MODULE_1__.isArrayBuffer)(data)) {
            // Format is binary but still need to read JSON text from handshake response
            var binaryData = new Uint8Array(data);
            var separatorIndex = binaryData.indexOf(_TextMessageFormat__WEBPACK_IMPORTED_MODULE_0__.TextMessageFormat.RecordSeparatorCode);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            var responseLength = separatorIndex + 1;
            messageData = String.fromCharCode.apply(null, binaryData.slice(0, responseLength));
            remainingData = (binaryData.byteLength > responseLength) ? binaryData.slice(responseLength).buffer : null;
        }
        else {
            var textData = data;
            var separatorIndex = textData.indexOf(_TextMessageFormat__WEBPACK_IMPORTED_MODULE_0__.TextMessageFormat.RecordSeparator);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            var responseLength = separatorIndex + 1;
            messageData = textData.substring(0, responseLength);
            remainingData = (textData.length > responseLength) ? textData.substring(responseLength) : null;
        }
        // At this point we should have just the single handshake message
        var messages = _TextMessageFormat__WEBPACK_IMPORTED_MODULE_0__.TextMessageFormat.parse(messageData);
        responseMessage = JSON.parse(messages[0]);
        // multiple messages could have arrived with handshake
        // return additional data to be parsed as usual, or null if all parsed
        return [remainingData, responseMessage];
    };
    return HandshakeProtocol;
}());

//# sourceMappingURL=HandshakeProtocol.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/HttpClient.js":
/*!*************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/HttpClient.js ***!
  \*************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HttpResponse": () => (/* binding */ HttpResponse),
/* harmony export */   "HttpClient": () => (/* binding */ HttpClient),
/* harmony export */   "DefaultHttpClient": () => (/* binding */ DefaultHttpClient)
/* harmony export */ });
/* harmony import */ var _Errors__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Errors */ "./node_modules/@aspnet/signalr/dist/esm/Errors.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};


/** Represents an HTTP response. */
var HttpResponse = /** @class */ (function () {
    function HttpResponse(statusCode, statusText, content) {
        this.statusCode = statusCode;
        this.statusText = statusText;
        this.content = content;
    }
    return HttpResponse;
}());

/** Abstraction over an HTTP client.
 *
 * This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
 */
var HttpClient = /** @class */ (function () {
    function HttpClient() {
    }
    HttpClient.prototype.get = function (url, options) {
        return this.send(__assign({}, options, { method: "GET", url: url }));
    };
    HttpClient.prototype.post = function (url, options) {
        return this.send(__assign({}, options, { method: "POST", url: url }));
    };
    HttpClient.prototype.delete = function (url, options) {
        return this.send(__assign({}, options, { method: "DELETE", url: url }));
    };
    return HttpClient;
}());

/** Default implementation of {@link @aspnet/signalr.HttpClient}. */
var DefaultHttpClient = /** @class */ (function (_super) {
    __extends(DefaultHttpClient, _super);
    /** Creates a new instance of the {@link @aspnet/signalr.DefaultHttpClient}, using the provided {@link @aspnet/signalr.ILogger} to log messages. */
    function DefaultHttpClient(logger) {
        var _this = _super.call(this) || this;
        _this.logger = logger;
        return _this;
    }
    /** @inheritDoc */
    DefaultHttpClient.prototype.send = function (request) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open(request.method, request.url, true);
            xhr.withCredentials = true;
            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            // Explicitly setting the Content-Type header for React Native on Android platform.
            xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
            if (request.headers) {
                Object.keys(request.headers)
                    .forEach(function (header) { return xhr.setRequestHeader(header, request.headers[header]); });
            }
            if (request.responseType) {
                xhr.responseType = request.responseType;
            }
            if (request.abortSignal) {
                request.abortSignal.onabort = function () {
                    xhr.abort();
                };
            }
            if (request.timeout) {
                xhr.timeout = request.timeout;
            }
            xhr.onload = function () {
                if (request.abortSignal) {
                    request.abortSignal.onabort = null;
                }
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                }
                else {
                    reject(new _Errors__WEBPACK_IMPORTED_MODULE_0__.HttpError(xhr.statusText, xhr.status));
                }
            };
            xhr.onerror = function () {
                _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Warning, "Error from HTTP request. " + xhr.status + ": " + xhr.statusText);
                reject(new _Errors__WEBPACK_IMPORTED_MODULE_0__.HttpError(xhr.statusText, xhr.status));
            };
            xhr.ontimeout = function () {
                _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Warning, "Timeout from HTTP request.");
                reject(new _Errors__WEBPACK_IMPORTED_MODULE_0__.TimeoutError());
            };
            xhr.send(request.content || "");
        });
    };
    return DefaultHttpClient;
}(HttpClient));

//# sourceMappingURL=HttpClient.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/HttpConnection.js":
/*!*****************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/HttpConnection.js ***!
  \*****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HttpConnection": () => (/* binding */ HttpConnection)
/* harmony export */ });
/* harmony import */ var _HttpClient__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./HttpClient */ "./node_modules/@aspnet/signalr/dist/esm/HttpClient.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _LongPollingTransport__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./LongPollingTransport */ "./node_modules/@aspnet/signalr/dist/esm/LongPollingTransport.js");
/* harmony import */ var _ServerSentEventsTransport__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./ServerSentEventsTransport */ "./node_modules/@aspnet/signalr/dist/esm/ServerSentEventsTransport.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
/* harmony import */ var _WebSocketTransport__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./WebSocketTransport */ "./node_modules/@aspnet/signalr/dist/esm/WebSocketTransport.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};







var MAX_REDIRECTS = 100;
/** @private */
var HttpConnection = /** @class */ (function () {
    function HttpConnection(url, options) {
        if (options === void 0) { options = {}; }
        this.features = {};
        _Utils__WEBPACK_IMPORTED_MODULE_5__.Arg.isRequired(url, "url");
        this.logger = (0,_Utils__WEBPACK_IMPORTED_MODULE_5__.createLogger)(options.logger);
        this.baseUrl = this.resolveUrl(url);
        options = options || {};
        options.accessTokenFactory = options.accessTokenFactory || (function () { return null; });
        options.logMessageContent = options.logMessageContent || false;
        this.httpClient = options.httpClient || new _HttpClient__WEBPACK_IMPORTED_MODULE_0__.DefaultHttpClient(this.logger);
        this.connectionState = 2 /* Disconnected */;
        this.options = options;
    }
    HttpConnection.prototype.start = function (transferFormat) {
        transferFormat = transferFormat || _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat.Binary;
        _Utils__WEBPACK_IMPORTED_MODULE_5__.Arg.isIn(transferFormat, _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat, "transferFormat");
        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Starting connection with transfer format '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat[transferFormat] + "'.");
        if (this.connectionState !== 2 /* Disconnected */) {
            return Promise.reject(new Error("Cannot start a connection that is not in the 'Disconnected' state."));
        }
        this.connectionState = 0 /* Connecting */;
        this.startPromise = this.startInternal(transferFormat);
        return this.startPromise;
    };
    HttpConnection.prototype.send = function (data) {
        if (this.connectionState !== 1 /* Connected */) {
            throw new Error("Cannot send data if the connection is not in the 'Connected' State.");
        }
        return this.transport.send(data);
    };
    HttpConnection.prototype.stop = function (error) {
        return __awaiter(this, void 0, void 0, function () {
            var e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.connectionState = 2 /* Disconnected */;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this.startPromise];
                    case 2:
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3:
                        e_1 = _a.sent();
                        return [3 /*break*/, 4];
                    case 4:
                        if (!this.transport) return [3 /*break*/, 6];
                        this.stopError = error;
                        return [4 /*yield*/, this.transport.stop()];
                    case 5:
                        _a.sent();
                        this.transport = null;
                        _a.label = 6;
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.startInternal = function (transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var url, negotiateResponse, redirects, _loop_1, this_1, state_1, e_2;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url = this.baseUrl;
                        this.accessTokenFactory = this.options.accessTokenFactory;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 12, , 13]);
                        if (!this.options.skipNegotiation) return [3 /*break*/, 5];
                        if (!(this.options.transport === _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.WebSockets)) return [3 /*break*/, 3];
                        // No need to add a connection ID in this case
                        this.transport = this.constructTransport(_ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.WebSockets);
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        return [4 /*yield*/, this.transport.connect(url, transferFormat)];
                    case 2:
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3: throw Error("Negotiation can only be skipped when using the WebSocket transport directly.");
                    case 4: return [3 /*break*/, 11];
                    case 5:
                        negotiateResponse = null;
                        redirects = 0;
                        _loop_1 = function () {
                            var accessToken_1;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this_1.getNegotiationResponse(url)];
                                    case 1:
                                        negotiateResponse = _a.sent();
                                        // the user tries to stop the connection when it is being started
                                        if (this_1.connectionState === 2 /* Disconnected */) {
                                            return [2 /*return*/, { value: void 0 }];
                                        }
                                        if (negotiateResponse.url) {
                                            url = negotiateResponse.url;
                                        }
                                        if (negotiateResponse.accessToken) {
                                            accessToken_1 = negotiateResponse.accessToken;
                                            this_1.accessTokenFactory = function () { return accessToken_1; };
                                        }
                                        redirects++;
                                        return [2 /*return*/];
                                }
                            });
                        };
                        this_1 = this;
                        _a.label = 6;
                    case 6: return [5 /*yield**/, _loop_1()];
                    case 7:
                        state_1 = _a.sent();
                        if (typeof state_1 === "object")
                            return [2 /*return*/, state_1.value];
                        _a.label = 8;
                    case 8:
                        if (negotiateResponse.url && redirects < MAX_REDIRECTS) return [3 /*break*/, 6];
                        _a.label = 9;
                    case 9:
                        if (redirects === MAX_REDIRECTS && negotiateResponse.url) {
                            throw Error("Negotiate redirection limit exceeded.");
                        }
                        return [4 /*yield*/, this.createTransport(url, this.options.transport, negotiateResponse, transferFormat)];
                    case 10:
                        _a.sent();
                        _a.label = 11;
                    case 11:
                        if (this.transport instanceof _LongPollingTransport__WEBPACK_IMPORTED_MODULE_3__.LongPollingTransport) {
                            this.features.inherentKeepAlive = true;
                        }
                        this.transport.onreceive = this.onreceive;
                        this.transport.onclose = function (e) { return _this.stopConnection(e); };
                        // only change the state if we were connecting to not overwrite
                        // the state if the connection is already marked as Disconnected
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [3 /*break*/, 13];
                    case 12:
                        e_2 = _a.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Error, "Failed to start the connection: " + e_2);
                        this.connectionState = 2 /* Disconnected */;
                        this.transport = null;
                        throw e_2;
                    case 13: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.getNegotiationResponse = function (url) {
        return __awaiter(this, void 0, void 0, function () {
            var _a, token, headers, negotiateUrl, response, e_3;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0: return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _b.sent();
                        if (token) {
                            headers = (_a = {},
                                _a["Authorization"] = "Bearer " + token,
                                _a);
                        }
                        negotiateUrl = this.resolveNegotiateUrl(url);
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Sending negotiation request: " + negotiateUrl);
                        _b.label = 2;
                    case 2:
                        _b.trys.push([2, 4, , 5]);
                        return [4 /*yield*/, this.httpClient.post(negotiateUrl, {
                                content: "",
                                headers: headers,
                            })];
                    case 3:
                        response = _b.sent();
                        if (response.statusCode !== 200) {
                            throw Error("Unexpected status code returned from negotiate " + response.statusCode);
                        }
                        return [2 /*return*/, JSON.parse(response.content)];
                    case 4:
                        e_3 = _b.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Error, "Failed to complete negotiation with the server: " + e_3);
                        throw e_3;
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.createConnectUrl = function (url, connectionId) {
        return url + (url.indexOf("?") === -1 ? "?" : "&") + ("id=" + connectionId);
    };
    HttpConnection.prototype.createTransport = function (url, requestedTransport, negotiateResponse, requestedTransferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var connectUrl, transports, _i, transports_1, endpoint, transport, ex_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        connectUrl = this.createConnectUrl(url, negotiateResponse.connectionId);
                        if (!this.isITransport(requestedTransport)) return [3 /*break*/, 2];
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Connection was provided an instance of ITransport, using that directly.");
                        this.transport = requestedTransport;
                        return [4 /*yield*/, this.transport.connect(connectUrl, requestedTransferFormat)];
                    case 1:
                        _a.sent();
                        // only change the state if we were connecting to not overwrite
                        // the state if the connection is already marked as Disconnected
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [2 /*return*/];
                    case 2:
                        transports = negotiateResponse.availableTransports;
                        _i = 0, transports_1 = transports;
                        _a.label = 3;
                    case 3:
                        if (!(_i < transports_1.length)) return [3 /*break*/, 9];
                        endpoint = transports_1[_i];
                        this.connectionState = 0 /* Connecting */;
                        transport = this.resolveTransport(endpoint, requestedTransport, requestedTransferFormat);
                        if (!(typeof transport === "number")) return [3 /*break*/, 8];
                        this.transport = this.constructTransport(transport);
                        if (!(negotiateResponse.connectionId === null)) return [3 /*break*/, 5];
                        return [4 /*yield*/, this.getNegotiationResponse(url)];
                    case 4:
                        negotiateResponse = _a.sent();
                        connectUrl = this.createConnectUrl(url, negotiateResponse.connectionId);
                        _a.label = 5;
                    case 5:
                        _a.trys.push([5, 7, , 8]);
                        return [4 /*yield*/, this.transport.connect(connectUrl, requestedTransferFormat)];
                    case 6:
                        _a.sent();
                        this.changeState(0 /* Connecting */, 1 /* Connected */);
                        return [2 /*return*/];
                    case 7:
                        ex_1 = _a.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Error, "Failed to start the transport '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[transport] + "': " + ex_1);
                        this.connectionState = 2 /* Disconnected */;
                        negotiateResponse.connectionId = null;
                        return [3 /*break*/, 8];
                    case 8:
                        _i++;
                        return [3 /*break*/, 3];
                    case 9: throw new Error("Unable to initialize any of the available transports.");
                }
            });
        });
    };
    HttpConnection.prototype.constructTransport = function (transport) {
        switch (transport) {
            case _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.WebSockets:
                return new _WebSocketTransport__WEBPACK_IMPORTED_MODULE_6__.WebSocketTransport(this.accessTokenFactory, this.logger, this.options.logMessageContent);
            case _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.ServerSentEvents:
                return new _ServerSentEventsTransport__WEBPACK_IMPORTED_MODULE_4__.ServerSentEventsTransport(this.httpClient, this.accessTokenFactory, this.logger, this.options.logMessageContent);
            case _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.LongPolling:
                return new _LongPollingTransport__WEBPACK_IMPORTED_MODULE_3__.LongPollingTransport(this.httpClient, this.accessTokenFactory, this.logger, this.options.logMessageContent);
            default:
                throw new Error("Unknown transport: " + transport + ".");
        }
    };
    HttpConnection.prototype.resolveTransport = function (endpoint, requestedTransport, requestedTransferFormat) {
        var transport = _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[endpoint.transport];
        if (transport === null || transport === undefined) {
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Skipping transport '" + endpoint.transport + "' because it is not supported by this client.");
        }
        else {
            var transferFormats = endpoint.transferFormats.map(function (s) { return _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat[s]; });
            if (transportMatches(requestedTransport, transport)) {
                if (transferFormats.indexOf(requestedTransferFormat) >= 0) {
                    if ((transport === _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.WebSockets && typeof WebSocket === "undefined") ||
                        (transport === _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType.ServerSentEvents && typeof EventSource === "undefined")) {
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Skipping transport '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[transport] + "' because it is not supported in your environment.'");
                    }
                    else {
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Selecting transport '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[transport] + "'");
                        return transport;
                    }
                }
                else {
                    this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Skipping transport '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[transport] + "' because it does not support the requested transfer format '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat[requestedTransferFormat] + "'.");
                }
            }
            else {
                this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Debug, "Skipping transport '" + _ITransport__WEBPACK_IMPORTED_MODULE_2__.HttpTransportType[transport] + "' because it was disabled by the client.");
            }
        }
        return null;
    };
    HttpConnection.prototype.isITransport = function (transport) {
        return transport && typeof (transport) === "object" && "connect" in transport;
    };
    HttpConnection.prototype.changeState = function (from, to) {
        if (this.connectionState === from) {
            this.connectionState = to;
            return true;
        }
        return false;
    };
    HttpConnection.prototype.stopConnection = function (error) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                this.transport = null;
                // If we have a stopError, it takes precedence over the error from the transport
                error = this.stopError || error;
                if (error) {
                    this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Error, "Connection disconnected with error '" + error + "'.");
                }
                else {
                    this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Information, "Connection disconnected.");
                }
                this.connectionState = 2 /* Disconnected */;
                if (this.onclose) {
                    this.onclose(error);
                }
                return [2 /*return*/];
            });
        });
    };
    HttpConnection.prototype.resolveUrl = function (url) {
        // startsWith is not supported in IE
        if (url.lastIndexOf("https://", 0) === 0 || url.lastIndexOf("http://", 0) === 0) {
            return url;
        }
        if (typeof window === "undefined" || !window || !window.document) {
            throw new Error("Cannot resolve '" + url + "'.");
        }
        // Setting the url to the href propery of an anchor tag handles normalization
        // for us. There are 3 main cases.
        // 1. Relative  path normalization e.g "b" -> "http://localhost:5000/a/b"
        // 2. Absolute path normalization e.g "/a/b" -> "http://localhost:5000/a/b"
        // 3. Networkpath reference normalization e.g "//localhost:5000/a/b" -> "http://localhost:5000/a/b"
        var aTag = window.document.createElement("a");
        aTag.href = url;
        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Information, "Normalizing '" + url + "' to '" + aTag.href + "'.");
        return aTag.href;
    };
    HttpConnection.prototype.resolveNegotiateUrl = function (url) {
        var index = url.indexOf("?");
        var negotiateUrl = url.substring(0, index === -1 ? url.length : index);
        if (negotiateUrl[negotiateUrl.length - 1] !== "/") {
            negotiateUrl += "/";
        }
        negotiateUrl += "negotiate";
        negotiateUrl += index === -1 ? "" : url.substring(index);
        return negotiateUrl;
    };
    return HttpConnection;
}());

function transportMatches(requestedTransport, actualTransport) {
    return !requestedTransport || ((actualTransport & requestedTransport) !== 0);
}
//# sourceMappingURL=HttpConnection.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/HubConnection.js":
/*!****************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/HubConnection.js ***!
  \****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HubConnection": () => (/* binding */ HubConnection)
/* harmony export */ });
/* harmony import */ var _HandshakeProtocol__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./HandshakeProtocol */ "./node_modules/@aspnet/signalr/dist/esm/HandshakeProtocol.js");
/* harmony import */ var _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./IHubProtocol */ "./node_modules/@aspnet/signalr/dist/esm/IHubProtocol.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};




var DEFAULT_TIMEOUT_IN_MS = 30 * 1000;
/** Represents a connection to a SignalR Hub. */
var HubConnection = /** @class */ (function () {
    function HubConnection(connection, logger, protocol) {
        var _this = this;
        _Utils__WEBPACK_IMPORTED_MODULE_3__.Arg.isRequired(connection, "connection");
        _Utils__WEBPACK_IMPORTED_MODULE_3__.Arg.isRequired(logger, "logger");
        _Utils__WEBPACK_IMPORTED_MODULE_3__.Arg.isRequired(protocol, "protocol");
        this.serverTimeoutInMilliseconds = DEFAULT_TIMEOUT_IN_MS;
        this.logger = logger;
        this.protocol = protocol;
        this.connection = connection;
        this.handshakeProtocol = new _HandshakeProtocol__WEBPACK_IMPORTED_MODULE_0__.HandshakeProtocol();
        this.connection.onreceive = function (data) { return _this.processIncomingData(data); };
        this.connection.onclose = function (error) { return _this.connectionClosed(error); };
        this.callbacks = {};
        this.methods = {};
        this.closedCallbacks = [];
        this.id = 0;
    }
    /** @internal */
    // Using a public static factory method means we can have a private constructor and an _internal_
    // create method that can be used by HubConnectionBuilder. An "internal" constructor would just
    // be stripped away and the '.d.ts' file would have no constructor, which is interpreted as a
    // public parameter-less constructor.
    HubConnection.create = function (connection, logger, protocol) {
        return new HubConnection(connection, logger, protocol);
    };
    /** Starts the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully established, or rejects with an error.
     */
    HubConnection.prototype.start = function () {
        return __awaiter(this, void 0, void 0, function () {
            var handshakeRequest;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        handshakeRequest = {
                            protocol: this.protocol.name,
                            version: this.protocol.version,
                        };
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Debug, "Starting HubConnection.");
                        this.receivedHandshakeResponse = false;
                        return [4 /*yield*/, this.connection.start(this.protocol.transferFormat)];
                    case 1:
                        _a.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Debug, "Sending handshake request.");
                        return [4 /*yield*/, this.connection.send(this.handshakeProtocol.writeHandshakeRequest(handshakeRequest))];
                    case 2:
                        _a.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Information, "Using HubProtocol '" + this.protocol.name + "'.");
                        // defensively cleanup timeout in case we receive a message from the server before we finish start
                        this.cleanupTimeout();
                        this.configureTimeout();
                        return [2 /*return*/];
                }
            });
        });
    };
    /** Stops the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully terminated, or rejects with an error.
     */
    HubConnection.prototype.stop = function () {
        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Debug, "Stopping HubConnection.");
        this.cleanupTimeout();
        return this.connection.stop();
    };
    /** Invokes a streaming hub method on the server using the specified name and arguments.
     *
     * @typeparam T The type of the items returned by the server.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {IStreamResult<T>} An object that yields results from the server as they are received.
     */
    HubConnection.prototype.stream = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createStreamInvocation(methodName, args);
        var subject = new _Utils__WEBPACK_IMPORTED_MODULE_3__.Subject(function () {
            var cancelInvocation = _this.createCancelInvocation(invocationDescriptor.invocationId);
            var cancelMessage = _this.protocol.writeMessage(cancelInvocation);
            delete _this.callbacks[invocationDescriptor.invocationId];
            return _this.connection.send(cancelMessage);
        });
        this.callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
            if (error) {
                subject.error(error);
                return;
            }
            if (invocationEvent.type === _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Completion) {
                if (invocationEvent.error) {
                    subject.error(new Error(invocationEvent.error));
                }
                else {
                    subject.complete();
                }
            }
            else {
                subject.next((invocationEvent.item));
            }
        };
        var message = this.protocol.writeMessage(invocationDescriptor);
        this.connection.send(message)
            .catch(function (e) {
            subject.error(e);
            delete _this.callbacks[invocationDescriptor.invocationId];
        });
        return subject;
    };
    /** Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
     *
     * The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
     * be processing the invocation.
     *
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<void>} A Promise that resolves when the invocation has been successfully sent, or rejects with an error.
     */
    HubConnection.prototype.send = function (methodName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createInvocation(methodName, args, true);
        var message = this.protocol.writeMessage(invocationDescriptor);
        return this.connection.send(message);
    };
    /** Invokes a hub method on the server using the specified name and arguments.
     *
     * The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
     * resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
     * resolving the Promise.
     *
     * @typeparam T The expected return type.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<T>} A Promise that resolves with the result of the server method (if any), or rejects with an error.
     */
    HubConnection.prototype.invoke = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var invocationDescriptor = this.createInvocation(methodName, args, false);
        var p = new Promise(function (resolve, reject) {
            _this.callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
                if (error) {
                    reject(error);
                    return;
                }
                if (invocationEvent.type === _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Completion) {
                    var completionMessage = invocationEvent;
                    if (completionMessage.error) {
                        reject(new Error(completionMessage.error));
                    }
                    else {
                        resolve(completionMessage.result);
                    }
                }
                else {
                    reject(new Error("Unexpected message type: " + invocationEvent.type));
                }
            };
            var message = _this.protocol.writeMessage(invocationDescriptor);
            _this.connection.send(message)
                .catch(function (e) {
                reject(e);
                delete _this.callbacks[invocationDescriptor.invocationId];
            });
        });
        return p;
    };
    /** Registers a handler that will be invoked when the hub method with the specified method name is invoked.
     *
     * @param {string} methodName The name of the hub method to define.
     * @param {Function} newMethod The handler that will be raised when the hub method is invoked.
     */
    HubConnection.prototype.on = function (methodName, newMethod) {
        if (!methodName || !newMethod) {
            return;
        }
        methodName = methodName.toLowerCase();
        if (!this.methods[methodName]) {
            this.methods[methodName] = [];
        }
        // Preventing adding the same handler multiple times.
        if (this.methods[methodName].indexOf(newMethod) !== -1) {
            return;
        }
        this.methods[methodName].push(newMethod);
    };
    HubConnection.prototype.off = function (methodName, method) {
        if (!methodName) {
            return;
        }
        methodName = methodName.toLowerCase();
        var handlers = this.methods[methodName];
        if (!handlers) {
            return;
        }
        if (method) {
            var removeIdx = handlers.indexOf(method);
            if (removeIdx !== -1) {
                handlers.splice(removeIdx, 1);
                if (handlers.length === 0) {
                    delete this.methods[methodName];
                }
            }
        }
        else {
            delete this.methods[methodName];
        }
    };
    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
     */
    HubConnection.prototype.onclose = function (callback) {
        if (callback) {
            this.closedCallbacks.push(callback);
        }
    };
    HubConnection.prototype.processIncomingData = function (data) {
        this.cleanupTimeout();
        if (!this.receivedHandshakeResponse) {
            data = this.processHandshakeResponse(data);
            this.receivedHandshakeResponse = true;
        }
        // Data may have all been read when processing handshake response
        if (data) {
            // Parse the messages
            var messages = this.protocol.parseMessages(data, this.logger);
            for (var _i = 0, messages_1 = messages; _i < messages_1.length; _i++) {
                var message = messages_1[_i];
                switch (message.type) {
                    case _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Invocation:
                        this.invokeClientMethod(message);
                        break;
                    case _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.StreamItem:
                    case _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Completion:
                        var callback = this.callbacks[message.invocationId];
                        if (callback != null) {
                            if (message.type === _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Completion) {
                                delete this.callbacks[message.invocationId];
                            }
                            callback(message);
                        }
                        break;
                    case _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Ping:
                        // Don't care about pings
                        break;
                    case _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Close:
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Information, "Close message received from server.");
                        // We don't want to wait on the stop itself.
                        // tslint:disable-next-line:no-floating-promises
                        this.connection.stop(message.error ? new Error("Server returned an error on close: " + message.error) : null);
                        break;
                    default:
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Warning, "Invalid message type: " + message.type);
                        break;
                }
            }
        }
        this.configureTimeout();
    };
    HubConnection.prototype.processHandshakeResponse = function (data) {
        var _a;
        var responseMessage;
        var remainingData;
        try {
            _a = this.handshakeProtocol.parseHandshakeResponse(data), remainingData = _a[0], responseMessage = _a[1];
        }
        catch (e) {
            var message = "Error parsing handshake response: " + e;
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Error, message);
            var error = new Error(message);
            // We don't want to wait on the stop itself.
            // tslint:disable-next-line:no-floating-promises
            this.connection.stop(error);
            throw error;
        }
        if (responseMessage.error) {
            var message = "Server returned handshake error: " + responseMessage.error;
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Error, message);
            // We don't want to wait on the stop itself.
            // tslint:disable-next-line:no-floating-promises
            this.connection.stop(new Error(message));
        }
        else {
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Debug, "Server handshake complete.");
        }
        return remainingData;
    };
    HubConnection.prototype.configureTimeout = function () {
        var _this = this;
        if (!this.connection.features || !this.connection.features.inherentKeepAlive) {
            // Set the timeout timer
            this.timeoutHandle = setTimeout(function () { return _this.serverTimeout(); }, this.serverTimeoutInMilliseconds);
        }
    };
    HubConnection.prototype.serverTimeout = function () {
        // The server hasn't talked to us in a while. It doesn't like us anymore ... :(
        // Terminate the connection, but we don't need to wait on the promise.
        // tslint:disable-next-line:no-floating-promises
        this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."));
    };
    HubConnection.prototype.invokeClientMethod = function (invocationMessage) {
        var _this = this;
        var methods = this.methods[invocationMessage.target.toLowerCase()];
        if (methods) {
            methods.forEach(function (m) { return m.apply(_this, invocationMessage.arguments); });
            if (invocationMessage.invocationId) {
                // This is not supported in v1. So we return an error to avoid blocking the server waiting for the response.
                var message = "Server requested a response, which is not supported in this version of the client.";
                this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Error, message);
                // We don't need to wait on this Promise.
                // tslint:disable-next-line:no-floating-promises
                this.connection.stop(new Error(message));
            }
        }
        else {
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Warning, "No client method with the name '" + invocationMessage.target + "' found.");
        }
    };
    HubConnection.prototype.connectionClosed = function (error) {
        var _this = this;
        var callbacks = this.callbacks;
        this.callbacks = {};
        Object.keys(callbacks)
            .forEach(function (key) {
            var callback = callbacks[key];
            callback(undefined, error ? error : new Error("Invocation canceled due to connection being closed."));
        });
        this.cleanupTimeout();
        this.closedCallbacks.forEach(function (c) { return c.apply(_this, [error]); });
    };
    HubConnection.prototype.cleanupTimeout = function () {
        if (this.timeoutHandle) {
            clearTimeout(this.timeoutHandle);
        }
    };
    HubConnection.prototype.createInvocation = function (methodName, args, nonblocking) {
        if (nonblocking) {
            return {
                arguments: args,
                target: methodName,
                type: _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Invocation,
            };
        }
        else {
            var id = this.id;
            this.id++;
            return {
                arguments: args,
                invocationId: id.toString(),
                target: methodName,
                type: _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.Invocation,
            };
        }
    };
    HubConnection.prototype.createStreamInvocation = function (methodName, args) {
        var id = this.id;
        this.id++;
        return {
            arguments: args,
            invocationId: id.toString(),
            target: methodName,
            type: _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.StreamInvocation,
        };
    };
    HubConnection.prototype.createCancelInvocation = function (id) {
        return {
            invocationId: id,
            type: _IHubProtocol__WEBPACK_IMPORTED_MODULE_1__.MessageType.CancelInvocation,
        };
    };
    return HubConnection;
}());

//# sourceMappingURL=HubConnection.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/HubConnectionBuilder.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/HubConnectionBuilder.js ***!
  \***********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HubConnectionBuilder": () => (/* binding */ HubConnectionBuilder)
/* harmony export */ });
/* harmony import */ var _HttpConnection__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./HttpConnection */ "./node_modules/@aspnet/signalr/dist/esm/HttpConnection.js");
/* harmony import */ var _HubConnection__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./HubConnection */ "./node_modules/@aspnet/signalr/dist/esm/HubConnection.js");
/* harmony import */ var _JsonHubProtocol__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./JsonHubProtocol */ "./node_modules/@aspnet/signalr/dist/esm/JsonHubProtocol.js");
/* harmony import */ var _Loggers__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./Loggers */ "./node_modules/@aspnet/signalr/dist/esm/Loggers.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.





/** A builder for configuring {@link @aspnet/signalr.HubConnection} instances. */
var HubConnectionBuilder = /** @class */ (function () {
    function HubConnectionBuilder() {
    }
    HubConnectionBuilder.prototype.configureLogging = function (logging) {
        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isRequired(logging, "logging");
        if (isLogger(logging)) {
            this.logger = logging;
        }
        else {
            this.logger = new _Utils__WEBPACK_IMPORTED_MODULE_4__.ConsoleLogger(logging);
        }
        return this;
    };
    HubConnectionBuilder.prototype.withUrl = function (url, transportTypeOrOptions) {
        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isRequired(url, "url");
        this.url = url;
        // Flow-typing knows where it's at. Since HttpTransportType is a number and IHttpConnectionOptions is guaranteed
        // to be an object, we know (as does TypeScript) this comparison is all we need to figure out which overload was called.
        if (typeof transportTypeOrOptions === "object") {
            this.httpConnectionOptions = transportTypeOrOptions;
        }
        else {
            this.httpConnectionOptions = {
                transport: transportTypeOrOptions,
            };
        }
        return this;
    };
    /** Configures the {@link @aspnet/signalr.HubConnection} to use the specified Hub Protocol.
     *
     * @param {IHubProtocol} protocol The {@link @aspnet/signalr.IHubProtocol} implementation to use.
     */
    HubConnectionBuilder.prototype.withHubProtocol = function (protocol) {
        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isRequired(protocol, "protocol");
        this.protocol = protocol;
        return this;
    };
    /** Creates a {@link @aspnet/signalr.HubConnection} from the configuration options specified in this builder.
     *
     * @returns {HubConnection} The configured {@link @aspnet/signalr.HubConnection}.
     */
    HubConnectionBuilder.prototype.build = function () {
        // If httpConnectionOptions has a logger, use it. Otherwise, override it with the one
        // provided to configureLogger
        var httpConnectionOptions = this.httpConnectionOptions || {};
        // If it's 'null', the user **explicitly** asked for null, don't mess with it.
        if (httpConnectionOptions.logger === undefined) {
            // If our logger is undefined or null, that's OK, the HttpConnection constructor will handle it.
            httpConnectionOptions.logger = this.logger;
        }
        // Now create the connection
        if (!this.url) {
            throw new Error("The 'HubConnectionBuilder.withUrl' method must be called before building the connection.");
        }
        var connection = new _HttpConnection__WEBPACK_IMPORTED_MODULE_0__.HttpConnection(this.url, httpConnectionOptions);
        return _HubConnection__WEBPACK_IMPORTED_MODULE_1__.HubConnection.create(connection, this.logger || _Loggers__WEBPACK_IMPORTED_MODULE_3__.NullLogger.instance, this.protocol || new _JsonHubProtocol__WEBPACK_IMPORTED_MODULE_2__.JsonHubProtocol());
    };
    return HubConnectionBuilder;
}());

function isLogger(logger) {
    return logger.log !== undefined;
}
//# sourceMappingURL=HubConnectionBuilder.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/IHubProtocol.js":
/*!***************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/IHubProtocol.js ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "MessageType": () => (/* binding */ MessageType)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** Defines the type of a Hub Message. */
var MessageType;
(function (MessageType) {
    /** Indicates the message is an Invocation message and implements the {@link @aspnet/signalr.InvocationMessage} interface. */
    MessageType[MessageType["Invocation"] = 1] = "Invocation";
    /** Indicates the message is a StreamItem message and implements the {@link @aspnet/signalr.StreamItemMessage} interface. */
    MessageType[MessageType["StreamItem"] = 2] = "StreamItem";
    /** Indicates the message is a Completion message and implements the {@link @aspnet/signalr.CompletionMessage} interface. */
    MessageType[MessageType["Completion"] = 3] = "Completion";
    /** Indicates the message is a Stream Invocation message and implements the {@link @aspnet/signalr.StreamInvocationMessage} interface. */
    MessageType[MessageType["StreamInvocation"] = 4] = "StreamInvocation";
    /** Indicates the message is a Cancel Invocation message and implements the {@link @aspnet/signalr.CancelInvocationMessage} interface. */
    MessageType[MessageType["CancelInvocation"] = 5] = "CancelInvocation";
    /** Indicates the message is a Ping message and implements the {@link @aspnet/signalr.PingMessage} interface. */
    MessageType[MessageType["Ping"] = 6] = "Ping";
    /** Indicates the message is a Close message and implements the {@link @aspnet/signalr.CloseMessage} interface. */
    MessageType[MessageType["Close"] = 7] = "Close";
})(MessageType || (MessageType = {}));
//# sourceMappingURL=IHubProtocol.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js":
/*!**********************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/ILogger.js ***!
  \**********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "LogLevel": () => (/* binding */ LogLevel)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// These values are designed to match the ASP.NET Log Levels since that's the pattern we're emulating here.
/** Indicates the severity of a log message.
 *
 * Log Levels are ordered in increasing severity. So `Debug` is more severe than `Trace`, etc.
 */
var LogLevel;
(function (LogLevel) {
    /** Log level for very low severity diagnostic messages. */
    LogLevel[LogLevel["Trace"] = 0] = "Trace";
    /** Log level for low severity diagnostic messages. */
    LogLevel[LogLevel["Debug"] = 1] = "Debug";
    /** Log level for informational diagnostic messages. */
    LogLevel[LogLevel["Information"] = 2] = "Information";
    /** Log level for diagnostic messages that indicate a non-fatal problem. */
    LogLevel[LogLevel["Warning"] = 3] = "Warning";
    /** Log level for diagnostic messages that indicate a failure in the current operation. */
    LogLevel[LogLevel["Error"] = 4] = "Error";
    /** Log level for diagnostic messages that indicate a failure that will terminate the entire application. */
    LogLevel[LogLevel["Critical"] = 5] = "Critical";
    /** The highest possible log level. Used when configuring logging to indicate that no log messages should be emitted. */
    LogLevel[LogLevel["None"] = 6] = "None";
})(LogLevel || (LogLevel = {}));
//# sourceMappingURL=ILogger.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js":
/*!*************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/ITransport.js ***!
  \*************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HttpTransportType": () => (/* binding */ HttpTransportType),
/* harmony export */   "TransferFormat": () => (/* binding */ TransferFormat)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This will be treated as a bit flag in the future, so we keep it using power-of-two values.
/** Specifies a specific HTTP transport type. */
var HttpTransportType;
(function (HttpTransportType) {
    /** Specifies no transport preference. */
    HttpTransportType[HttpTransportType["None"] = 0] = "None";
    /** Specifies the WebSockets transport. */
    HttpTransportType[HttpTransportType["WebSockets"] = 1] = "WebSockets";
    /** Specifies the Server-Sent Events transport. */
    HttpTransportType[HttpTransportType["ServerSentEvents"] = 2] = "ServerSentEvents";
    /** Specifies the Long Polling transport. */
    HttpTransportType[HttpTransportType["LongPolling"] = 4] = "LongPolling";
})(HttpTransportType || (HttpTransportType = {}));
/** Specifies the transfer format for a connection. */
var TransferFormat;
(function (TransferFormat) {
    /** Specifies that only text data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Text"] = 1] = "Text";
    /** Specifies that binary data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Binary"] = 2] = "Binary";
})(TransferFormat || (TransferFormat = {}));
//# sourceMappingURL=ITransport.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/JsonHubProtocol.js":
/*!******************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/JsonHubProtocol.js ***!
  \******************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "JsonHubProtocol": () => (/* binding */ JsonHubProtocol)
/* harmony export */ });
/* harmony import */ var _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./IHubProtocol */ "./node_modules/@aspnet/signalr/dist/esm/IHubProtocol.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _Loggers__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./Loggers */ "./node_modules/@aspnet/signalr/dist/esm/Loggers.js");
/* harmony import */ var _TextMessageFormat__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./TextMessageFormat */ "./node_modules/@aspnet/signalr/dist/esm/TextMessageFormat.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.





var JSON_HUB_PROTOCOL_NAME = "json";
/** Implements the JSON Hub Protocol. */
var JsonHubProtocol = /** @class */ (function () {
    function JsonHubProtocol() {
        /** @inheritDoc */
        this.name = JSON_HUB_PROTOCOL_NAME;
        /** @inheritDoc */
        this.version = 1;
        /** @inheritDoc */
        this.transferFormat = _ITransport__WEBPACK_IMPORTED_MODULE_2__.TransferFormat.Text;
    }
    /** Creates an array of {@link @aspnet/signalr.HubMessage} objects from the specified serialized representation.
     *
     * @param {string} input A string containing the serialized representation.
     * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
     */
    JsonHubProtocol.prototype.parseMessages = function (input, logger) {
        // The interface does allow "ArrayBuffer" to be passed in, but this implementation does not. So let's throw a useful error.
        if (typeof input !== "string") {
            throw new Error("Invalid input for JSON hub protocol. Expected a string.");
        }
        if (!input) {
            return [];
        }
        if (logger === null) {
            logger = _Loggers__WEBPACK_IMPORTED_MODULE_3__.NullLogger.instance;
        }
        // Parse the messages
        var messages = _TextMessageFormat__WEBPACK_IMPORTED_MODULE_4__.TextMessageFormat.parse(input);
        var hubMessages = [];
        for (var _i = 0, messages_1 = messages; _i < messages_1.length; _i++) {
            var message = messages_1[_i];
            var parsedMessage = JSON.parse(message);
            if (typeof parsedMessage.type !== "number") {
                throw new Error("Invalid payload.");
            }
            switch (parsedMessage.type) {
                case _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__.MessageType.Invocation:
                    this.isInvocationMessage(parsedMessage);
                    break;
                case _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__.MessageType.StreamItem:
                    this.isStreamItemMessage(parsedMessage);
                    break;
                case _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__.MessageType.Completion:
                    this.isCompletionMessage(parsedMessage);
                    break;
                case _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__.MessageType.Ping:
                    // Single value, no need to validate
                    break;
                case _IHubProtocol__WEBPACK_IMPORTED_MODULE_0__.MessageType.Close:
                    // All optional values, no need to validate
                    break;
                default:
                    // Future protocol changes can add message types, old clients can ignore them
                    logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_1__.LogLevel.Information, "Unknown message type '" + parsedMessage.type + "' ignored.");
                    continue;
            }
            hubMessages.push(parsedMessage);
        }
        return hubMessages;
    };
    /** Writes the specified {@link @aspnet/signalr.HubMessage} to a string and returns it.
     *
     * @param {HubMessage} message The message to write.
     * @returns {string} A string containing the serialized representation of the message.
     */
    JsonHubProtocol.prototype.writeMessage = function (message) {
        return _TextMessageFormat__WEBPACK_IMPORTED_MODULE_4__.TextMessageFormat.write(JSON.stringify(message));
    };
    JsonHubProtocol.prototype.isInvocationMessage = function (message) {
        this.assertNotEmptyString(message.target, "Invalid payload for Invocation message.");
        if (message.invocationId !== undefined) {
            this.assertNotEmptyString(message.invocationId, "Invalid payload for Invocation message.");
        }
    };
    JsonHubProtocol.prototype.isStreamItemMessage = function (message) {
        this.assertNotEmptyString(message.invocationId, "Invalid payload for StreamItem message.");
        if (message.item === undefined) {
            throw new Error("Invalid payload for StreamItem message.");
        }
    };
    JsonHubProtocol.prototype.isCompletionMessage = function (message) {
        if (message.result && message.error) {
            throw new Error("Invalid payload for Completion message.");
        }
        if (!message.result && message.error) {
            this.assertNotEmptyString(message.error, "Invalid payload for Completion message.");
        }
        this.assertNotEmptyString(message.invocationId, "Invalid payload for Completion message.");
    };
    JsonHubProtocol.prototype.assertNotEmptyString = function (value, errorMessage) {
        if (typeof value !== "string" || value === "") {
            throw new Error(errorMessage);
        }
    };
    return JsonHubProtocol;
}());

//# sourceMappingURL=JsonHubProtocol.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/Loggers.js":
/*!**********************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/Loggers.js ***!
  \**********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "NullLogger": () => (/* binding */ NullLogger)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
/** A logger that does nothing when log messages are sent to it. */
var NullLogger = /** @class */ (function () {
    function NullLogger() {
    }
    /** @inheritDoc */
    // tslint:disable-next-line
    NullLogger.prototype.log = function (_logLevel, _message) {
    };
    /** The singleton instance of the {@link @aspnet/signalr.NullLogger}. */
    NullLogger.instance = new NullLogger();
    return NullLogger;
}());

//# sourceMappingURL=Loggers.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/LongPollingTransport.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/LongPollingTransport.js ***!
  \***********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "LongPollingTransport": () => (/* binding */ LongPollingTransport)
/* harmony export */ });
/* harmony import */ var _AbortController__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./AbortController */ "./node_modules/@aspnet/signalr/dist/esm/AbortController.js");
/* harmony import */ var _Errors__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Errors */ "./node_modules/@aspnet/signalr/dist/esm/Errors.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





var SHUTDOWN_TIMEOUT = 5 * 1000;
// Not exported from 'index', this type is internal.
/** @private */
var LongPollingTransport = /** @class */ (function () {
    function LongPollingTransport(httpClient, accessTokenFactory, logger, logMessageContent, shutdownTimeout) {
        this.httpClient = httpClient;
        this.accessTokenFactory = accessTokenFactory || (function () { return null; });
        this.logger = logger;
        this.pollAbort = new _AbortController__WEBPACK_IMPORTED_MODULE_0__.AbortController();
        this.logMessageContent = logMessageContent;
        this.shutdownTimeout = shutdownTimeout || SHUTDOWN_TIMEOUT;
    }
    Object.defineProperty(LongPollingTransport.prototype, "pollAborted", {
        // This is an internal type, not exported from 'index' so this is really just internal.
        get: function () {
            return this.pollAbort.aborted;
        },
        enumerable: true,
        configurable: true
    });
    LongPollingTransport.prototype.connect = function (url, transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var pollOptions, token, closeError, pollUrl, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isRequired(url, "url");
                        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isRequired(transferFormat, "transferFormat");
                        _Utils__WEBPACK_IMPORTED_MODULE_4__.Arg.isIn(transferFormat, _ITransport__WEBPACK_IMPORTED_MODULE_3__.TransferFormat, "transferFormat");
                        this.url = url;
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Connecting");
                        if (transferFormat === _ITransport__WEBPACK_IMPORTED_MODULE_3__.TransferFormat.Binary && (typeof new XMLHttpRequest().responseType !== "string")) {
                            // This will work if we fix: https://github.com/aspnet/SignalR/issues/742
                            throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");
                        }
                        pollOptions = {
                            abortSignal: this.pollAbort.signal,
                            headers: {},
                            timeout: 90000,
                        };
                        if (transferFormat === _ITransport__WEBPACK_IMPORTED_MODULE_3__.TransferFormat.Binary) {
                            pollOptions.responseType = "arraybuffer";
                        }
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        this.updateHeaderToken(pollOptions, token);
                        pollUrl = url + "&_=" + Date.now();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) polling: " + pollUrl);
                        return [4 /*yield*/, this.httpClient.get(pollUrl, pollOptions)];
                    case 2:
                        response = _a.sent();
                        if (response.statusCode !== 200) {
                            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Error, "(LongPolling transport) Unexpected response code: " + response.statusCode);
                            // Mark running as false so that the poll immediately ends and runs the close logic
                            closeError = new _Errors__WEBPACK_IMPORTED_MODULE_1__.HttpError(response.statusText, response.statusCode);
                            this.running = false;
                        }
                        else {
                            this.running = true;
                        }
                        // tslint:disable-next-line:no-floating-promises
                        this.poll(this.url, pollOptions, closeError);
                        return [2 /*return*/, Promise.resolve()];
                }
            });
        });
    };
    LongPollingTransport.prototype.updateHeaderToken = function (request, token) {
        if (token) {
            // tslint:disable-next-line:no-string-literal
            request.headers["Authorization"] = "Bearer " + token;
            return;
        }
        // tslint:disable-next-line:no-string-literal
        if (request.headers["Authorization"]) {
            // tslint:disable-next-line:no-string-literal
            delete request.headers["Authorization"];
        }
    };
    LongPollingTransport.prototype.poll = function (url, pollOptions, closeError) {
        return __awaiter(this, void 0, void 0, function () {
            var token, pollUrl, response, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, , 8, 9]);
                        _a.label = 1;
                    case 1:
                        if (!this.running) return [3 /*break*/, 7];
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 2:
                        token = _a.sent();
                        this.updateHeaderToken(pollOptions, token);
                        _a.label = 3;
                    case 3:
                        _a.trys.push([3, 5, , 6]);
                        pollUrl = url + "&_=" + Date.now();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) polling: " + pollUrl);
                        return [4 /*yield*/, this.httpClient.get(pollUrl, pollOptions)];
                    case 4:
                        response = _a.sent();
                        if (response.statusCode === 204) {
                            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Information, "(LongPolling transport) Poll terminated by server");
                            this.running = false;
                        }
                        else if (response.statusCode !== 200) {
                            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Error, "(LongPolling transport) Unexpected response code: " + response.statusCode);
                            // Unexpected status code
                            closeError = new _Errors__WEBPACK_IMPORTED_MODULE_1__.HttpError(response.statusText, response.statusCode);
                            this.running = false;
                        }
                        else {
                            // Process the response
                            if (response.content) {
                                this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) data received. " + (0,_Utils__WEBPACK_IMPORTED_MODULE_4__.getDataDetail)(response.content, this.logMessageContent));
                                if (this.onreceive) {
                                    this.onreceive(response.content);
                                }
                            }
                            else {
                                // This is another way timeout manifest.
                                this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                        }
                        return [3 /*break*/, 6];
                    case 5:
                        e_1 = _a.sent();
                        if (!this.running) {
                            // Log but disregard errors that occur after we were stopped by DELETE
                            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Poll errored after shutdown: " + e_1.message);
                        }
                        else {
                            if (e_1 instanceof _Errors__WEBPACK_IMPORTED_MODULE_1__.TimeoutError) {
                                // Ignore timeouts and reissue the poll.
                                this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                            else {
                                // Close the connection with the error as the result.
                                closeError = e_1;
                                this.running = false;
                            }
                        }
                        return [3 /*break*/, 6];
                    case 6: return [3 /*break*/, 1];
                    case 7: return [3 /*break*/, 9];
                    case 8:
                        // Indicate that we've stopped so the shutdown timer doesn't get registered.
                        this.stopped = true;
                        // Clean up the shutdown timer if it was registered
                        if (this.shutdownTimer) {
                            clearTimeout(this.shutdownTimer);
                        }
                        // Fire our onclosed event
                        if (this.onclose) {
                            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Firing onclose event. Error: " + (closeError || "<undefined>"));
                            this.onclose(closeError);
                        }
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) Transport finished.");
                        return [7 /*endfinally*/];
                    case 9: return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype.send = function (data) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                if (!this.running) {
                    return [2 /*return*/, Promise.reject(new Error("Cannot send until the transport is connected"))];
                }
                return [2 /*return*/, (0,_Utils__WEBPACK_IMPORTED_MODULE_4__.sendMessage)(this.logger, "LongPolling", this.httpClient, this.url, this.accessTokenFactory, data, this.logMessageContent)];
            });
        });
    };
    LongPollingTransport.prototype.stop = function () {
        return __awaiter(this, void 0, void 0, function () {
            var deleteOptions, token;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, , 3, 4]);
                        this.running = false;
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) sending DELETE request to " + this.url + ".");
                        deleteOptions = {
                            headers: {},
                        };
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        this.updateHeaderToken(deleteOptions, token);
                        return [4 /*yield*/, this.httpClient.delete(this.url, deleteOptions)];
                    case 2:
                        _a.sent();
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Trace, "(LongPolling transport) DELETE request accepted.");
                        return [3 /*break*/, 4];
                    case 3:
                        // Abort the poll after the shutdown timeout if the server doesn't stop the poll.
                        if (!this.stopped) {
                            this.shutdownTimer = setTimeout(function () {
                                _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_2__.LogLevel.Warning, "(LongPolling transport) server did not terminate after DELETE request, canceling poll.");
                                // Abort any outstanding poll
                                _this.pollAbort.abort();
                            }, this.shutdownTimeout);
                        }
                        return [7 /*endfinally*/];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    return LongPollingTransport;
}());

//# sourceMappingURL=LongPollingTransport.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/ServerSentEventsTransport.js":
/*!****************************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/ServerSentEventsTransport.js ***!
  \****************************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "ServerSentEventsTransport": () => (/* binding */ ServerSentEventsTransport)
/* harmony export */ });
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};



/** @private */
var ServerSentEventsTransport = /** @class */ (function () {
    function ServerSentEventsTransport(httpClient, accessTokenFactory, logger, logMessageContent) {
        this.httpClient = httpClient;
        this.accessTokenFactory = accessTokenFactory || (function () { return null; });
        this.logger = logger;
        this.logMessageContent = logMessageContent;
    }
    ServerSentEventsTransport.prototype.connect = function (url, transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var token;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isRequired(url, "url");
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isRequired(transferFormat, "transferFormat");
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isIn(transferFormat, _ITransport__WEBPACK_IMPORTED_MODULE_1__.TransferFormat, "transferFormat");
                        if (typeof (EventSource) === "undefined") {
                            throw new Error("'EventSource' is not supported in your environment.");
                        }
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(SSE transport) Connecting");
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        if (token) {
                            url += (url.indexOf("?") < 0 ? "?" : "&") + ("access_token=" + encodeURIComponent(token));
                        }
                        this.url = url;
                        return [2 /*return*/, new Promise(function (resolve, reject) {
                                var opened = false;
                                if (transferFormat !== _ITransport__WEBPACK_IMPORTED_MODULE_1__.TransferFormat.Text) {
                                    reject(new Error("The Server-Sent Events transport only supports the 'Text' transfer format"));
                                }
                                var eventSource = new EventSource(url, { withCredentials: true });
                                try {
                                    eventSource.onmessage = function (e) {
                                        if (_this.onreceive) {
                                            try {
                                                _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(SSE transport) data received. " + (0,_Utils__WEBPACK_IMPORTED_MODULE_2__.getDataDetail)(e.data, _this.logMessageContent) + ".");
                                                _this.onreceive(e.data);
                                            }
                                            catch (error) {
                                                if (_this.onclose) {
                                                    _this.onclose(error);
                                                }
                                                return;
                                            }
                                        }
                                    };
                                    eventSource.onerror = function (e) {
                                        var error = new Error(e.message || "Error occurred");
                                        if (opened) {
                                            _this.close(error);
                                        }
                                        else {
                                            reject(error);
                                        }
                                    };
                                    eventSource.onopen = function () {
                                        _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Information, "SSE connected to " + _this.url);
                                        _this.eventSource = eventSource;
                                        opened = true;
                                        resolve();
                                    };
                                }
                                catch (e) {
                                    return Promise.reject(e);
                                }
                            })];
                }
            });
        });
    };
    ServerSentEventsTransport.prototype.send = function (data) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                if (!this.eventSource) {
                    return [2 /*return*/, Promise.reject(new Error("Cannot send until the transport is connected"))];
                }
                return [2 /*return*/, (0,_Utils__WEBPACK_IMPORTED_MODULE_2__.sendMessage)(this.logger, "SSE", this.httpClient, this.url, this.accessTokenFactory, data, this.logMessageContent)];
            });
        });
    };
    ServerSentEventsTransport.prototype.stop = function () {
        this.close();
        return Promise.resolve();
    };
    ServerSentEventsTransport.prototype.close = function (e) {
        if (this.eventSource) {
            this.eventSource.close();
            this.eventSource = null;
            if (this.onclose) {
                this.onclose(e);
            }
        }
    };
    return ServerSentEventsTransport;
}());

//# sourceMappingURL=ServerSentEventsTransport.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/TextMessageFormat.js":
/*!********************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/TextMessageFormat.js ***!
  \********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "TextMessageFormat": () => (/* binding */ TextMessageFormat)
/* harmony export */ });
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Not exported from index
/** @private */
var TextMessageFormat = /** @class */ (function () {
    function TextMessageFormat() {
    }
    TextMessageFormat.write = function (output) {
        return "" + output + TextMessageFormat.RecordSeparator;
    };
    TextMessageFormat.parse = function (input) {
        if (input[input.length - 1] !== TextMessageFormat.RecordSeparator) {
            throw new Error("Message is incomplete.");
        }
        var messages = input.split(TextMessageFormat.RecordSeparator);
        messages.pop();
        return messages;
    };
    TextMessageFormat.RecordSeparatorCode = 0x1e;
    TextMessageFormat.RecordSeparator = String.fromCharCode(TextMessageFormat.RecordSeparatorCode);
    return TextMessageFormat;
}());

//# sourceMappingURL=TextMessageFormat.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/Utils.js":
/*!********************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/Utils.js ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "Arg": () => (/* binding */ Arg),
/* harmony export */   "getDataDetail": () => (/* binding */ getDataDetail),
/* harmony export */   "formatArrayBuffer": () => (/* binding */ formatArrayBuffer),
/* harmony export */   "sendMessage": () => (/* binding */ sendMessage),
/* harmony export */   "createLogger": () => (/* binding */ createLogger),
/* harmony export */   "Subject": () => (/* binding */ Subject),
/* harmony export */   "SubjectSubscription": () => (/* binding */ SubjectSubscription),
/* harmony export */   "ConsoleLogger": () => (/* binding */ ConsoleLogger),
/* harmony export */   "isArrayBuffer": () => (/* binding */ isArrayBuffer)
/* harmony export */ });
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _Loggers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Loggers */ "./node_modules/@aspnet/signalr/dist/esm/Loggers.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};


/** @private */
var Arg = /** @class */ (function () {
    function Arg() {
    }
    Arg.isRequired = function (val, name) {
        if (val === null || val === undefined) {
            throw new Error("The '" + name + "' argument is required.");
        }
    };
    Arg.isIn = function (val, values, name) {
        // TypeScript enums have keys for **both** the name and the value of each enum member on the type itself.
        if (!(val in values)) {
            throw new Error("Unknown " + name + " value: " + val + ".");
        }
    };
    return Arg;
}());

/** @private */
function getDataDetail(data, includeContent) {
    var length = null;
    if (isArrayBuffer(data)) {
        length = "Binary data of length " + data.byteLength;
        if (includeContent) {
            length += ". Content: '" + formatArrayBuffer(data) + "'";
        }
    }
    else if (typeof data === "string") {
        length = "String data of length " + data.length;
        if (includeContent) {
            length += ". Content: '" + data + "'.";
        }
    }
    return length;
}
/** @private */
function formatArrayBuffer(data) {
    var view = new Uint8Array(data);
    // Uint8Array.map only supports returning another Uint8Array?
    var str = "";
    view.forEach(function (num) {
        var pad = num < 16 ? "0" : "";
        str += "0x" + pad + num.toString(16) + " ";
    });
    // Trim of trailing space.
    return str.substr(0, str.length - 1);
}
/** @private */
function sendMessage(logger, transportName, httpClient, url, accessTokenFactory, content, logMessageContent) {
    return __awaiter(this, void 0, void 0, function () {
        var _a, headers, token, response;
        return __generator(this, function (_b) {
            switch (_b.label) {
                case 0: return [4 /*yield*/, accessTokenFactory()];
                case 1:
                    token = _b.sent();
                    if (token) {
                        headers = (_a = {},
                            _a["Authorization"] = "Bearer " + token,
                            _a);
                    }
                    logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(" + transportName + " transport) sending data. " + getDataDetail(content, logMessageContent) + ".");
                    return [4 /*yield*/, httpClient.post(url, {
                            content: content,
                            headers: headers,
                        })];
                case 2:
                    response = _b.sent();
                    logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(" + transportName + " transport) request complete. Response status: " + response.statusCode + ".");
                    return [2 /*return*/];
            }
        });
    });
}
/** @private */
function createLogger(logger) {
    if (logger === undefined) {
        return new ConsoleLogger(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Information);
    }
    if (logger === null) {
        return _Loggers__WEBPACK_IMPORTED_MODULE_1__.NullLogger.instance;
    }
    if (logger.log) {
        return logger;
    }
    return new ConsoleLogger(logger);
}
/** @private */
var Subject = /** @class */ (function () {
    function Subject(cancelCallback) {
        this.observers = [];
        this.cancelCallback = cancelCallback;
    }
    Subject.prototype.next = function (item) {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            observer.next(item);
        }
    };
    Subject.prototype.error = function (err) {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            if (observer.error) {
                observer.error(err);
            }
        }
    };
    Subject.prototype.complete = function () {
        for (var _i = 0, _a = this.observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            if (observer.complete) {
                observer.complete();
            }
        }
    };
    Subject.prototype.subscribe = function (observer) {
        this.observers.push(observer);
        return new SubjectSubscription(this, observer);
    };
    return Subject;
}());

/** @private */
var SubjectSubscription = /** @class */ (function () {
    function SubjectSubscription(subject, observer) {
        this.subject = subject;
        this.observer = observer;
    }
    SubjectSubscription.prototype.dispose = function () {
        var index = this.subject.observers.indexOf(this.observer);
        if (index > -1) {
            this.subject.observers.splice(index, 1);
        }
        if (this.subject.observers.length === 0) {
            this.subject.cancelCallback().catch(function (_) { });
        }
    };
    return SubjectSubscription;
}());

/** @private */
var ConsoleLogger = /** @class */ (function () {
    function ConsoleLogger(minimumLogLevel) {
        this.minimumLogLevel = minimumLogLevel;
    }
    ConsoleLogger.prototype.log = function (logLevel, message) {
        if (logLevel >= this.minimumLogLevel) {
            switch (logLevel) {
                case _ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Critical:
                case _ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Error:
                    console.error(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel[logLevel] + ": " + message);
                    break;
                case _ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Warning:
                    console.warn(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel[logLevel] + ": " + message);
                    break;
                case _ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Information:
                    console.info(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel[logLevel] + ": " + message);
                    break;
                default:
                    // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
                    console.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel[logLevel] + ": " + message);
                    break;
            }
        }
    };
    return ConsoleLogger;
}());

/** @private */
function isArrayBuffer(val) {
    return val && typeof ArrayBuffer !== "undefined" &&
        (val instanceof ArrayBuffer ||
            // Sometimes we get an ArrayBuffer that doesn't satisfy instanceof
            (val.constructor && val.constructor.name === "ArrayBuffer"));
}
//# sourceMappingURL=Utils.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/WebSocketTransport.js":
/*!*********************************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/WebSocketTransport.js ***!
  \*********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "WebSocketTransport": () => (/* binding */ WebSocketTransport)
/* harmony export */ });
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _Utils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Utils */ "./node_modules/@aspnet/signalr/dist/esm/Utils.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};



/** @private */
var WebSocketTransport = /** @class */ (function () {
    function WebSocketTransport(accessTokenFactory, logger, logMessageContent) {
        this.logger = logger;
        this.accessTokenFactory = accessTokenFactory || (function () { return null; });
        this.logMessageContent = logMessageContent;
    }
    WebSocketTransport.prototype.connect = function (url, transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var token;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isRequired(url, "url");
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isRequired(transferFormat, "transferFormat");
                        _Utils__WEBPACK_IMPORTED_MODULE_2__.Arg.isIn(transferFormat, _ITransport__WEBPACK_IMPORTED_MODULE_1__.TransferFormat, "transferFormat");
                        if (typeof (WebSocket) === "undefined") {
                            throw new Error("'WebSocket' is not supported in your environment.");
                        }
                        this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(WebSockets transport) Connecting");
                        return [4 /*yield*/, this.accessTokenFactory()];
                    case 1:
                        token = _a.sent();
                        if (token) {
                            url += (url.indexOf("?") < 0 ? "?" : "&") + ("access_token=" + encodeURIComponent(token));
                        }
                        return [2 /*return*/, new Promise(function (resolve, reject) {
                                url = url.replace(/^http/, "ws");
                                var webSocket = new WebSocket(url);
                                if (transferFormat === _ITransport__WEBPACK_IMPORTED_MODULE_1__.TransferFormat.Binary) {
                                    webSocket.binaryType = "arraybuffer";
                                }
                                // tslint:disable-next-line:variable-name
                                webSocket.onopen = function (_event) {
                                    _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Information, "WebSocket connected to " + url);
                                    _this.webSocket = webSocket;
                                    resolve();
                                };
                                webSocket.onerror = function (event) {
                                    reject(event.error);
                                };
                                webSocket.onmessage = function (message) {
                                    _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(WebSockets transport) data received. " + (0,_Utils__WEBPACK_IMPORTED_MODULE_2__.getDataDetail)(message.data, _this.logMessageContent) + ".");
                                    if (_this.onreceive) {
                                        _this.onreceive(message.data);
                                    }
                                };
                                webSocket.onclose = function (event) {
                                    // webSocket will be null if the transport did not start successfully
                                    _this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(WebSockets transport) socket closed.");
                                    if (_this.onclose) {
                                        if (event.wasClean === false || event.code !== 1000) {
                                            _this.onclose(new Error("Websocket closed with status code: " + event.code + " (" + event.reason + ")"));
                                        }
                                        else {
                                            _this.onclose();
                                        }
                                    }
                                };
                            })];
                }
            });
        });
    };
    WebSocketTransport.prototype.send = function (data) {
        if (this.webSocket && this.webSocket.readyState === WebSocket.OPEN) {
            this.logger.log(_ILogger__WEBPACK_IMPORTED_MODULE_0__.LogLevel.Trace, "(WebSockets transport) sending data. " + (0,_Utils__WEBPACK_IMPORTED_MODULE_2__.getDataDetail)(data, this.logMessageContent) + ".");
            this.webSocket.send(data);
            return Promise.resolve();
        }
        return Promise.reject("WebSocket is not in the OPEN state");
    };
    WebSocketTransport.prototype.stop = function () {
        if (this.webSocket) {
            this.webSocket.close();
            this.webSocket = null;
        }
        return Promise.resolve();
    };
    return WebSocketTransport;
}());

//# sourceMappingURL=WebSocketTransport.js.map

/***/ }),

/***/ "./node_modules/@aspnet/signalr/dist/esm/index.js":
/*!********************************************************!*\
  !*** ./node_modules/@aspnet/signalr/dist/esm/index.js ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "VERSION": () => (/* binding */ VERSION),
/* harmony export */   "HttpError": () => (/* reexport safe */ _Errors__WEBPACK_IMPORTED_MODULE_0__.HttpError),
/* harmony export */   "TimeoutError": () => (/* reexport safe */ _Errors__WEBPACK_IMPORTED_MODULE_0__.TimeoutError),
/* harmony export */   "DefaultHttpClient": () => (/* reexport safe */ _HttpClient__WEBPACK_IMPORTED_MODULE_1__.DefaultHttpClient),
/* harmony export */   "HttpClient": () => (/* reexport safe */ _HttpClient__WEBPACK_IMPORTED_MODULE_1__.HttpClient),
/* harmony export */   "HttpResponse": () => (/* reexport safe */ _HttpClient__WEBPACK_IMPORTED_MODULE_1__.HttpResponse),
/* harmony export */   "HubConnection": () => (/* reexport safe */ _HubConnection__WEBPACK_IMPORTED_MODULE_2__.HubConnection),
/* harmony export */   "HubConnectionBuilder": () => (/* reexport safe */ _HubConnectionBuilder__WEBPACK_IMPORTED_MODULE_3__.HubConnectionBuilder),
/* harmony export */   "MessageType": () => (/* reexport safe */ _IHubProtocol__WEBPACK_IMPORTED_MODULE_4__.MessageType),
/* harmony export */   "LogLevel": () => (/* reexport safe */ _ILogger__WEBPACK_IMPORTED_MODULE_5__.LogLevel),
/* harmony export */   "HttpTransportType": () => (/* reexport safe */ _ITransport__WEBPACK_IMPORTED_MODULE_6__.HttpTransportType),
/* harmony export */   "TransferFormat": () => (/* reexport safe */ _ITransport__WEBPACK_IMPORTED_MODULE_6__.TransferFormat),
/* harmony export */   "NullLogger": () => (/* reexport safe */ _Loggers__WEBPACK_IMPORTED_MODULE_7__.NullLogger),
/* harmony export */   "JsonHubProtocol": () => (/* reexport safe */ _JsonHubProtocol__WEBPACK_IMPORTED_MODULE_8__.JsonHubProtocol)
/* harmony export */ });
/* harmony import */ var _Errors__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Errors */ "./node_modules/@aspnet/signalr/dist/esm/Errors.js");
/* harmony import */ var _HttpClient__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./HttpClient */ "./node_modules/@aspnet/signalr/dist/esm/HttpClient.js");
/* harmony import */ var _HubConnection__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./HubConnection */ "./node_modules/@aspnet/signalr/dist/esm/HubConnection.js");
/* harmony import */ var _HubConnectionBuilder__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./HubConnectionBuilder */ "./node_modules/@aspnet/signalr/dist/esm/HubConnectionBuilder.js");
/* harmony import */ var _IHubProtocol__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./IHubProtocol */ "./node_modules/@aspnet/signalr/dist/esm/IHubProtocol.js");
/* harmony import */ var _ILogger__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./ILogger */ "./node_modules/@aspnet/signalr/dist/esm/ILogger.js");
/* harmony import */ var _ITransport__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./ITransport */ "./node_modules/@aspnet/signalr/dist/esm/ITransport.js");
/* harmony import */ var _Loggers__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./Loggers */ "./node_modules/@aspnet/signalr/dist/esm/Loggers.js");
/* harmony import */ var _JsonHubProtocol__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./JsonHubProtocol */ "./node_modules/@aspnet/signalr/dist/esm/JsonHubProtocol.js");
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Version token that will be replaced by the prepack command
/** The version of the SignalR client. */
var VERSION = "1.0.27";









//# sourceMappingURL=index.js.map

/***/ }),

/***/ "./node_modules/@babel/runtime/helpers/esm/extends.js":
/*!************************************************************!*\
  !*** ./node_modules/@babel/runtime/helpers/esm/extends.js ***!
  \************************************************************/
/***/ ((__unused_webpack___webpack_module__, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (/* binding */ _extends)
/* harmony export */ });
function _extends() {
  _extends = Object.assign || function (target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i];

      for (var key in source) {
        if (Object.prototype.hasOwnProperty.call(source, key)) {
          target[key] = source[key];
        }
      }
    }

    return target;
  };

  return _extends.apply(this, arguments);
}

/***/ }),

/***/ "./node_modules/@babel/runtime/helpers/esm/inheritsLoose.js":
/*!******************************************************************!*\
  !*** ./node_modules/@babel/runtime/helpers/esm/inheritsLoose.js ***!
  \******************************************************************/
/***/ ((__unused_webpack___webpack_module__, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (/* binding */ _inheritsLoose)
/* harmony export */ });
function _inheritsLoose(subClass, superClass) {
  subClass.prototype = Object.create(superClass.prototype);
  subClass.prototype.constructor = subClass;
  subClass.__proto__ = superClass;
}

/***/ }),

/***/ "./node_modules/@babel/runtime/helpers/esm/objectWithoutPropertiesLoose.js":
/*!*********************************************************************************!*\
  !*** ./node_modules/@babel/runtime/helpers/esm/objectWithoutPropertiesLoose.js ***!
  \*********************************************************************************/
/***/ ((__unused_webpack___webpack_module__, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (/* binding */ _objectWithoutPropertiesLoose)
/* harmony export */ });
function _objectWithoutPropertiesLoose(source, excluded) {
  if (source == null) return {};
  var target = {};
  var sourceKeys = Object.keys(source);
  var key, i;

  for (i = 0; i < sourceKeys.length; i++) {
    key = sourceKeys[i];
    if (excluded.indexOf(key) >= 0) continue;
    target[key] = source[key];
  }

  return target;
}

/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/alerts.css":
/*!****************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/alerts.css ***!
  \****************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".main-alert-section {\r\n    /* border: 2px solid black; */\r\n    /* height: 20px; */\r\n    position: fixed;\r\n    /* width: 100%; */\r\n    bottom: 0px;\r\n}\r\n\r\n.absolute-one-alert {\r\n    max-width: 100%;\r\n}", "",{"version":3,"sources":["webpack://./style/alerts.css"],"names":[],"mappings":"AAAA;IACI,6BAA6B;IAC7B,kBAAkB;IAClB,eAAe;IACf,iBAAiB;IACjB,WAAW;AACf;;AAEA;IACI,eAAe;AACnB","sourcesContent":[".main-alert-section {\r\n    /* border: 2px solid black; */\r\n    /* height: 20px; */\r\n    position: fixed;\r\n    /* width: 100%; */\r\n    bottom: 0px;\r\n}\r\n\r\n.absolute-one-alert {\r\n    max-width: 100%;\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/auth.css":
/*!**************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/auth.css ***!
  \**************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, "\r\ndiv.main-auth-container{\r\n    padding-top: 30px;\r\n    padding-bottom: 30px;\r\n}\r\n\r\ndiv.auth-container-inner{\r\n/* border:2px solid black; */\r\nbox-shadow: 0 0 10px rgba(0,0,0,0.5);\r\n}\r\n\r\na.auth-switcher-link{\r\n    width:100%;\r\n}\r\n\r\ndiv.auth-switcher{\r\n    padding-bottom: 20px;\r\n}\r\n\r\n", "",{"version":3,"sources":["webpack://./style/auth.css"],"names":[],"mappings":";AACA;IACI,iBAAiB;IACjB,oBAAoB;AACxB;;AAEA;AACA,4BAA4B;AAC5B,oCAAoC;AACpC;;AAEA;IACI,UAAU;AACd;;AAEA;IACI,oBAAoB;AACxB","sourcesContent":["\r\ndiv.main-auth-container{\r\n    padding-top: 30px;\r\n    padding-bottom: 30px;\r\n}\r\n\r\ndiv.auth-container-inner{\r\n/* border:2px solid black; */\r\nbox-shadow: 0 0 10px rgba(0,0,0,0.5);\r\n}\r\n\r\na.auth-switcher-link{\r\n    width:100%;\r\n}\r\n\r\ndiv.auth-switcher{\r\n    padding-bottom: 20px;\r\n}\r\n\r\n"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/body.css":
/*!**************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/body.css ***!
  \**************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".body{\r\n    background-color: #edeef0;\r\n}\r\n\r\n\r\n\r\n.main-body {\r\n    /* height: 400px; */\r\n    /* border: 2px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    padding-top: 15px;\r\n}\r\n\r\n", "",{"version":3,"sources":["webpack://./style/body.css"],"names":[],"mappings":"AAAA;IACI,yBAAyB;AAC7B;;;;AAIA;IACI,mBAAmB;IACnB,6BAA6B;IAC7B,mBAAmB;IACnB,iBAAiB;AACrB","sourcesContent":[".body{\r\n    background-color: #edeef0;\r\n}\r\n\r\n\r\n\r\n.main-body {\r\n    /* height: 400px; */\r\n    /* border: 2px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    padding-top: 15px;\r\n}\r\n\r\n"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/footer.css":
/*!****************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/footer.css ***!
  \****************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, "div.main-footer{\r\n    /* height: 400px; */\r\n    border:2px solid black;\r\n    background-color: #343943;\r\n}\r\n\r\ndiv.footer-social-link{\r\n    height: 40px;\r\n    width: 40px;\r\n    border:2px solid black;\r\n}\r\n\r\ndiv.sub-footer-under{\r\n    background-color: #3e4550;\r\n    height: 60px;\r\n}\r\n\r\np.social-link-head{\r\n    color: white;\r\n    font-weight: 500;\r\n}\r\n\r\np.contacts-head{\r\n    color: white;\r\n    font-weight: 500;\r\n}\r\n\r\ndiv.footer-contacts{\r\n    color: white;\r\n}", "",{"version":3,"sources":["webpack://./style/footer.css"],"names":[],"mappings":"AAAA;IACI,mBAAmB;IACnB,sBAAsB;IACtB,yBAAyB;AAC7B;;AAEA;IACI,YAAY;IACZ,WAAW;IACX,sBAAsB;AAC1B;;AAEA;IACI,yBAAyB;IACzB,YAAY;AAChB;;AAEA;IACI,YAAY;IACZ,gBAAgB;AACpB;;AAEA;IACI,YAAY;IACZ,gBAAgB;AACpB;;AAEA;IACI,YAAY;AAChB","sourcesContent":["div.main-footer{\r\n    /* height: 400px; */\r\n    border:2px solid black;\r\n    background-color: #343943;\r\n}\r\n\r\ndiv.footer-social-link{\r\n    height: 40px;\r\n    width: 40px;\r\n    border:2px solid black;\r\n}\r\n\r\ndiv.sub-footer-under{\r\n    background-color: #3e4550;\r\n    height: 60px;\r\n}\r\n\r\np.social-link-head{\r\n    color: white;\r\n    font-weight: 500;\r\n}\r\n\r\np.contacts-head{\r\n    color: white;\r\n    font-weight: 500;\r\n}\r\n\r\ndiv.footer-contacts{\r\n    color: white;\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/header.css":
/*!****************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/header.css ***!
  \****************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, "div.main-header {\r\n    height: 60px;\r\n    /* border: 2px solid black; */\r\n    background-color: #343943;\r\n}\r\ndiv.main-header-inner{\r\n    height: 100%;\r\n}\r\ndiv.main-header-row {\r\n    height: 100%;\r\n}\r\n\r\nimg.main-header-logo-img {\r\n    height: 100%;\r\n    width: 70px;\r\n}\r\n\r\ndiv.main-header-logo {\r\n    height: 100%;\r\n}\r\n\r\ndiv.header-user-block {\r\n    height: 100%;\r\n    /* transition: 1s; */\r\n    /* border-left: 1px solid white; */\r\n    /* cursor: pointer; */\r\n}\r\n\r\ndiv.header-user-dropdown {\r\n    height: 100%;\r\n    /* cursor: pointer; */\r\n    text-align: right;\r\n}\r\n\r\ndiv.header-auth-dropdown {\r\n    height: 100%;\r\n    /* cursor: pointer; */\r\n    font-size: 20px;\r\n    text-align: center;\r\n}\r\n\r\nimg.header-user-img {\r\n    height: 100%;\r\n    border-radius: 50%;\r\n}\r\n\r\nspan.header-user-name-text {\r\n    display: block;\r\n    max-width: 50%;\r\n    overflow: hidden;\r\n    /* padding-right: 10px; */\r\n    margin-right: 10px;\r\n}\r\n\r\nspan.header-user-img {\r\n    height: 100%;\r\n}\r\n\r\ndiv.header-user-block-inner {\r\n    height: 100%;\r\n    cursor: pointer;\r\n}\r\n\r\ndiv.header-user-block-inner:hover {\r\n    background-color: greenyellow;\r\n    /* transition: 1s; */\r\n}", "",{"version":3,"sources":["webpack://./style/header.css"],"names":[],"mappings":"AAAA;IACI,YAAY;IACZ,6BAA6B;IAC7B,yBAAyB;AAC7B;AACA;IACI,YAAY;AAChB;AACA;IACI,YAAY;AAChB;;AAEA;IACI,YAAY;IACZ,WAAW;AACf;;AAEA;IACI,YAAY;AAChB;;AAEA;IACI,YAAY;IACZ,oBAAoB;IACpB,kCAAkC;IAClC,qBAAqB;AACzB;;AAEA;IACI,YAAY;IACZ,qBAAqB;IACrB,iBAAiB;AACrB;;AAEA;IACI,YAAY;IACZ,qBAAqB;IACrB,eAAe;IACf,kBAAkB;AACtB;;AAEA;IACI,YAAY;IACZ,kBAAkB;AACtB;;AAEA;IACI,cAAc;IACd,cAAc;IACd,gBAAgB;IAChB,yBAAyB;IACzB,kBAAkB;AACtB;;AAEA;IACI,YAAY;AAChB;;AAEA;IACI,YAAY;IACZ,eAAe;AACnB;;AAEA;IACI,6BAA6B;IAC7B,oBAAoB;AACxB","sourcesContent":["div.main-header {\r\n    height: 60px;\r\n    /* border: 2px solid black; */\r\n    background-color: #343943;\r\n}\r\ndiv.main-header-inner{\r\n    height: 100%;\r\n}\r\ndiv.main-header-row {\r\n    height: 100%;\r\n}\r\n\r\nimg.main-header-logo-img {\r\n    height: 100%;\r\n    width: 70px;\r\n}\r\n\r\ndiv.main-header-logo {\r\n    height: 100%;\r\n}\r\n\r\ndiv.header-user-block {\r\n    height: 100%;\r\n    /* transition: 1s; */\r\n    /* border-left: 1px solid white; */\r\n    /* cursor: pointer; */\r\n}\r\n\r\ndiv.header-user-dropdown {\r\n    height: 100%;\r\n    /* cursor: pointer; */\r\n    text-align: right;\r\n}\r\n\r\ndiv.header-auth-dropdown {\r\n    height: 100%;\r\n    /* cursor: pointer; */\r\n    font-size: 20px;\r\n    text-align: center;\r\n}\r\n\r\nimg.header-user-img {\r\n    height: 100%;\r\n    border-radius: 50%;\r\n}\r\n\r\nspan.header-user-name-text {\r\n    display: block;\r\n    max-width: 50%;\r\n    overflow: hidden;\r\n    /* padding-right: 10px; */\r\n    margin-right: 10px;\r\n}\r\n\r\nspan.header-user-img {\r\n    height: 100%;\r\n}\r\n\r\ndiv.header-user-block-inner {\r\n    height: 100%;\r\n    cursor: pointer;\r\n}\r\n\r\ndiv.header-user-block-inner:hover {\r\n    background-color: greenyellow;\r\n    /* transition: 1s; */\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/main.css":
/*!**************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/main.css ***!
  \**************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".nopadding {\r\n    padding: 0 !important;\r\n    margin: 0 !important;\r\n }\r\n\r\n\r\n.display_none{\r\n    display: none;\r\n}\r\n\r\n .persent-100-width{\r\n     width:100%;\r\n }\r\n\r\n .persent-100-width-height{\r\n    width:100%;\r\n    height: 100%;\r\n }\r\n\r\n .padding-10-top{\r\n     padding-top: 10px;\r\n }\r\n\r\n\r\n/* \r\n color: red;\r\n    -webkit-mask-image: -webkit-gradient(linear, left top, left bottom, \r\n    from(rgba(0,0,0,1)), to(rgba(0,0,0,0))); */\r\n\r\n\r\n    /* background: linear-gradient(transparent, gray); */\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n ", "",{"version":3,"sources":["webpack://./style/main.css"],"names":[],"mappings":"AAAA;IACI,qBAAqB;IACrB,oBAAoB;CACvB;;;AAGD;IACI,aAAa;AACjB;;CAEC;KACI,UAAU;CACd;;CAEA;IACG,UAAU;IACV,YAAY;CACf;;CAEA;KACI,iBAAiB;CACrB;;;AAGD;;;8CAG8C;;;IAG1C,oDAAoD","sourcesContent":[".nopadding {\r\n    padding: 0 !important;\r\n    margin: 0 !important;\r\n }\r\n\r\n\r\n.display_none{\r\n    display: none;\r\n}\r\n\r\n .persent-100-width{\r\n     width:100%;\r\n }\r\n\r\n .persent-100-width-height{\r\n    width:100%;\r\n    height: 100%;\r\n }\r\n\r\n .padding-10-top{\r\n     padding-top: 10px;\r\n }\r\n\r\n\r\n/* \r\n color: red;\r\n    -webkit-mask-image: -webkit-gradient(linear, left top, left bottom, \r\n    from(rgba(0,0,0,1)), to(rgba(0,0,0,0))); */\r\n\r\n\r\n    /* background: linear-gradient(transparent, gray); */\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n "],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/menu.css":
/*!**************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/menu.css ***!
  \**************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, "\r\n.app-one-item{\r\n    /* width:20px;\r\n    height:20px; */\r\n    \r\n    text-align: center;\r\n    padding: 20px;\r\n}\r\n\r\n.app-one-item-inner{\r\n    border: 1px solid rgb(0, 0, 0);\r\n    max-height: 300px;\r\n    max-width: 300px;\r\n}", "",{"version":3,"sources":["webpack://./style/menu.css"],"names":[],"mappings":";AACA;IACI;kBACc;;IAEd,kBAAkB;IAClB,aAAa;AACjB;;AAEA;IACI,8BAA8B;IAC9B,iBAAiB;IACjB,gBAAgB;AACpB","sourcesContent":["\r\n.app-one-item{\r\n    /* width:20px;\r\n    height:20px; */\r\n    \r\n    text-align: center;\r\n    padding: 20px;\r\n}\r\n\r\n.app-one-item-inner{\r\n    border: 1px solid rgb(0, 0, 0);\r\n    max-height: 300px;\r\n    max-width: 300px;\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/menu_app.css":
/*!******************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/menu_app.css ***!
  \******************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".card-list-body{\r\n    background-color: white;\r\n}\r\n\r\n\r\n\r\ndiv.one-menu-card-inner {\r\n    width: 100%;\r\n    height: 100%;\r\n    transition: 0.3s;\r\n}\r\n\r\ndiv.one-menu-card-inner:hover {\r\n    box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);\r\n    transition: 0.3s;\r\n}\r\n\r\ndiv.one-card-button {\r\n    width: 30px;\r\n    height: 30px;\r\n    /* border: 2px solid black; */\r\n    position: absolute;\r\n    z-index: 5;\r\n    cursor: pointer;\r\n}\r\n\r\ndiv.edit-one-card-button {\r\n    left: 100%;\r\n    margin-left: -30px;\r\n   \r\n}\r\n\r\ndiv.follow-one-card-button {\r\n    left: 100%;\r\n    margin-left: -60px;\r\n}\r\n\r\ndiv.cancel-edit-one-card-button{\r\n    left: 100%;\r\n    margin-left: -30px;\r\n}\r\ndiv.save-one-card-button{\r\n    left: 100%;\r\n    margin-left: -60px;\r\n}\r\n\r\ndiv.card-list-preloader{\r\n    height: 400px;\r\n    width: 400px;\r\n    /* TODO  при маленьком экране фул криво */\r\n}\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n", "",{"version":3,"sources":["webpack://./style/menu_app.css"],"names":[],"mappings":"AAAA;IACI,uBAAuB;AAC3B;;;;AAIA;IACI,WAAW;IACX,YAAY;IACZ,gBAAgB;AACpB;;AAEA;IACI,uCAAuC;IACvC,gBAAgB;AACpB;;AAEA;IACI,WAAW;IACX,YAAY;IACZ,6BAA6B;IAC7B,kBAAkB;IAClB,UAAU;IACV,eAAe;AACnB;;AAEA;IACI,UAAU;IACV,kBAAkB;;AAEtB;;AAEA;IACI,UAAU;IACV,kBAAkB;AACtB;;AAEA;IACI,UAAU;IACV,kBAAkB;AACtB;AACA;IACI,UAAU;IACV,kBAAkB;AACtB;;AAEA;IACI,aAAa;IACb,YAAY;IACZ,yCAAyC;AAC7C","sourcesContent":[".card-list-body{\r\n    background-color: white;\r\n}\r\n\r\n\r\n\r\ndiv.one-menu-card-inner {\r\n    width: 100%;\r\n    height: 100%;\r\n    transition: 0.3s;\r\n}\r\n\r\ndiv.one-menu-card-inner:hover {\r\n    box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);\r\n    transition: 0.3s;\r\n}\r\n\r\ndiv.one-card-button {\r\n    width: 30px;\r\n    height: 30px;\r\n    /* border: 2px solid black; */\r\n    position: absolute;\r\n    z-index: 5;\r\n    cursor: pointer;\r\n}\r\n\r\ndiv.edit-one-card-button {\r\n    left: 100%;\r\n    margin-left: -30px;\r\n   \r\n}\r\n\r\ndiv.follow-one-card-button {\r\n    left: 100%;\r\n    margin-left: -60px;\r\n}\r\n\r\ndiv.cancel-edit-one-card-button{\r\n    left: 100%;\r\n    margin-left: -30px;\r\n}\r\ndiv.save-one-card-button{\r\n    left: 100%;\r\n    margin-left: -60px;\r\n}\r\n\r\ndiv.card-list-preloader{\r\n    height: 400px;\r\n    width: 400px;\r\n    /* TODO  при маленьком экране фул криво */\r\n}\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/menu_app_one_card.css":
/*!***************************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/menu_app_one_card.css ***!
  \***************************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, "\r\n.one-card-header{\r\n    /* border:2px solid black; */\r\n    /* box-shadow: 0 0 5px; */\r\n}\r\n\r\n.one-card-header-info{\r\n    /* border:2px solid black; */\r\n    padding-bottom: 5px;\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-header-image{\r\n    /* border:2px solid black; */\r\n    text-align: center;\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-body-info{\r\n    /* border:2px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-page-follow-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-edit-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-save-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-cancel-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n\r\ndiv.datail-one-card-button {\r\n    width: 30px;\r\n    height: 30px;\r\n    /* border: 2px solid black; */\r\n    /* position: absolute; */\r\n    z-index: 5;\r\n    cursor: pointer;\r\n}\r\n", "",{"version":3,"sources":["webpack://./style/menu_app_one_card.css"],"names":[],"mappings":";AACA;IACI,4BAA4B;IAC5B,yBAAyB;AAC7B;;AAEA;IACI,4BAA4B;IAC5B,mBAAmB;IACnB,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI,4BAA4B;IAC5B,kBAAkB;IAClB,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI,4BAA4B;IAC5B,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI;mBACe;AACnB;;AAEA;IACI;mBACe;AACnB;;AAEA;IACI;mBACe;AACnB;;AAEA;IACI;mBACe;AACnB;;;AAGA;IACI,WAAW;IACX,YAAY;IACZ,6BAA6B;IAC7B,wBAAwB;IACxB,UAAU;IACV,eAAe;AACnB","sourcesContent":["\r\n.one-card-header{\r\n    /* border:2px solid black; */\r\n    /* box-shadow: 0 0 5px; */\r\n}\r\n\r\n.one-card-header-info{\r\n    /* border:2px solid black; */\r\n    padding-bottom: 5px;\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-header-image{\r\n    /* border:2px solid black; */\r\n    text-align: center;\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-body-info{\r\n    /* border:2px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-card-page-follow-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-edit-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-save-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n.one-card-page-cancel-button{\r\n    /* width: 20px;\r\n    height: 20px; */\r\n}\r\n\r\n\r\ndiv.datail-one-card-button {\r\n    width: 30px;\r\n    height: 30px;\r\n    /* border: 2px solid black; */\r\n    /* position: absolute; */\r\n    z-index: 5;\r\n    cursor: pointer;\r\n}\r\n"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/planing_poker.css":
/*!***********************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/planing_poker.css ***!
  \***********************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".planit-room-left-part {\r\n    /* border: 2px solid black; */\r\n    background: linear-gradient(to right, transparent, #745959);\r\n    /* #2a4061); */\r\n    min-height: 500px;\r\n}\r\n\r\n.planit-room-right-part {\r\n    /* border: 2px solid black; */\r\n    background: linear-gradient(to left, transparent, gray);\r\n    min-height: 500px;\r\n}\r\n\r\n.one-planing-vote-card {\r\n    width: 100px;\r\n    height: 100px;\r\n    border: 2px solid rgb(65 152 174);\r\n    font-size: 60px;\r\n    display: flex;\r\n    justify-content: center;\r\n    align-items: center;\r\n    border-radius: 5px;\r\n}\r\n\r\n.one-planing-vote-card:hover {\r\n    cursor: pointer;\r\n}\r\n\r\n.one-planing-selected-vote-card {\r\n    background-color: pink;\r\n}\r\n\r\n.planing-cards-container {\r\n    display: flex;\r\n    flex-wrap: wrap;\r\n    justify-content: space-around;\r\n    align-content: space-between;\r\n    gap: 12px;\r\n    padding-top: 10px;\r\n    /* calc() */\r\n}\r\n\r\n.planing-cards-container>div {\r\n    margin: 6px;\r\n}\r\n\r\n.planing-user-voted {\r\n    background-color: lawngreen;\r\n}\r\n\r\n.planing-user-not-voted {\r\n    background-color: lavenderblush;\r\n}\r\n\r\n.planing-user-voted-min {\r\n    background-color: darksalmon;\r\n}\r\n\r\n.planing-user-voted-max {\r\n    background-color: peru;\r\n}\r\n\r\n.planing-user {\r\n    box-shadow: 0 0 10px rgb(0 0 0 / 50%);\r\n    border-radius: 5px;\r\n    padding: 10px;\r\n    /* background-color: white; */\r\n}\r\n\r\n.planning-vote-settings {\r\n    border-bottom: 1px solid #db5353;\r\n}\r\n\r\n.planing-enter-main {\r\n    padding-top: 30px;\r\n    padding-bottom: 30px;\r\n}\r\n\r\n.planing-enter-inner {\r\n    box-shadow: 0 0 10px rgb(0 0 0 / 50%);\r\n}\r\n\r\n.planing-current-story-main {\r\n    /* border: 2px solid black; */\r\n}\r\n\r\n.planing-stories-list-main {\r\n    /* border: 2px solid black; */\r\n}\r\n\r\n.planing-poker-left-one-section {\r\n    background-color: white;\r\n    border-radius: 20px;\r\n    padding-left: 10px;\r\n}\r\n\r\n.stories-data-list {\r\n    max-height: 500px;\r\n    overflow: auto;\r\n}\r\n\r\n.planing-story-in-list {}\r\n\r\n.not-completed-story {\r\n    background-color: #e9f3a8;\r\n}\r\n\r\n.completed-story {\r\n    background-color: #b2f593;\r\n}\r\n\r\n.planing-room-not-auth {\r\n    border-radius: 50%;\r\n    width: 50px;\r\n    height: 50px;\r\n    border: 1px solid #07135c;\r\n    /* display: block; */\r\n    text-align: center;\r\n    color: white;\r\n    background-color: #e53535;\r\n    font-size: 180%;\r\n}", "",{"version":3,"sources":["webpack://./style/planing_poker.css"],"names":[],"mappings":"AAAA;IACI,6BAA6B;IAC7B,2DAA2D;IAC3D,cAAc;IACd,iBAAiB;AACrB;;AAEA;IACI,6BAA6B;IAC7B,uDAAuD;IACvD,iBAAiB;AACrB;;AAEA;IACI,YAAY;IACZ,aAAa;IACb,iCAAiC;IACjC,eAAe;IACf,aAAa;IACb,uBAAuB;IACvB,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI,eAAe;AACnB;;AAEA;IACI,sBAAsB;AAC1B;;AAEA;IACI,aAAa;IACb,eAAe;IACf,6BAA6B;IAC7B,4BAA4B;IAC5B,SAAS;IACT,iBAAiB;IACjB,WAAW;AACf;;AAEA;IACI,WAAW;AACf;;AAEA;IACI,2BAA2B;AAC/B;;AAEA;IACI,+BAA+B;AACnC;;AAEA;IACI,4BAA4B;AAChC;;AAEA;IACI,sBAAsB;AAC1B;;AAEA;IACI,qCAAqC;IACrC,kBAAkB;IAClB,aAAa;IACb,6BAA6B;AACjC;;AAEA;IACI,gCAAgC;AACpC;;AAEA;IACI,iBAAiB;IACjB,oBAAoB;AACxB;;AAEA;IACI,qCAAqC;AACzC;;AAEA;IACI,6BAA6B;AACjC;;AAEA;IACI,6BAA6B;AACjC;;AAEA;IACI,uBAAuB;IACvB,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI,iBAAiB;IACjB,cAAc;AAClB;;AAEA,wBAAwB;;AAExB;IACI,yBAAyB;AAC7B;;AAEA;IACI,yBAAyB;AAC7B;;AAEA;IACI,kBAAkB;IAClB,WAAW;IACX,YAAY;IACZ,yBAAyB;IACzB,oBAAoB;IACpB,kBAAkB;IAClB,YAAY;IACZ,yBAAyB;IACzB,eAAe;AACnB","sourcesContent":[".planit-room-left-part {\r\n    /* border: 2px solid black; */\r\n    background: linear-gradient(to right, transparent, #745959);\r\n    /* #2a4061); */\r\n    min-height: 500px;\r\n}\r\n\r\n.planit-room-right-part {\r\n    /* border: 2px solid black; */\r\n    background: linear-gradient(to left, transparent, gray);\r\n    min-height: 500px;\r\n}\r\n\r\n.one-planing-vote-card {\r\n    width: 100px;\r\n    height: 100px;\r\n    border: 2px solid rgb(65 152 174);\r\n    font-size: 60px;\r\n    display: flex;\r\n    justify-content: center;\r\n    align-items: center;\r\n    border-radius: 5px;\r\n}\r\n\r\n.one-planing-vote-card:hover {\r\n    cursor: pointer;\r\n}\r\n\r\n.one-planing-selected-vote-card {\r\n    background-color: pink;\r\n}\r\n\r\n.planing-cards-container {\r\n    display: flex;\r\n    flex-wrap: wrap;\r\n    justify-content: space-around;\r\n    align-content: space-between;\r\n    gap: 12px;\r\n    padding-top: 10px;\r\n    /* calc() */\r\n}\r\n\r\n.planing-cards-container>div {\r\n    margin: 6px;\r\n}\r\n\r\n.planing-user-voted {\r\n    background-color: lawngreen;\r\n}\r\n\r\n.planing-user-not-voted {\r\n    background-color: lavenderblush;\r\n}\r\n\r\n.planing-user-voted-min {\r\n    background-color: darksalmon;\r\n}\r\n\r\n.planing-user-voted-max {\r\n    background-color: peru;\r\n}\r\n\r\n.planing-user {\r\n    box-shadow: 0 0 10px rgb(0 0 0 / 50%);\r\n    border-radius: 5px;\r\n    padding: 10px;\r\n    /* background-color: white; */\r\n}\r\n\r\n.planning-vote-settings {\r\n    border-bottom: 1px solid #db5353;\r\n}\r\n\r\n.planing-enter-main {\r\n    padding-top: 30px;\r\n    padding-bottom: 30px;\r\n}\r\n\r\n.planing-enter-inner {\r\n    box-shadow: 0 0 10px rgb(0 0 0 / 50%);\r\n}\r\n\r\n.planing-current-story-main {\r\n    /* border: 2px solid black; */\r\n}\r\n\r\n.planing-stories-list-main {\r\n    /* border: 2px solid black; */\r\n}\r\n\r\n.planing-poker-left-one-section {\r\n    background-color: white;\r\n    border-radius: 20px;\r\n    padding-left: 10px;\r\n}\r\n\r\n.stories-data-list {\r\n    max-height: 500px;\r\n    overflow: auto;\r\n}\r\n\r\n.planing-story-in-list {}\r\n\r\n.not-completed-story {\r\n    background-color: #e9f3a8;\r\n}\r\n\r\n.completed-story {\r\n    background-color: #b2f593;\r\n}\r\n\r\n.planing-room-not-auth {\r\n    border-radius: 50%;\r\n    width: 50px;\r\n    height: 50px;\r\n    border: 1px solid #07135c;\r\n    /* display: block; */\r\n    text-align: center;\r\n    color: white;\r\n    background-color: #e53535;\r\n    font-size: 180%;\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/cjs.js!./style/word_cards.css":
/*!********************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./style/word_cards.css ***!
  \********************************************************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/cssWithMappingToString.js */ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ "./node_modules/css-loader/dist/runtime/api.js");
/* harmony import */ var _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1__);
// Imports


var ___CSS_LOADER_EXPORT___ = _node_modules_css_loader_dist_runtime_api_js__WEBPACK_IMPORTED_MODULE_1___default()((_node_modules_css_loader_dist_runtime_cssWithMappingToString_js__WEBPACK_IMPORTED_MODULE_0___default()));
// Module
___CSS_LOADER_EXPORT___.push([module.id, ".word-card-card-main {\r\n    /* border: 1px solid black;\r\n    min-height: 150px; */\r\n}\r\n\r\n.word-card-card-inner {\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n    min-height: 150px;\r\n}\r\n\r\n.word-card-cards-list-main {}\r\n\r\n.word-card-cards-list-inner {\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n    height: 300px;\r\n    overflow-y: scroll;\r\n    padding: 5px;\r\n}\r\n\r\n.word-in-list {\r\n    cursor: pointer;\r\n    border-bottom: 1px solid black;\r\n    min-height: 24px;\r\n    overflow: hidden;\r\n}\r\n\r\n.words-cards-list-actions {\r\n    min-height: 150px;\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.word-in-list-selected {\r\n    /* border: 1px solid black; */\r\n    background-color: #9bbfb2;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-word-card-image {\r\n    width: 200px;\r\n    height: 200px;\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.one-word-card-image-line {\r\n    text-align: center;\r\n}\r\n\r\n.word-card-hidden-status {\r\n    font-size: 10pt;\r\n}\r\n\r\n.force-add-one-card {}\r\n\r\n.force-add-one-card-inner{\r\n    box-shadow: 0 0 5px;\r\n}\r\n\r\n.force-add-cards-list {\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.work-words-lists-main{\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.work-words-one-list{\r\n\r\n}\r\n\r\n.work-words-one-list-inner{\r\n    box-shadow: 0 0 5px;\r\n}", "",{"version":3,"sources":["webpack://./style/word_cards.css"],"names":[],"mappings":"AAAA;IACI;wBACoB;AACxB;;AAEA;IACI,6BAA6B;IAC7B,mBAAmB;IACnB,kBAAkB;IAClB,iBAAiB;AACrB;;AAEA,4BAA4B;;AAE5B;IACI,6BAA6B;IAC7B,mBAAmB;IACnB,kBAAkB;IAClB,aAAa;IACb,kBAAkB;IAClB,YAAY;AAChB;;AAEA;IACI,eAAe;IACf,8BAA8B;IAC9B,gBAAgB;IAChB,gBAAgB;AACpB;;AAEA;IACI,iBAAiB;IACjB,6BAA6B;IAC7B,mBAAmB;IACnB,kBAAkB;AACtB;;AAEA;IACI,6BAA6B;IAC7B,yBAAyB;IACzB,kBAAkB;AACtB;;AAEA;IACI,YAAY;IACZ,aAAa;IACb,iBAAiB;IACjB,kBAAkB;AACtB;;AAEA;IACI,kBAAkB;AACtB;;AAEA;IACI,eAAe;AACnB;;AAEA,qBAAqB;;AAErB;IACI,mBAAmB;AACvB;;AAEA;IACI,iBAAiB;IACjB,kBAAkB;AACtB;;AAEA;IACI,iBAAiB;IACjB,kBAAkB;AACtB;;AAEA;;AAEA;;AAEA;IACI,mBAAmB;AACvB","sourcesContent":[".word-card-card-main {\r\n    /* border: 1px solid black;\r\n    min-height: 150px; */\r\n}\r\n\r\n.word-card-card-inner {\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n    min-height: 150px;\r\n}\r\n\r\n.word-card-cards-list-main {}\r\n\r\n.word-card-cards-list-inner {\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n    height: 300px;\r\n    overflow-y: scroll;\r\n    padding: 5px;\r\n}\r\n\r\n.word-in-list {\r\n    cursor: pointer;\r\n    border-bottom: 1px solid black;\r\n    min-height: 24px;\r\n    overflow: hidden;\r\n}\r\n\r\n.words-cards-list-actions {\r\n    min-height: 150px;\r\n    /* border: 1px solid black; */\r\n    box-shadow: 0 0 5px;\r\n    border-radius: 3px;\r\n}\r\n\r\n.word-in-list-selected {\r\n    /* border: 1px solid black; */\r\n    background-color: #9bbfb2;\r\n    border-radius: 3px;\r\n}\r\n\r\n.one-word-card-image {\r\n    width: 200px;\r\n    height: 200px;\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.one-word-card-image-line {\r\n    text-align: center;\r\n}\r\n\r\n.word-card-hidden-status {\r\n    font-size: 10pt;\r\n}\r\n\r\n.force-add-one-card {}\r\n\r\n.force-add-one-card-inner{\r\n    box-shadow: 0 0 5px;\r\n}\r\n\r\n.force-add-cards-list {\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.work-words-lists-main{\r\n    margin-left: auto;\r\n    margin-right: auto;\r\n}\r\n\r\n.work-words-one-list{\r\n\r\n}\r\n\r\n.work-words-one-list-inner{\r\n    box-shadow: 0 0 5px;\r\n}"],"sourceRoot":""}]);
// Exports
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (___CSS_LOADER_EXPORT___);


/***/ }),

/***/ "./node_modules/css-loader/dist/runtime/api.js":
/*!*****************************************************!*\
  !*** ./node_modules/css-loader/dist/runtime/api.js ***!
  \*****************************************************/
/***/ ((module) => {

"use strict";


/*
  MIT License http://www.opensource.org/licenses/mit-license.php
  Author Tobias Koppers @sokra
*/
// css base code, injected by the css-loader
// eslint-disable-next-line func-names
module.exports = function (cssWithMappingToString) {
  var list = []; // return the list of modules as css string

  list.toString = function toString() {
    return this.map(function (item) {
      var content = cssWithMappingToString(item);

      if (item[2]) {
        return "@media ".concat(item[2], " {").concat(content, "}");
      }

      return content;
    }).join('');
  }; // import a list of modules into the list
  // eslint-disable-next-line func-names


  list.i = function (modules, mediaQuery, dedupe) {
    if (typeof modules === 'string') {
      // eslint-disable-next-line no-param-reassign
      modules = [[null, modules, '']];
    }

    var alreadyImportedModules = {};

    if (dedupe) {
      for (var i = 0; i < this.length; i++) {
        // eslint-disable-next-line prefer-destructuring
        var id = this[i][0];

        if (id != null) {
          alreadyImportedModules[id] = true;
        }
      }
    }

    for (var _i = 0; _i < modules.length; _i++) {
      var item = [].concat(modules[_i]);

      if (dedupe && alreadyImportedModules[item[0]]) {
        // eslint-disable-next-line no-continue
        continue;
      }

      if (mediaQuery) {
        if (!item[2]) {
          item[2] = mediaQuery;
        } else {
          item[2] = "".concat(mediaQuery, " and ").concat(item[2]);
        }
      }

      list.push(item);
    }
  };

  return list;
};

/***/ }),

/***/ "./node_modules/css-loader/dist/runtime/cssWithMappingToString.js":
/*!************************************************************************!*\
  !*** ./node_modules/css-loader/dist/runtime/cssWithMappingToString.js ***!
  \************************************************************************/
/***/ ((module) => {

"use strict";


function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _iterableToArrayLimit(arr, i) { if (typeof Symbol === "undefined" || !(Symbol.iterator in Object(arr))) return; var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

module.exports = function cssWithMappingToString(item) {
  var _item = _slicedToArray(item, 4),
      content = _item[1],
      cssMapping = _item[3];

  if (typeof btoa === 'function') {
    // eslint-disable-next-line no-undef
    var base64 = btoa(unescape(encodeURIComponent(JSON.stringify(cssMapping))));
    var data = "sourceMappingURL=data:application/json;charset=utf-8;base64,".concat(base64);
    var sourceMapping = "/*# ".concat(data, " */");
    var sourceURLs = cssMapping.sources.map(function (source) {
      return "/*# sourceURL=".concat(cssMapping.sourceRoot || '').concat(source, " */");
    });
    return [content].concat(sourceURLs).concat([sourceMapping]).join('\n');
  }

  return [content].join('\n');
};

/***/ }),

/***/ "./node_modules/history/esm/history.js":
/*!*********************************************!*\
  !*** ./node_modules/history/esm/history.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "createBrowserHistory": () => (/* binding */ createBrowserHistory),
/* harmony export */   "createHashHistory": () => (/* binding */ createHashHistory),
/* harmony export */   "createMemoryHistory": () => (/* binding */ createMemoryHistory),
/* harmony export */   "createLocation": () => (/* binding */ createLocation),
/* harmony export */   "locationsAreEqual": () => (/* binding */ locationsAreEqual),
/* harmony export */   "parsePath": () => (/* binding */ parsePath),
/* harmony export */   "createPath": () => (/* binding */ createPath)
/* harmony export */ });
/* harmony import */ var _babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @babel/runtime/helpers/esm/extends */ "./node_modules/@babel/runtime/helpers/esm/extends.js");
/* harmony import */ var resolve_pathname__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! resolve-pathname */ "./node_modules/resolve-pathname/esm/resolve-pathname.js");
/* harmony import */ var value_equal__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! value-equal */ "./node_modules/value-equal/esm/value-equal.js");
/* harmony import */ var tiny_warning__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! tiny-warning */ "./node_modules/tiny-warning/dist/tiny-warning.esm.js");
/* harmony import */ var tiny_invariant__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! tiny-invariant */ "./node_modules/tiny-invariant/dist/tiny-invariant.esm.js");






function addLeadingSlash(path) {
  return path.charAt(0) === '/' ? path : '/' + path;
}
function stripLeadingSlash(path) {
  return path.charAt(0) === '/' ? path.substr(1) : path;
}
function hasBasename(path, prefix) {
  return path.toLowerCase().indexOf(prefix.toLowerCase()) === 0 && '/?#'.indexOf(path.charAt(prefix.length)) !== -1;
}
function stripBasename(path, prefix) {
  return hasBasename(path, prefix) ? path.substr(prefix.length) : path;
}
function stripTrailingSlash(path) {
  return path.charAt(path.length - 1) === '/' ? path.slice(0, -1) : path;
}
function parsePath(path) {
  var pathname = path || '/';
  var search = '';
  var hash = '';
  var hashIndex = pathname.indexOf('#');

  if (hashIndex !== -1) {
    hash = pathname.substr(hashIndex);
    pathname = pathname.substr(0, hashIndex);
  }

  var searchIndex = pathname.indexOf('?');

  if (searchIndex !== -1) {
    search = pathname.substr(searchIndex);
    pathname = pathname.substr(0, searchIndex);
  }

  return {
    pathname: pathname,
    search: search === '?' ? '' : search,
    hash: hash === '#' ? '' : hash
  };
}
function createPath(location) {
  var pathname = location.pathname,
      search = location.search,
      hash = location.hash;
  var path = pathname || '/';
  if (search && search !== '?') path += search.charAt(0) === '?' ? search : "?" + search;
  if (hash && hash !== '#') path += hash.charAt(0) === '#' ? hash : "#" + hash;
  return path;
}

function createLocation(path, state, key, currentLocation) {
  var location;

  if (typeof path === 'string') {
    // Two-arg form: push(path, state)
    location = parsePath(path);
    location.state = state;
  } else {
    // One-arg form: push(location)
    location = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_0__.default)({}, path);
    if (location.pathname === undefined) location.pathname = '';

    if (location.search) {
      if (location.search.charAt(0) !== '?') location.search = '?' + location.search;
    } else {
      location.search = '';
    }

    if (location.hash) {
      if (location.hash.charAt(0) !== '#') location.hash = '#' + location.hash;
    } else {
      location.hash = '';
    }

    if (state !== undefined && location.state === undefined) location.state = state;
  }

  try {
    location.pathname = decodeURI(location.pathname);
  } catch (e) {
    if (e instanceof URIError) {
      throw new URIError('Pathname "' + location.pathname + '" could not be decoded. ' + 'This is likely caused by an invalid percent-encoding.');
    } else {
      throw e;
    }
  }

  if (key) location.key = key;

  if (currentLocation) {
    // Resolve incomplete/relative pathname relative to current location.
    if (!location.pathname) {
      location.pathname = currentLocation.pathname;
    } else if (location.pathname.charAt(0) !== '/') {
      location.pathname = (0,resolve_pathname__WEBPACK_IMPORTED_MODULE_1__.default)(location.pathname, currentLocation.pathname);
    }
  } else {
    // When there is no prior location and pathname is empty, set it to /
    if (!location.pathname) {
      location.pathname = '/';
    }
  }

  return location;
}
function locationsAreEqual(a, b) {
  return a.pathname === b.pathname && a.search === b.search && a.hash === b.hash && a.key === b.key && (0,value_equal__WEBPACK_IMPORTED_MODULE_2__.default)(a.state, b.state);
}

function createTransitionManager() {
  var prompt = null;

  function setPrompt(nextPrompt) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(prompt == null, 'A history supports only one prompt at a time') : 0;
    prompt = nextPrompt;
    return function () {
      if (prompt === nextPrompt) prompt = null;
    };
  }

  function confirmTransitionTo(location, action, getUserConfirmation, callback) {
    // TODO: If another transition starts while we're still confirming
    // the previous one, we may end up in a weird state. Figure out the
    // best way to handle this.
    if (prompt != null) {
      var result = typeof prompt === 'function' ? prompt(location, action) : prompt;

      if (typeof result === 'string') {
        if (typeof getUserConfirmation === 'function') {
          getUserConfirmation(result, callback);
        } else {
           true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(false, 'A history needs a getUserConfirmation function in order to use a prompt message') : 0;
          callback(true);
        }
      } else {
        // Return false from a transition hook to cancel the transition.
        callback(result !== false);
      }
    } else {
      callback(true);
    }
  }

  var listeners = [];

  function appendListener(fn) {
    var isActive = true;

    function listener() {
      if (isActive) fn.apply(void 0, arguments);
    }

    listeners.push(listener);
    return function () {
      isActive = false;
      listeners = listeners.filter(function (item) {
        return item !== listener;
      });
    };
  }

  function notifyListeners() {
    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    listeners.forEach(function (listener) {
      return listener.apply(void 0, args);
    });
  }

  return {
    setPrompt: setPrompt,
    confirmTransitionTo: confirmTransitionTo,
    appendListener: appendListener,
    notifyListeners: notifyListeners
  };
}

var canUseDOM = !!(typeof window !== 'undefined' && window.document && window.document.createElement);
function getConfirmation(message, callback) {
  callback(window.confirm(message)); // eslint-disable-line no-alert
}
/**
 * Returns true if the HTML5 history API is supported. Taken from Modernizr.
 *
 * https://github.com/Modernizr/Modernizr/blob/master/LICENSE
 * https://github.com/Modernizr/Modernizr/blob/master/feature-detects/history.js
 * changed to avoid false negatives for Windows Phones: https://github.com/reactjs/react-router/issues/586
 */

function supportsHistory() {
  var ua = window.navigator.userAgent;
  if ((ua.indexOf('Android 2.') !== -1 || ua.indexOf('Android 4.0') !== -1) && ua.indexOf('Mobile Safari') !== -1 && ua.indexOf('Chrome') === -1 && ua.indexOf('Windows Phone') === -1) return false;
  return window.history && 'pushState' in window.history;
}
/**
 * Returns true if browser fires popstate on hash change.
 * IE10 and IE11 do not.
 */

function supportsPopStateOnHashChange() {
  return window.navigator.userAgent.indexOf('Trident') === -1;
}
/**
 * Returns false if using go(n) with hash history causes a full page reload.
 */

function supportsGoWithoutReloadUsingHash() {
  return window.navigator.userAgent.indexOf('Firefox') === -1;
}
/**
 * Returns true if a given popstate event is an extraneous WebKit event.
 * Accounts for the fact that Chrome on iOS fires real popstate events
 * containing undefined state when pressing the back button.
 */

function isExtraneousPopstateEvent(event) {
  return event.state === undefined && navigator.userAgent.indexOf('CriOS') === -1;
}

var PopStateEvent = 'popstate';
var HashChangeEvent = 'hashchange';

function getHistoryState() {
  try {
    return window.history.state || {};
  } catch (e) {
    // IE 11 sometimes throws when accessing window.history.state
    // See https://github.com/ReactTraining/history/pull/289
    return {};
  }
}
/**
 * Creates a history object that uses the HTML5 history API including
 * pushState, replaceState, and the popstate event.
 */


function createBrowserHistory(props) {
  if (props === void 0) {
    props = {};
  }

  !canUseDOM ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_4__.default)(false, 'Browser history needs a DOM') : 0 : void 0;
  var globalHistory = window.history;
  var canUseHistory = supportsHistory();
  var needsHashChangeListener = !supportsPopStateOnHashChange();
  var _props = props,
      _props$forceRefresh = _props.forceRefresh,
      forceRefresh = _props$forceRefresh === void 0 ? false : _props$forceRefresh,
      _props$getUserConfirm = _props.getUserConfirmation,
      getUserConfirmation = _props$getUserConfirm === void 0 ? getConfirmation : _props$getUserConfirm,
      _props$keyLength = _props.keyLength,
      keyLength = _props$keyLength === void 0 ? 6 : _props$keyLength;
  var basename = props.basename ? stripTrailingSlash(addLeadingSlash(props.basename)) : '';

  function getDOMLocation(historyState) {
    var _ref = historyState || {},
        key = _ref.key,
        state = _ref.state;

    var _window$location = window.location,
        pathname = _window$location.pathname,
        search = _window$location.search,
        hash = _window$location.hash;
    var path = pathname + search + hash;
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!basename || hasBasename(path, basename), 'You are attempting to use a basename on a page whose URL path does not begin ' + 'with the basename. Expected path "' + path + '" to begin with "' + basename + '".') : 0;
    if (basename) path = stripBasename(path, basename);
    return createLocation(path, state, key);
  }

  function createKey() {
    return Math.random().toString(36).substr(2, keyLength);
  }

  var transitionManager = createTransitionManager();

  function setState(nextState) {
    (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_0__.default)(history, nextState);

    history.length = globalHistory.length;
    transitionManager.notifyListeners(history.location, history.action);
  }

  function handlePopState(event) {
    // Ignore extraneous popstate events in WebKit.
    if (isExtraneousPopstateEvent(event)) return;
    handlePop(getDOMLocation(event.state));
  }

  function handleHashChange() {
    handlePop(getDOMLocation(getHistoryState()));
  }

  var forceNextPop = false;

  function handlePop(location) {
    if (forceNextPop) {
      forceNextPop = false;
      setState();
    } else {
      var action = 'POP';
      transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
        if (ok) {
          setState({
            action: action,
            location: location
          });
        } else {
          revertPop(location);
        }
      });
    }
  }

  function revertPop(fromLocation) {
    var toLocation = history.location; // TODO: We could probably make this more reliable by
    // keeping a list of keys we've seen in sessionStorage.
    // Instead, we just default to 0 for keys we don't know.

    var toIndex = allKeys.indexOf(toLocation.key);
    if (toIndex === -1) toIndex = 0;
    var fromIndex = allKeys.indexOf(fromLocation.key);
    if (fromIndex === -1) fromIndex = 0;
    var delta = toIndex - fromIndex;

    if (delta) {
      forceNextPop = true;
      go(delta);
    }
  }

  var initialLocation = getDOMLocation(getHistoryState());
  var allKeys = [initialLocation.key]; // Public interface

  function createHref(location) {
    return basename + createPath(location);
  }

  function push(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!(typeof path === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to push when the 1st ' + 'argument is a location-like object that already has state; it is ignored') : 0;
    var action = 'PUSH';
    var location = createLocation(path, state, createKey(), history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      var href = createHref(location);
      var key = location.key,
          state = location.state;

      if (canUseHistory) {
        globalHistory.pushState({
          key: key,
          state: state
        }, null, href);

        if (forceRefresh) {
          window.location.href = href;
        } else {
          var prevIndex = allKeys.indexOf(history.location.key);
          var nextKeys = allKeys.slice(0, prevIndex + 1);
          nextKeys.push(location.key);
          allKeys = nextKeys;
          setState({
            action: action,
            location: location
          });
        }
      } else {
         true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(state === undefined, 'Browser history cannot push state in browsers that do not support HTML5 history') : 0;
        window.location.href = href;
      }
    });
  }

  function replace(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!(typeof path === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to replace when the 1st ' + 'argument is a location-like object that already has state; it is ignored') : 0;
    var action = 'REPLACE';
    var location = createLocation(path, state, createKey(), history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      var href = createHref(location);
      var key = location.key,
          state = location.state;

      if (canUseHistory) {
        globalHistory.replaceState({
          key: key,
          state: state
        }, null, href);

        if (forceRefresh) {
          window.location.replace(href);
        } else {
          var prevIndex = allKeys.indexOf(history.location.key);
          if (prevIndex !== -1) allKeys[prevIndex] = location.key;
          setState({
            action: action,
            location: location
          });
        }
      } else {
         true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(state === undefined, 'Browser history cannot replace state in browsers that do not support HTML5 history') : 0;
        window.location.replace(href);
      }
    });
  }

  function go(n) {
    globalHistory.go(n);
  }

  function goBack() {
    go(-1);
  }

  function goForward() {
    go(1);
  }

  var listenerCount = 0;

  function checkDOMListeners(delta) {
    listenerCount += delta;

    if (listenerCount === 1 && delta === 1) {
      window.addEventListener(PopStateEvent, handlePopState);
      if (needsHashChangeListener) window.addEventListener(HashChangeEvent, handleHashChange);
    } else if (listenerCount === 0) {
      window.removeEventListener(PopStateEvent, handlePopState);
      if (needsHashChangeListener) window.removeEventListener(HashChangeEvent, handleHashChange);
    }
  }

  var isBlocked = false;

  function block(prompt) {
    if (prompt === void 0) {
      prompt = false;
    }

    var unblock = transitionManager.setPrompt(prompt);

    if (!isBlocked) {
      checkDOMListeners(1);
      isBlocked = true;
    }

    return function () {
      if (isBlocked) {
        isBlocked = false;
        checkDOMListeners(-1);
      }

      return unblock();
    };
  }

  function listen(listener) {
    var unlisten = transitionManager.appendListener(listener);
    checkDOMListeners(1);
    return function () {
      checkDOMListeners(-1);
      unlisten();
    };
  }

  var history = {
    length: globalHistory.length,
    action: 'POP',
    location: initialLocation,
    createHref: createHref,
    push: push,
    replace: replace,
    go: go,
    goBack: goBack,
    goForward: goForward,
    block: block,
    listen: listen
  };
  return history;
}

var HashChangeEvent$1 = 'hashchange';
var HashPathCoders = {
  hashbang: {
    encodePath: function encodePath(path) {
      return path.charAt(0) === '!' ? path : '!/' + stripLeadingSlash(path);
    },
    decodePath: function decodePath(path) {
      return path.charAt(0) === '!' ? path.substr(1) : path;
    }
  },
  noslash: {
    encodePath: stripLeadingSlash,
    decodePath: addLeadingSlash
  },
  slash: {
    encodePath: addLeadingSlash,
    decodePath: addLeadingSlash
  }
};

function stripHash(url) {
  var hashIndex = url.indexOf('#');
  return hashIndex === -1 ? url : url.slice(0, hashIndex);
}

function getHashPath() {
  // We can't use window.location.hash here because it's not
  // consistent across browsers - Firefox will pre-decode it!
  var href = window.location.href;
  var hashIndex = href.indexOf('#');
  return hashIndex === -1 ? '' : href.substring(hashIndex + 1);
}

function pushHashPath(path) {
  window.location.hash = path;
}

function replaceHashPath(path) {
  window.location.replace(stripHash(window.location.href) + '#' + path);
}

function createHashHistory(props) {
  if (props === void 0) {
    props = {};
  }

  !canUseDOM ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_4__.default)(false, 'Hash history needs a DOM') : 0 : void 0;
  var globalHistory = window.history;
  var canGoWithoutReload = supportsGoWithoutReloadUsingHash();
  var _props = props,
      _props$getUserConfirm = _props.getUserConfirmation,
      getUserConfirmation = _props$getUserConfirm === void 0 ? getConfirmation : _props$getUserConfirm,
      _props$hashType = _props.hashType,
      hashType = _props$hashType === void 0 ? 'slash' : _props$hashType;
  var basename = props.basename ? stripTrailingSlash(addLeadingSlash(props.basename)) : '';
  var _HashPathCoders$hashT = HashPathCoders[hashType],
      encodePath = _HashPathCoders$hashT.encodePath,
      decodePath = _HashPathCoders$hashT.decodePath;

  function getDOMLocation() {
    var path = decodePath(getHashPath());
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!basename || hasBasename(path, basename), 'You are attempting to use a basename on a page whose URL path does not begin ' + 'with the basename. Expected path "' + path + '" to begin with "' + basename + '".') : 0;
    if (basename) path = stripBasename(path, basename);
    return createLocation(path);
  }

  var transitionManager = createTransitionManager();

  function setState(nextState) {
    (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_0__.default)(history, nextState);

    history.length = globalHistory.length;
    transitionManager.notifyListeners(history.location, history.action);
  }

  var forceNextPop = false;
  var ignorePath = null;

  function locationsAreEqual$$1(a, b) {
    return a.pathname === b.pathname && a.search === b.search && a.hash === b.hash;
  }

  function handleHashChange() {
    var path = getHashPath();
    var encodedPath = encodePath(path);

    if (path !== encodedPath) {
      // Ensure we always have a properly-encoded hash.
      replaceHashPath(encodedPath);
    } else {
      var location = getDOMLocation();
      var prevLocation = history.location;
      if (!forceNextPop && locationsAreEqual$$1(prevLocation, location)) return; // A hashchange doesn't always == location change.

      if (ignorePath === createPath(location)) return; // Ignore this change; we already setState in push/replace.

      ignorePath = null;
      handlePop(location);
    }
  }

  function handlePop(location) {
    if (forceNextPop) {
      forceNextPop = false;
      setState();
    } else {
      var action = 'POP';
      transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
        if (ok) {
          setState({
            action: action,
            location: location
          });
        } else {
          revertPop(location);
        }
      });
    }
  }

  function revertPop(fromLocation) {
    var toLocation = history.location; // TODO: We could probably make this more reliable by
    // keeping a list of paths we've seen in sessionStorage.
    // Instead, we just default to 0 for paths we don't know.

    var toIndex = allPaths.lastIndexOf(createPath(toLocation));
    if (toIndex === -1) toIndex = 0;
    var fromIndex = allPaths.lastIndexOf(createPath(fromLocation));
    if (fromIndex === -1) fromIndex = 0;
    var delta = toIndex - fromIndex;

    if (delta) {
      forceNextPop = true;
      go(delta);
    }
  } // Ensure the hash is encoded properly before doing anything else.


  var path = getHashPath();
  var encodedPath = encodePath(path);
  if (path !== encodedPath) replaceHashPath(encodedPath);
  var initialLocation = getDOMLocation();
  var allPaths = [createPath(initialLocation)]; // Public interface

  function createHref(location) {
    var baseTag = document.querySelector('base');
    var href = '';

    if (baseTag && baseTag.getAttribute('href')) {
      href = stripHash(window.location.href);
    }

    return href + '#' + encodePath(basename + createPath(location));
  }

  function push(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(state === undefined, 'Hash history cannot push state; it is ignored') : 0;
    var action = 'PUSH';
    var location = createLocation(path, undefined, undefined, history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      var path = createPath(location);
      var encodedPath = encodePath(basename + path);
      var hashChanged = getHashPath() !== encodedPath;

      if (hashChanged) {
        // We cannot tell if a hashchange was caused by a PUSH, so we'd
        // rather setState here and ignore the hashchange. The caveat here
        // is that other hash histories in the page will consider it a POP.
        ignorePath = path;
        pushHashPath(encodedPath);
        var prevIndex = allPaths.lastIndexOf(createPath(history.location));
        var nextPaths = allPaths.slice(0, prevIndex + 1);
        nextPaths.push(path);
        allPaths = nextPaths;
        setState({
          action: action,
          location: location
        });
      } else {
         true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(false, 'Hash history cannot PUSH the same path; a new entry will not be added to the history stack') : 0;
        setState();
      }
    });
  }

  function replace(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(state === undefined, 'Hash history cannot replace state; it is ignored') : 0;
    var action = 'REPLACE';
    var location = createLocation(path, undefined, undefined, history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      var path = createPath(location);
      var encodedPath = encodePath(basename + path);
      var hashChanged = getHashPath() !== encodedPath;

      if (hashChanged) {
        // We cannot tell if a hashchange was caused by a REPLACE, so we'd
        // rather setState here and ignore the hashchange. The caveat here
        // is that other hash histories in the page will consider it a POP.
        ignorePath = path;
        replaceHashPath(encodedPath);
      }

      var prevIndex = allPaths.indexOf(createPath(history.location));
      if (prevIndex !== -1) allPaths[prevIndex] = path;
      setState({
        action: action,
        location: location
      });
    });
  }

  function go(n) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(canGoWithoutReload, 'Hash history go(n) causes a full page reload in this browser') : 0;
    globalHistory.go(n);
  }

  function goBack() {
    go(-1);
  }

  function goForward() {
    go(1);
  }

  var listenerCount = 0;

  function checkDOMListeners(delta) {
    listenerCount += delta;

    if (listenerCount === 1 && delta === 1) {
      window.addEventListener(HashChangeEvent$1, handleHashChange);
    } else if (listenerCount === 0) {
      window.removeEventListener(HashChangeEvent$1, handleHashChange);
    }
  }

  var isBlocked = false;

  function block(prompt) {
    if (prompt === void 0) {
      prompt = false;
    }

    var unblock = transitionManager.setPrompt(prompt);

    if (!isBlocked) {
      checkDOMListeners(1);
      isBlocked = true;
    }

    return function () {
      if (isBlocked) {
        isBlocked = false;
        checkDOMListeners(-1);
      }

      return unblock();
    };
  }

  function listen(listener) {
    var unlisten = transitionManager.appendListener(listener);
    checkDOMListeners(1);
    return function () {
      checkDOMListeners(-1);
      unlisten();
    };
  }

  var history = {
    length: globalHistory.length,
    action: 'POP',
    location: initialLocation,
    createHref: createHref,
    push: push,
    replace: replace,
    go: go,
    goBack: goBack,
    goForward: goForward,
    block: block,
    listen: listen
  };
  return history;
}

function clamp(n, lowerBound, upperBound) {
  return Math.min(Math.max(n, lowerBound), upperBound);
}
/**
 * Creates a history object that stores locations in memory.
 */


function createMemoryHistory(props) {
  if (props === void 0) {
    props = {};
  }

  var _props = props,
      getUserConfirmation = _props.getUserConfirmation,
      _props$initialEntries = _props.initialEntries,
      initialEntries = _props$initialEntries === void 0 ? ['/'] : _props$initialEntries,
      _props$initialIndex = _props.initialIndex,
      initialIndex = _props$initialIndex === void 0 ? 0 : _props$initialIndex,
      _props$keyLength = _props.keyLength,
      keyLength = _props$keyLength === void 0 ? 6 : _props$keyLength;
  var transitionManager = createTransitionManager();

  function setState(nextState) {
    (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_0__.default)(history, nextState);

    history.length = history.entries.length;
    transitionManager.notifyListeners(history.location, history.action);
  }

  function createKey() {
    return Math.random().toString(36).substr(2, keyLength);
  }

  var index = clamp(initialIndex, 0, initialEntries.length - 1);
  var entries = initialEntries.map(function (entry) {
    return typeof entry === 'string' ? createLocation(entry, undefined, createKey()) : createLocation(entry, undefined, entry.key || createKey());
  }); // Public interface

  var createHref = createPath;

  function push(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!(typeof path === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to push when the 1st ' + 'argument is a location-like object that already has state; it is ignored') : 0;
    var action = 'PUSH';
    var location = createLocation(path, state, createKey(), history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      var prevIndex = history.index;
      var nextIndex = prevIndex + 1;
      var nextEntries = history.entries.slice(0);

      if (nextEntries.length > nextIndex) {
        nextEntries.splice(nextIndex, nextEntries.length - nextIndex, location);
      } else {
        nextEntries.push(location);
      }

      setState({
        action: action,
        location: location,
        index: nextIndex,
        entries: nextEntries
      });
    });
  }

  function replace(path, state) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)(!(typeof path === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to replace when the 1st ' + 'argument is a location-like object that already has state; it is ignored') : 0;
    var action = 'REPLACE';
    var location = createLocation(path, state, createKey(), history.location);
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;
      history.entries[history.index] = location;
      setState({
        action: action,
        location: location
      });
    });
  }

  function go(n) {
    var nextIndex = clamp(history.index + n, 0, history.entries.length - 1);
    var action = 'POP';
    var location = history.entries[nextIndex];
    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (ok) {
        setState({
          action: action,
          location: location,
          index: nextIndex
        });
      } else {
        // Mimic the behavior of DOM histories by
        // causing a render after a cancelled POP.
        setState();
      }
    });
  }

  function goBack() {
    go(-1);
  }

  function goForward() {
    go(1);
  }

  function canGo(n) {
    var nextIndex = history.index + n;
    return nextIndex >= 0 && nextIndex < history.entries.length;
  }

  function block(prompt) {
    if (prompt === void 0) {
      prompt = false;
    }

    return transitionManager.setPrompt(prompt);
  }

  function listen(listener) {
    return transitionManager.appendListener(listener);
  }

  var history = {
    length: entries.length,
    action: 'POP',
    location: entries[index],
    index: index,
    entries: entries,
    createHref: createHref,
    push: push,
    replace: replace,
    go: go,
    goBack: goBack,
    goForward: goForward,
    canGo: canGo,
    block: block,
    listen: listen
  };
  return history;
}




/***/ }),

/***/ "./node_modules/hoist-non-react-statics/dist/hoist-non-react-statics.cjs.js":
/*!**********************************************************************************!*\
  !*** ./node_modules/hoist-non-react-statics/dist/hoist-non-react-statics.cjs.js ***!
  \**********************************************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";


var reactIs = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");

/**
 * Copyright 2015, Yahoo! Inc.
 * Copyrights licensed under the New BSD License. See the accompanying LICENSE file for terms.
 */
var REACT_STATICS = {
  childContextTypes: true,
  contextType: true,
  contextTypes: true,
  defaultProps: true,
  displayName: true,
  getDefaultProps: true,
  getDerivedStateFromError: true,
  getDerivedStateFromProps: true,
  mixins: true,
  propTypes: true,
  type: true
};
var KNOWN_STATICS = {
  name: true,
  length: true,
  prototype: true,
  caller: true,
  callee: true,
  arguments: true,
  arity: true
};
var FORWARD_REF_STATICS = {
  '$$typeof': true,
  render: true,
  defaultProps: true,
  displayName: true,
  propTypes: true
};
var MEMO_STATICS = {
  '$$typeof': true,
  compare: true,
  defaultProps: true,
  displayName: true,
  propTypes: true,
  type: true
};
var TYPE_STATICS = {};
TYPE_STATICS[reactIs.ForwardRef] = FORWARD_REF_STATICS;
TYPE_STATICS[reactIs.Memo] = MEMO_STATICS;

function getStatics(component) {
  // React v16.11 and below
  if (reactIs.isMemo(component)) {
    return MEMO_STATICS;
  } // React v16.12 and above


  return TYPE_STATICS[component['$$typeof']] || REACT_STATICS;
}

var defineProperty = Object.defineProperty;
var getOwnPropertyNames = Object.getOwnPropertyNames;
var getOwnPropertySymbols = Object.getOwnPropertySymbols;
var getOwnPropertyDescriptor = Object.getOwnPropertyDescriptor;
var getPrototypeOf = Object.getPrototypeOf;
var objectPrototype = Object.prototype;
function hoistNonReactStatics(targetComponent, sourceComponent, blacklist) {
  if (typeof sourceComponent !== 'string') {
    // don't hoist over string (html) components
    if (objectPrototype) {
      var inheritedComponent = getPrototypeOf(sourceComponent);

      if (inheritedComponent && inheritedComponent !== objectPrototype) {
        hoistNonReactStatics(targetComponent, inheritedComponent, blacklist);
      }
    }

    var keys = getOwnPropertyNames(sourceComponent);

    if (getOwnPropertySymbols) {
      keys = keys.concat(getOwnPropertySymbols(sourceComponent));
    }

    var targetStatics = getStatics(targetComponent);
    var sourceStatics = getStatics(sourceComponent);

    for (var i = 0; i < keys.length; ++i) {
      var key = keys[i];

      if (!KNOWN_STATICS[key] && !(blacklist && blacklist[key]) && !(sourceStatics && sourceStatics[key]) && !(targetStatics && targetStatics[key])) {
        var descriptor = getOwnPropertyDescriptor(sourceComponent, key);

        try {
          // Avoid failures from read-only properties
          defineProperty(targetComponent, key, descriptor);
        } catch (e) {}
      }
    }
  }

  return targetComponent;
}

module.exports = hoistNonReactStatics;


/***/ }),

/***/ "./node_modules/isarray/index.js":
/*!***************************************!*\
  !*** ./node_modules/isarray/index.js ***!
  \***************************************/
/***/ ((module) => {

module.exports = Array.isArray || function (arr) {
  return Object.prototype.toString.call(arr) == '[object Array]';
};


/***/ }),

/***/ "./node_modules/mini-create-react-context/dist/esm/index.js":
/*!******************************************************************!*\
  !*** ./node_modules/mini-create-react-context/dist/esm/index.js ***!
  \******************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @babel/runtime/helpers/esm/inheritsLoose */ "./node_modules/@babel/runtime/helpers/esm/inheritsLoose.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var tiny_warning__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! tiny-warning */ "./node_modules/tiny-warning/dist/tiny-warning.esm.js");





var MAX_SIGNED_31_BIT_INT = 1073741823;
var commonjsGlobal = typeof globalThis !== 'undefined' ? globalThis : typeof window !== 'undefined' ? window : typeof __webpack_require__.g !== 'undefined' ? __webpack_require__.g : {};

function getUniqueId() {
  var key = '__global_unique_id__';
  return commonjsGlobal[key] = (commonjsGlobal[key] || 0) + 1;
}

function objectIs(x, y) {
  if (x === y) {
    return x !== 0 || 1 / x === 1 / y;
  } else {
    return x !== x && y !== y;
  }
}

function createEventEmitter(value) {
  var handlers = [];
  return {
    on: function on(handler) {
      handlers.push(handler);
    },
    off: function off(handler) {
      handlers = handlers.filter(function (h) {
        return h !== handler;
      });
    },
    get: function get() {
      return value;
    },
    set: function set(newValue, changedBits) {
      value = newValue;
      handlers.forEach(function (handler) {
        return handler(value, changedBits);
      });
    }
  };
}

function onlyChild(children) {
  return Array.isArray(children) ? children[0] : children;
}

function createReactContext(defaultValue, calculateChangedBits) {
  var _Provider$childContex, _Consumer$contextType;

  var contextProp = '__create-react-context-' + getUniqueId() + '__';

  var Provider = /*#__PURE__*/function (_Component) {
    (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__.default)(Provider, _Component);

    function Provider() {
      var _this;

      _this = _Component.apply(this, arguments) || this;
      _this.emitter = createEventEmitter(_this.props.value);
      return _this;
    }

    var _proto = Provider.prototype;

    _proto.getChildContext = function getChildContext() {
      var _ref;

      return _ref = {}, _ref[contextProp] = this.emitter, _ref;
    };

    _proto.componentWillReceiveProps = function componentWillReceiveProps(nextProps) {
      if (this.props.value !== nextProps.value) {
        var oldValue = this.props.value;
        var newValue = nextProps.value;
        var changedBits;

        if (objectIs(oldValue, newValue)) {
          changedBits = 0;
        } else {
          changedBits = typeof calculateChangedBits === 'function' ? calculateChangedBits(oldValue, newValue) : MAX_SIGNED_31_BIT_INT;

          if (true) {
            (0,tiny_warning__WEBPACK_IMPORTED_MODULE_3__.default)((changedBits & MAX_SIGNED_31_BIT_INT) === changedBits, 'calculateChangedBits: Expected the return value to be a ' + '31-bit integer. Instead received: ' + changedBits);
          }

          changedBits |= 0;

          if (changedBits !== 0) {
            this.emitter.set(nextProps.value, changedBits);
          }
        }
      }
    };

    _proto.render = function render() {
      return this.props.children;
    };

    return Provider;
  }(react__WEBPACK_IMPORTED_MODULE_0__.Component);

  Provider.childContextTypes = (_Provider$childContex = {}, _Provider$childContex[contextProp] = (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object.isRequired), _Provider$childContex);

  var Consumer = /*#__PURE__*/function (_Component2) {
    (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__.default)(Consumer, _Component2);

    function Consumer() {
      var _this2;

      _this2 = _Component2.apply(this, arguments) || this;
      _this2.state = {
        value: _this2.getValue()
      };

      _this2.onUpdate = function (newValue, changedBits) {
        var observedBits = _this2.observedBits | 0;

        if ((observedBits & changedBits) !== 0) {
          _this2.setState({
            value: _this2.getValue()
          });
        }
      };

      return _this2;
    }

    var _proto2 = Consumer.prototype;

    _proto2.componentWillReceiveProps = function componentWillReceiveProps(nextProps) {
      var observedBits = nextProps.observedBits;
      this.observedBits = observedBits === undefined || observedBits === null ? MAX_SIGNED_31_BIT_INT : observedBits;
    };

    _proto2.componentDidMount = function componentDidMount() {
      if (this.context[contextProp]) {
        this.context[contextProp].on(this.onUpdate);
      }

      var observedBits = this.props.observedBits;
      this.observedBits = observedBits === undefined || observedBits === null ? MAX_SIGNED_31_BIT_INT : observedBits;
    };

    _proto2.componentWillUnmount = function componentWillUnmount() {
      if (this.context[contextProp]) {
        this.context[contextProp].off(this.onUpdate);
      }
    };

    _proto2.getValue = function getValue() {
      if (this.context[contextProp]) {
        return this.context[contextProp].get();
      } else {
        return defaultValue;
      }
    };

    _proto2.render = function render() {
      return onlyChild(this.props.children)(this.state.value);
    };

    return Consumer;
  }(react__WEBPACK_IMPORTED_MODULE_0__.Component);

  Consumer.contextTypes = (_Consumer$contextType = {}, _Consumer$contextType[contextProp] = (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object), _Consumer$contextType);
  return {
    Provider: Provider,
    Consumer: Consumer
  };
}

var index = (react__WEBPACK_IMPORTED_MODULE_0___default().createContext) || createReactContext;

/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (index);


/***/ }),

/***/ "./node_modules/object-assign/index.js":
/*!*********************************************!*\
  !*** ./node_modules/object-assign/index.js ***!
  \*********************************************/
/***/ ((module) => {

"use strict";
/*
object-assign
(c) Sindre Sorhus
@license MIT
*/


/* eslint-disable no-unused-vars */
var getOwnPropertySymbols = Object.getOwnPropertySymbols;
var hasOwnProperty = Object.prototype.hasOwnProperty;
var propIsEnumerable = Object.prototype.propertyIsEnumerable;

function toObject(val) {
	if (val === null || val === undefined) {
		throw new TypeError('Object.assign cannot be called with null or undefined');
	}

	return Object(val);
}

function shouldUseNative() {
	try {
		if (!Object.assign) {
			return false;
		}

		// Detect buggy property enumeration order in older V8 versions.

		// https://bugs.chromium.org/p/v8/issues/detail?id=4118
		var test1 = new String('abc');  // eslint-disable-line no-new-wrappers
		test1[5] = 'de';
		if (Object.getOwnPropertyNames(test1)[0] === '5') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test2 = {};
		for (var i = 0; i < 10; i++) {
			test2['_' + String.fromCharCode(i)] = i;
		}
		var order2 = Object.getOwnPropertyNames(test2).map(function (n) {
			return test2[n];
		});
		if (order2.join('') !== '0123456789') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test3 = {};
		'abcdefghijklmnopqrst'.split('').forEach(function (letter) {
			test3[letter] = letter;
		});
		if (Object.keys(Object.assign({}, test3)).join('') !==
				'abcdefghijklmnopqrst') {
			return false;
		}

		return true;
	} catch (err) {
		// We don't expect any of the above to throw, but better to be safe.
		return false;
	}
}

module.exports = shouldUseNative() ? Object.assign : function (target, source) {
	var from;
	var to = toObject(target);
	var symbols;

	for (var s = 1; s < arguments.length; s++) {
		from = Object(arguments[s]);

		for (var key in from) {
			if (hasOwnProperty.call(from, key)) {
				to[key] = from[key];
			}
		}

		if (getOwnPropertySymbols) {
			symbols = getOwnPropertySymbols(from);
			for (var i = 0; i < symbols.length; i++) {
				if (propIsEnumerable.call(from, symbols[i])) {
					to[symbols[i]] = from[symbols[i]];
				}
			}
		}
	}

	return to;
};


/***/ }),

/***/ "./node_modules/path-to-regexp/index.js":
/*!**********************************************!*\
  !*** ./node_modules/path-to-regexp/index.js ***!
  \**********************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

var isarray = __webpack_require__(/*! isarray */ "./node_modules/isarray/index.js")

/**
 * Expose `pathToRegexp`.
 */
module.exports = pathToRegexp
module.exports.parse = parse
module.exports.compile = compile
module.exports.tokensToFunction = tokensToFunction
module.exports.tokensToRegExp = tokensToRegExp

/**
 * The main path matching regexp utility.
 *
 * @type {RegExp}
 */
var PATH_REGEXP = new RegExp([
  // Match escaped characters that would otherwise appear in future matches.
  // This allows the user to escape special characters that won't transform.
  '(\\\\.)',
  // Match Express-style parameters and un-named parameters with a prefix
  // and optional suffixes. Matches appear as:
  //
  // "/:test(\\d+)?" => ["/", "test", "\d+", undefined, "?", undefined]
  // "/route(\\d+)"  => [undefined, undefined, undefined, "\d+", undefined, undefined]
  // "/*"            => ["/", undefined, undefined, undefined, undefined, "*"]
  '([\\/.])?(?:(?:\\:(\\w+)(?:\\(((?:\\\\.|[^\\\\()])+)\\))?|\\(((?:\\\\.|[^\\\\()])+)\\))([+*?])?|(\\*))'
].join('|'), 'g')

/**
 * Parse a string for the raw tokens.
 *
 * @param  {string}  str
 * @param  {Object=} options
 * @return {!Array}
 */
function parse (str, options) {
  var tokens = []
  var key = 0
  var index = 0
  var path = ''
  var defaultDelimiter = options && options.delimiter || '/'
  var res

  while ((res = PATH_REGEXP.exec(str)) != null) {
    var m = res[0]
    var escaped = res[1]
    var offset = res.index
    path += str.slice(index, offset)
    index = offset + m.length

    // Ignore already escaped sequences.
    if (escaped) {
      path += escaped[1]
      continue
    }

    var next = str[index]
    var prefix = res[2]
    var name = res[3]
    var capture = res[4]
    var group = res[5]
    var modifier = res[6]
    var asterisk = res[7]

    // Push the current path onto the tokens.
    if (path) {
      tokens.push(path)
      path = ''
    }

    var partial = prefix != null && next != null && next !== prefix
    var repeat = modifier === '+' || modifier === '*'
    var optional = modifier === '?' || modifier === '*'
    var delimiter = res[2] || defaultDelimiter
    var pattern = capture || group

    tokens.push({
      name: name || key++,
      prefix: prefix || '',
      delimiter: delimiter,
      optional: optional,
      repeat: repeat,
      partial: partial,
      asterisk: !!asterisk,
      pattern: pattern ? escapeGroup(pattern) : (asterisk ? '.*' : '[^' + escapeString(delimiter) + ']+?')
    })
  }

  // Match any characters still remaining.
  if (index < str.length) {
    path += str.substr(index)
  }

  // If the path exists, push it onto the end.
  if (path) {
    tokens.push(path)
  }

  return tokens
}

/**
 * Compile a string to a template function for the path.
 *
 * @param  {string}             str
 * @param  {Object=}            options
 * @return {!function(Object=, Object=)}
 */
function compile (str, options) {
  return tokensToFunction(parse(str, options), options)
}

/**
 * Prettier encoding of URI path segments.
 *
 * @param  {string}
 * @return {string}
 */
function encodeURIComponentPretty (str) {
  return encodeURI(str).replace(/[\/?#]/g, function (c) {
    return '%' + c.charCodeAt(0).toString(16).toUpperCase()
  })
}

/**
 * Encode the asterisk parameter. Similar to `pretty`, but allows slashes.
 *
 * @param  {string}
 * @return {string}
 */
function encodeAsterisk (str) {
  return encodeURI(str).replace(/[?#]/g, function (c) {
    return '%' + c.charCodeAt(0).toString(16).toUpperCase()
  })
}

/**
 * Expose a method for transforming tokens into the path function.
 */
function tokensToFunction (tokens, options) {
  // Compile all the tokens into regexps.
  var matches = new Array(tokens.length)

  // Compile all the patterns before compilation.
  for (var i = 0; i < tokens.length; i++) {
    if (typeof tokens[i] === 'object') {
      matches[i] = new RegExp('^(?:' + tokens[i].pattern + ')$', flags(options))
    }
  }

  return function (obj, opts) {
    var path = ''
    var data = obj || {}
    var options = opts || {}
    var encode = options.pretty ? encodeURIComponentPretty : encodeURIComponent

    for (var i = 0; i < tokens.length; i++) {
      var token = tokens[i]

      if (typeof token === 'string') {
        path += token

        continue
      }

      var value = data[token.name]
      var segment

      if (value == null) {
        if (token.optional) {
          // Prepend partial segment prefixes.
          if (token.partial) {
            path += token.prefix
          }

          continue
        } else {
          throw new TypeError('Expected "' + token.name + '" to be defined')
        }
      }

      if (isarray(value)) {
        if (!token.repeat) {
          throw new TypeError('Expected "' + token.name + '" to not repeat, but received `' + JSON.stringify(value) + '`')
        }

        if (value.length === 0) {
          if (token.optional) {
            continue
          } else {
            throw new TypeError('Expected "' + token.name + '" to not be empty')
          }
        }

        for (var j = 0; j < value.length; j++) {
          segment = encode(value[j])

          if (!matches[i].test(segment)) {
            throw new TypeError('Expected all "' + token.name + '" to match "' + token.pattern + '", but received `' + JSON.stringify(segment) + '`')
          }

          path += (j === 0 ? token.prefix : token.delimiter) + segment
        }

        continue
      }

      segment = token.asterisk ? encodeAsterisk(value) : encode(value)

      if (!matches[i].test(segment)) {
        throw new TypeError('Expected "' + token.name + '" to match "' + token.pattern + '", but received "' + segment + '"')
      }

      path += token.prefix + segment
    }

    return path
  }
}

/**
 * Escape a regular expression string.
 *
 * @param  {string} str
 * @return {string}
 */
function escapeString (str) {
  return str.replace(/([.+*?=^!:${}()[\]|\/\\])/g, '\\$1')
}

/**
 * Escape the capturing group by escaping special characters and meaning.
 *
 * @param  {string} group
 * @return {string}
 */
function escapeGroup (group) {
  return group.replace(/([=!:$\/()])/g, '\\$1')
}

/**
 * Attach the keys as a property of the regexp.
 *
 * @param  {!RegExp} re
 * @param  {Array}   keys
 * @return {!RegExp}
 */
function attachKeys (re, keys) {
  re.keys = keys
  return re
}

/**
 * Get the flags for a regexp from the options.
 *
 * @param  {Object} options
 * @return {string}
 */
function flags (options) {
  return options && options.sensitive ? '' : 'i'
}

/**
 * Pull out keys from a regexp.
 *
 * @param  {!RegExp} path
 * @param  {!Array}  keys
 * @return {!RegExp}
 */
function regexpToRegexp (path, keys) {
  // Use a negative lookahead to match only capturing groups.
  var groups = path.source.match(/\((?!\?)/g)

  if (groups) {
    for (var i = 0; i < groups.length; i++) {
      keys.push({
        name: i,
        prefix: null,
        delimiter: null,
        optional: false,
        repeat: false,
        partial: false,
        asterisk: false,
        pattern: null
      })
    }
  }

  return attachKeys(path, keys)
}

/**
 * Transform an array into a regexp.
 *
 * @param  {!Array}  path
 * @param  {Array}   keys
 * @param  {!Object} options
 * @return {!RegExp}
 */
function arrayToRegexp (path, keys, options) {
  var parts = []

  for (var i = 0; i < path.length; i++) {
    parts.push(pathToRegexp(path[i], keys, options).source)
  }

  var regexp = new RegExp('(?:' + parts.join('|') + ')', flags(options))

  return attachKeys(regexp, keys)
}

/**
 * Create a path regexp from string input.
 *
 * @param  {string}  path
 * @param  {!Array}  keys
 * @param  {!Object} options
 * @return {!RegExp}
 */
function stringToRegexp (path, keys, options) {
  return tokensToRegExp(parse(path, options), keys, options)
}

/**
 * Expose a function for taking tokens and returning a RegExp.
 *
 * @param  {!Array}          tokens
 * @param  {(Array|Object)=} keys
 * @param  {Object=}         options
 * @return {!RegExp}
 */
function tokensToRegExp (tokens, keys, options) {
  if (!isarray(keys)) {
    options = /** @type {!Object} */ (keys || options)
    keys = []
  }

  options = options || {}

  var strict = options.strict
  var end = options.end !== false
  var route = ''

  // Iterate over the tokens and create our regexp string.
  for (var i = 0; i < tokens.length; i++) {
    var token = tokens[i]

    if (typeof token === 'string') {
      route += escapeString(token)
    } else {
      var prefix = escapeString(token.prefix)
      var capture = '(?:' + token.pattern + ')'

      keys.push(token)

      if (token.repeat) {
        capture += '(?:' + prefix + capture + ')*'
      }

      if (token.optional) {
        if (!token.partial) {
          capture = '(?:' + prefix + '(' + capture + '))?'
        } else {
          capture = prefix + '(' + capture + ')?'
        }
      } else {
        capture = prefix + '(' + capture + ')'
      }

      route += capture
    }
  }

  var delimiter = escapeString(options.delimiter || '/')
  var endsWithDelimiter = route.slice(-delimiter.length) === delimiter

  // In non-strict mode we allow a slash at the end of match. If the path to
  // match already ends with a slash, we remove it for consistency. The slash
  // is valid at the end of a path match, not in the middle. This is important
  // in non-ending mode, where "/test/" shouldn't match "/test//route".
  if (!strict) {
    route = (endsWithDelimiter ? route.slice(0, -delimiter.length) : route) + '(?:' + delimiter + '(?=$))?'
  }

  if (end) {
    route += '$'
  } else {
    // In non-ending mode, we need the capturing groups to match as much as
    // possible by using a positive lookahead to the end or next path segment.
    route += strict && endsWithDelimiter ? '' : '(?=' + delimiter + '|$)'
  }

  return attachKeys(new RegExp('^' + route, flags(options)), keys)
}

/**
 * Normalize the given path string, returning a regular expression.
 *
 * An empty array can be passed in for the keys, which will hold the
 * placeholder key descriptions. For example, using `/user/:id`, `keys` will
 * contain `[{ name: 'id', delimiter: '/', optional: false, repeat: false }]`.
 *
 * @param  {(string|RegExp|Array)} path
 * @param  {(Array|Object)=}       keys
 * @param  {Object=}               options
 * @return {!RegExp}
 */
function pathToRegexp (path, keys, options) {
  if (!isarray(keys)) {
    options = /** @type {!Object} */ (keys || options)
    keys = []
  }

  options = options || {}

  if (path instanceof RegExp) {
    return regexpToRegexp(path, /** @type {!Array} */ (keys))
  }

  if (isarray(path)) {
    return arrayToRegexp(/** @type {!Array} */ (path), /** @type {!Array} */ (keys), options)
  }

  return stringToRegexp(/** @type {string} */ (path), /** @type {!Array} */ (keys), options)
}


/***/ }),

/***/ "./node_modules/prop-types/checkPropTypes.js":
/*!***************************************************!*\
  !*** ./node_modules/prop-types/checkPropTypes.js ***!
  \***************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var printWarning = function() {};

if (true) {
  var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "./node_modules/prop-types/lib/ReactPropTypesSecret.js");
  var loggedTypeFailures = {};
  var has = Function.call.bind(Object.prototype.hasOwnProperty);

  printWarning = function(text) {
    var message = 'Warning: ' + text;
    if (typeof console !== 'undefined') {
      console.error(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };
}

/**
 * Assert that the values match with the type specs.
 * Error messages are memorized and will only be shown once.
 *
 * @param {object} typeSpecs Map of name to a ReactPropType
 * @param {object} values Runtime values that need to be type-checked
 * @param {string} location e.g. "prop", "context", "child context"
 * @param {string} componentName Name of the component for error messages.
 * @param {?Function} getStack Returns the component stack.
 * @private
 */
function checkPropTypes(typeSpecs, values, location, componentName, getStack) {
  if (true) {
    for (var typeSpecName in typeSpecs) {
      if (has(typeSpecs, typeSpecName)) {
        var error;
        // Prop type validation may throw. In case they do, we don't want to
        // fail the render phase where it didn't fail before. So we log it.
        // After these have been cleaned up, we'll let them throw.
        try {
          // This is intentionally an invariant that gets caught. It's the same
          // behavior as without this statement except with a better message.
          if (typeof typeSpecs[typeSpecName] !== 'function') {
            var err = Error(
              (componentName || 'React class') + ': ' + location + ' type `' + typeSpecName + '` is invalid; ' +
              'it must be a function, usually from the `prop-types` package, but received `' + typeof typeSpecs[typeSpecName] + '`.'
            );
            err.name = 'Invariant Violation';
            throw err;
          }
          error = typeSpecs[typeSpecName](values, typeSpecName, componentName, location, null, ReactPropTypesSecret);
        } catch (ex) {
          error = ex;
        }
        if (error && !(error instanceof Error)) {
          printWarning(
            (componentName || 'React class') + ': type specification of ' +
            location + ' `' + typeSpecName + '` is invalid; the type checker ' +
            'function must return `null` or an `Error` but returned a ' + typeof error + '. ' +
            'You may have forgotten to pass an argument to the type checker ' +
            'creator (arrayOf, instanceOf, objectOf, oneOf, oneOfType, and ' +
            'shape all require an argument).'
          );
        }
        if (error instanceof Error && !(error.message in loggedTypeFailures)) {
          // Only monitor this failure once because there tends to be a lot of the
          // same error.
          loggedTypeFailures[error.message] = true;

          var stack = getStack ? getStack() : '';

          printWarning(
            'Failed ' + location + ' type: ' + error.message + (stack != null ? stack : '')
          );
        }
      }
    }
  }
}

/**
 * Resets warning cache when testing.
 *
 * @private
 */
checkPropTypes.resetWarningCache = function() {
  if (true) {
    loggedTypeFailures = {};
  }
}

module.exports = checkPropTypes;


/***/ }),

/***/ "./node_modules/prop-types/factoryWithTypeCheckers.js":
/*!************************************************************!*\
  !*** ./node_modules/prop-types/factoryWithTypeCheckers.js ***!
  \************************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactIs = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");
var assign = __webpack_require__(/*! object-assign */ "./node_modules/object-assign/index.js");

var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "./node_modules/prop-types/lib/ReactPropTypesSecret.js");
var checkPropTypes = __webpack_require__(/*! ./checkPropTypes */ "./node_modules/prop-types/checkPropTypes.js");

var has = Function.call.bind(Object.prototype.hasOwnProperty);
var printWarning = function() {};

if (true) {
  printWarning = function(text) {
    var message = 'Warning: ' + text;
    if (typeof console !== 'undefined') {
      console.error(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };
}

function emptyFunctionThatReturnsNull() {
  return null;
}

module.exports = function(isValidElement, throwOnDirectAccess) {
  /* global Symbol */
  var ITERATOR_SYMBOL = typeof Symbol === 'function' && Symbol.iterator;
  var FAUX_ITERATOR_SYMBOL = '@@iterator'; // Before Symbol spec.

  /**
   * Returns the iterator method function contained on the iterable object.
   *
   * Be sure to invoke the function with the iterable as context:
   *
   *     var iteratorFn = getIteratorFn(myIterable);
   *     if (iteratorFn) {
   *       var iterator = iteratorFn.call(myIterable);
   *       ...
   *     }
   *
   * @param {?object} maybeIterable
   * @return {?function}
   */
  function getIteratorFn(maybeIterable) {
    var iteratorFn = maybeIterable && (ITERATOR_SYMBOL && maybeIterable[ITERATOR_SYMBOL] || maybeIterable[FAUX_ITERATOR_SYMBOL]);
    if (typeof iteratorFn === 'function') {
      return iteratorFn;
    }
  }

  /**
   * Collection of methods that allow declaration and validation of props that are
   * supplied to React components. Example usage:
   *
   *   var Props = require('ReactPropTypes');
   *   var MyArticle = React.createClass({
   *     propTypes: {
   *       // An optional string prop named "description".
   *       description: Props.string,
   *
   *       // A required enum prop named "category".
   *       category: Props.oneOf(['News','Photos']).isRequired,
   *
   *       // A prop named "dialog" that requires an instance of Dialog.
   *       dialog: Props.instanceOf(Dialog).isRequired
   *     },
   *     render: function() { ... }
   *   });
   *
   * A more formal specification of how these methods are used:
   *
   *   type := array|bool|func|object|number|string|oneOf([...])|instanceOf(...)
   *   decl := ReactPropTypes.{type}(.isRequired)?
   *
   * Each and every declaration produces a function with the same signature. This
   * allows the creation of custom validation functions. For example:
   *
   *  var MyLink = React.createClass({
   *    propTypes: {
   *      // An optional string or URI prop named "href".
   *      href: function(props, propName, componentName) {
   *        var propValue = props[propName];
   *        if (propValue != null && typeof propValue !== 'string' &&
   *            !(propValue instanceof URI)) {
   *          return new Error(
   *            'Expected a string or an URI for ' + propName + ' in ' +
   *            componentName
   *          );
   *        }
   *      }
   *    },
   *    render: function() {...}
   *  });
   *
   * @internal
   */

  var ANONYMOUS = '<<anonymous>>';

  // Important!
  // Keep this list in sync with production version in `./factoryWithThrowingShims.js`.
  var ReactPropTypes = {
    array: createPrimitiveTypeChecker('array'),
    bool: createPrimitiveTypeChecker('boolean'),
    func: createPrimitiveTypeChecker('function'),
    number: createPrimitiveTypeChecker('number'),
    object: createPrimitiveTypeChecker('object'),
    string: createPrimitiveTypeChecker('string'),
    symbol: createPrimitiveTypeChecker('symbol'),

    any: createAnyTypeChecker(),
    arrayOf: createArrayOfTypeChecker,
    element: createElementTypeChecker(),
    elementType: createElementTypeTypeChecker(),
    instanceOf: createInstanceTypeChecker,
    node: createNodeChecker(),
    objectOf: createObjectOfTypeChecker,
    oneOf: createEnumTypeChecker,
    oneOfType: createUnionTypeChecker,
    shape: createShapeTypeChecker,
    exact: createStrictShapeTypeChecker,
  };

  /**
   * inlined Object.is polyfill to avoid requiring consumers ship their own
   * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/is
   */
  /*eslint-disable no-self-compare*/
  function is(x, y) {
    // SameValue algorithm
    if (x === y) {
      // Steps 1-5, 7-10
      // Steps 6.b-6.e: +0 != -0
      return x !== 0 || 1 / x === 1 / y;
    } else {
      // Step 6.a: NaN == NaN
      return x !== x && y !== y;
    }
  }
  /*eslint-enable no-self-compare*/

  /**
   * We use an Error-like object for backward compatibility as people may call
   * PropTypes directly and inspect their output. However, we don't use real
   * Errors anymore. We don't inspect their stack anyway, and creating them
   * is prohibitively expensive if they are created too often, such as what
   * happens in oneOfType() for any type before the one that matched.
   */
  function PropTypeError(message) {
    this.message = message;
    this.stack = '';
  }
  // Make `instanceof Error` still work for returned errors.
  PropTypeError.prototype = Error.prototype;

  function createChainableTypeChecker(validate) {
    if (true) {
      var manualPropTypeCallCache = {};
      var manualPropTypeWarningCount = 0;
    }
    function checkType(isRequired, props, propName, componentName, location, propFullName, secret) {
      componentName = componentName || ANONYMOUS;
      propFullName = propFullName || propName;

      if (secret !== ReactPropTypesSecret) {
        if (throwOnDirectAccess) {
          // New behavior only for users of `prop-types` package
          var err = new Error(
            'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
            'Use `PropTypes.checkPropTypes()` to call them. ' +
            'Read more at http://fb.me/use-check-prop-types'
          );
          err.name = 'Invariant Violation';
          throw err;
        } else if ( true && typeof console !== 'undefined') {
          // Old behavior for people using React.PropTypes
          var cacheKey = componentName + ':' + propName;
          if (
            !manualPropTypeCallCache[cacheKey] &&
            // Avoid spamming the console because they are often not actionable except for lib authors
            manualPropTypeWarningCount < 3
          ) {
            printWarning(
              'You are manually calling a React.PropTypes validation ' +
              'function for the `' + propFullName + '` prop on `' + componentName  + '`. This is deprecated ' +
              'and will throw in the standalone `prop-types` package. ' +
              'You may be seeing this warning due to a third-party PropTypes ' +
              'library. See https://fb.me/react-warning-dont-call-proptypes ' + 'for details.'
            );
            manualPropTypeCallCache[cacheKey] = true;
            manualPropTypeWarningCount++;
          }
        }
      }
      if (props[propName] == null) {
        if (isRequired) {
          if (props[propName] === null) {
            return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required ' + ('in `' + componentName + '`, but its value is `null`.'));
          }
          return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required in ' + ('`' + componentName + '`, but its value is `undefined`.'));
        }
        return null;
      } else {
        return validate(props, propName, componentName, location, propFullName);
      }
    }

    var chainedCheckType = checkType.bind(null, false);
    chainedCheckType.isRequired = checkType.bind(null, true);

    return chainedCheckType;
  }

  function createPrimitiveTypeChecker(expectedType) {
    function validate(props, propName, componentName, location, propFullName, secret) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== expectedType) {
        // `propValue` being instance of, say, date/regexp, pass the 'object'
        // check, but we can offer a more precise error message here rather than
        // 'of type `object`'.
        var preciseType = getPreciseType(propValue);

        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + preciseType + '` supplied to `' + componentName + '`, expected ') + ('`' + expectedType + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createAnyTypeChecker() {
    return createChainableTypeChecker(emptyFunctionThatReturnsNull);
  }

  function createArrayOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside arrayOf.');
      }
      var propValue = props[propName];
      if (!Array.isArray(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an array.'));
      }
      for (var i = 0; i < propValue.length; i++) {
        var error = typeChecker(propValue, i, componentName, location, propFullName + '[' + i + ']', ReactPropTypesSecret);
        if (error instanceof Error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createElementTypeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      if (!isValidElement(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected a single ReactElement.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createElementTypeTypeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      if (!ReactIs.isValidElementType(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected a single ReactElement type.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createInstanceTypeChecker(expectedClass) {
    function validate(props, propName, componentName, location, propFullName) {
      if (!(props[propName] instanceof expectedClass)) {
        var expectedClassName = expectedClass.name || ANONYMOUS;
        var actualClassName = getClassName(props[propName]);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + actualClassName + '` supplied to `' + componentName + '`, expected ') + ('instance of `' + expectedClassName + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createEnumTypeChecker(expectedValues) {
    if (!Array.isArray(expectedValues)) {
      if (true) {
        if (arguments.length > 1) {
          printWarning(
            'Invalid arguments supplied to oneOf, expected an array, got ' + arguments.length + ' arguments. ' +
            'A common mistake is to write oneOf(x, y, z) instead of oneOf([x, y, z]).'
          );
        } else {
          printWarning('Invalid argument supplied to oneOf, expected an array.');
        }
      }
      return emptyFunctionThatReturnsNull;
    }

    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      for (var i = 0; i < expectedValues.length; i++) {
        if (is(propValue, expectedValues[i])) {
          return null;
        }
      }

      var valuesString = JSON.stringify(expectedValues, function replacer(key, value) {
        var type = getPreciseType(value);
        if (type === 'symbol') {
          return String(value);
        }
        return value;
      });
      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of value `' + String(propValue) + '` ' + ('supplied to `' + componentName + '`, expected one of ' + valuesString + '.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createObjectOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside objectOf.');
      }
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an object.'));
      }
      for (var key in propValue) {
        if (has(propValue, key)) {
          var error = typeChecker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
          if (error instanceof Error) {
            return error;
          }
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createUnionTypeChecker(arrayOfTypeCheckers) {
    if (!Array.isArray(arrayOfTypeCheckers)) {
       true ? printWarning('Invalid argument supplied to oneOfType, expected an instance of array.') : 0;
      return emptyFunctionThatReturnsNull;
    }

    for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
      var checker = arrayOfTypeCheckers[i];
      if (typeof checker !== 'function') {
        printWarning(
          'Invalid argument supplied to oneOfType. Expected an array of check functions, but ' +
          'received ' + getPostfixForTypeWarning(checker) + ' at index ' + i + '.'
        );
        return emptyFunctionThatReturnsNull;
      }
    }

    function validate(props, propName, componentName, location, propFullName) {
      for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
        var checker = arrayOfTypeCheckers[i];
        if (checker(props, propName, componentName, location, propFullName, ReactPropTypesSecret) == null) {
          return null;
        }
      }

      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createNodeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      if (!isNode(props[propName])) {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`, expected a ReactNode.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      for (var key in shapeTypes) {
        var checker = shapeTypes[key];
        if (!checker) {
          continue;
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createStrictShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      // We need to check all keys in case some are required but missing from
      // props.
      var allKeys = assign({}, props[propName], shapeTypes);
      for (var key in allKeys) {
        var checker = shapeTypes[key];
        if (!checker) {
          return new PropTypeError(
            'Invalid ' + location + ' `' + propFullName + '` key `' + key + '` supplied to `' + componentName + '`.' +
            '\nBad object: ' + JSON.stringify(props[propName], null, '  ') +
            '\nValid keys: ' +  JSON.stringify(Object.keys(shapeTypes), null, '  ')
          );
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }

    return createChainableTypeChecker(validate);
  }

  function isNode(propValue) {
    switch (typeof propValue) {
      case 'number':
      case 'string':
      case 'undefined':
        return true;
      case 'boolean':
        return !propValue;
      case 'object':
        if (Array.isArray(propValue)) {
          return propValue.every(isNode);
        }
        if (propValue === null || isValidElement(propValue)) {
          return true;
        }

        var iteratorFn = getIteratorFn(propValue);
        if (iteratorFn) {
          var iterator = iteratorFn.call(propValue);
          var step;
          if (iteratorFn !== propValue.entries) {
            while (!(step = iterator.next()).done) {
              if (!isNode(step.value)) {
                return false;
              }
            }
          } else {
            // Iterator will provide entry [k,v] tuples rather than values.
            while (!(step = iterator.next()).done) {
              var entry = step.value;
              if (entry) {
                if (!isNode(entry[1])) {
                  return false;
                }
              }
            }
          }
        } else {
          return false;
        }

        return true;
      default:
        return false;
    }
  }

  function isSymbol(propType, propValue) {
    // Native Symbol.
    if (propType === 'symbol') {
      return true;
    }

    // falsy value can't be a Symbol
    if (!propValue) {
      return false;
    }

    // 19.4.3.5 Symbol.prototype[@@toStringTag] === 'Symbol'
    if (propValue['@@toStringTag'] === 'Symbol') {
      return true;
    }

    // Fallback for non-spec compliant Symbols which are polyfilled.
    if (typeof Symbol === 'function' && propValue instanceof Symbol) {
      return true;
    }

    return false;
  }

  // Equivalent of `typeof` but with special handling for array and regexp.
  function getPropType(propValue) {
    var propType = typeof propValue;
    if (Array.isArray(propValue)) {
      return 'array';
    }
    if (propValue instanceof RegExp) {
      // Old webkits (at least until Android 4.0) return 'function' rather than
      // 'object' for typeof a RegExp. We'll normalize this here so that /bla/
      // passes PropTypes.object.
      return 'object';
    }
    if (isSymbol(propType, propValue)) {
      return 'symbol';
    }
    return propType;
  }

  // This handles more types than `getPropType`. Only used for error messages.
  // See `createPrimitiveTypeChecker`.
  function getPreciseType(propValue) {
    if (typeof propValue === 'undefined' || propValue === null) {
      return '' + propValue;
    }
    var propType = getPropType(propValue);
    if (propType === 'object') {
      if (propValue instanceof Date) {
        return 'date';
      } else if (propValue instanceof RegExp) {
        return 'regexp';
      }
    }
    return propType;
  }

  // Returns a string that is postfixed to a warning about an invalid type.
  // For example, "undefined" or "of type array"
  function getPostfixForTypeWarning(value) {
    var type = getPreciseType(value);
    switch (type) {
      case 'array':
      case 'object':
        return 'an ' + type;
      case 'boolean':
      case 'date':
      case 'regexp':
        return 'a ' + type;
      default:
        return type;
    }
  }

  // Returns class name of the object, if any.
  function getClassName(propValue) {
    if (!propValue.constructor || !propValue.constructor.name) {
      return ANONYMOUS;
    }
    return propValue.constructor.name;
  }

  ReactPropTypes.checkPropTypes = checkPropTypes;
  ReactPropTypes.resetWarningCache = checkPropTypes.resetWarningCache;
  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),

/***/ "./node_modules/prop-types/index.js":
/*!******************************************!*\
  !*** ./node_modules/prop-types/index.js ***!
  \******************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (true) {
  var ReactIs = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");

  // By explicitly using `prop-types` you are opting into new development behavior.
  // http://fb.me/prop-types-in-prod
  var throwOnDirectAccess = true;
  module.exports = __webpack_require__(/*! ./factoryWithTypeCheckers */ "./node_modules/prop-types/factoryWithTypeCheckers.js")(ReactIs.isElement, throwOnDirectAccess);
} else {}


/***/ }),

/***/ "./node_modules/prop-types/lib/ReactPropTypesSecret.js":
/*!*************************************************************!*\
  !*** ./node_modules/prop-types/lib/ReactPropTypesSecret.js ***!
  \*************************************************************/
/***/ ((module) => {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),

/***/ "./node_modules/react-is/cjs/react-is.development.js":
/*!***********************************************************!*\
  !*** ./node_modules/react-is/cjs/react-is.development.js ***!
  \***********************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";
/** @license React v16.13.1
 * react-is.development.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */





if (true) {
  (function() {
'use strict';

// The Symbol used to tag the ReactElement-like types. If there is no native Symbol
// nor polyfill, then a plain number is used for performance.
var hasSymbol = typeof Symbol === 'function' && Symbol.for;
var REACT_ELEMENT_TYPE = hasSymbol ? Symbol.for('react.element') : 0xeac7;
var REACT_PORTAL_TYPE = hasSymbol ? Symbol.for('react.portal') : 0xeaca;
var REACT_FRAGMENT_TYPE = hasSymbol ? Symbol.for('react.fragment') : 0xeacb;
var REACT_STRICT_MODE_TYPE = hasSymbol ? Symbol.for('react.strict_mode') : 0xeacc;
var REACT_PROFILER_TYPE = hasSymbol ? Symbol.for('react.profiler') : 0xead2;
var REACT_PROVIDER_TYPE = hasSymbol ? Symbol.for('react.provider') : 0xeacd;
var REACT_CONTEXT_TYPE = hasSymbol ? Symbol.for('react.context') : 0xeace; // TODO: We don't use AsyncMode or ConcurrentMode anymore. They were temporary
// (unstable) APIs that have been removed. Can we remove the symbols?

var REACT_ASYNC_MODE_TYPE = hasSymbol ? Symbol.for('react.async_mode') : 0xeacf;
var REACT_CONCURRENT_MODE_TYPE = hasSymbol ? Symbol.for('react.concurrent_mode') : 0xeacf;
var REACT_FORWARD_REF_TYPE = hasSymbol ? Symbol.for('react.forward_ref') : 0xead0;
var REACT_SUSPENSE_TYPE = hasSymbol ? Symbol.for('react.suspense') : 0xead1;
var REACT_SUSPENSE_LIST_TYPE = hasSymbol ? Symbol.for('react.suspense_list') : 0xead8;
var REACT_MEMO_TYPE = hasSymbol ? Symbol.for('react.memo') : 0xead3;
var REACT_LAZY_TYPE = hasSymbol ? Symbol.for('react.lazy') : 0xead4;
var REACT_BLOCK_TYPE = hasSymbol ? Symbol.for('react.block') : 0xead9;
var REACT_FUNDAMENTAL_TYPE = hasSymbol ? Symbol.for('react.fundamental') : 0xead5;
var REACT_RESPONDER_TYPE = hasSymbol ? Symbol.for('react.responder') : 0xead6;
var REACT_SCOPE_TYPE = hasSymbol ? Symbol.for('react.scope') : 0xead7;

function isValidElementType(type) {
  return typeof type === 'string' || typeof type === 'function' || // Note: its typeof might be other than 'symbol' or 'number' if it's a polyfill.
  type === REACT_FRAGMENT_TYPE || type === REACT_CONCURRENT_MODE_TYPE || type === REACT_PROFILER_TYPE || type === REACT_STRICT_MODE_TYPE || type === REACT_SUSPENSE_TYPE || type === REACT_SUSPENSE_LIST_TYPE || typeof type === 'object' && type !== null && (type.$$typeof === REACT_LAZY_TYPE || type.$$typeof === REACT_MEMO_TYPE || type.$$typeof === REACT_PROVIDER_TYPE || type.$$typeof === REACT_CONTEXT_TYPE || type.$$typeof === REACT_FORWARD_REF_TYPE || type.$$typeof === REACT_FUNDAMENTAL_TYPE || type.$$typeof === REACT_RESPONDER_TYPE || type.$$typeof === REACT_SCOPE_TYPE || type.$$typeof === REACT_BLOCK_TYPE);
}

function typeOf(object) {
  if (typeof object === 'object' && object !== null) {
    var $$typeof = object.$$typeof;

    switch ($$typeof) {
      case REACT_ELEMENT_TYPE:
        var type = object.type;

        switch (type) {
          case REACT_ASYNC_MODE_TYPE:
          case REACT_CONCURRENT_MODE_TYPE:
          case REACT_FRAGMENT_TYPE:
          case REACT_PROFILER_TYPE:
          case REACT_STRICT_MODE_TYPE:
          case REACT_SUSPENSE_TYPE:
            return type;

          default:
            var $$typeofType = type && type.$$typeof;

            switch ($$typeofType) {
              case REACT_CONTEXT_TYPE:
              case REACT_FORWARD_REF_TYPE:
              case REACT_LAZY_TYPE:
              case REACT_MEMO_TYPE:
              case REACT_PROVIDER_TYPE:
                return $$typeofType;

              default:
                return $$typeof;
            }

        }

      case REACT_PORTAL_TYPE:
        return $$typeof;
    }
  }

  return undefined;
} // AsyncMode is deprecated along with isAsyncMode

var AsyncMode = REACT_ASYNC_MODE_TYPE;
var ConcurrentMode = REACT_CONCURRENT_MODE_TYPE;
var ContextConsumer = REACT_CONTEXT_TYPE;
var ContextProvider = REACT_PROVIDER_TYPE;
var Element = REACT_ELEMENT_TYPE;
var ForwardRef = REACT_FORWARD_REF_TYPE;
var Fragment = REACT_FRAGMENT_TYPE;
var Lazy = REACT_LAZY_TYPE;
var Memo = REACT_MEMO_TYPE;
var Portal = REACT_PORTAL_TYPE;
var Profiler = REACT_PROFILER_TYPE;
var StrictMode = REACT_STRICT_MODE_TYPE;
var Suspense = REACT_SUSPENSE_TYPE;
var hasWarnedAboutDeprecatedIsAsyncMode = false; // AsyncMode should be deprecated

function isAsyncMode(object) {
  {
    if (!hasWarnedAboutDeprecatedIsAsyncMode) {
      hasWarnedAboutDeprecatedIsAsyncMode = true; // Using console['warn'] to evade Babel and ESLint

      console['warn']('The ReactIs.isAsyncMode() alias has been deprecated, ' + 'and will be removed in React 17+. Update your code to use ' + 'ReactIs.isConcurrentMode() instead. It has the exact same API.');
    }
  }

  return isConcurrentMode(object) || typeOf(object) === REACT_ASYNC_MODE_TYPE;
}
function isConcurrentMode(object) {
  return typeOf(object) === REACT_CONCURRENT_MODE_TYPE;
}
function isContextConsumer(object) {
  return typeOf(object) === REACT_CONTEXT_TYPE;
}
function isContextProvider(object) {
  return typeOf(object) === REACT_PROVIDER_TYPE;
}
function isElement(object) {
  return typeof object === 'object' && object !== null && object.$$typeof === REACT_ELEMENT_TYPE;
}
function isForwardRef(object) {
  return typeOf(object) === REACT_FORWARD_REF_TYPE;
}
function isFragment(object) {
  return typeOf(object) === REACT_FRAGMENT_TYPE;
}
function isLazy(object) {
  return typeOf(object) === REACT_LAZY_TYPE;
}
function isMemo(object) {
  return typeOf(object) === REACT_MEMO_TYPE;
}
function isPortal(object) {
  return typeOf(object) === REACT_PORTAL_TYPE;
}
function isProfiler(object) {
  return typeOf(object) === REACT_PROFILER_TYPE;
}
function isStrictMode(object) {
  return typeOf(object) === REACT_STRICT_MODE_TYPE;
}
function isSuspense(object) {
  return typeOf(object) === REACT_SUSPENSE_TYPE;
}

exports.AsyncMode = AsyncMode;
exports.ConcurrentMode = ConcurrentMode;
exports.ContextConsumer = ContextConsumer;
exports.ContextProvider = ContextProvider;
exports.Element = Element;
exports.ForwardRef = ForwardRef;
exports.Fragment = Fragment;
exports.Lazy = Lazy;
exports.Memo = Memo;
exports.Portal = Portal;
exports.Profiler = Profiler;
exports.StrictMode = StrictMode;
exports.Suspense = Suspense;
exports.isAsyncMode = isAsyncMode;
exports.isConcurrentMode = isConcurrentMode;
exports.isContextConsumer = isContextConsumer;
exports.isContextProvider = isContextProvider;
exports.isElement = isElement;
exports.isForwardRef = isForwardRef;
exports.isFragment = isFragment;
exports.isLazy = isLazy;
exports.isMemo = isMemo;
exports.isPortal = isPortal;
exports.isProfiler = isProfiler;
exports.isStrictMode = isStrictMode;
exports.isSuspense = isSuspense;
exports.isValidElementType = isValidElementType;
exports.typeOf = typeOf;
  })();
}


/***/ }),

/***/ "./node_modules/react-is/index.js":
/*!****************************************!*\
  !*** ./node_modules/react-is/index.js ***!
  \****************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";


if (false) {} else {
  module.exports = __webpack_require__(/*! ./cjs/react-is.development.js */ "./node_modules/react-is/cjs/react-is.development.js");
}


/***/ }),

/***/ "./node_modules/react-router-dom/esm/react-router-dom.js":
/*!***************************************************************!*\
  !*** ./node_modules/react-router-dom/esm/react-router-dom.js ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "MemoryRouter": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.MemoryRouter),
/* harmony export */   "Prompt": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.Prompt),
/* harmony export */   "Redirect": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.Redirect),
/* harmony export */   "Route": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.Route),
/* harmony export */   "Router": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.Router),
/* harmony export */   "StaticRouter": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.StaticRouter),
/* harmony export */   "Switch": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.Switch),
/* harmony export */   "generatePath": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.generatePath),
/* harmony export */   "matchPath": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.matchPath),
/* harmony export */   "useHistory": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.useHistory),
/* harmony export */   "useLocation": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.useLocation),
/* harmony export */   "useParams": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.useParams),
/* harmony export */   "useRouteMatch": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.useRouteMatch),
/* harmony export */   "withRouter": () => (/* reexport safe */ react_router__WEBPACK_IMPORTED_MODULE_0__.withRouter),
/* harmony export */   "BrowserRouter": () => (/* binding */ BrowserRouter),
/* harmony export */   "HashRouter": () => (/* binding */ HashRouter),
/* harmony export */   "Link": () => (/* binding */ Link),
/* harmony export */   "NavLink": () => (/* binding */ NavLink)
/* harmony export */ });
/* harmony import */ var react_router__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react-router */ "./node_modules/react-router/esm/react-router.js");
/* harmony import */ var _babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @babel/runtime/helpers/esm/inheritsLoose */ "./node_modules/@babel/runtime/helpers/esm/inheritsLoose.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var history__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! history */ "./node_modules/history/esm/history.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var tiny_warning__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! tiny-warning */ "./node_modules/tiny-warning/dist/tiny-warning.esm.js");
/* harmony import */ var _babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @babel/runtime/helpers/esm/extends */ "./node_modules/@babel/runtime/helpers/esm/extends.js");
/* harmony import */ var _babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @babel/runtime/helpers/esm/objectWithoutPropertiesLoose */ "./node_modules/@babel/runtime/helpers/esm/objectWithoutPropertiesLoose.js");
/* harmony import */ var tiny_invariant__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! tiny-invariant */ "./node_modules/tiny-invariant/dist/tiny-invariant.esm.js");











/**
 * The public API for a <Router> that uses HTML5 history.
 */

var BrowserRouter =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__.default)(BrowserRouter, _React$Component);

  function BrowserRouter() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;
    _this.history = (0,history__WEBPACK_IMPORTED_MODULE_6__.createBrowserHistory)(_this.props);
    return _this;
  }

  var _proto = BrowserRouter.prototype;

  _proto.render = function render() {
    return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(react_router__WEBPACK_IMPORTED_MODULE_0__.Router, {
      history: this.history,
      children: this.props.children
    });
  };

  return BrowserRouter;
}((react__WEBPACK_IMPORTED_MODULE_2___default().Component));

if (true) {
  BrowserRouter.propTypes = {
    basename: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().string),
    children: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().node),
    forceRefresh: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().bool),
    getUserConfirmation: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func),
    keyLength: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().number)
  };

  BrowserRouter.prototype.componentDidMount = function () {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_7__.default)(!this.props.history, "<BrowserRouter> ignores the history prop. To use a custom history, " + "use `import { Router }` instead of `import { BrowserRouter as Router }`.") : 0;
  };
}

/**
 * The public API for a <Router> that uses window.location.hash.
 */

var HashRouter =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_1__.default)(HashRouter, _React$Component);

  function HashRouter() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;
    _this.history = (0,history__WEBPACK_IMPORTED_MODULE_6__.createHashHistory)(_this.props);
    return _this;
  }

  var _proto = HashRouter.prototype;

  _proto.render = function render() {
    return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(react_router__WEBPACK_IMPORTED_MODULE_0__.Router, {
      history: this.history,
      children: this.props.children
    });
  };

  return HashRouter;
}((react__WEBPACK_IMPORTED_MODULE_2___default().Component));

if (true) {
  HashRouter.propTypes = {
    basename: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().string),
    children: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().node),
    getUserConfirmation: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func),
    hashType: prop_types__WEBPACK_IMPORTED_MODULE_3___default().oneOf(["hashbang", "noslash", "slash"])
  };

  HashRouter.prototype.componentDidMount = function () {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_7__.default)(!this.props.history, "<HashRouter> ignores the history prop. To use a custom history, " + "use `import { Router }` instead of `import { HashRouter as Router }`.") : 0;
  };
}

var resolveToLocation = function resolveToLocation(to, currentLocation) {
  return typeof to === "function" ? to(currentLocation) : to;
};
var normalizeToLocation = function normalizeToLocation(to, currentLocation) {
  return typeof to === "string" ? (0,history__WEBPACK_IMPORTED_MODULE_6__.createLocation)(to, null, null, currentLocation) : to;
};

var forwardRefShim = function forwardRefShim(C) {
  return C;
};

var forwardRef = (react__WEBPACK_IMPORTED_MODULE_2___default().forwardRef);

if (typeof forwardRef === "undefined") {
  forwardRef = forwardRefShim;
}

function isModifiedEvent(event) {
  return !!(event.metaKey || event.altKey || event.ctrlKey || event.shiftKey);
}

var LinkAnchor = forwardRef(function (_ref, forwardedRef) {
  var innerRef = _ref.innerRef,
      navigate = _ref.navigate,
      _onClick = _ref.onClick,
      rest = (0,_babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_5__.default)(_ref, ["innerRef", "navigate", "onClick"]);

  var target = rest.target;

  var props = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, rest, {
    onClick: function onClick(event) {
      try {
        if (_onClick) _onClick(event);
      } catch (ex) {
        event.preventDefault();
        throw ex;
      }

      if (!event.defaultPrevented && // onClick prevented default
      event.button === 0 && ( // ignore everything but left clicks
      !target || target === "_self") && // let browser handle "target=_blank" etc.
      !isModifiedEvent(event) // ignore clicks with modifier keys
      ) {
          event.preventDefault();
          navigate();
        }
    }
  }); // React 15 compat


  if (forwardRefShim !== forwardRef) {
    props.ref = forwardedRef || innerRef;
  } else {
    props.ref = innerRef;
  }
  /* eslint-disable-next-line jsx-a11y/anchor-has-content */


  return react__WEBPACK_IMPORTED_MODULE_2___default().createElement("a", props);
});

if (true) {
  LinkAnchor.displayName = "LinkAnchor";
}
/**
 * The public API for rendering a history-aware <a>.
 */


var Link = forwardRef(function (_ref2, forwardedRef) {
  var _ref2$component = _ref2.component,
      component = _ref2$component === void 0 ? LinkAnchor : _ref2$component,
      replace = _ref2.replace,
      to = _ref2.to,
      innerRef = _ref2.innerRef,
      rest = (0,_babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_5__.default)(_ref2, ["component", "replace", "to", "innerRef"]);

  return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(react_router__WEBPACK_IMPORTED_MODULE_0__.__RouterContext.Consumer, null, function (context) {
    !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_8__.default)(false, "You should not use <Link> outside a <Router>") : 0 : void 0;
    var history = context.history;
    var location = normalizeToLocation(resolveToLocation(to, context.location), context.location);
    var href = location ? history.createHref(location) : "";

    var props = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, rest, {
      href: href,
      navigate: function navigate() {
        var location = resolveToLocation(to, context.location);
        var method = replace ? history.replace : history.push;
        method(location);
      }
    }); // React 15 compat


    if (forwardRefShim !== forwardRef) {
      props.ref = forwardedRef || innerRef;
    } else {
      props.innerRef = innerRef;
    }

    return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(component, props);
  });
});

if (true) {
  var toType = prop_types__WEBPACK_IMPORTED_MODULE_3___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_3___default().string), (prop_types__WEBPACK_IMPORTED_MODULE_3___default().object), (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func)]);
  var refType = prop_types__WEBPACK_IMPORTED_MODULE_3___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_3___default().string), (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func), prop_types__WEBPACK_IMPORTED_MODULE_3___default().shape({
    current: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().any)
  })]);
  Link.displayName = "Link";
  Link.propTypes = {
    innerRef: refType,
    onClick: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func),
    replace: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().bool),
    target: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().string),
    to: toType.isRequired
  };
}

var forwardRefShim$1 = function forwardRefShim(C) {
  return C;
};

var forwardRef$1 = (react__WEBPACK_IMPORTED_MODULE_2___default().forwardRef);

if (typeof forwardRef$1 === "undefined") {
  forwardRef$1 = forwardRefShim$1;
}

function joinClassnames() {
  for (var _len = arguments.length, classnames = new Array(_len), _key = 0; _key < _len; _key++) {
    classnames[_key] = arguments[_key];
  }

  return classnames.filter(function (i) {
    return i;
  }).join(" ");
}
/**
 * A <Link> wrapper that knows if it's "active" or not.
 */


var NavLink = forwardRef$1(function (_ref, forwardedRef) {
  var _ref$ariaCurrent = _ref["aria-current"],
      ariaCurrent = _ref$ariaCurrent === void 0 ? "page" : _ref$ariaCurrent,
      _ref$activeClassName = _ref.activeClassName,
      activeClassName = _ref$activeClassName === void 0 ? "active" : _ref$activeClassName,
      activeStyle = _ref.activeStyle,
      classNameProp = _ref.className,
      exact = _ref.exact,
      isActiveProp = _ref.isActive,
      locationProp = _ref.location,
      sensitive = _ref.sensitive,
      strict = _ref.strict,
      styleProp = _ref.style,
      to = _ref.to,
      innerRef = _ref.innerRef,
      rest = (0,_babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_5__.default)(_ref, ["aria-current", "activeClassName", "activeStyle", "className", "exact", "isActive", "location", "sensitive", "strict", "style", "to", "innerRef"]);

  return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(react_router__WEBPACK_IMPORTED_MODULE_0__.__RouterContext.Consumer, null, function (context) {
    !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_8__.default)(false, "You should not use <NavLink> outside a <Router>") : 0 : void 0;
    var currentLocation = locationProp || context.location;
    var toLocation = normalizeToLocation(resolveToLocation(to, currentLocation), currentLocation);
    var path = toLocation.pathname; // Regex taken from: https://github.com/pillarjs/path-to-regexp/blob/master/index.js#L202

    var escapedPath = path && path.replace(/([.+*?=^!:${}()[\]|/\\])/g, "\\$1");
    var match = escapedPath ? (0,react_router__WEBPACK_IMPORTED_MODULE_0__.matchPath)(currentLocation.pathname, {
      path: escapedPath,
      exact: exact,
      sensitive: sensitive,
      strict: strict
    }) : null;
    var isActive = !!(isActiveProp ? isActiveProp(match, currentLocation) : match);
    var className = isActive ? joinClassnames(classNameProp, activeClassName) : classNameProp;
    var style = isActive ? (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, styleProp, {}, activeStyle) : styleProp;

    var props = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({
      "aria-current": isActive && ariaCurrent || null,
      className: className,
      style: style,
      to: toLocation
    }, rest); // React 15 compat


    if (forwardRefShim$1 !== forwardRef$1) {
      props.ref = forwardedRef || innerRef;
    } else {
      props.innerRef = innerRef;
    }

    return react__WEBPACK_IMPORTED_MODULE_2___default().createElement(Link, props);
  });
});

if (true) {
  NavLink.displayName = "NavLink";
  var ariaCurrentType = prop_types__WEBPACK_IMPORTED_MODULE_3___default().oneOf(["page", "step", "location", "date", "time", "true"]);
  NavLink.propTypes = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, Link.propTypes, {
    "aria-current": ariaCurrentType,
    activeClassName: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().string),
    activeStyle: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().object),
    className: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().string),
    exact: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().bool),
    isActive: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().func),
    location: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().object),
    sensitive: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().bool),
    strict: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().bool),
    style: (prop_types__WEBPACK_IMPORTED_MODULE_3___default().object)
  });
}


//# sourceMappingURL=react-router-dom.js.map


/***/ }),

/***/ "./node_modules/react-router/esm/react-router.js":
/*!*******************************************************!*\
  !*** ./node_modules/react-router/esm/react-router.js ***!
  \*******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "MemoryRouter": () => (/* binding */ MemoryRouter),
/* harmony export */   "Prompt": () => (/* binding */ Prompt),
/* harmony export */   "Redirect": () => (/* binding */ Redirect),
/* harmony export */   "Route": () => (/* binding */ Route),
/* harmony export */   "Router": () => (/* binding */ Router),
/* harmony export */   "StaticRouter": () => (/* binding */ StaticRouter),
/* harmony export */   "Switch": () => (/* binding */ Switch),
/* harmony export */   "__HistoryContext": () => (/* binding */ historyContext),
/* harmony export */   "__RouterContext": () => (/* binding */ context),
/* harmony export */   "generatePath": () => (/* binding */ generatePath),
/* harmony export */   "matchPath": () => (/* binding */ matchPath),
/* harmony export */   "useHistory": () => (/* binding */ useHistory),
/* harmony export */   "useLocation": () => (/* binding */ useLocation),
/* harmony export */   "useParams": () => (/* binding */ useParams),
/* harmony export */   "useRouteMatch": () => (/* binding */ useRouteMatch),
/* harmony export */   "withRouter": () => (/* binding */ withRouter)
/* harmony export */ });
/* harmony import */ var _babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @babel/runtime/helpers/esm/inheritsLoose */ "./node_modules/@babel/runtime/helpers/esm/inheritsLoose.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var history__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! history */ "./node_modules/history/esm/history.js");
/* harmony import */ var tiny_warning__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! tiny-warning */ "./node_modules/tiny-warning/dist/tiny-warning.esm.js");
/* harmony import */ var mini_create_react_context__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! mini-create-react-context */ "./node_modules/mini-create-react-context/dist/esm/index.js");
/* harmony import */ var tiny_invariant__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! tiny-invariant */ "./node_modules/tiny-invariant/dist/tiny-invariant.esm.js");
/* harmony import */ var _babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @babel/runtime/helpers/esm/extends */ "./node_modules/@babel/runtime/helpers/esm/extends.js");
/* harmony import */ var path_to_regexp__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! path-to-regexp */ "./node_modules/path-to-regexp/index.js");
/* harmony import */ var path_to_regexp__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(path_to_regexp__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var react_is__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");
/* harmony import */ var _babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @babel/runtime/helpers/esm/objectWithoutPropertiesLoose */ "./node_modules/@babel/runtime/helpers/esm/objectWithoutPropertiesLoose.js");
/* harmony import */ var hoist_non_react_statics__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! hoist-non-react-statics */ "./node_modules/hoist-non-react-statics/dist/hoist-non-react-statics.cjs.js");
/* harmony import */ var hoist_non_react_statics__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(hoist_non_react_statics__WEBPACK_IMPORTED_MODULE_8__);













// TODO: Replace with React.createContext once we can assume React 16+

var createNamedContext = function createNamedContext(name) {
  var context = (0,mini_create_react_context__WEBPACK_IMPORTED_MODULE_3__.default)();
  context.displayName = name;
  return context;
};

var historyContext =
/*#__PURE__*/
createNamedContext("Router-History");

// TODO: Replace with React.createContext once we can assume React 16+

var createNamedContext$1 = function createNamedContext(name) {
  var context = (0,mini_create_react_context__WEBPACK_IMPORTED_MODULE_3__.default)();
  context.displayName = name;
  return context;
};

var context =
/*#__PURE__*/
createNamedContext$1("Router");

/**
 * The public API for putting history on context.
 */

var Router =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(Router, _React$Component);

  Router.computeRootMatch = function computeRootMatch(pathname) {
    return {
      path: "/",
      url: "/",
      params: {},
      isExact: pathname === "/"
    };
  };

  function Router(props) {
    var _this;

    _this = _React$Component.call(this, props) || this;
    _this.state = {
      location: props.history.location
    }; // This is a bit of a hack. We have to start listening for location
    // changes here in the constructor in case there are any <Redirect>s
    // on the initial render. If there are, they will replace/push when
    // they mount and since cDM fires in children before parents, we may
    // get a new location before the <Router> is mounted.

    _this._isMounted = false;
    _this._pendingLocation = null;

    if (!props.staticContext) {
      _this.unlisten = props.history.listen(function (location) {
        if (_this._isMounted) {
          _this.setState({
            location: location
          });
        } else {
          _this._pendingLocation = location;
        }
      });
    }

    return _this;
  }

  var _proto = Router.prototype;

  _proto.componentDidMount = function componentDidMount() {
    this._isMounted = true;

    if (this._pendingLocation) {
      this.setState({
        location: this._pendingLocation
      });
    }
  };

  _proto.componentWillUnmount = function componentWillUnmount() {
    if (this.unlisten) this.unlisten();
  };

  _proto.render = function render() {
    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Provider, {
      value: {
        history: this.props.history,
        location: this.state.location,
        match: Router.computeRootMatch(this.state.location.pathname),
        staticContext: this.props.staticContext
      }
    }, react__WEBPACK_IMPORTED_MODULE_1___default().createElement(historyContext.Provider, {
      children: this.props.children || null,
      value: this.props.history
    }));
  };

  return Router;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

if (true) {
  Router.propTypes = {
    children: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().node),
    history: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object.isRequired),
    staticContext: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object)
  };

  Router.prototype.componentDidUpdate = function (prevProps) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(prevProps.history === this.props.history, "You cannot change <Router history>") : 0;
  };
}

/**
 * The public API for a <Router> that stores location in memory.
 */

var MemoryRouter =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(MemoryRouter, _React$Component);

  function MemoryRouter() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;
    _this.history = (0,history__WEBPACK_IMPORTED_MODULE_10__.createMemoryHistory)(_this.props);
    return _this;
  }

  var _proto = MemoryRouter.prototype;

  _proto.render = function render() {
    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(Router, {
      history: this.history,
      children: this.props.children
    });
  };

  return MemoryRouter;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

if (true) {
  MemoryRouter.propTypes = {
    initialEntries: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().array),
    initialIndex: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().number),
    getUserConfirmation: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().func),
    keyLength: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().number),
    children: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().node)
  };

  MemoryRouter.prototype.componentDidMount = function () {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!this.props.history, "<MemoryRouter> ignores the history prop. To use a custom history, " + "use `import { Router }` instead of `import { MemoryRouter as Router }`.") : 0;
  };
}

var Lifecycle =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(Lifecycle, _React$Component);

  function Lifecycle() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Lifecycle.prototype;

  _proto.componentDidMount = function componentDidMount() {
    if (this.props.onMount) this.props.onMount.call(this, this);
  };

  _proto.componentDidUpdate = function componentDidUpdate(prevProps) {
    if (this.props.onUpdate) this.props.onUpdate.call(this, this, prevProps);
  };

  _proto.componentWillUnmount = function componentWillUnmount() {
    if (this.props.onUnmount) this.props.onUnmount.call(this, this);
  };

  _proto.render = function render() {
    return null;
  };

  return Lifecycle;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

/**
 * The public API for prompting the user before navigating away from a screen.
 */

function Prompt(_ref) {
  var message = _ref.message,
      _ref$when = _ref.when,
      when = _ref$when === void 0 ? true : _ref$when;
  return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Consumer, null, function (context) {
    !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You should not use <Prompt> outside a <Router>") : 0 : void 0;
    if (!when || context.staticContext) return null;
    var method = context.history.block;
    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(Lifecycle, {
      onMount: function onMount(self) {
        self.release = method(message);
      },
      onUpdate: function onUpdate(self, prevProps) {
        if (prevProps.message !== message) {
          self.release();
          self.release = method(message);
        }
      },
      onUnmount: function onUnmount(self) {
        self.release();
      },
      message: message
    });
  });
}

if (true) {
  var messageType = prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().func), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().string)]);
  Prompt.propTypes = {
    when: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().bool),
    message: messageType.isRequired
  };
}

var cache = {};
var cacheLimit = 10000;
var cacheCount = 0;

function compilePath(path) {
  if (cache[path]) return cache[path];
  var generator = path_to_regexp__WEBPACK_IMPORTED_MODULE_5___default().compile(path);

  if (cacheCount < cacheLimit) {
    cache[path] = generator;
    cacheCount++;
  }

  return generator;
}
/**
 * Public API for generating a URL pathname from a path and parameters.
 */


function generatePath(path, params) {
  if (path === void 0) {
    path = "/";
  }

  if (params === void 0) {
    params = {};
  }

  return path === "/" ? path : compilePath(path)(params, {
    pretty: true
  });
}

/**
 * The public API for navigating programmatically with a component.
 */

function Redirect(_ref) {
  var computedMatch = _ref.computedMatch,
      to = _ref.to,
      _ref$push = _ref.push,
      push = _ref$push === void 0 ? false : _ref$push;
  return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Consumer, null, function (context) {
    !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You should not use <Redirect> outside a <Router>") : 0 : void 0;
    var history = context.history,
        staticContext = context.staticContext;
    var method = push ? history.push : history.replace;
    var location = (0,history__WEBPACK_IMPORTED_MODULE_10__.createLocation)(computedMatch ? typeof to === "string" ? generatePath(to, computedMatch.params) : (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, to, {
      pathname: generatePath(to.pathname, computedMatch.params)
    }) : to); // When rendering in a static context,
    // set the new location immediately.

    if (staticContext) {
      method(location);
      return null;
    }

    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(Lifecycle, {
      onMount: function onMount() {
        method(location);
      },
      onUpdate: function onUpdate(self, prevProps) {
        var prevLocation = (0,history__WEBPACK_IMPORTED_MODULE_10__.createLocation)(prevProps.to);

        if (!(0,history__WEBPACK_IMPORTED_MODULE_10__.locationsAreEqual)(prevLocation, (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, location, {
          key: prevLocation.key
        }))) {
          method(location);
        }
      },
      to: to
    });
  });
}

if (true) {
  Redirect.propTypes = {
    push: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().bool),
    from: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().string),
    to: prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().string), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object)]).isRequired
  };
}

var cache$1 = {};
var cacheLimit$1 = 10000;
var cacheCount$1 = 0;

function compilePath$1(path, options) {
  var cacheKey = "" + options.end + options.strict + options.sensitive;
  var pathCache = cache$1[cacheKey] || (cache$1[cacheKey] = {});
  if (pathCache[path]) return pathCache[path];
  var keys = [];
  var regexp = path_to_regexp__WEBPACK_IMPORTED_MODULE_5___default()(path, keys, options);
  var result = {
    regexp: regexp,
    keys: keys
  };

  if (cacheCount$1 < cacheLimit$1) {
    pathCache[path] = result;
    cacheCount$1++;
  }

  return result;
}
/**
 * Public API for matching a URL pathname to a path.
 */


function matchPath(pathname, options) {
  if (options === void 0) {
    options = {};
  }

  if (typeof options === "string" || Array.isArray(options)) {
    options = {
      path: options
    };
  }

  var _options = options,
      path = _options.path,
      _options$exact = _options.exact,
      exact = _options$exact === void 0 ? false : _options$exact,
      _options$strict = _options.strict,
      strict = _options$strict === void 0 ? false : _options$strict,
      _options$sensitive = _options.sensitive,
      sensitive = _options$sensitive === void 0 ? false : _options$sensitive;
  var paths = [].concat(path);
  return paths.reduce(function (matched, path) {
    if (!path && path !== "") return null;
    if (matched) return matched;

    var _compilePath = compilePath$1(path, {
      end: exact,
      strict: strict,
      sensitive: sensitive
    }),
        regexp = _compilePath.regexp,
        keys = _compilePath.keys;

    var match = regexp.exec(pathname);
    if (!match) return null;
    var url = match[0],
        values = match.slice(1);
    var isExact = pathname === url;
    if (exact && !isExact) return null;
    return {
      path: path,
      // the path used to match
      url: path === "/" && url === "" ? "/" : url,
      // the matched portion of the URL
      isExact: isExact,
      // whether or not we matched exactly
      params: keys.reduce(function (memo, key, index) {
        memo[key.name] = values[index];
        return memo;
      }, {})
    };
  }, null);
}

function isEmptyChildren(children) {
  return react__WEBPACK_IMPORTED_MODULE_1___default().Children.count(children) === 0;
}

function evalChildrenDev(children, props, path) {
  var value = children(props);
   true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(value !== undefined, "You returned `undefined` from the `children` function of " + ("<Route" + (path ? " path=\"" + path + "\"" : "") + ">, but you ") + "should have returned a React element or `null`") : 0;
  return value || null;
}
/**
 * The public API for matching a single path and rendering.
 */


var Route =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(Route, _React$Component);

  function Route() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Route.prototype;

  _proto.render = function render() {
    var _this = this;

    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Consumer, null, function (context$1) {
      !context$1 ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You should not use <Route> outside a <Router>") : 0 : void 0;
      var location = _this.props.location || context$1.location;
      var match = _this.props.computedMatch ? _this.props.computedMatch // <Switch> already computed the match for us
      : _this.props.path ? matchPath(location.pathname, _this.props) : context$1.match;

      var props = (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, context$1, {
        location: location,
        match: match
      });

      var _this$props = _this.props,
          children = _this$props.children,
          component = _this$props.component,
          render = _this$props.render; // Preact uses an empty array as children by
      // default, so use null if that's the case.

      if (Array.isArray(children) && children.length === 0) {
        children = null;
      }

      return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Provider, {
        value: props
      }, props.match ? children ? typeof children === "function" ?  true ? evalChildrenDev(children, props, _this.props.path) : 0 : children : component ? react__WEBPACK_IMPORTED_MODULE_1___default().createElement(component, props) : render ? render(props) : null : typeof children === "function" ?  true ? evalChildrenDev(children, props, _this.props.path) : 0 : null);
    });
  };

  return Route;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

if (true) {
  Route.propTypes = {
    children: prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().func), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().node)]),
    component: function component(props, propName) {
      if (props[propName] && !(0,react_is__WEBPACK_IMPORTED_MODULE_6__.isValidElementType)(props[propName])) {
        return new Error("Invalid prop 'component' supplied to 'Route': the prop is not a valid React component");
      }
    },
    exact: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().bool),
    location: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object),
    path: prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().string), prop_types__WEBPACK_IMPORTED_MODULE_2___default().arrayOf((prop_types__WEBPACK_IMPORTED_MODULE_2___default().string))]),
    render: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().func),
    sensitive: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().bool),
    strict: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().bool)
  };

  Route.prototype.componentDidMount = function () {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(this.props.children && !isEmptyChildren(this.props.children) && this.props.component), "You should not use <Route component> and <Route children> in the same route; <Route component> will be ignored") : 0;
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(this.props.children && !isEmptyChildren(this.props.children) && this.props.render), "You should not use <Route render> and <Route children> in the same route; <Route render> will be ignored") : 0;
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(this.props.component && this.props.render), "You should not use <Route component> and <Route render> in the same route; <Route render> will be ignored") : 0;
  };

  Route.prototype.componentDidUpdate = function (prevProps) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(this.props.location && !prevProps.location), '<Route> elements should not change from uncontrolled to controlled (or vice versa). You initially used no "location" prop and then provided one on a subsequent render.') : 0;
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(!this.props.location && prevProps.location), '<Route> elements should not change from controlled to uncontrolled (or vice versa). You provided a "location" prop initially but omitted it on a subsequent render.') : 0;
  };
}

function addLeadingSlash(path) {
  return path.charAt(0) === "/" ? path : "/" + path;
}

function addBasename(basename, location) {
  if (!basename) return location;
  return (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, location, {
    pathname: addLeadingSlash(basename) + location.pathname
  });
}

function stripBasename(basename, location) {
  if (!basename) return location;
  var base = addLeadingSlash(basename);
  if (location.pathname.indexOf(base) !== 0) return location;
  return (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, location, {
    pathname: location.pathname.substr(base.length)
  });
}

function createURL(location) {
  return typeof location === "string" ? location : (0,history__WEBPACK_IMPORTED_MODULE_10__.createPath)(location);
}

function staticHandler(methodName) {
  return function () {
      true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You cannot %s with <StaticRouter>", methodName) : 0 ;
  };
}

function noop() {}
/**
 * The public top-level API for a "static" <Router>, so-called because it
 * can't actually change the current location. Instead, it just records
 * location changes in a context object. Useful mainly in testing and
 * server-rendering scenarios.
 */


var StaticRouter =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(StaticRouter, _React$Component);

  function StaticRouter() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.handlePush = function (location) {
      return _this.navigateTo(location, "PUSH");
    };

    _this.handleReplace = function (location) {
      return _this.navigateTo(location, "REPLACE");
    };

    _this.handleListen = function () {
      return noop;
    };

    _this.handleBlock = function () {
      return noop;
    };

    return _this;
  }

  var _proto = StaticRouter.prototype;

  _proto.navigateTo = function navigateTo(location, action) {
    var _this$props = this.props,
        _this$props$basename = _this$props.basename,
        basename = _this$props$basename === void 0 ? "" : _this$props$basename,
        _this$props$context = _this$props.context,
        context = _this$props$context === void 0 ? {} : _this$props$context;
    context.action = action;
    context.location = addBasename(basename, (0,history__WEBPACK_IMPORTED_MODULE_10__.createLocation)(location));
    context.url = createURL(context.location);
  };

  _proto.render = function render() {
    var _this$props2 = this.props,
        _this$props2$basename = _this$props2.basename,
        basename = _this$props2$basename === void 0 ? "" : _this$props2$basename,
        _this$props2$context = _this$props2.context,
        context = _this$props2$context === void 0 ? {} : _this$props2$context,
        _this$props2$location = _this$props2.location,
        location = _this$props2$location === void 0 ? "/" : _this$props2$location,
        rest = (0,_babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_7__.default)(_this$props2, ["basename", "context", "location"]);

    var history = {
      createHref: function createHref(path) {
        return addLeadingSlash(basename + createURL(path));
      },
      action: "POP",
      location: stripBasename(basename, (0,history__WEBPACK_IMPORTED_MODULE_10__.createLocation)(location)),
      push: this.handlePush,
      replace: this.handleReplace,
      go: staticHandler("go"),
      goBack: staticHandler("goBack"),
      goForward: staticHandler("goForward"),
      listen: this.handleListen,
      block: this.handleBlock
    };
    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(Router, (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, rest, {
      history: history,
      staticContext: context
    }));
  };

  return StaticRouter;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

if (true) {
  StaticRouter.propTypes = {
    basename: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().string),
    context: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object),
    location: prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().string), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object)])
  };

  StaticRouter.prototype.componentDidMount = function () {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!this.props.history, "<StaticRouter> ignores the history prop. To use a custom history, " + "use `import { Router }` instead of `import { StaticRouter as Router }`.") : 0;
  };
}

/**
 * The public API for rendering the first <Route> that matches.
 */

var Switch =
/*#__PURE__*/
function (_React$Component) {
  (0,_babel_runtime_helpers_esm_inheritsLoose__WEBPACK_IMPORTED_MODULE_0__.default)(Switch, _React$Component);

  function Switch() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Switch.prototype;

  _proto.render = function render() {
    var _this = this;

    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Consumer, null, function (context) {
      !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You should not use <Switch> outside a <Router>") : 0 : void 0;
      var location = _this.props.location || context.location;
      var element, match; // We use React.Children.forEach instead of React.Children.toArray().find()
      // here because toArray adds keys to all child elements and we do not want
      // to trigger an unmount/remount for two <Route>s that render the same
      // component at different URLs.

      react__WEBPACK_IMPORTED_MODULE_1___default().Children.forEach(_this.props.children, function (child) {
        if (match == null && react__WEBPACK_IMPORTED_MODULE_1___default().isValidElement(child)) {
          element = child;
          var path = child.props.path || child.props.from;
          match = path ? matchPath(location.pathname, (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, child.props, {
            path: path
          })) : context.match;
        }
      });
      return match ? react__WEBPACK_IMPORTED_MODULE_1___default().cloneElement(element, {
        location: location,
        computedMatch: match
      }) : null;
    });
  };

  return Switch;
}((react__WEBPACK_IMPORTED_MODULE_1___default().Component));

if (true) {
  Switch.propTypes = {
    children: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().node),
    location: (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object)
  };

  Switch.prototype.componentDidUpdate = function (prevProps) {
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(this.props.location && !prevProps.location), '<Switch> elements should not change from uncontrolled to controlled (or vice versa). You initially used no "location" prop and then provided one on a subsequent render.') : 0;
     true ? (0,tiny_warning__WEBPACK_IMPORTED_MODULE_9__.default)(!(!this.props.location && prevProps.location), '<Switch> elements should not change from controlled to uncontrolled (or vice versa). You provided a "location" prop initially but omitted it on a subsequent render.') : 0;
  };
}

/**
 * A public higher-order component to access the imperative API
 */

function withRouter(Component) {
  var displayName = "withRouter(" + (Component.displayName || Component.name) + ")";

  var C = function C(props) {
    var wrappedComponentRef = props.wrappedComponentRef,
        remainingProps = (0,_babel_runtime_helpers_esm_objectWithoutPropertiesLoose__WEBPACK_IMPORTED_MODULE_7__.default)(props, ["wrappedComponentRef"]);

    return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(context.Consumer, null, function (context) {
      !context ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You should not use <" + displayName + " /> outside a <Router>") : 0 : void 0;
      return react__WEBPACK_IMPORTED_MODULE_1___default().createElement(Component, (0,_babel_runtime_helpers_esm_extends__WEBPACK_IMPORTED_MODULE_4__.default)({}, remainingProps, context, {
        ref: wrappedComponentRef
      }));
    });
  };

  C.displayName = displayName;
  C.WrappedComponent = Component;

  if (true) {
    C.propTypes = {
      wrappedComponentRef: prop_types__WEBPACK_IMPORTED_MODULE_2___default().oneOfType([(prop_types__WEBPACK_IMPORTED_MODULE_2___default().string), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().func), (prop_types__WEBPACK_IMPORTED_MODULE_2___default().object)])
    };
  }

  return hoist_non_react_statics__WEBPACK_IMPORTED_MODULE_8___default()(C, Component);
}

var useContext = (react__WEBPACK_IMPORTED_MODULE_1___default().useContext);
function useHistory() {
  if (true) {
    !(typeof useContext === "function") ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You must use React >= 16.8 in order to use useHistory()") : 0 : void 0;
  }

  return useContext(historyContext);
}
function useLocation() {
  if (true) {
    !(typeof useContext === "function") ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You must use React >= 16.8 in order to use useLocation()") : 0 : void 0;
  }

  return useContext(context).location;
}
function useParams() {
  if (true) {
    !(typeof useContext === "function") ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You must use React >= 16.8 in order to use useParams()") : 0 : void 0;
  }

  var match = useContext(context).match;
  return match ? match.params : {};
}
function useRouteMatch(path) {
  if (true) {
    !(typeof useContext === "function") ?  true ? (0,tiny_invariant__WEBPACK_IMPORTED_MODULE_11__.default)(false, "You must use React >= 16.8 in order to use useRouteMatch()") : 0 : void 0;
  }

  var location = useLocation();
  var match = useContext(context).match;
  return path ? matchPath(location.pathname, path) : match;
}

if (true) {
  if (typeof window !== "undefined") {
    var global = window;
    var key = "__react_router_build__";
    var buildNames = {
      cjs: "CommonJS",
      esm: "ES modules",
      umd: "UMD"
    };

    if (global[key] && global[key] !== "esm") {
      var initialBuildName = buildNames[global[key]];
      var secondaryBuildName = buildNames["esm"]; // TODO: Add link to article that explains in detail how to avoid
      // loading 2 different builds.

      throw new Error("You are loading the " + secondaryBuildName + " build of React Router " + ("on a page that is already running the " + initialBuildName + " ") + "build, so things won't work right.");
    }

    global[key] = "esm";
  }
}


//# sourceMappingURL=react-router.js.map


/***/ }),

/***/ "./node_modules/resolve-pathname/esm/resolve-pathname.js":
/*!***************************************************************!*\
  !*** ./node_modules/resolve-pathname/esm/resolve-pathname.js ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
function isAbsolute(pathname) {
  return pathname.charAt(0) === '/';
}

// About 1.5x faster than the two-arg version of Array#splice()
function spliceOne(list, index) {
  for (var i = index, k = i + 1, n = list.length; k < n; i += 1, k += 1) {
    list[i] = list[k];
  }

  list.pop();
}

// This implementation is based heavily on node's url.parse
function resolvePathname(to, from) {
  if (from === undefined) from = '';

  var toParts = (to && to.split('/')) || [];
  var fromParts = (from && from.split('/')) || [];

  var isToAbs = to && isAbsolute(to);
  var isFromAbs = from && isAbsolute(from);
  var mustEndAbs = isToAbs || isFromAbs;

  if (to && isAbsolute(to)) {
    // to is absolute
    fromParts = toParts;
  } else if (toParts.length) {
    // to is relative, drop the filename
    fromParts.pop();
    fromParts = fromParts.concat(toParts);
  }

  if (!fromParts.length) return '/';

  var hasTrailingSlash;
  if (fromParts.length) {
    var last = fromParts[fromParts.length - 1];
    hasTrailingSlash = last === '.' || last === '..' || last === '';
  } else {
    hasTrailingSlash = false;
  }

  var up = 0;
  for (var i = fromParts.length; i >= 0; i--) {
    var part = fromParts[i];

    if (part === '.') {
      spliceOne(fromParts, i);
    } else if (part === '..') {
      spliceOne(fromParts, i);
      up++;
    } else if (up) {
      spliceOne(fromParts, i);
      up--;
    }
  }

  if (!mustEndAbs) for (; up--; up) fromParts.unshift('..');

  if (
    mustEndAbs &&
    fromParts[0] !== '' &&
    (!fromParts[0] || !isAbsolute(fromParts[0]))
  )
    fromParts.unshift('');

  var result = fromParts.join('/');

  if (hasTrailingSlash && result.substr(-1) !== '/') result += '/';

  return result;
}

/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (resolvePathname);


/***/ }),

/***/ "./style/alerts.css":
/*!**************************!*\
  !*** ./style/alerts.css ***!
  \**************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_alerts_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./alerts.css */ "./node_modules/css-loader/dist/cjs.js!./style/alerts.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_alerts_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_alerts_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/auth.css":
/*!************************!*\
  !*** ./style/auth.css ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_auth_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./auth.css */ "./node_modules/css-loader/dist/cjs.js!./style/auth.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_auth_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_auth_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/body.css":
/*!************************!*\
  !*** ./style/body.css ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_body_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./body.css */ "./node_modules/css-loader/dist/cjs.js!./style/body.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_body_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_body_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/footer.css":
/*!**************************!*\
  !*** ./style/footer.css ***!
  \**************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_footer_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./footer.css */ "./node_modules/css-loader/dist/cjs.js!./style/footer.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_footer_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_footer_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/header.css":
/*!**************************!*\
  !*** ./style/header.css ***!
  \**************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_header_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./header.css */ "./node_modules/css-loader/dist/cjs.js!./style/header.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_header_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_header_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/main.css":
/*!************************!*\
  !*** ./style/main.css ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_main_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./main.css */ "./node_modules/css-loader/dist/cjs.js!./style/main.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_main_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_main_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/menu.css":
/*!************************!*\
  !*** ./style/menu.css ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_menu_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./menu.css */ "./node_modules/css-loader/dist/cjs.js!./style/menu.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_menu_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_menu_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/menu_app.css":
/*!****************************!*\
  !*** ./style/menu_app.css ***!
  \****************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_menu_app_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./menu_app.css */ "./node_modules/css-loader/dist/cjs.js!./style/menu_app.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_menu_app_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_menu_app_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/menu_app_one_card.css":
/*!*************************************!*\
  !*** ./style/menu_app_one_card.css ***!
  \*************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_menu_app_one_card_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./menu_app_one_card.css */ "./node_modules/css-loader/dist/cjs.js!./style/menu_app_one_card.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_menu_app_one_card_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_menu_app_one_card_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/planing_poker.css":
/*!*********************************!*\
  !*** ./style/planing_poker.css ***!
  \*********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_planing_poker_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./planing_poker.css */ "./node_modules/css-loader/dist/cjs.js!./style/planing_poker.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_planing_poker_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_planing_poker_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./style/word_cards.css":
/*!******************************!*\
  !*** ./style/word_cards.css ***!
  \******************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! !../node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js */ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js");
/* harmony import */ var _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _node_modules_css_loader_dist_cjs_js_word_cards_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! !!../node_modules/css-loader/dist/cjs.js!./word_cards.css */ "./node_modules/css-loader/dist/cjs.js!./style/word_cards.css");

            

var options = {};

options.insert = "head";
options.singleton = false;

var update = _node_modules_style_loader_dist_runtime_injectStylesIntoStyleTag_js__WEBPACK_IMPORTED_MODULE_0___default()(_node_modules_css_loader_dist_cjs_js_word_cards_css__WEBPACK_IMPORTED_MODULE_1__.default, options);



/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (_node_modules_css_loader_dist_cjs_js_word_cards_css__WEBPACK_IMPORTED_MODULE_1__.default.locals || {});

/***/ }),

/***/ "./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js":
/*!****************************************************************************!*\
  !*** ./node_modules/style-loader/dist/runtime/injectStylesIntoStyleTag.js ***!
  \****************************************************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";


var isOldIE = function isOldIE() {
  var memo;
  return function memorize() {
    if (typeof memo === 'undefined') {
      // Test for IE <= 9 as proposed by Browserhacks
      // @see http://browserhacks.com/#hack-e71d8692f65334173fee715c222cb805
      // Tests for existence of standard globals is to allow style-loader
      // to operate correctly into non-standard environments
      // @see https://github.com/webpack-contrib/style-loader/issues/177
      memo = Boolean(window && document && document.all && !window.atob);
    }

    return memo;
  };
}();

var getTarget = function getTarget() {
  var memo = {};
  return function memorize(target) {
    if (typeof memo[target] === 'undefined') {
      var styleTarget = document.querySelector(target); // Special case to return head of iframe instead of iframe itself

      if (window.HTMLIFrameElement && styleTarget instanceof window.HTMLIFrameElement) {
        try {
          // This will throw an exception if access to iframe is blocked
          // due to cross-origin restrictions
          styleTarget = styleTarget.contentDocument.head;
        } catch (e) {
          // istanbul ignore next
          styleTarget = null;
        }
      }

      memo[target] = styleTarget;
    }

    return memo[target];
  };
}();

var stylesInDom = [];

function getIndexByIdentifier(identifier) {
  var result = -1;

  for (var i = 0; i < stylesInDom.length; i++) {
    if (stylesInDom[i].identifier === identifier) {
      result = i;
      break;
    }
  }

  return result;
}

function modulesToDom(list, options) {
  var idCountMap = {};
  var identifiers = [];

  for (var i = 0; i < list.length; i++) {
    var item = list[i];
    var id = options.base ? item[0] + options.base : item[0];
    var count = idCountMap[id] || 0;
    var identifier = "".concat(id, " ").concat(count);
    idCountMap[id] = count + 1;
    var index = getIndexByIdentifier(identifier);
    var obj = {
      css: item[1],
      media: item[2],
      sourceMap: item[3]
    };

    if (index !== -1) {
      stylesInDom[index].references++;
      stylesInDom[index].updater(obj);
    } else {
      stylesInDom.push({
        identifier: identifier,
        updater: addStyle(obj, options),
        references: 1
      });
    }

    identifiers.push(identifier);
  }

  return identifiers;
}

function insertStyleElement(options) {
  var style = document.createElement('style');
  var attributes = options.attributes || {};

  if (typeof attributes.nonce === 'undefined') {
    var nonce =  true ? __webpack_require__.nc : 0;

    if (nonce) {
      attributes.nonce = nonce;
    }
  }

  Object.keys(attributes).forEach(function (key) {
    style.setAttribute(key, attributes[key]);
  });

  if (typeof options.insert === 'function') {
    options.insert(style);
  } else {
    var target = getTarget(options.insert || 'head');

    if (!target) {
      throw new Error("Couldn't find a style target. This probably means that the value for the 'insert' parameter is invalid.");
    }

    target.appendChild(style);
  }

  return style;
}

function removeStyleElement(style) {
  // istanbul ignore if
  if (style.parentNode === null) {
    return false;
  }

  style.parentNode.removeChild(style);
}
/* istanbul ignore next  */


var replaceText = function replaceText() {
  var textStore = [];
  return function replace(index, replacement) {
    textStore[index] = replacement;
    return textStore.filter(Boolean).join('\n');
  };
}();

function applyToSingletonTag(style, index, remove, obj) {
  var css = remove ? '' : obj.media ? "@media ".concat(obj.media, " {").concat(obj.css, "}") : obj.css; // For old IE

  /* istanbul ignore if  */

  if (style.styleSheet) {
    style.styleSheet.cssText = replaceText(index, css);
  } else {
    var cssNode = document.createTextNode(css);
    var childNodes = style.childNodes;

    if (childNodes[index]) {
      style.removeChild(childNodes[index]);
    }

    if (childNodes.length) {
      style.insertBefore(cssNode, childNodes[index]);
    } else {
      style.appendChild(cssNode);
    }
  }
}

function applyToTag(style, options, obj) {
  var css = obj.css;
  var media = obj.media;
  var sourceMap = obj.sourceMap;

  if (media) {
    style.setAttribute('media', media);
  } else {
    style.removeAttribute('media');
  }

  if (sourceMap && typeof btoa !== 'undefined') {
    css += "\n/*# sourceMappingURL=data:application/json;base64,".concat(btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap)))), " */");
  } // For old IE

  /* istanbul ignore if  */


  if (style.styleSheet) {
    style.styleSheet.cssText = css;
  } else {
    while (style.firstChild) {
      style.removeChild(style.firstChild);
    }

    style.appendChild(document.createTextNode(css));
  }
}

var singleton = null;
var singletonCounter = 0;

function addStyle(obj, options) {
  var style;
  var update;
  var remove;

  if (options.singleton) {
    var styleIndex = singletonCounter++;
    style = singleton || (singleton = insertStyleElement(options));
    update = applyToSingletonTag.bind(null, style, styleIndex, false);
    remove = applyToSingletonTag.bind(null, style, styleIndex, true);
  } else {
    style = insertStyleElement(options);
    update = applyToTag.bind(null, style, options);

    remove = function remove() {
      removeStyleElement(style);
    };
  }

  update(obj);
  return function updateStyle(newObj) {
    if (newObj) {
      if (newObj.css === obj.css && newObj.media === obj.media && newObj.sourceMap === obj.sourceMap) {
        return;
      }

      update(obj = newObj);
    } else {
      remove();
    }
  };
}

module.exports = function (list, options) {
  options = options || {}; // Force single-tag solution on IE6-9, which has a hard limit on the # of <style>
  // tags it will allow on a page

  if (!options.singleton && typeof options.singleton !== 'boolean') {
    options.singleton = isOldIE();
  }

  list = list || [];
  var lastIdentifiers = modulesToDom(list, options);
  return function update(newList) {
    newList = newList || [];

    if (Object.prototype.toString.call(newList) !== '[object Array]') {
      return;
    }

    for (var i = 0; i < lastIdentifiers.length; i++) {
      var identifier = lastIdentifiers[i];
      var index = getIndexByIdentifier(identifier);
      stylesInDom[index].references--;
    }

    var newLastIdentifiers = modulesToDom(newList, options);

    for (var _i = 0; _i < lastIdentifiers.length; _i++) {
      var _identifier = lastIdentifiers[_i];

      var _index = getIndexByIdentifier(_identifier);

      if (stylesInDom[_index].references === 0) {
        stylesInDom[_index].updater();

        stylesInDom.splice(_index, 1);
      }
    }

    lastIdentifiers = newLastIdentifiers;
  };
};

/***/ }),

/***/ "./node_modules/tiny-invariant/dist/tiny-invariant.esm.js":
/*!****************************************************************!*\
  !*** ./node_modules/tiny-invariant/dist/tiny-invariant.esm.js ***!
  \****************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
var isProduction = "development" === 'production';
var prefix = 'Invariant failed';
function invariant(condition, message) {
    if (condition) {
        return;
    }
    if (isProduction) {
        throw new Error(prefix);
    }
    throw new Error(prefix + ": " + (message || ''));
}

/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (invariant);


/***/ }),

/***/ "./node_modules/tiny-warning/dist/tiny-warning.esm.js":
/*!************************************************************!*\
  !*** ./node_modules/tiny-warning/dist/tiny-warning.esm.js ***!
  \************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
var isProduction = "development" === 'production';
function warning(condition, message) {
  if (!isProduction) {
    if (condition) {
      return;
    }

    var text = "Warning: " + message;

    if (typeof console !== 'undefined') {
      console.warn(text);
    }

    try {
      throw Error(text);
    } catch (x) {}
  }
}

/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (warning);


/***/ }),

/***/ "./src/components/Alerts/MainAlertAbsolute.tsx":
/*!*****************************************************!*\
  !*** ./src/components/Alerts/MainAlertAbsolute.tsx ***!
  \*****************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MainAlertAbsolute = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneAlert_1 = __webpack_require__(/*! ./OneAlert */ "./src/components/Alerts/OneAlert.tsx");
var MainAlertAbsolute = /** @class */ (function (_super) {
    __extends(MainAlertAbsolute, _super);
    function MainAlertAbsolute(props) {
        return _super.call(this, props) || this;
    }
    MainAlertAbsolute.prototype.render = function () {
        var _this = this;
        if (!this.props.Data) {
            return React.createElement("div", { className: 'main-alert-section' });
        }
        // return <input placeholder="Поиск" onChange={this.onTextChanged} />;
        return React.createElement("div", { className: 'main-alert-section' }, this.props.Data.map(function (el) {
            return React.createElement(OneAlert_1.OneAlert, { Data: el, RemoveALert: _this.props.RemoveALert, key: 'absolute_alert_' + el.Key });
        }));
    };
    return MainAlertAbsolute;
}(React.Component));
exports.MainAlertAbsolute = MainAlertAbsolute;


/***/ }),

/***/ "./src/components/Alerts/OneAlert.tsx":
/*!********************************************!*\
  !*** ./src/components/Alerts/OneAlert.tsx ***!
  \********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneAlert = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var AlertData_1 = __webpack_require__(/*! ../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var OneAlert = /** @class */ (function (_super) {
    __extends(OneAlert, _super);
    function OneAlert(props) {
        var _this = _super.call(this, props) || this;
        _this.Remove = _this.Remove.bind(_this);
        return _this;
    }
    OneAlert.prototype.Remove = function () {
        this.props.RemoveALert(this.props.Data.Key);
    };
    OneAlert.prototype.render = function () {
        var alertClassDiv = "alert-danger";
        var alertName = "Error";
        if (this.props.Data.Type == AlertData_1.AlertTypeEnum.Success) {
            alertClassDiv = "alert-success";
            alertName = "Success";
        }
        // return <input placeholder="Поиск" onChange={this.onTextChanged} />;
        return React.createElement("div", { className: 'absolute-one-alert' },
            React.createElement("div", { className: "alert " + alertClassDiv + " alert-dismissible fade show", role: "alert" },
                React.createElement("strong", null, alertName),
                " ",
                this.props.Data.Text,
                React.createElement("button", { onClick: this.Remove, type: "button", className: "close", "aria-label": "Close" },
                    React.createElement("span", { "aria-hidden": "true" }, "\u00D7"))));
    };
    return OneAlert;
}(React.Component));
exports.OneAlert = OneAlert;


/***/ }),

/***/ "./src/components/AppRouter.tsx":
/*!**************************************!*\
  !*** ./src/components/AppRouter.tsx ***!
  \**************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AppRouter = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var MainAuth_1 = __webpack_require__(/*! ./Body/Auth/MainAuth */ "./src/components/Body/Auth/MainAuth.tsx");
// export interface IHeaderLogoProps {
// }
// import {
//     BrowserRouter as Router,
//     Switch,
//     Route,
//     Link
// } from "react-router-dom";
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
// import { BodyMain } from "./Body/BodyMain";
var MenuMain_1 = __webpack_require__(/*! ./Body/Menu/MenuMain */ "./src/components/Body/Menu/MenuMain.tsx");
var MenuAppMain_1 = __webpack_require__(/*! ./Body/MenuApp/MenuAppMain */ "./src/components/Body/MenuApp/MenuAppMain.tsx");
var WordsCardsAppMain_1 = __webpack_require__(/*! ./Body/WordsCardsApp/WordsCardsAppMain */ "./src/components/Body/WordsCardsApp/WordsCardsAppMain.tsx");
var PlaningPokerMain_1 = __importDefault(__webpack_require__(/*! ./Body/PlaningPoker/PlaningPokerMain */ "./src/components/Body/PlaningPoker/PlaningPokerMain.tsx"));
var AppRouter = /** @class */ (function (_super) {
    __extends(AppRouter, _super);
    function AppRouter(props) {
        return _super.call(this, props) || this;
    }
    AppRouter.prototype.render = function () {
        // return <MainAuth login={true}/>
        // return <BodyCardsListMain />
        //TODO попробовать достучаться незалогиненным по ссылкам и поправить то что вылезет
        // return <BodyCardsListMain/> 
        return React.createElement(react_router_dom_1.Switch, null,
            React.createElement(react_router_dom_1.Route, { exact: true, path: "/menu", component: MenuMain_1.MenuMain }),
            React.createElement(react_router_dom_1.Route, { path: "/menu-app/", component: MenuAppMain_1.MenuAppMain }),
            React.createElement(react_router_dom_1.Route, { path: "/words-cards-app", component: WordsCardsAppMain_1.WordsCardsAppMain }),
            React.createElement(react_router_dom_1.Route, { path: "/planing-poker", component: PlaningPokerMain_1.default }),
            React.createElement(react_router_dom_1.Route, { path: "/menu/auth/login", render: function (props) { return (React.createElement(MainAuth_1.MainAuth, __assign({}, props, { LoginPage: true }))); } }),
            React.createElement(react_router_dom_1.Route, { path: "/menu/auth/register", render: function (props) { return (React.createElement(MainAuth_1.MainAuth, __assign({}, props, { LoginPage: false }))); } }));
    };
    return AppRouter;
}(React.Component));
exports.AppRouter = AppRouter;
// </helloprops>


/***/ }),

/***/ "./src/components/Body/Auth/Login.tsx":
/*!********************************************!*\
  !*** ./src/components/Body/Auth/Login.tsx ***!
  \********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.Login = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var Login = /** @class */ (function (_super) {
    __extends(Login, _super);
    function Login(props) {
        var _this = _super.call(this, props) || this;
        var newState = {
            Login: null,
            Password: null,
        };
        _this.state = newState;
        _this.LoginOnChange = _this.LoginOnChange.bind(_this);
        _this.PasswordOnChange = _this.PasswordOnChange.bind(_this);
        _this.TryLogin = _this.TryLogin.bind(_this);
        return _this;
    }
    Login.prototype.LoginOnChange = function (e) {
        var newLogin = e.target.value.trim();
        var newState = __assign({}, this.state); //Object.assign({}, this.state);
        newState.Login = newLogin;
        this.setState(newState);
    };
    Login.prototype.PasswordOnChange = function (e) {
        var newPassword = e.target.value.trim();
        var newState = __assign({}, this.state); //Object.assign({}, this.state);
        newState.Password = newPassword;
        this.setState(newState);
    };
    Login.prototype.TryLogin = function () {
        return __awaiter(this, void 0, void 0, function () {
            var data, onSuccess;
            return __generator(this, function (_a) {
                data = {
                    Email: this.state.Login,
                    Password: this.state.Password,
                };
                onSuccess = function (error) {
                    if (!error) {
                        document.location.href = "/menu";
                    }
                };
                window.G_AuthenticateController.Login(data, onSuccess);
                return [2 /*return*/];
            });
        });
    };
    Login.prototype.render = function () {
        return React.createElement("div", { className: 'persent-100-width' },
            React.createElement("div", { className: 'persent-100-width' },
                React.createElement("div", { className: 'persent-100-width padding-10-top' },
                    React.createElement("input", { className: 'form-control persent-100-width', type: 'text', placeholder: 'email', onChange: this.LoginOnChange })),
                React.createElement("div", { className: 'persent-100-width padding-10-top' },
                    React.createElement("input", { className: 'form-control persent-100-width', type: 'password', placeholder: 'password', onChange: this.PasswordOnChange })),
                React.createElement("button", { className: 'btn persent-100-width', onClick: this.TryLogin }, "\u0412\u043E\u0439\u0442\u0438")));
    };
    return Login;
}(React.Component));
exports.Login = Login;


/***/ }),

/***/ "./src/components/Body/Auth/MainAuth.tsx":
/*!***********************************************!*\
  !*** ./src/components/Body/Auth/MainAuth.tsx ***!
  \***********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MainAuth = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var Login_1 = __webpack_require__(/*! ./Login */ "./src/components/Body/Auth/Login.tsx");
var Register_1 = __webpack_require__(/*! ./Register */ "./src/components/Body/Auth/Register.tsx");
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var MainAuth = /** @class */ (function (_super) {
    __extends(MainAuth, _super);
    function MainAuth(props) {
        var _this = _super.call(this, props) || this;
        _this.SwithLogic = _this.SwithLogic.bind(_this);
        _this.Switcher = _this.Switcher.bind(_this);
        return _this;
    }
    MainAuth.prototype.SwithLogic = function () {
        // console.log(this.props);
        if (this.props.LoginPage) {
            return React.createElement(Login_1.Login, null);
        }
        else {
            return React.createElement(Register_1.Register, null);
        }
    };
    MainAuth.prototype.Switcher = function () {
        return React.createElement("div", { className: 'row auth-switcher' },
            React.createElement("div", { className: 'col-sm-6' },
                React.createElement(react_router_dom_1.Link, { className: ('auth-switcher-link btn' + (this.props.LoginPage ? ' btn-primary' : ' btn-light')), to: "/menu/auth/login/" }, "\u0412\u0445\u043E\u0434")),
            React.createElement("div", { className: 'col-sm-6' },
                React.createElement(react_router_dom_1.Link, { className: ('auth-switcher-link btn' + (!this.props.LoginPage ? ' btn-primary' : ' btn-light')), to: "/menu/auth/register/" }, "\u0420\u0435\u0433\u0438\u0441\u0442\u0430\u0440\u0446\u0438\u044F")));
    };
    MainAuth.prototype.render = function () {
        return React.createElement("div", { className: 'main-auth-container' },
            React.createElement("div", { className: 'auth-container-inner col-sm-6 col-md-5 col-lg-4 offset-sm-3 offset-lg-4' },
                this.Switcher(),
                this.SwithLogic()));
    };
    return MainAuth;
}(React.Component));
exports.MainAuth = MainAuth;


/***/ }),

/***/ "./src/components/Body/Auth/Register.tsx":
/*!***********************************************!*\
  !*** ./src/components/Body/Auth/Register.tsx ***!
  \***********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.Register = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var Register = /** @class */ (function (_super) {
    __extends(Register, _super);
    function Register(props) {
        var _this = _super.call(this, props) || this;
        var newState = {
            Login: null,
            Password: null,
            ConfirmPassword: null,
        };
        _this.state = newState;
        _this.LoginOnChange = _this.LoginOnChange.bind(_this);
        _this.PasswordOnChange = _this.PasswordOnChange.bind(_this);
        _this.ConfirmPasswordOnChange = _this.ConfirmPasswordOnChange.bind(_this);
        _this.TryRegister = _this.TryRegister.bind(_this);
        return _this;
    }
    Register.prototype.LoginOnChange = function (e) {
        var newLogin = e.target.value.trim();
        var newState = __assign({}, this.state);
        newState.Login = newLogin;
        this.setState(newState);
    };
    Register.prototype.PasswordOnChange = function (e) {
        var newPassword = e.target.value.trim();
        // let newState = Object.assign({}, this.state);
        var newState = __assign({}, this.state);
        newState.Password = newPassword;
        this.setState(newState);
    };
    Register.prototype.ConfirmPasswordOnChange = function (e) {
        var newPassword = e.target.value.trim();
        var newState = __assign({}, this.state);
        newState.ConfirmPassword = newPassword;
        this.setState(newState);
    };
    Register.prototype.TryRegister = function () {
        //TODO отправляем запрос и чистим state?
        var data = {
            Email: this.state.Login,
            Password: this.state.Password,
            ConfirmPassword: this.state.ConfirmPassword,
        };
        var onSuccess = function (error) {
            if (!error) {
                document.location.href = "/menu";
            }
        };
        window.G_AuthenticateController.Register(data, onSuccess);
        // let data = {
        //     'email': this.state.Login,
        //     'password': this.state.Password,
        //     "password_confirm": this.state.ConfirmPassword,
        // };
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //         }
        //         else {
        //             //TODO записываем полученные токены
        //             document.location.href = "/menu";
        //         }
        //     },
        //     Url: G_PathToServer + 'api/authenticate/register',
        // });
    };
    //style={{align:"center"}}
    Register.prototype.render = function () {
        return React.createElement("div", { className: 'persent-100-width' },
            React.createElement("div", { className: 'persent-100-width' },
                React.createElement("div", { className: 'persent-100-width padding-10-top' },
                    React.createElement("input", { className: 'form-control persent-100-width', type: 'text', placeholder: 'email', onChange: this.LoginOnChange })),
                React.createElement("div", { className: 'persent-100-width padding-10-top' },
                    React.createElement("input", { className: 'form-control persent-100-width', type: 'password', placeholder: 'password', onChange: this.PasswordOnChange })),
                React.createElement("div", { className: 'persent-100-width padding-10-top' },
                    React.createElement("input", { className: 'form-control persent-100-width', type: 'password', placeholder: 'confirm password', onChange: this.ConfirmPasswordOnChange })),
                React.createElement("button", { className: 'btn persent-100-width', onClick: this.TryRegister }, "\u0417\u0430\u0440\u0435\u0433\u0438\u0441\u0442\u0440\u0438\u0440\u043E\u0432\u0430\u0442\u044C\u0441\u044F")));
    };
    return Register;
}(React.Component));
exports.Register = Register;


/***/ }),

/***/ "./src/components/Body/MenuApp/CardsList/BodyCardsListMain.tsx":
/*!*********************************************************************!*\
  !*** ./src/components/Body/MenuApp/CardsList/BodyCardsListMain.tsx ***!
  \*********************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../../../../../typings/globals.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.BodyCardsListMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var MenuCardList_1 = __webpack_require__(/*! ./MenuCardList */ "./src/components/Body/MenuApp/CardsList/MenuCardList.tsx");
var CardsFilters_1 = __webpack_require__(/*! ./CardsFilters */ "./src/components/Body/MenuApp/CardsList/CardsFilters.tsx");
var OneCardInListData_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/MenuApp/OneCardInListData */ "./src/components/_ComponentsLink/Models/MenuApp/OneCardInListData.ts");
var BodyCardsListMain = /** @class */ (function (_super) {
    __extends(BodyCardsListMain, _super);
    function BodyCardsListMain(props) {
        var _this = _super.call(this, props) || this;
        // this.onTextChanged = this.onTextChanged.bind(this);
        _this.state = {
            AllCardsData: [],
            NewCardTemplate: false,
            CardsListFilters: {
                FollowOnly: false,
            },
            EmptyImagePath: G_EmptyImagePath,
            CardsLoaded: false,
        };
        _this.UpdateElement = _this.UpdateElement.bind(_this);
        _this.FollowRequstSuccess = _this.FollowRequstSuccess.bind(_this);
        _this.ShowCreateTemplate = _this.ShowCreateTemplate.bind(_this);
        _this.ChangeFilterFollow = _this.ChangeFilterFollow.bind(_this);
        _this.RenderCardsOrPreloader = _this.RenderCardsOrPreloader.bind(_this);
        _this.RemoveNewTemplate = _this.RemoveNewTemplate.bind(_this);
        return _this;
    }
    BodyCardsListMain.prototype.componentDidMount = function () {
        return __awaiter(this, void 0, void 0, function () {
            var refThis, success;
            return __generator(this, function (_a) {
                refThis = this;
                success = function (error, data) {
                    if (error || !data) {
                        return;
                    }
                    var dataFront = [];
                    data.forEach(function (bk) {
                        dataFront.push(new OneCardInListData_1.OneCardInListData(bk));
                    });
                    refThis.setState({
                        AllCardsData: dataFront,
                        CardsLoaded: true,
                        // FollowedCards: followed,
                        // NotFollowedCards: notFollowed,
                    });
                };
                window.G_ArticleController.GetAllShortForUser(success);
                return [2 /*return*/];
            });
        });
    };
    ///add or update
    BodyCardsListMain.prototype.UpdateElement = function (newElement, isNew) {
        if (isNew) {
            var newState = __assign({}, this.state);
            // let elForAdd = new OneCardInListData();
            // elForAdd.FillByBackModel(newEl);
            newState.AllCardsData.push(newElement);
            newState.NewCardTemplate = false;
            this.setState(newState);
        }
        else {
            var newState = __assign({}, this.state);
            for (var i = 0; i < newState.AllCardsData.length; ++i) {
                if (newState.AllCardsData[i].Id == newElement.Id) {
                    newState.AllCardsData[i] = newElement;
                    this.setState(newState);
                    return;
                }
            }
        }
    };
    BodyCardsListMain.prototype.RemoveNewTemplate = function () {
        var newState = __assign({}, this.state);
        newState.NewCardTemplate = false;
        this.setState(newState);
    };
    BodyCardsListMain.prototype.FollowRequstSuccess = function (id, result) {
        for (var i = 0; i < this.state.AllCardsData.length; ++i) {
            if (this.state.AllCardsData[i].Id == id) {
                var newState = __assign({}, this.state);
                newState.AllCardsData[i].Followed = result;
                this.setState(newState);
                return;
            }
        }
    };
    BodyCardsListMain.prototype.ShowCreateTemplate = function () {
        if (this.state.NewCardTemplate) {
            return;
        }
        var newState = __assign({}, this.state);
        newState.NewCardTemplate = true;
        this.setState(newState);
    };
    BodyCardsListMain.prototype.ChangeFilterFollow = function (e) {
        // e.persist();
        var newState = __assign({}, this.state);
        newState.CardsListFilters.FollowOnly = !newState.CardsListFilters.FollowOnly;
        this.setState(newState);
    };
    BodyCardsListMain.prototype.RenderCardsOrPreloader = function () {
        if (this.state.CardsLoaded) {
            return React.createElement("div", { className: 'card-list-body' },
                React.createElement(CardsFilters_1.CardsFilters, { FollowOnly: this.state.CardsListFilters.FollowOnly, FollowOnlyChanged: this.ChangeFilterFollow }),
                React.createElement("p", null,
                    React.createElement("button", { className: "btn btn-primary", type: "button", "data-toggle": "collapse", "data-target": "#body-all-cards-section", "aria-expanded": "false", "aria-controls": "body-all-cards-section" }, "\u041A\u0430\u0440\u0442\u043E\u0447\u043A\u0438")),
                React.createElement("div", null,
                    React.createElement("div", { className: "collapse", id: "body-all-cards-section" },
                        React.createElement("div", { className: "card card-body" },
                            React.createElement("div", null,
                                React.createElement("button", { className: 'btn btn-primary', onClick: this.ShowCreateTemplate }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C")),
                            React.createElement(MenuCardList_1.MenuCardList, { CardFilters: this.state.CardsListFilters, NewCardTemplate: this.state.NewCardTemplate, CardsList: this.state.AllCardsData, UpdateElement: this.UpdateElement, FollowRequstSuccess: this.FollowRequstSuccess, RemoveNewTemplate: this.RemoveNewTemplate })))));
        }
        else {
            return React.createElement("div", { className: 'card-list-preloader' },
                React.createElement("div", { className: "spinner-border persent-100-width-height", role: "status" },
                    React.createElement("span", { className: "sr-only" }, "Loading...")));
        }
    };
    BodyCardsListMain.prototype.render = function () {
        // return <input placeholder="Поиск" onChange={this.onTextChanged} />;
        return React.createElement("div", { className: 'main-body container' }, this.RenderCardsOrPreloader());
    };
    return BodyCardsListMain;
}(React.Component));
exports.BodyCardsListMain = BodyCardsListMain;
// </helloprops>


/***/ }),

/***/ "./src/components/Body/MenuApp/CardsList/CardsFilters.tsx":
/*!****************************************************************!*\
  !*** ./src/components/Body/MenuApp/CardsList/CardsFilters.tsx ***!
  \****************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.CardsFilters = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var CardsFilters = /** @class */ (function (_super) {
    __extends(CardsFilters, _super);
    function CardsFilters(props) {
        return _super.call(this, props) || this;
    }
    CardsFilters.prototype.render = function () {
        return React.createElement("div", null,
            React.createElement("p", null, "\u0424\u0438\u043B\u044C\u0442\u0440\u044B"),
            React.createElement("div", null,
                React.createElement("input", { type: "checkbox", defaultChecked: this.props.FollowOnly, onChange: this.props.FollowOnlyChanged })));
    };
    return CardsFilters;
}(React.Component));
exports.CardsFilters = CardsFilters;
// </helloprops>


/***/ }),

/***/ "./src/components/Body/MenuApp/CardsList/MenuCardList.tsx":
/*!****************************************************************!*\
  !*** ./src/components/Body/MenuApp/CardsList/MenuCardList.tsx ***!
  \****************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MenuCardList = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneMenuCard_1 = __webpack_require__(/*! ./OneMenuCard */ "./src/components/Body/MenuApp/CardsList/OneMenuCard.tsx");
var MenuCardList = /** @class */ (function (_super) {
    __extends(MenuCardList, _super);
    function MenuCardList(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {};
        _this.RenderCreateTemplate = _this.RenderCreateTemplate.bind(_this);
        _this.RenderCardByFilters = _this.RenderCardByFilters.bind(_this);
        _this.RenderListByFilters = _this.RenderListByFilters.bind(_this);
        return _this;
    }
    MenuCardList.prototype.RenderCreateTemplate = function () {
        if (this.props.NewCardTemplate) {
            return React.createElement(OneMenuCard_1.OneMenuCard, { key: -1, IsNewTemplate: true, UpdateElement: this.props.UpdateElement, RemoveNewTemplate: this.props.RemoveNewTemplate });
        }
        return React.createElement("div", null);
    };
    MenuCardList.prototype.RenderCardByFilters = function (item) {
        if (!this.props.CardFilters.FollowOnly || item.Followed) {
            return React.createElement(OneMenuCard_1.OneMenuCard, { key: item.Id, CardData: item, UpdateElement: this.props.UpdateElement, FollowRequstSuccess: this.props.FollowRequstSuccess });
        }
        // console.log(this.props);
        return null;
    };
    MenuCardList.prototype.RenderListByFilters = function () {
        var _this = this;
        return this.props.CardsList.map(function (item) { return _this.RenderCardByFilters(item); });
    };
    MenuCardList.prototype.render = function () {
        return React.createElement("div", { className: 'row' },
            this.RenderCreateTemplate(),
            this.RenderListByFilters());
    };
    return MenuCardList;
}(React.Component));
exports.MenuCardList = MenuCardList;


/***/ }),

/***/ "./src/components/Body/MenuApp/CardsList/OneMenuCard.tsx":
/*!***************************************************************!*\
  !*** ./src/components/Body/MenuApp/CardsList/OneMenuCard.tsx ***!
  \***************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../../../../../typings/globals.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneMenuCard = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneCardInListData_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/MenuApp/OneCardInListData */ "./src/components/_ComponentsLink/Models/MenuApp/OneCardInListData.ts");
// export interface IHeaderLogoProps {
// }
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var OneCardInListDataBack_1 = __webpack_require__(/*! ../../../_ComponentsLink/BackModel/MenuApp/OneCardInListDataBack */ "./src/components/_ComponentsLink/BackModel/MenuApp/OneCardInListDataBack.ts");
var OneMenuCard = /** @class */ (function (_super) {
    __extends(OneMenuCard, _super);
    function OneMenuCard(props) {
        var _this = _super.call(this, props) || this;
        // this.onTextChanged = this.onTextChanged.bind(this);
        // this.state = props.data;
        _this.state = {
            EditNow: false,
            NewState: null,
        };
        _this.ActionButton = _this.ActionButton.bind(_this);
        _this.EditOnClick = _this.EditOnClick.bind(_this);
        _this.SaveOnClick = _this.SaveOnClick.bind(_this);
        _this.FollowCard = _this.FollowCard.bind(_this);
        _this.CancelEditOnClick = _this.CancelEditOnClick.bind(_this);
        _this.TitleRender = _this.TitleRender.bind(_this);
        _this.BodyRender = _this.BodyRender.bind(_this);
        _this.ImageRender = _this.ImageRender.bind(_this);
        _this.TitleOnChange = _this.TitleOnChange.bind(_this);
        _this.BodyOnChange = _this.BodyOnChange.bind(_this);
        _this.RenderFollowButton = _this.RenderFollowButton.bind(_this);
        _this.RenderFollowIcon = _this.RenderFollowIcon.bind(_this);
        return _this;
        // this.bodyRender = this.bodyRender.bind(this);
        // this.bodyRender = this.bodyRender.bind(this);
    }
    OneMenuCard.prototype.componentDidMount = function () {
        // console.log(JSON.stringify(this.props));
        if (this.props.IsNewTemplate && !this.state.NewState) {
            var newState = __assign({}, this.state);
            newState.EditNow = true;
            newState.NewState = {
                Id: -1,
                Title: 'Новая',
                Body: 'Новая',
                Image: G_EmptyImagePath,
                Followed: false,
            }; //TODO надо проверить сломается что то или нет,
            this.setState(newState);
        }
    };
    OneMenuCard.prototype.RenderFollowButton = function () {
        if (this.props.CardData && this.props.CardData.Id > 0) {
            return React.createElement("div", { className: 'follow-one-card-button one-card-button', onClick: this.FollowCard }, this.RenderFollowIcon());
        }
        return React.createElement("div", null);
    };
    OneMenuCard.prototype.RenderFollowIcon = function () {
        // return <div></div>
        var imgPath = G_PathToBaseImages;
        if (this.props.CardData.Followed) {
            imgPath += 'red_heart.png';
        }
        else {
            imgPath += 'white_heart.jpg';
        }
        return React.createElement("img", { className: 'persent-100-width-height', src: imgPath, alt: "Follow" });
    };
    OneMenuCard.prototype.ActionButton = function () {
        if (this.state.EditNow) {
            return React.createElement("div", null,
                React.createElement("div", { className: 'cancel-edit-one-card-button one-card-button', onClick: this.CancelEditOnClick },
                    React.createElement("img", { className: 'persent-100-width-height', src: G_PathToBaseImages + 'cancel.png', alt: "Cancel" })),
                React.createElement("div", { className: 'save-one-card-button one-card-button', onClick: this.SaveOnClick },
                    React.createElement("img", { className: 'persent-100-width-height', src: G_PathToBaseImages + 'save-icon.png', alt: "Save" })));
        }
        else {
            return React.createElement("div", null,
                React.createElement("div", { className: 'edit-one-card-button one-card-button', onClick: this.EditOnClick },
                    React.createElement("img", { className: 'persent-100-width-height', src: G_PathToBaseImages + 'edit-1.svg', alt: "Edit" })),
                this.RenderFollowButton());
        }
    };
    OneMenuCard.prototype.EditOnClick = function () {
        var newState = __assign({}, this.state); //Object.assign({}, );
        newState.EditNow = true;
        newState.NewState = Object.assign(new OneCardInListData_1.OneCardInListData(), this.props.CardData);
        this.setState(newState);
    };
    OneMenuCard.prototype.CancelEditOnClick = function () {
        var newState = __assign({}, this.state);
        newState.EditNow = false;
        newState.NewState = null;
        // console.log(newState)
        this.setState(newState);
        this.props.IsNewTemplate;
        if (this.props.IsNewTemplate && this.props.RemoveNewTemplate) {
            this.props.RemoveNewTemplate();
        }
    };
    OneMenuCard.prototype.FollowCard = function () {
        if (!this.props.FollowRequstSuccess) {
            return;
        }
        //TODO запрос
        var thisRef = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            if (data.result === true) {
                thisRef.props.FollowRequstSuccess(thisRef.props.CardData.Id, true);
            }
            else if (data.result === false) {
                thisRef.props.FollowRequstSuccess(thisRef.props.CardData.Id, false);
            }
        };
        window.G_ArticleController.Follow({ Id: this.props.CardData.Id }, success);
        // let data = {
        //     "id": this.props.CardData.Id,
        // };
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let boolRes = xhr as BoolResultBack;
        //             if (boolRes.result === true) {
        //                 this.props.FollowRequstSuccess(this.props.CardData.Id, true);
        //             }
        //             else if (boolRes.result === false) {
        //                 this.props.FollowRequstSuccess(this.props.CardData.Id, false);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/follow',
        // });
    };
    OneMenuCard.prototype.SaveOnClick = function () {
        var _this = this;
        //
        var cardForUpdate = this.state.NewState;
        // console.log(JSON.stringify(cardForUpdate));
        if (this.props.IsNewTemplate) {
            cardForUpdate.Id = -1;
            this.CreateCardInListRequest(cardForUpdate, function (newEl) {
                var newCardData = new OneCardInListData_1.OneCardInListData(newEl);
                var newState = __assign({}, _this.state);
                // let stateForUpdate = newState.NewState;
                newState.NewState = null;
                newState.EditNow = false;
                _this.setState(newState);
                _this.props.UpdateElement(newCardData, true);
            });
        }
        else {
            cardForUpdate.Id = this.props.CardData.Id;
            this.EditCardInListRequest(cardForUpdate, function (fromBack) {
                // let newCardData = new OneCardInListData(cardForUpdate);
                var newState = __assign({}, _this.state);
                // let stateForUpdate = newState.NewState;
                newState.NewState = null;
                newState.EditNow = false;
                _this.setState(newState);
                cardForUpdate.FillByBackModel(fromBack);
                _this.props.UpdateElement(cardForUpdate, false);
            });
        }
        //дальше работаем и в компоненты выше передаем то что вернулось в бэка
    };
    OneMenuCard.prototype.TitleRender = function () {
        var _a, _b;
        var title = (_a = this.state.NewState) === null || _a === void 0 ? void 0 : _a.Title;
        var id = (_b = this.state.NewState) === null || _b === void 0 ? void 0 : _b.Id;
        if (!this.state.NewState) {
            title = this.props.CardData.Title;
            id = this.props.CardData.Id;
        }
        if (this.state.EditNow) {
            return React.createElement("input", { type: "text", className: 'persent-100-width form-control', value: title, onChange: this.TitleOnChange });
        }
        else {
            return React.createElement(react_router_dom_1.Link, { to: "/menu-app/detail/" + id },
                React.createElement("h5", { className: "card-title" }, title));
        }
    };
    OneMenuCard.prototype.ImageRender = function () {
        var _a, _b;
        var localImagePath = G_EmptyImagePath;
        if ((_a = this.props.CardData) === null || _a === void 0 ? void 0 : _a.Image) {
            localImagePath = (_b = this.props.CardData) === null || _b === void 0 ? void 0 : _b.Image;
        }
        return React.createElement("img", { src: localImagePath, className: "card-img-top", alt: "..." });
    };
    OneMenuCard.prototype.BodyRender = function () {
        var _a;
        if (this.state.EditNow) {
            var bodyText = (_a = this.state.NewState) === null || _a === void 0 ? void 0 : _a.Body;
            if (!this.state.NewState) {
                bodyText = this.state.NewState.Body;
            }
            return React.createElement("input", { type: "text", className: 'persent-100-width form-control', value: bodyText, onChange: this.BodyOnChange });
        }
        else {
            if (this.props.CardData) {
                return React.createElement("p", { className: "card-text" }, this.props.CardData.Body);
            }
            else {
                return React.createElement("p", { className: "card-text" });
            }
        }
    };
    OneMenuCard.prototype.TitleOnChange = function (e) {
        // console.log(this.state);
        var newState = Object.assign({}, this.state);
        newState.NewState.Title = e.target.value;
        // console.log(newState);
        this.setState(newState);
    };
    OneMenuCard.prototype.BodyOnChange = function (e) {
        var newState = Object.assign({}, this.state);
        newState.NewState.Body = e.target.value;
        this.setState(newState);
    };
    OneMenuCard.prototype.render = function () {
        if (this.props.CardData || this.state.NewState) {
            return React.createElement("div", { className: 'col-sm-6 col-md-4 col-lg-3', style: { padding: '20px' } },
                React.createElement("div", { className: "card one-menu-card-inner" },
                    this.ActionButton(),
                    this.ImageRender(),
                    React.createElement("div", { className: "card-body" },
                        this.TitleRender(),
                        this.BodyRender())));
        }
        else {
            return null;
        }
    };
    //----------------------------------------------------------------------------PRIVATE
    ///редактирование именно из списка карт
    OneMenuCard.prototype.EditCardInListRequest = function (newElement, callBack) {
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var forApply = new OneCardInListDataBack_1.OneCardInListDataBack();
            forApply.FillByFullMode(data);
            callBack(forApply);
        };
        var forSend = __assign(__assign({}, newElement), { NeedDeleteMainImage: false });
        window.G_ArticleController.Edit(forSend, success);
        // let data = {
        //     "id": newElement.Id,
        //     "title": newElement.Title,
        //     "body": newElement.Body,
        //     // "main_image_new":newElement.Image,
        // };
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let res = xhr as IOneCardInListDataBack;
        //             if (res.id && res.id > 0) {
        //                 callBack(res);
        //             }
        //             else {
        //                 //какая то ошибка
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/edit',
        // });
    };
    ///создание именно из списка карт
    OneMenuCard.prototype.CreateCardInListRequest = function (newElement, callBack) {
        // let thisRef = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var forApply = new OneCardInListDataBack_1.OneCardInListDataBack();
            forApply.FillByFullMode(data);
            callBack(forApply);
        };
        window.G_ArticleController.Create(newElement, success);
        // let data = {
        //     "title": newElement.Title,
        //     "body": newElement.Body,
        //     // "main_image_new":newElement.Image,
        // };
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let resBack = xhr as IOneCardInListDataBack;
        //             if (Number.isInteger(resBack.id) && resBack.id > 0) {
        //                 callBack(resBack);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/create',
        // });
    };
    return OneMenuCard;
}(React.Component));
exports.OneMenuCard = OneMenuCard;


/***/ }),

/***/ "./src/components/Body/MenuApp/MenuAppMain.tsx":
/*!*****************************************************!*\
  !*** ./src/components/Body/MenuApp/MenuAppMain.tsx ***!
  \*****************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MenuAppMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var BodyCardsListMain_1 = __webpack_require__(/*! ./CardsList/BodyCardsListMain */ "./src/components/Body/MenuApp/CardsList/BodyCardsListMain.tsx");
var OneCardDetailMain_1 = __webpack_require__(/*! ./OneCardDetail/OneCardDetailMain */ "./src/components/Body/MenuApp/OneCardDetail/OneCardDetailMain.tsx");
var MenuAppMain = /** @class */ (function (_super) {
    __extends(MenuAppMain, _super);
    function MenuAppMain(props) {
        return _super.call(this, props) || this;
    }
    MenuAppMain.prototype.render = function () {
        // return <MainAuth login={true}/>
        // return <BodyCardsListMain />
        //TODO попробовать достучаться незалогиненным по ссылкам и поправить то что вылезет
        // return <BodyCardsListMain/> 
        return React.createElement(react_router_dom_1.Switch, null,
            React.createElement(react_router_dom_1.Route, { exact: true, path: "/menu-app", component: BodyCardsListMain_1.BodyCardsListMain }),
            React.createElement(react_router_dom_1.Route, { path: "/menu-app/detail", component: OneCardDetailMain_1.OneCardDetailMain }));
    };
    return MenuAppMain;
}(React.Component));
exports.MenuAppMain = MenuAppMain;


/***/ }),

/***/ "./src/components/Body/MenuApp/OneCardDetail/AdditionalImages.tsx":
/*!************************************************************************!*\
  !*** ./src/components/Body/MenuApp/OneCardDetail/AdditionalImages.tsx ***!
  \************************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../../../../../typings/globals.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AdditionalImages = exports.CustomImageEdit = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var CustomImage_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/CustomImage */ "./src/components/_ComponentsLink/Models/CustomImage.ts");
// export interface IHeaderLogoProps {
// }
var CustomImageEdit = /** @class */ (function (_super) {
    __extends(CustomImageEdit, _super);
    function CustomImageEdit() {
        var _this = _super.call(this) || this;
        _this.NeedRemove = false;
        return _this;
    }
    CustomImageEdit.prototype.FillByCustomImage = function (newData) {
        Object.assign(this, newData);
    };
    return CustomImageEdit;
}(CustomImage_1.CustomImage));
exports.CustomImageEdit = CustomImageEdit;
var AdditionalImages = /** @class */ (function (_super) {
    __extends(AdditionalImages, _super);
    function AdditionalImages(props) {
        var _this = _super.call(this, props) || this;
        //this.props.location.search
        _this.RenderOneAdditionalImageActions = _this.RenderOneAdditionalImageActions.bind(_this);
        return _this;
    }
    // RestoreRemovedAdditionalImage(id: number) {
    //   this.props.RestoreRemovedAdditionalImage(id);
    // }
    AdditionalImages.prototype.RenderOneAdditionalImageActions = function (img) {
        var _this = this;
        if (this.props.EditNow) {
            if (img.NeedRemove) {
                // <p>в списке на удаление</p>
                return React.createElement("button", { className: "btn", onClick: function () { _this.props.RestoreRemovedAdditionalImage(img.Id); } }, " \u0432\u043E\u0441\u0441\u0442\u0430\u043D\u043E\u0432\u0438\u0442\u044C \u041A\u0410\u0420\u0422\u0418\u041D\u041A\u0423");
            }
            else {
                return React.createElement("button", { className: "btn", onClick: function () { _this.props.AddToRemoveAdditionalImage(img.Id); } }, "\u0423\u0414\u0410\u041B\u0418\u0422\u042C \u041A\u0410\u0420\u0422\u0418\u041D\u041A\u0423");
            }
        }
    };
    AdditionalImages.prototype.render = function () {
        var _this = this;
        var LoadlFileInput = React.createElement("div", null);
        if (this.props.EditNow) {
            LoadlFileInput = React.createElement("input", { id: "additional_images_input", multiple: true, type: "file" });
        }
        // let imageArr: CustomImage[] | CustomImageEdit[];
        // if (this.props.EditNow) {
        //   imageArr = this.props.ImagesEdit;
        // }
        // else {
        //   imageArr = this.props.Images;
        // }
        if (this.props.Images) {
            return React.createElement("div", null,
                LoadlFileInput,
                React.createElement("div", { id: "carouselExampleControls", className: "carousel slide carousel-fade", "data-ride": "false", "data-interval": "false" },
                    React.createElement("div", { className: "carousel-inner" }, this.props.Images.map(function (x, index) {
                        var actv = '';
                        if (index == 0) {
                            actv = ' active';
                        }
                        return React.createElement("div", { className: "carousel-item" + actv, key: index },
                            _this.RenderOneAdditionalImageActions(x),
                            React.createElement("img", { src: x.Path, className: "d-block w-100", alt: "..." }));
                    })),
                    React.createElement("a", { className: "carousel-control-prev", href: "#carouselExampleControls", role: "button", "data-slide": "prev" },
                        React.createElement("span", { className: "carousel-control-prev-icon", "aria-hidden": "true" }),
                        React.createElement("span", { className: "sr-only" }, "Previous")),
                    React.createElement("a", { className: "carousel-control-next", href: "#carouselExampleControls", role: "button", "data-slide": "next" },
                        React.createElement("span", { className: "carousel-control-next-icon", "aria-hidden": "true" }),
                        React.createElement("span", { className: "sr-only" }, "Next"))));
        }
        return React.createElement("div", null);
    };
    return AdditionalImages;
}(React.Component));
exports.AdditionalImages = AdditionalImages;


/***/ }),

/***/ "./src/components/Body/MenuApp/OneCardDetail/OneCardDetailMain.tsx":
/*!*************************************************************************!*\
  !*** ./src/components/Body/MenuApp/OneCardDetail/OneCardDetailMain.tsx ***!
  \*************************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../../../../../typings/globals.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneCardDetailMain = exports.OneCardFullDataView = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var AdditionalImages_1 = __webpack_require__(/*! ./AdditionalImages */ "./src/components/Body/MenuApp/OneCardDetail/AdditionalImages.tsx");
// import { IOneCardFullData, OneCardFullData } from '../../../_ComponentsLink/Models/MenuApp/OneCardFullData';
var OneCardInListData_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/MenuApp/OneCardInListData */ "./src/components/_ComponentsLink/Models/MenuApp/OneCardInListData.ts");
var IOneCardFullDataEdit_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/MenuApp/Poco/IOneCardFullDataEdit */ "./src/components/_ComponentsLink/Models/MenuApp/Poco/IOneCardFullDataEdit.ts");
var OneCardFullDataView = /** @class */ (function () {
    function OneCardFullDataView() {
    }
    OneCardFullDataView.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.Title = newData.title;
        this.Body = newData.body;
        this.Image = newData.main_image_path;
        this.Followed = newData.followed;
        this.AdditionalImages = newData.additional_images.map(function (x) {
            var res = new AdditionalImages_1.CustomImageEdit();
            res.FillByBackModel(x);
            return res;
        });
    };
    return OneCardFullDataView;
}());
exports.OneCardFullDataView = OneCardFullDataView;
var OneCardDetailMain = /** @class */ (function (_super) {
    __extends(OneCardDetailMain, _super);
    function OneCardDetailMain(props) {
        var _this = _super.call(this, props) || this;
        //this.props.location.search
        var newState = {
            EditNow: false,
        };
        if (_this.props.CardDataFromList) {
            //что то передали из списка, можно это отобразить пока грузятся норм данные
            newState.Card = new OneCardFullDataView();
            newState.Card.Id = _this.props.CardDataFromList.Id;
            newState.Card.Body = _this.props.CardDataFromList.Body;
            newState.Card.Followed = _this.props.CardDataFromList.Followed;
            newState.Card.Image = _this.props.CardDataFromList.Image;
            newState.Card.Title = _this.props.CardDataFromList.Title;
        }
        _this.state = newState;
        _this.RenderFollowBlock = _this.RenderFollowBlock.bind(_this);
        _this.FollowButtonClick = _this.FollowButtonClick.bind(_this);
        _this.RenderTitle = _this.RenderTitle.bind(_this);
        _this.BodyTextRender = _this.BodyTextRender.bind(_this);
        _this.RenderImage = _this.RenderImage.bind(_this);
        _this.RenderCardOrPreloader = _this.RenderCardOrPreloader.bind(_this);
        _this.RenderEditBlock = _this.RenderEditBlock.bind(_this);
        _this.EditButtonClick = _this.EditButtonClick.bind(_this);
        _this.CancelButtonClick = _this.CancelButtonClick.bind(_this);
        _this.RenderCancelBlock = _this.RenderCancelBlock.bind(_this);
        _this.TitleOnChange = _this.TitleOnChange.bind(_this);
        _this.BodyOnChange = _this.BodyOnChange.bind(_this);
        _this.SaveButtonClick = _this.SaveButtonClick.bind(_this);
        _this.DeleteMainImageClick = _this.DeleteMainImageClick.bind(_this);
        _this.AddToRemoveAdditionalImage = _this.AddToRemoveAdditionalImage.bind(_this);
        _this.RestoreRemovedAdditionalImage = _this.RestoreRemovedAdditionalImage.bind(_this);
        return _this;
    }
    OneCardDetailMain.prototype.componentDidMount = function () {
        //TODO по
        //надо как то прокинуть из урла
        var cardId = -1;
        if (this.props.Id) {
            cardId = this.props.Id;
        }
        else {
            var urlArr = window.location.pathname.split('/');
            // cardId = urlArr[urlArr.length - 1];
            var idStr = urlArr[urlArr.length - 1];
            // console.log(JSON.stringify(idStr));
            if (/^\d+$/.test(idStr)) { //TODO вынести в метод strContainsNum
                cardId = +idStr;
            }
            // console.log(JSON.stringify(cardId));
        }
        var thisRef = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, thisRef.state);
            newState.Card = new OneCardFullDataView();
            newState.Card.FillByBackModel(data);
            thisRef.setState(newState);
        };
        window.G_ArticleController.Detail({ Id: cardId }, success);
        // let data = {
        //     "id": cardId,
        // };
        // let thisRef = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IOneCardFullDataBack;
        //             if (dataBack.id && dataBack.id > 0) {
        //                 let newState = { ...thisRef.state };
        //                 newState.Card = new OneCardFullDataView();
        //                 newState.Card.FillByBackModel(dataBack);
        //                 thisRef.setState(newState);
        //             }
        //             else {
        //                 //ошибка
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/detail',
        // });
    };
    OneCardDetailMain.prototype.componentWillUnmount = function () {
        this.state = null;
    };
    OneCardDetailMain.prototype.RenderFollowBlock = function () {
        if (!this.state) {
            return React.createElement("div", null);
        }
        var imgPath = G_PathToBaseImages;
        //стоит ли сюда прокидывать из списка событие?
        // return <button>follow</button>
        if (this.state.Card && this.state.Card.Followed) {
            imgPath += 'red_heart.png';
        }
        else {
            imgPath += 'white_heart.jpg';
        }
        return React.createElement("div", { className: 'one-card-page-follow-button datail-one-card-button', onClick: this.FollowButtonClick },
            React.createElement("img", { className: 'persent-100-width-height', src: imgPath, alt: "Follow" }));
    };
    OneCardDetailMain.prototype.RenderCancelBlock = function () {
        if (!this.state) {
            return null;
        }
        var imgPath = G_PathToBaseImages + 'cancel.png';
        //стоит ли сюда прокидывать из списка событие?
        // return <button>follow</button>
        if (!this.state.EditNow) {
            return null;
        }
        return React.createElement("div", { className: 'one-card-page-cancel-button datail-one-card-button', onClick: this.CancelButtonClick },
            React.createElement("img", { className: 'persent-100-width-height', src: imgPath, alt: "Edit" }));
    };
    OneCardDetailMain.prototype.RenderEditBlock = function () {
        if (!this.state) {
            return null;
        }
        var imgPath = G_PathToBaseImages + 'edit-1.svg';
        //стоит ли сюда прокидывать из списка событие?
        // return <button>follow</button>
        if (this.state.EditNow) {
            return null;
        }
        return React.createElement("div", { className: 'one-card-page-edit-button datail-one-card-button', onClick: this.EditButtonClick },
            React.createElement("img", { className: 'persent-100-width-height', src: imgPath, alt: "Edit" }));
    };
    OneCardDetailMain.prototype.RenderSaveBlock = function () {
        if (!this.state) {
            return null;
        }
        var imgPath = G_PathToBaseImages + 'save-icon.png';
        //стоит ли сюда прокидывать из списка событие?
        // return <button>follow</button>
        if (!this.state.EditNow) {
            return null;
        }
        return React.createElement("div", { className: 'one-card-page-save-button datail-one-card-button', onClick: this.SaveButtonClick },
            React.createElement("img", { className: 'persent-100-width-height', src: imgPath, alt: "Save" }));
    };
    OneCardDetailMain.prototype.EditButtonClick = function () {
        var newState = __assign({}, this.state);
        newState.EditNow = true;
        newState.NewCardData = Object.assign(new IOneCardFullDataEdit_1.OneCardFullDataEdit(), __assign(__assign({}, newState.Card), { NeedDeleteMainImage: false }));
        // newState.NewCardData.AdditionalImages = newState.Card.AdditionalImages.map(x => {
        //     let res = new CustomImageEdit();
        //     res.FillByCustomImage(x);
        //     return res;
        // });
        this.setState(newState);
    };
    OneCardDetailMain.prototype.SaveButtonClick = function () {
        var _this = this;
        var cardForUpdate = __assign({}, this.state.NewCardData);
        cardForUpdate.Id = this.state.Card.Id;
        cardForUpdate.MainImageSave = $('#main_image_input')[0].files[0];
        cardForUpdate.AdditionalImagesSave = Array.from($('#additional_images_input')[0].files);
        //...Array.from(($('#additional_images_input')[0] as HTMLInputElement).files)
        this.EditCardInListRequest(cardForUpdate, function (fromBack) {
            // let newCardData = new OneCardInListData(cardForUpdate);
            var newState = __assign({}, _this.state);
            newState.Card.FillByBackModel(fromBack);
            // newState.Card.Title=newState.NewCardData.Title;
            // newState.Card.Title=newState.NewCardData.body;
            newState.NewCardData = null;
            newState.EditNow = false;
            _this.setState(newState);
            if (_this.props.UpdateElement) {
                var upd = new OneCardInListData_1.OneCardInListData();
                upd.FillByFullModel(newState.Card);
                _this.props.UpdateElement(upd, false);
            }
        });
    };
    OneCardDetailMain.prototype.CancelButtonClick = function () {
        var newState = __assign({}, this.state);
        newState.EditNow = false;
        newState.NewCardData = null;
        this.setState(newState);
    };
    OneCardDetailMain.prototype.FollowButtonClick = function () {
        var thisRef = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, thisRef.state);
            if (data.result === true) {
                newState.Card.Followed = true;
                thisRef.setState(newState);
            }
            else if (data.result === false) {
                newState.Card.Followed = false;
                thisRef.setState(newState);
            }
        };
        window.G_ArticleController.Follow({ Id: this.state.Card.Id }, success);
        // let data = {
        //     "id": this.state.Card.Id,
        // };
        // let newState = { ...this.state };
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let boolRes = xhr as BoolResultBack;
        //             if (boolRes.result === true) {
        //                 newState.Card.Followed = true;
        //                 this.setState(newState);
        //             }
        //             else if (boolRes.result === false) {
        //                 newState.Card.Followed = false;
        //                 this.setState(newState);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/follow',
        // });
    };
    OneCardDetailMain.prototype.DeleteMainImageClick = function () {
        if (!this.state.EditNow) {
            return;
        }
        var newState = __assign({}, this.state);
        newState.NewCardData.NeedDeleteMainImage = !newState.NewCardData.NeedDeleteMainImage;
        this.setState(newState);
    };
    OneCardDetailMain.prototype.TitleOnChange = function (e) {
        // console.log(this.state);
        var newState = Object.assign({}, this.state);
        newState.NewCardData.Title = e.target.value;
        // console.log(newState);
        this.setState(newState);
    };
    OneCardDetailMain.prototype.BodyOnChange = function (e) {
        var newState = Object.assign({}, this.state);
        newState.NewCardData.Body = e.target.value;
        this.setState(newState);
    };
    OneCardDetailMain.prototype.AddToRemoveAdditionalImage = function (id) {
        this.ChangeRemoveStatusAdditionalImage(id, true);
    };
    OneCardDetailMain.prototype.RestoreRemovedAdditionalImage = function (id) {
        this.ChangeRemoveStatusAdditionalImage(id, false);
    };
    OneCardDetailMain.prototype.RenderTitle = function () {
        var _a;
        if ((_a = this.state) === null || _a === void 0 ? void 0 : _a.Card) {
            var title = this.state.Card.Title;
            if (this.state.NewCardData) {
                title = this.state.NewCardData.Title;
            }
            if (this.state.EditNow) {
                return React.createElement("input", { type: "text", className: 'persent-100-width form-control', value: title, onChange: this.TitleOnChange });
            }
            else {
                return React.createElement("h1", null, title);
            }
        }
        else {
            return React.createElement("h1", null, "WAITING");
        }
    };
    OneCardDetailMain.prototype.BodyTextRender = function () {
        var _a;
        if ((_a = this.state) === null || _a === void 0 ? void 0 : _a.Card) {
            var body = this.state.Card.Body;
            if (this.state.NewCardData) {
                body = this.state.NewCardData.Body;
            }
            if (this.state.EditNow) {
                // return <input type="text" className='persent-100-width form-control' value={body} onChange={this.BodyOnChange} />
                return React.createElement("textarea", { className: 'persent-100-width form-control', value: body, onChange: this.BodyOnChange });
            }
            else {
                return React.createElement("p", null, body);
            }
        }
        else {
            return React.createElement("p", null, "WAITING");
        }
    };
    OneCardDetailMain.prototype.RenderImage = function () {
        var imgPath = G_EmptyImagePath;
        if (this.state.Card && this.state.Card.Image) {
            imgPath = this.state.Card.Image;
        }
        var editFunc = React.createElement("div", null);
        if (this.state.EditNow) {
            editFunc = React.createElement("div", null,
                React.createElement("input", { id: "main_image_input", type: "file" }),
                React.createElement("p", null, "\u0443\u0434\u0430\u043B\u0438\u0442\u044C \u043A\u0430\u0440\u0442\u0438\u043D\u043A\u0443"),
                React.createElement("input", { onClick: this.DeleteMainImageClick, type: 'checkbox' }));
        }
        return React.createElement("div", null,
            React.createElement("img", { className: "persent-100-width-height", src: imgPath }),
            editFunc);
    };
    // AdditionalImageRender() {
    // }
    OneCardDetailMain.prototype.RenderActionButton = function () {
        return React.createElement("div", null,
            this.RenderTitle(),
            this.RenderFollowBlock(),
            this.RenderEditBlock(),
            this.RenderSaveBlock(),
            this.RenderCancelBlock());
    };
    OneCardDetailMain.prototype.RenderCardOrPreloader = function () {
        if (!this.state.Card) { //TODO нужен кастомный
            return React.createElement("div", { className: 'card-list-preloader' },
                React.createElement("div", { className: "spinner-border persent-100-width-height", role: "status" },
                    React.createElement("span", { className: "sr-only" }, "Loading...")));
        }
        return React.createElement("div", { className: "container" },
            React.createElement("div", null,
                React.createElement("div", { className: "one-card-header row padding-10-top" },
                    React.createElement("div", { className: 'col-sm-6 col-md-7 one-card-header-info' }, this.RenderActionButton()),
                    React.createElement("div", { className: 'col-md-1 col-sm-1 padding-10-top' }),
                    React.createElement("div", { className: 'col-sm-5 col-md-4 one-card-header-image padding-10-top' }, this.RenderImage())),
                React.createElement("div", { className: 'padding-10-top' }),
                React.createElement("div", { className: "one-card-body-info row padding-10-top" },
                    React.createElement(AdditionalImages_1.AdditionalImages, { Images: this.state.Card.AdditionalImages, EditNow: this.state.EditNow, AddToRemoveAdditionalImage: this.AddToRemoveAdditionalImage, RestoreRemovedAdditionalImage: this.RestoreRemovedAdditionalImage }),
                    React.createElement("div", { className: 'col-sm-12' }, this.BodyTextRender()),
                    React.createElement("div", { className: 'col-sm-12' }, "MORE INFO"))));
    };
    OneCardDetailMain.prototype.render = function () {
        // return <input placeholder="Поиск" onChange={this.onTextChanged} />;
        // return <BodyCardsListMain />
        return this.RenderCardOrPreloader();
    };
    OneCardDetailMain.prototype.EditCardInListRequest = function (newElement, callBack) {
        // let data = {
        //     "id": newElement.Id,
        //     "title": newElement.Title,
        //     "body": newElement.Body,
        //     // "main_image_new": $('#main_image_input').val(),
        //     // "main_image_new":newElement.Image,
        // };
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            callBack(data);
        };
        window.G_ArticleController.Edit(newElement, success);
        // let data = new FormData();
        // data.append('id', newElement.Id + '');
        // data.append('title', newElement.Title);
        // data.append('body', newElement.Body);
        // data.append('delete_main_image', JSON.stringify(newElement.NeedDeleteMainImage));
        // if (newElement.MainImageSave) {
        //     data.append('main_image_new', newElement.MainImageSave);
        // }
        // newElement.AdditionalImagesSave.forEach((addImage, index) => {
        //     data.append('additional_images', addImage);//' + index + '
        // });
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let res = xhr as IOneCardFullDataBack;
        //             if (res.id && res.id > 0) {
        //                 callBack(res);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/article/edit',
        // }, true);
    };
    OneCardDetailMain.prototype.ChangeRemoveStatusAdditionalImage = function (id, newStatus) {
        var _this = this;
        var newState = __assign({}, this.state);
        newState.Card.AdditionalImages.forEach(function (img) {
            if (img.Id == id) {
                img.NeedRemove = newStatus;
                _this.setState(newState);
                return;
            }
        });
    };
    return OneCardDetailMain;
}(React.Component));
exports.OneCardDetailMain = OneCardDetailMain;


/***/ }),

/***/ "./src/components/Body/Menu/AppList.tsx":
/*!**********************************************!*\
  !*** ./src/components/Body/Menu/AppList.tsx ***!
  \**********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AppList = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var AppItem_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/Poco/AppItem */ "./src/components/_ComponentsLink/Models/Poco/AppItem.ts");
var AppListItem_1 = __webpack_require__(/*! ./AppListItem */ "./src/components/Body/Menu/AppListItem.tsx");
var AppList = /** @class */ (function (_super) {
    __extends(AppList, _super);
    function AppList(props) {
        var _this = _super.call(this, props) || this;
        var arr = [
            new AppItem_1.AppItem({ Logo: G_EmptyImagePath, Name: "MenuApp", Path: "/menu-app" }),
            new AppItem_1.AppItem({ Logo: G_EmptyImagePath, Name: "Dict", Path: "/words-cards-app" }),
            new AppItem_1.AppItem({ Logo: G_EmptyImagePath, Name: "TimeBooking", Path: "/menu-app" }),
            new AppItem_1.AppItem({ Logo: G_EmptyImagePath, Name: "PlaningPoker", Path: "/planing-poker" }),
            new AppItem_1.AppItem({ Logo: G_EmptyImagePath, Name: "MenuApp", Path: "/menu-app" }),
        ];
        _this.state = {
            Apps: arr,
        };
        return _this;
    }
    AppList.prototype.render = function () {
        return React.createElement("div", { className: "container" },
            React.createElement("div", { className: "row" }, this.state.Apps.map(function (x, index) {
                return React.createElement(AppListItem_1.AppListItem, { key: index, Data: x });
            })));
    };
    return AppList;
}(React.Component));
exports.AppList = AppList;


/***/ }),

/***/ "./src/components/Body/Menu/AppListItem.tsx":
/*!**************************************************!*\
  !*** ./src/components/Body/Menu/AppListItem.tsx ***!
  \**************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AppListItem = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var AppListItem = /** @class */ (function (_super) {
    __extends(AppListItem, _super);
    function AppListItem(props) {
        return _super.call(this, props) || this;
    }
    AppListItem.prototype.render = function () {
        //offset-md-1
        var imgLogo = this.props.Data.Logo;
        if (this.props.Data.Logo) {
            imgLogo = G_EmptyImagePath;
        }
        return React.createElement("div", { className: "app-one-item col-sm-12 col-md-3 col-lg-2" },
            React.createElement("div", { className: "app-one-item-inner" },
                React.createElement("img", { className: "persent-100-width-height", src: imgLogo }),
                React.createElement("a", { href: this.props.Data.Path }, this.props.Data.Name)));
    };
    return AppListItem;
}(React.Component));
exports.AppListItem = AppListItem;


/***/ }),

/***/ "./src/components/Body/Menu/MenuMain.tsx":
/*!***********************************************!*\
  !*** ./src/components/Body/Menu/MenuMain.tsx ***!
  \***********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MenuMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
// import { Switch, Route } from "react-router-dom";
var AppList_1 = __webpack_require__(/*! ./AppList */ "./src/components/Body/Menu/AppList.tsx");
var MenuMain = /** @class */ (function (_super) {
    __extends(MenuMain, _super);
    function MenuMain(props) {
        return _super.call(this, props) || this;
    }
    MenuMain.prototype.render = function () {
        // return <MainAuth login={true}/>
        // return <BodyCardsListMain />
        //TODO попробовать достучаться незалогиненным по ссылкам и поправить то что вылезет
        // return <BodyCardsListMain/> 
        return React.createElement(AppList_1.AppList, null);
        // <Switch>
        //     <Route exact path="/menu" component={AppList} />
        // </Switch>
    };
    return MenuMain;
}(React.Component));
exports.MenuMain = MenuMain;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/Index.tsx":
/*!****************************************************!*\
  !*** ./src/components/Body/PlaningPoker/Index.tsx ***!
  \****************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
var react_1 = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var AlertData_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var IndexState = /** @class */ (function () {
    // RoomName: string;
    // RoomPassword: string;
    function IndexState() {
        // this.RoomName = "";
        // this.RoomPassword = "";
    }
    return IndexState;
}());
var IndexProps = /** @class */ (function () {
    function IndexProps() {
    }
    return IndexProps;
}());
var Index = function (props) {
    // const initState = new IndexState();
    // const [localState, setLocalState] = useState(initState);
    // const [test, setTestLocalState] = useState(false);
    // const [withoutPasswordState, setWithoutPasswordState] = useState(false);
    // if (!test) {
    //     setTestLocalState(true);
    // }
    react_1.useEffect(function () {
        var pathNameUrlSplit = document.location.pathname.split('/');
        if (pathNameUrlSplit && pathNameUrlSplit.length > 2) {
            props.RoomNameChanged(pathNameUrlSplit[2]);
        }
        // console.log("Index");
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.RoomNotCreated, function () {
            var alert = new AlertData_1.AlertData();
            alert.Text = "Комната не создана";
            alert.Type = 1;
            window.G_AddAbsoluteAlertToState(alert);
            return;
        });
        return function cleanUp() {
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.RoomNotCreated);
        };
    }, []);
    var createRoom = function () {
        //этот метод вроде как  может подождать результат выполнения и как то получить ответ
        // props.MyHubConnection.invoke("CreateRoom", localState.RoomName, localState.RoomPassword, props.Username);
        //а вот этот не ждет
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.CreateRoom, props.RoomInfo.Name, props.RoomInfo.Password, props.Username);
    };
    var enterInRoom = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.EnterInRoom, props.RoomInfo.Name, props.RoomInfo.Password, props.Username);
    };
    // let withoutPasswordOnClick = () => {
    //     setWithoutPasswordState(!withoutPasswordState);
    // }
    // let passwordAreaRender = () => {
    //     if (withoutPasswordState) {
    //         return <div></div>
    //     }
    //     return
    // }
    //React.Fragment
    var actionsButton = react_1.default.createElement("div", null);
    if (props.Username) {
        actionsButton = react_1.default.createElement("div", null,
            react_1.default.createElement("button", { className: "btn btn-primary", onClick: createRoom }, "\u0441\u043E\u0437\u0434\u0430\u0442\u044C \u043A\u043E\u043C\u043D\u0430\u0442\u0443"),
            react_1.default.createElement("button", { className: "btn btn-primary", onClick: enterInRoom }, "\u043F\u043E\u0434\u043A\u043B\u044E\u0447\u0438\u0442\u044C\u0441\u044F \u043A \u0441\u0443\u0449\u0435\u0441\u0442\u0432\u0443\u044E\u0449\u0435\u0439 \u043A\u043E\u043C\u043D\u0430\u0442\u0435"));
    }
    return react_1.default.createElement("div", { className: "planing-enter-main" },
        react_1.default.createElement("div", { className: "planing-enter-inner col-sm-6 col-md-5 col-lg-4 offset-sm-3 offset-lg-4" },
            react_1.default.createElement("div", null,
                react_1.default.createElement("p", null,
                    "\u0438\u0437\u043C\u0435\u043D\u0438\u0442\u044C \u0438\u043C\u044F: ",
                    props.Username),
                react_1.default.createElement("input", { className: "form-control persent-100-width", onChange: function (e) { return props.ChangeUserName(e.target.value); }, type: "text", value: props.Username })),
            react_1.default.createElement("div", null,
                react_1.default.createElement("p", null, "\u043D\u0430\u0437\u0432\u0430\u043D\u0438\u0435"),
                react_1.default.createElement("input", { className: "form-control persent-100-width", type: "text", value: props.RoomInfo.Name, onChange: function (e) { props.RoomNameChanged(e.target.value); } }),
                react_1.default.createElement("div", null,
                    react_1.default.createElement("p", null, "\u043F\u0430\u0440\u043E\u043B\u044C(\u043D\u0435\u043E\u0431\u044F\u0437\u0430\u0442\u0435\u043B\u044C\u043D\u043E)"),
                    react_1.default.createElement("input", { className: "form-control persent-100-width", type: "text", value: props.RoomInfo.Password, onChange: function (e) { props.RoomPasswordChanged(e.target.value); } })),
                react_1.default.createElement("p", null, "\u0435\u0441\u043B\u0438 \u0441\u043E\u0437\u0434\u0430\u0442\u044C \u043A\u043E\u043C\u043D\u0430\u0442\u0443 \u0431\u0435\u0437 \u0430\u0432\u0442\u043E\u0440\u0438\u0437\u0430\u0446\u0438\u0438 \u0432 \u043E\u0441\u043D\u043E\u0432\u043D\u043E\u043C \u043F\u0440\u0438\u043B\u043E\u0436\u0435\u043D\u0438\u0438, \u0441\u043E\u0437\u0434\u0430\u0435\u0442\u0441\u044F \u043E\u0434\u043D\u043E\u0440\u0430\u0437\u043E\u0432\u0430\u044F \u043A\u043E\u043C\u043D\u0430\u0442\u0430(\u0431\u0443\u0434\u0435\u0442 \u0443\u0434\u0430\u043B\u0435\u043D\u0430 \u0447\u0435\u0440\u0435\u0437 \u043D\u0435\u043A\u043E\u0442\u043E\u0440\u043E\u0435 \u0432\u0440\u0435\u043C\u044F)")),
            actionsButton,
            react_1.default.createElement("div", { className: "display_none" },
                react_1.default.createElement(react_router_dom_1.Link, { id: "move_to_room_link_react", to: "/planing-poker/room/" + props.RoomInfo.Name }, "hidden"))));
};
exports.default = Index;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/Models/RoomInfo.ts":
/*!*************************************************************!*\
  !*** ./src/components/Body/PlaningPoker/Models/RoomInfo.ts ***!
  \*************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.StoriesHelper = exports.Story = exports.VoteInfo = exports.PlaningPokerUserInfo = exports.UserInRoom = exports.UserRoles = exports.RoomStatus = exports.RoomInfo = void 0;
//todo хорошо бы по файликам раскидать
var RoomInfo = /** @class */ (function () {
    function RoomInfo() {
        this.Name = "";
        this.Password = "";
        this.InRoom = false;
    }
    return RoomInfo;
}());
exports.RoomInfo = RoomInfo;
var RoomStatus;
(function (RoomStatus) {
    RoomStatus[RoomStatus["None"] = 0] = "None";
    RoomStatus[RoomStatus["AllCanVote"] = 1] = "AllCanVote";
    RoomStatus[RoomStatus["CloseVote"] = 2] = "CloseVote";
})(RoomStatus = exports.RoomStatus || (exports.RoomStatus = {}));
;
var UserRoles = /** @class */ (function () {
    function UserRoles() {
    }
    UserRoles.User = "User";
    UserRoles.Admin = "Admin";
    UserRoles.Creator = "Creator";
    UserRoles.Observer = "Observer";
    return UserRoles;
}());
exports.UserRoles = UserRoles;
var UserInRoom = /** @class */ (function () {
    function UserInRoom() {
        var _this = this;
        this.IsAdmin = function () {
            return _this.Roles.includes(UserRoles.Creator) || _this.Roles.includes(UserRoles.Admin);
        };
        this.CanVote = function () {
            return !_this.Roles.includes(UserRoles.Observer);
        };
    }
    UserInRoom.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.Name = newData.name;
        this.Vote = newData.vote;
        this.Roles = newData.roles;
        this.HasVote = newData.has_vote;
    };
    return UserInRoom;
}());
exports.UserInRoom = UserInRoom;
var PlaningPokerUserInfo = /** @class */ (function () {
    function PlaningPokerUserInfo() {
        this.UserName = "";
        this.UserId = "";
        this.UserConnectionId = "";
        this.LoginnedInMainApp = false;
    }
    return PlaningPokerUserInfo;
}());
exports.PlaningPokerUserInfo = PlaningPokerUserInfo;
var VoteInfo = /** @class */ (function () {
    function VoteInfo() {
        this.MaxVote = 0;
        this.MinVote = 0;
        this.AverageVote = 0;
    }
    return VoteInfo;
}());
exports.VoteInfo = VoteInfo;
var Story = /** @class */ (function () {
    function Story() {
        this.Id = "";
        // this.InitWithServer = false;
        this.Name = "";
        this.Description = "";
        this.Vote = null;
        this.Date = null;
        this.Completed = false;
    }
    Story.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.Name = newData.name;
        this.Description = newData.description;
        this.Vote = newData.vote;
        this.Date = newData.date;
        this.Completed = newData.completed;
    };
    return Story;
}());
exports.Story = Story;
var StoriesHelper = /** @class */ (function () {
    function StoriesHelper() {
        var _this = this;
        this.GetStoryIndexById = function (stories, storyId) {
            if (!storyId) {
                return -1;
            }
            var index = stories.findIndex(function (x) { return x.Id === storyId; });
            if (index < 0 || index >= stories.length) {
                return -1;
            }
            return index;
        };
        this.GetStoryById = function (stories, storyId) {
            var index = _this.GetStoryIndexById(stories, storyId);
            if (index < 0) {
                return;
            }
            return stories[index];
        };
    }
    return StoriesHelper;
}());
exports.StoriesHelper = StoriesHelper;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/OneVoteCard.tsx":
/*!**********************************************************!*\
  !*** ./src/components/Body/PlaningPoker/OneVoteCard.tsx ***!
  \**********************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
var react_1 = __importDefault(__webpack_require__(/*! react */ "react"));
var OneVoteCardProp = /** @class */ (function () {
    function OneVoteCardProp() {
    }
    return OneVoteCardProp;
}());
var OneVoteCard = function (props) {
    var selectedClass = "";
    if (props.NeedSelect) {
        selectedClass += " one-planing-selected-vote-card";
    }
    return react_1.default.createElement("div", { className: "one-planing-vote-card" + selectedClass, "data-vote": "" + props.Num }, props.Num);
};
exports.default = OneVoteCard;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/PlaningPokerMain.tsx":
/*!***************************************************************!*\
  !*** ./src/components/Body/PlaningPoker/PlaningPokerMain.tsx ***!
  \***************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
// import *, {useEffect} as React from "react";
var react_1 = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
// import * as signalR from "@microsoft/signalr";
var Index_1 = __importDefault(__webpack_require__(/*! ./Index */ "./src/components/Body/PlaningPoker/Index.tsx"));
var Room_1 = __importDefault(__webpack_require__(/*! ./Room */ "./src/components/Body/PlaningPoker/Room.tsx"));
var AlertData_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var RoomInfo_1 = __webpack_require__(/*! ./Models/RoomInfo */ "./src/components/Body/PlaningPoker/Models/RoomInfo.ts");
// import { HubConnection } from '@microsoft/signalr';
// import signalR, { HubConnection } from "@aspnet/signalr";
var signalR = __importStar(__webpack_require__(/*! @aspnet/signalr */ "./node_modules/@aspnet/signalr/dist/esm/index.js"));
// import * as signalR from '@aspnet/signalr'
// "@microsoft/signalr": "^5.0.6",
//
var PlaningPokerMainState = /** @class */ (function () {
    // InRoom: boolean;//по сути надо дернуть обновление стейта для перерендера
    function PlaningPokerMainState() {
        this.MyHubConnection = null;
        this.User = new RoomInfo_1.PlaningPokerUserInfo();
        this.RoomInfo = new RoomInfo_1.RoomInfo();
    }
    return PlaningPokerMainState;
}());
__webpack_require__(/*! ../../../../style/planing_poker.css */ "./style/planing_poker.css");
var PlaningPokerMain = function () {
    var initState = new PlaningPokerMainState();
    initState.User.UserName = "enter_your_name";
    var hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/planing-poker-hub"
    // , {
    //     skipNegotiation: true,
    //     transport: signalR.HttpTransportType.WebSockets//TODO эти 2 строки вроде как костыль
    // }
    )
        .build();
    // https://stackoverflow.com/questions/52086158/angular-signalr-error-failed-to-complete-negotiation-with-the-server
    //https://github.com/aspnet/SignalR/issues/2608
    //https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-typescript-webpack?view=aspnetcore-2.1&tabs=visual-studio
    initState.MyHubConnection = hubConnection;
    var _a = react_1.useState(initState), localState = _a[0], setLocalState = _a[1];
    var _b = react_1.useState(false), hubConnected = _b[0], sethubConnectedState = _b[1];
    //componentdidmount, должен вызваться уже когда childs отрендерятся
    react_1.useEffect(function () {
        hubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.PlaningNotifyFromServer, function (data) {
            var alert = new AlertData_1.AlertData();
            alert.Text = data.text;
            alert.Type = data.status;
            window.G_AddAbsoluteAlertToState(alert);
        });
        hubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.EnteredInRoom, function (roomUserId, loginnedInMainApp) {
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                newState.RoomInfo.InRoom = true;
                newState.User.UserId = roomUserId;
                newState.User.LoginnedInMainApp = loginnedInMainApp;
                newState.RoomInfo.Password = "";
                return newState;
            });
            document.cookie = "planing_poker_roomname=" + localState.RoomInfo.Name + "; path=/;";
            var lk = document.getElementById('move_to_room_link_react');
            //todo типо костыль
            //если этой линки нет, значит мы уже на странице румы
            if (lk) {
                // history.pushState(null, '/planing-poker/room/' + localState.RoomInfo.Name);
                lk.click();
            }
            // history.pushState(null, '/');
            // history.pushState(null, '/messages');
            // window.document.title
        });
        hubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.ConnectedToRoomError, function () {
            var alert = new AlertData_1.AlertData();
            alert.Text = "подключение не удалось";
            alert.Type = 1;
            window.G_AddAbsoluteAlertToState(alert);
            if (!location.href.includes("/planing-poker") || location.href.includes("/planing-poker/room")) { // && !location.href.endsWith("/planing-poker/")) {
                var roomName = localState.RoomInfo.Name || "";
                window.location.href = "/planing-poker/" + roomName;
            }
            return;
        });
        hubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.NeedRefreshTokens, function () {
            window.G_AuthenticateController.RefreshAccessToken(true, null);
        });
        // возможно тут стоит выделить в child components методы которые вызвать до старта
        //Update, не стоит тк они актуальны только в самих компонентах
        hubConnection.start()
            .then(function () {
            hubConnection.invoke(G_PlaningPokerController.EndPoints.EndpointsBack.GetConnectionId)
                .then(function (connectionId) {
                // let newState = { ...localState };
                // newState.User.UserId = connectionId;
                // setLocalState(newState);
                setLocalState(function (prevState) {
                    var newState = __assign({}, prevState);
                    newState.User.UserConnectionId = connectionId;
                    return newState;
                });
                //sethubConnectedState(true);
                sethubConnectedState(function (prevState) {
                    return true;
                });
            });
        }).catch(function () {
            alert("что то не так с подключением обновите страницу");
            // sethubConnectedState(false);
            sethubConnectedState(function (prevState) {
                return false;
            });
        });
        return function cleanUp() {
            hubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.ConnectedToRoomError);
            hubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.EnteredInRoom);
            hubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.PlaningNotifyFromServer);
            hubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.NeedRefreshTokens);
        };
    }, []);
    var userNameChange = function (newName) {
        setLocalState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.User.UserName = newName;
            return newState;
        });
    };
    var roomNameChanged = function (name) {
        // let newState = { ...localState };
        // newState.RoomInfo.Name = name;
        // setLocalState(newState);
        setLocalState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.RoomInfo.Name = name;
            return newState;
        });
    };
    var roomPasswordChanged = function (password) {
        // let newState = { ...localState };
        // newState.RoomInfo.Password = password;
        // setLocalState(newState);
        setLocalState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.RoomInfo.Password = password;
            return newState;
        });
    };
    var clearUserId = function () {
        setLocalState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.User.UserId = "";
            return newState;
        });
    };
    return react_1.default.createElement("div", null,
        react_1.default.createElement(react_router_dom_1.Switch, null,
            react_1.default.createElement(react_router_dom_1.Route, { path: "/planing-poker/room", render: function () {
                    return react_1.default.createElement(Room_1.default
                    //  InRoom={localState.InRoom}
                    , { 
                        //  InRoom={localState.InRoom}
                        UserInfo: localState.User, RoomInfo: localState.RoomInfo, MyHubConnection: localState.MyHubConnection, RoomNameChanged: roomNameChanged, ChangeUserName: userNameChange, HubConnected: hubConnected, ClearUserId: clearUserId });
                } }),
            react_1.default.createElement(react_router_dom_1.Route, { path: "/planing-poker", render: function () {
                    return react_1.default.createElement(Index_1.default, { Username: localState.User.UserName, ChangeUserName: userNameChange, MyHubConnection: localState.MyHubConnection, RoomNameChanged: roomNameChanged, RoomPasswordChanged: roomPasswordChanged, RoomInfo: localState.RoomInfo });
                } })));
};
exports.default = PlaningPokerMain;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/Room.tsx":
/*!***************************************************!*\
  !*** ./src/components/Body/PlaningPoker/Room.tsx ***!
  \***************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
// import * as React from "react";
var react_1 = __importStar(__webpack_require__(/*! react */ "react"));
var RoomInfo_1 = __webpack_require__(/*! ./Models/RoomInfo */ "./src/components/Body/PlaningPoker/Models/RoomInfo.ts");
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var UserInList_1 = __importDefault(__webpack_require__(/*! ./UserInList */ "./src/components/Body/PlaningPoker/UserInList.tsx"));
var OneVoteCard_1 = __importDefault(__webpack_require__(/*! ./OneVoteCard */ "./src/components/Body/PlaningPoker/OneVoteCard.tsx"));
var AlertData_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var StoriesSection_1 = __importDefault(__webpack_require__(/*! ./StoriesSection */ "./src/components/Body/PlaningPoker/StoriesSection.tsx"));
var RoomProps = /** @class */ (function () {
    function RoomProps() {
    }
    return RoomProps;
}());
var RoomState = /** @class */ (function () {
    // SelectedVoteCard: number;
    function RoomState() {
        this.UsersList = [];
        // this.CurrentVote = null;
        // this.SelectedVoteCard = -1;
        this.VoteInfo = new RoomInfo_1.VoteInfo();
        // this.RoomStatus = RoomSatus.None;
    }
    return RoomState;
}());
var StoriesInfo = /** @class */ (function () {
    // NameForAdd: string;
    // DescriptionForAdd: string;
    function StoriesInfo() {
        this.Stories = [];
        this.CurrentStoryId = "";
        // this.ClearTmpFuncForStories = null;
        // this.NameForAdd = "";
        // this.DescriptionForAdd = "";
        // this.CurrentStoryName = "";
        // this.CurrentStoryDescription = "";
        this.CurrentStoryNameChange = "";
        this.CurrentStoryDescriptionChange = "";
    }
    return StoriesInfo;
}());
//TODO а так можно? не будут ли они затирать в теории методы с других компонентов с такими же названиями, может их переименовать более сложно?
var CurrentUserIsAdmin = function (users, userId) {
    var user = GetUserById(users, userId);
    if (user && user.IsAdmin()) {
        return true;
    }
    return false;
};
var CurrentUserCanVote = function (users, userId) {
    var user = GetUserById(users, userId);
    if (user && user.CanVote()) {
        return true;
    }
    return false;
};
var GetUserIndexById = function (users, userId) {
    if (!users || !userId) {
        return -1;
    }
    return users.findIndex(function (x) { return x.Id === userId; });
};
var GetUserById = function (users, userId) {
    var index = GetUserIndexById(users, userId);
    if (index < 0 || index >= users.length) {
        return null;
    }
    return users[index];
};
var Room = function (props) {
    // useEffect(() => {
    //     console.log("use_1");
    // }, [1]);
    //эффект для доступа по прямой ссылке
    //
    react_1.useEffect(function () {
        if (!props.RoomInfo.Name) {
            var pathNameUrlSplit = document.location.pathname.split('/');
            if (pathNameUrlSplit && pathNameUrlSplit.length > 3 && pathNameUrlSplit[3]) {
                props.RoomNameChanged(pathNameUrlSplit[3]);
            }
            else {
                //todo тут можно ошибку какую нибудь бахнуть, типо вход не удался
                window.location.href = "/planing-poker";
            }
        }
        // else {
        //     if (!props.RoomInfo.InRoom) {
        //         props.MyHubConnection.send("EnterInRoom", props.RoomInfo.Name, props.RoomInfo.Password, props.UserInfo.UserName);
        //     }
        // }
    }, [props.RoomInfo.Name]);
    react_1.useEffect(function () {
        if (props.HubConnected && props.RoomInfo.Name && !props.RoomInfo.InRoom) {
            props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.EnterInRoom, props.RoomInfo.Name, props.RoomInfo.Password, props.UserInfo.UserName);
        }
    }, [props.HubConnected]);
    // console.log("render Room");
    // console.log(props.RoomInfo.Name);
    // if (!props.UserInfo.UserName) {
    //     props.ChangeUserName("enter_your_name");//todo хотя бы math random сюда закинуть?
    //     return <div></div>
    // }
    // if (!props.RoomInfo.InRoom) {
    //     return <div></div>
    //     //означает что мы пришли по прямой ссылке, не через форму входа с index page 
    //     //и при этом комната еще не загружена\мы не вошли
    //     // if (!props.RoomInfo.Name) {
    //     //     //имя комнаты пустое. либо это первый рендер либо имя комнаты нет вообще
    //     //     return <div></div>
    //     // }
    //     //это уже не первый рендер тк имя комнаты спаршено из урла и не пустое, означает что хаб подключен
    //     //но мы еще не вошли у нее
    //     // props.MyHubConnection.send("EnterInRoom", props.RoomInfo.Name, props.RoomInfo.Password, props.Username);
    // }
    var initState = new RoomState();
    var _a = react_1.useState(initState), localState = _a[0], setLocalState = _a[1];
    //НЕ заносить в общий объект, перестает работать, начинает сбрасываться при ререндере
    var _b = react_1.useState(RoomInfo_1.RoomStatus.None), roomStatusState = _b[0], setRoomStatusState = _b[1];
    var _c = react_1.useState(-1), selectedVoteCard = _c[0], setSelectedVoteCard = _c[1];
    var _d = react_1.useState(false), hideVoteState = _d[0], setHideVoteState = _d[1];
    var _e = react_1.useState(props.UserInfo.UserName), userNameLocalState = _e[0], setUserNameLocalState = _e[1]; //для редактирования
    var initStories = new StoriesInfo();
    var _f = react_1.useState(initStories), storiesState = _f[0], setStoriesState = _f[1];
    var storiesHelper = new RoomInfo_1.StoriesHelper();
    var currentUserIsAdmin = CurrentUserIsAdmin(localState.UsersList, props.UserInfo.UserId);
    react_1.useEffect(function () {
        if (!props.RoomInfo.InRoom) {
            return;
        }
        //мы проставили все необходимые данные, подключились к хабу и готовы работать
        var getRoomInfo = function (error, data) {
            if (error) {
                //TODO выбить из комнаты?
                alert("todo что то пошло не так лучше обновить страницу");
                return;
            }
            if (data) {
                var newUsersData_1 = data.room.users.map(function (x) {
                    var us = new RoomInfo_1.UserInRoom();
                    us.FillByBackModel(x);
                    return us;
                });
                setRoomStatusState(function (prevState) {
                    // let newState = { ...prevState };
                    return data.room.status;
                    // return newState;
                });
                setLocalState(function (prevState) {
                    var _a;
                    var newState = __assign({}, prevState);
                    newState.UsersList.splice(0, newState.UsersList.length);
                    (_a = newState.UsersList).push.apply(_a, newUsersData_1);
                    fillVoteInfo(newState, data.end_vote_info);
                    return newState;
                });
                setStoriesState(function (prevState) {
                    var newStoriesState = __assign({}, prevState);
                    newStoriesState.CurrentStoryId = data.room.current_story_id;
                    newStoriesState.Stories = data.room.actual_stories.map(function (x) {
                        var st = new RoomInfo_1.Story();
                        st.FillByBackModel(x);
                        return st;
                    });
                    return newStoriesState;
                });
            }
        };
        window.G_PlaningPokerController.GetRoomInfo(props.RoomInfo.Name, props.UserInfo.UserConnectionId, getRoomInfo);
    }, [props.RoomInfo.InRoom]);
    react_1.useEffect(function () {
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.NewUserInRoom, function (data) {
            if (!data) {
                return;
            }
            var dataTyped = data;
            var us = new RoomInfo_1.UserInRoom();
            us.FillByBackModel(dataTyped);
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                var existUser = GetUserById(newState.UsersList, dataTyped.id);
                if (!existUser) {
                    newState.UsersList.push(us);
                }
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.UserNameChanged, function (userId, newUserName) {
            if (!userId) {
                return;
            }
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                var user = GetUserById(newState.UsersList, userId);
                if (!user) {
                    return newState;
                }
                user.Name = newUserName;
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.UserLeaved, function (usersId) {
            if (!usersId) {
                return;
            }
            usersId.forEach(function (x) {
                if (x == props.UserInfo.UserId) {
                    alert("you kicked or leave"); //TODO может как то получше сделать, и хорошо бы без перезагрузки\редиректа
                    window.location.href = "/planing-poker";
                    props.ClearUserId(); //todo тут наверное стоит еще что то чистить
                    return;
                }
            });
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                usersId.forEach(function (x) {
                    var userIndex = GetUserIndexById(newState.UsersList, x);
                    if (userIndex < 0) {
                        return newState;
                    }
                    newState.UsersList.splice(userIndex, 1);
                });
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.VoteChanged, function (userId, vote) {
            if (!userId) {
                return;
            }
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                var user = GetUserById(newState.UsersList, userId);
                if (!user) {
                    return newState;
                }
                user.HasVote = true;
                if (!isNaN(vote)) {
                    user.Vote = vote;
                }
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.UserRoleChanged, function (userId, changeType, role) {
            if (!userId) {
                return;
            }
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                var user = GetUserById(newState.UsersList, userId);
                if (!user) {
                    return newState;
                }
                if (changeType === 1) {
                    //добавлен
                    user.Roles.push(role);
                }
                else {
                    //удален
                    var index = user.Roles.findIndex(function (x) { return x === role; });
                    if (index >= 0) {
                        user.Roles.splice(index, 1);
                    }
                }
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.VoteStart, function () {
            setSelectedVoteCard(function (prevState) {
                return -1;
            });
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                newState.UsersList.forEach(function (x) {
                    x.Vote = null;
                    x.HasVote = false;
                });
                newState.VoteInfo = new RoomInfo_1.VoteInfo();
                return newState;
            });
            // setRoomStatusState(RoomSatus.AllCanVote);
            setRoomStatusState(function (prevState) {
                // let newState = { ...prevState };
                return RoomInfo_1.RoomStatus.AllCanVote;
                // return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.VoteEnd, function (data) {
            // fillVoteInfo(null, data);
            setLocalState(function (prevState) {
                var newState = __assign({}, prevState);
                fillVoteInfo(newState, data);
                return newState;
            });
            // setRoomStatusState(RoomSatus.CloseVote);
            setRoomStatusState(function (prevState) {
                // let newState = { ...prevState };
                return RoomInfo_1.RoomStatus.CloseVote;
                // return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.AddedNewStory, function (data) {
            setStoriesState(function (prevState) {
                var newState = __assign({}, prevState);
                var newStory = new RoomInfo_1.Story();
                newStory.FillByBackModel(data);
                newState.Stories.push(newStory);
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.NewCurrentStory, function (id) {
            //изменении в целом объекта текущей истории
            setStoriesState(function (prevState) {
                var newState = __assign({}, prevState);
                newState.CurrentStoryId = id;
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.CurrentStoryChanged, function (id, newName, newDescription) {
            //изменение данных текущей истории
            setStoriesState(function (prevState) {
                var newState = __assign({}, prevState);
                var story = storiesHelper.GetStoryById(newState.Stories, id);
                newState.CurrentStoryNameChange = newName;
                newState.CurrentStoryDescriptionChange = newDescription;
                if (story) {
                    newState.CurrentStoryId = id;
                    story.Name = newName;
                    story.Description = newDescription;
                }
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.DeletedStory, function (id) {
            //изменение данных текущей истории
            setStoriesState(function (prevState) {
                var newState = __assign({}, prevState);
                var storyIndex = storiesHelper.GetStoryIndexById(newState.Stories, id);
                if (storyIndex < 0) {
                    return newState;
                }
                newState.Stories.splice(storyIndex, 1);
                if (newState.CurrentStoryId == id) {
                    newState.CurrentStoryId = "";
                }
                return newState;
            });
        });
        props.MyHubConnection.on(G_PlaningPokerController.EndPoints.EndpointsFront.MovedStoryToComplete, function (oldId, newData) {
            if (!newData) {
                return;
            }
            setStoriesState(function (prevState) {
                var newState = __assign({}, prevState);
                var story = storiesHelper.GetStoryById(newState.Stories, oldId);
                if (story) {
                    story.Id = newData.id;
                    story.Completed = newData.completed;
                    story.Date = newData.date;
                    story.Vote = newData.vote;
                    if (newState.CurrentStoryId === newData.id) {
                        newState.CurrentStoryId = "";
                    }
                    newState.CurrentStoryDescriptionChange = "";
                    newState.CurrentStoryNameChange = "";
                }
                return newState;
            });
        });
        return function cleanUp() {
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.MovedStoryToComplete);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.DeletedStory);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.CurrentStoryChanged);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.NewCurrentStory);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.AddedNewStory);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.VoteEnd);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.VoteStart);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.UserRoleChanged);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.VoteChanged);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.UserLeaved);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.UserNameChanged);
            props.MyHubConnection.off(G_PlaningPokerController.EndPoints.EndpointsFront.NewUserInRoom);
        };
    }, []);
    var fillVoteInfo = function (state, data) {
        // let newState = state || { ...localState };
        // setSelectedVoteCard(-1);
        setSelectedVoteCard(function (prevState) {
            return -1;
        });
        state.UsersList.forEach(function (x) {
            var userFromRes = data.users_info.find(function (x1) { return x1.id === x.Id; });
            if (userFromRes) {
                x.Vote = userFromRes.vote;
            }
        });
        state.VoteInfo.MaxVote = data.max_vote;
        state.VoteInfo.MinVote = data.min_vote;
        state.VoteInfo.AverageVote = data.average_vote;
    };
    if (!props.RoomInfo.InRoom) {
        return react_1.default.createElement("h1", null, "\u043F\u044B\u0442\u0430\u0435\u043C\u0441\u044F \u0432\u043E\u0439\u0442\u0438");
    }
    var tryToRemoveUserFromRoom = function (userId) {
        // let isAdmin = CurrentUserIsAdmin(localState.UsersList, props.UserInfo.UserId);
        if (!currentUserIsAdmin) {
            return;
        }
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.KickUser, props.RoomInfo.Name, userId);
    };
    var doVote = function (voteCardBlock) { return __awaiter(void 0, void 0, void 0, function () {
        var alert_1, voted;
        var _a, _b;
        return __generator(this, function (_c) {
            switch (_c.label) {
                case 0:
                    // console.log(vote);
                    // console.dir(vote);
                    if (!((_b = (_a = voteCardBlock === null || voteCardBlock === void 0 ? void 0 : voteCardBlock.target) === null || _a === void 0 ? void 0 : _a.dataset) === null || _b === void 0 ? void 0 : _b.vote)) {
                        return [2 /*return*/];
                    }
                    if (!CurrentUserCanVote(localState.UsersList, props.UserInfo.UserId)) {
                        alert_1 = new AlertData_1.AlertData();
                        alert_1.Text = "У обсерверов нет прав голосовать";
                        alert_1.Type = AlertData_1.AlertTypeEnum.Error;
                        alert_1.Timeout = 5000;
                        window.G_AddAbsoluteAlertToState(alert_1);
                        return [2 /*return*/];
                    }
                    return [4 /*yield*/, props.MyHubConnection.invoke("Vote", props.RoomInfo.Name, +voteCardBlock.target.dataset.vote)];
                case 1:
                    voted = _c.sent();
                    if (!voted) {
                        return [2 /*return*/];
                    }
                    setSelectedVoteCard(function (prevState) {
                        return +voteCardBlock.target.dataset.vote;
                    });
                    return [2 /*return*/];
            }
        });
    }); };
    var renderVotePlaceIfNeed = function () {
        //TODO UNCOMMENT
        if (roomStatusState !== RoomInfo_1.RoomStatus.AllCanVote) {
            return react_1.default.createElement("div", null);
        }
        var voteArr = [1, 2, 3, 5, 7, 10, 13, 15, 18, 20, 25, 30, 35, 40, 50];
        return react_1.default.createElement("div", { onClick: function (e) { return doVote(e); }, className: "planing-cards-container" }, voteArr.map(function (x) { return react_1.default.createElement(OneVoteCard_1.default, { key: x, Num: x, NeedSelect: selectedVoteCard === x }); }));
    };
    var renderVoteResultIfNeed = function () {
        //UNCOMMENT
        if (roomStatusState !== RoomInfo_1.RoomStatus.CloseVote) {
            return react_1.default.createElement("div", null);
        }
        return react_1.default.createElement("div", null,
            react_1.default.createElement("div", { className: "padding-10-top" }),
            react_1.default.createElement("div", { className: "planing-poker-left-one-section" },
                react_1.default.createElement("p", null, "vote result"),
                react_1.default.createElement("p", null,
                    "Max: ",
                    localState.VoteInfo.MaxVote),
                react_1.default.createElement("p", null,
                    "Min: ",
                    localState.VoteInfo.MinVote),
                react_1.default.createElement("p", null,
                    "Average: ",
                    localState.VoteInfo.AverageVote)));
    };
    var tryStartVote = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.StartVote, props.RoomInfo.Name);
    };
    var tryEndVote = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.EndVote, props.RoomInfo.Name);
    };
    var makeCurrentStory = function (id) {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.MakeCurrentStory, props.RoomInfo.Name, id);
    };
    var deleteStory = function (id) {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.DeleteStory, props.RoomInfo.Name, id);
    };
    var saveRoom = function () {
        props.MyHubConnection.invoke(G_PlaningPokerController.EndPoints.EndpointsBack.SaveRoom, props.RoomInfo.Name).then(function () { return alert("Сохранено"); });
    };
    var deleteRoom = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.DeleteRoom, props.RoomInfo.Name);
    };
    var storiesLoaded = function (stories) {
        setStoriesState(function (prevState) {
            var newState = __assign({}, prevState);
            stories.forEach(function (newStory) {
                var story = storiesHelper.GetStoryById(newState.Stories, newStory.id);
                if (!story) {
                    var storyForAdd = new RoomInfo_1.Story();
                    storyForAdd.FillByBackModel(newStory);
                    newState.Stories.push(storyForAdd);
                }
            });
            return newState;
        });
    };
    var currentStoryDescriptionOnChange = function (str) {
        setStoriesState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.CurrentStoryDescriptionChange = str;
            return newState;
        });
    };
    var currentStoryNameOnChange = function (str) {
        setStoriesState(function (prevState) {
            var newState = __assign({}, prevState);
            newState.CurrentStoryNameChange = str;
            return newState;
        });
    };
    var roomMainActionButton = function () {
        // let isAdmin = CurrentUserIsAdmin(localState.UsersList, props.UserInfo.UserId);
        var saveBut = react_1.default.createElement(react_1.default.Fragment, null);
        if (props.UserInfo.LoginnedInMainApp) {
            saveBut = react_1.default.createElement("button", { className: "btn btn-danger", onClick: function () { return saveRoom(); } }, "\u0421\u043E\u0445\u0440\u0430\u043D\u0438\u0442\u044C \u043A\u043E\u043C\u043D\u0430\u0442\u0443");
        }
        if (currentUserIsAdmin) {
            return react_1.default.createElement("div", null,
                react_1.default.createElement("button", { className: "btn btn-primary", onClick: function () { return tryStartVote(); } }, "\u041D\u0430\u0447\u0430\u0442\u044C \u0433\u043E\u043B\u043E\u0441\u043E\u0432\u0430\u043D\u0438\u0435"),
                react_1.default.createElement("button", { className: "btn btn-primary", onClick: function () { return tryEndVote(); } }, "\u0417\u0430\u043A\u043E\u043D\u0447\u0438\u0442\u044C \u0433\u043E\u043B\u043E\u0441\u043E\u0432\u0430\u043D\u0438\u0435"),
                saveBut,
                react_1.default.createElement("button", { className: "btn btn-danger", onClick: function () { return deleteRoom(); } }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C \u043A\u043E\u043C\u043D\u0430\u0442\u0443"));
        }
        return react_1.default.createElement("div", null);
    };
    var settingsUpUserListRender = function () {
        var hideVotesSetting = react_1.default.createElement("div", null);
        // if (CurrentUserIsAdmin(localState.UsersList, props.UserInfo.UserId)) {
        if (currentUserIsAdmin) {
            hideVotesSetting = react_1.default.createElement("div", null,
                react_1.default.createElement("div", { className: "padding-10-top" }),
                react_1.default.createElement("div", { className: "planning-vote-settings" },
                    react_1.default.createElement("label", null, "\u0421\u043A\u0440\u044B\u0432\u0430\u0442\u044C \u043E\u0446\u0435\u043D\u043A\u0438"),
                    react_1.default.createElement("input", { onClick: function () {
                            // setHideVoteState(!hideVoteState)
                            setHideVoteState(function (prevState) {
                                return !hideVoteState;
                            });
                        }, type: "checkbox" })));
        }
        var changeUserName = function () {
            props.MyHubConnection.invoke(G_PlaningPokerController.EndPoints.EndpointsBack.UserNameChange, props.RoomInfo.Name, userNameLocalState).then(function (dt) {
                if (!dt) {
                    var alert_2 = new AlertData_1.AlertData();
                    alert_2.Text = "изменить имя не удалось";
                    alert_2.Type = AlertData_1.AlertTypeEnum.Error;
                    window.G_AddAbsoluteAlertToState(alert_2);
                    return;
                }
                props.ChangeUserName(userNameLocalState);
            });
        };
        var updateAllUsers = function () {
            var loadedUsers = function (error, data) {
                if (error) {
                    //TODO выбить из комнаты?
                    alert("todo что то пошло не так лучше обновить страницу");
                    return;
                }
                if (data) {
                    var newUsersData_2 = data.map(function (x) {
                        var us = new RoomInfo_1.UserInRoom();
                        us.FillByBackModel(x);
                        return us;
                    });
                    setLocalState(function (prevState) {
                        var _a;
                        var newState = __assign({}, prevState);
                        newState.UsersList.splice(0, newState.UsersList.length);
                        (_a = newState.UsersList).push.apply(_a, newUsersData_2);
                        return newState;
                    });
                }
            };
            // console.log(JSON.stringify(props));
            window.G_PlaningPokerController.GetUsersIsRoom(props.RoomInfo.Name, props.UserInfo.UserConnectionId, loadedUsers);
        };
        return react_1.default.createElement("div", null,
            react_1.default.createElement("p", null, "\u0434\u043E\u043F \u043D\u0430\u0441\u0442\u0440\u043E\u0439\u043A\u0438"),
            react_1.default.createElement("input", { type: "text", className: "persent-100-width form-control", onChange: function (e) {
                    // setUserNameLocalState(e.target.value)
                    setUserNameLocalState(function (prevState) {
                        return e.target.value;
                    });
                }, value: userNameLocalState }),
            react_1.default.createElement("button", { className: "btn btn-primary", onClick: function () { return changeUserName(); } }, "\u0418\u0437\u043C\u0435\u043D\u0438\u0442\u044C \u0438\u043C\u044F"),
            react_1.default.createElement("button", { className: "btn btn-primary", onClick: function () { return updateAllUsers(); } }, "\u041E\u0431\u043D\u043E\u0432\u0438\u0442\u044C \u0441\u043F\u0438\u0441\u043E\u043A \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u0435\u0439"),
            hideVotesSetting);
    };
    var renderNotAuthMessage = function () {
        if (props.UserInfo.LoginnedInMainApp) {
            return react_1.default.createElement("div", null);
        }
        return react_1.default.createElement("div", { className: "planing-room-not-auth", title: "при обновлении страницы, вы подключаетесь как новый пользователь(исключение-вы авторизованы в основном приложении)." +
                "если в комнате все пользвоатели - неавторизованы, комната не будет сохраняться" }, "!");
    };
    return react_1.default.createElement("div", { className: "container" },
        react_1.default.createElement("div", { className: "padding-10-top" }),
        react_1.default.createElement("h1", null,
            "Room: ",
            props.RoomInfo.Name),
        renderNotAuthMessage(),
        react_1.default.createElement("div", { className: "row" },
            react_1.default.createElement("div", { className: "planit-room-left-part col-12 col-md-9" },
                react_1.default.createElement("div", null,
                    roomMainActionButton(),
                    renderVotePlaceIfNeed(),
                    renderVoteResultIfNeed()),
                react_1.default.createElement(StoriesSection_1.default, { CurrentStoryId: storiesState.CurrentStoryId, MyHubConnection: props.MyHubConnection, RoomName: props.RoomInfo.Name, Stories: storiesState.Stories, DeleteStory: deleteStory, MakeCurrentStory: makeCurrentStory, IsAdmin: currentUserIsAdmin, CurrentStoryDescriptionChange: storiesState.CurrentStoryDescriptionChange, CurrentStoryNameChange: storiesState.CurrentStoryNameChange, CurrentStoryDescriptionOnChange: currentStoryDescriptionOnChange, CurrentStoryNameOnChange: currentStoryNameOnChange, RoomStatus: roomStatusState, StoriesLoaded: storiesLoaded })),
            react_1.default.createElement("div", { className: "planit-room-right-part col-12 col-md-3" },
                react_1.default.createElement("div", null, settingsUpUserListRender()),
                react_1.default.createElement("div", { className: "padding-10-top" }),
                react_1.default.createElement("div", null, "\u043B\u044E\u0434\u0438"),
                localState.UsersList.map(function (x) {
                    return react_1.default.createElement(UserInList_1.default, { key: x.Id, User: x, TryToRemoveUserFromRoom: tryToRemoveUserFromRoom, RenderForAdmin: currentUserIsAdmin, HideVote: hideVoteState, HasVote: x.HasVote, RoomStatus: roomStatusState, MaxVote: localState.VoteInfo.MaxVote, MinVote: localState.VoteInfo.MinVote, MyHubConnection: props.MyHubConnection, RoomName: props.RoomInfo.Name });
                })),
            react_1.default.createElement("div", { className: "display_none" },
                react_1.default.createElement(react_router_dom_1.Link, { id: "move_to_index_link_react", to: "/planing-poker/" }, "hidden"))));
};
exports.default = Room;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/StoriesSection.tsx":
/*!*************************************************************!*\
  !*** ./src/components/Body/PlaningPoker/StoriesSection.tsx ***!
  \*************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
var react_1 = __importStar(__webpack_require__(/*! react */ "react"));
var RoomInfo_1 = __webpack_require__(/*! ./Models/RoomInfo */ "./src/components/Body/PlaningPoker/Models/RoomInfo.ts");
var StoriesSectionProp = /** @class */ (function () {
    function StoriesSectionProp() {
    }
    return StoriesSectionProp;
}());
var StoriesSectionState = /** @class */ (function () {
    function StoriesSectionState() {
        this.NameForAdd = "";
        this.DescriptionForAdd = "";
        this.ShowOnlyCompleted = false;
        // this.CurrentStoryNameChange = "";
        // this.CurrentStoryDescriptionChange = "";
    }
    return StoriesSectionState;
}());
var StoriesSection = function (props) {
    var initStories = new StoriesSectionState();
    var _a = react_1.useState(initStories), storiesState = _a[0], setStoriesState = _a[1];
    var storiesHelper = new RoomInfo_1.StoriesHelper();
    react_1.useEffect(function () {
        ResetCurrentStoryById();
    }, [props.CurrentStoryId]);
    react_1.useEffect(function () {
    }, []);
    var cancelChangeCurrentStory = function () {
        var story = storiesHelper.GetStoryById(props.Stories, props.CurrentStoryId);
        props.CurrentStoryDescriptionOnChange(story.Description);
        props.CurrentStoryNameOnChange(story.Name);
    };
    var changeCurrentStory = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.ChangeCurrentStory, props.RoomName, props.CurrentStoryId, props.CurrentStoryNameChange, props.CurrentStoryDescriptionChange);
    };
    var loadOldStories = function () {
        props.MyHubConnection.invoke(G_PlaningPokerController.EndPoints.EndpointsBack.LoadNotActualStories, props.RoomName).then(function (data) {
            var dataTyped = data;
            props.StoriesLoaded(dataTyped);
        });
    };
    var ResetCurrentStoryById = function () {
        if (!props.CurrentStoryId) {
            props.CurrentStoryDescriptionOnChange("");
            props.CurrentStoryNameOnChange("");
            return;
        }
        var story = storiesHelper.GetStoryById(props.Stories, props.CurrentStoryId);
        props.CurrentStoryDescriptionOnChange(story.Description);
        props.CurrentStoryNameOnChange(story.Name);
    };
    var tryMakeStoryComplete = function () {
        //если не задан voteInfo как то уведомить что оценки не запишутся
        var save = true;
        if (props.RoomStatus !== RoomInfo_1.RoomStatus.CloseVote) { //todo возможно тут стоит не на статус завязаться а на voteinfo
            save = confirm('Комната не в статусе <закрытого голосования>, История будет сохранена без оценок. Сохранить?');
        }
        if (save) {
            props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.MakeStoryComplete, props.RoomName, props.CurrentStoryId);
        }
    };
    var completedStoryInfo = function (story) {
        if (!storiesState.ShowOnlyCompleted) {
            return react_1.default.createElement("div", null);
        }
        return react_1.default.createElement("div", null,
            react_1.default.createElement("p", null,
                "\u0414\u0430\u0442\u0430 \u043E\u0446\u0435\u043D\u043A\u0438: ",
                story.Date),
            react_1.default.createElement("p", null,
                "\u041E\u0446\u0435\u043D\u043A\u0430: ",
                story.Vote + ""));
    };
    var currentStoryDescriptionRender = function () {
        if (!props.CurrentStoryId) {
            return react_1.default.createElement("div", null);
        }
        var story = storiesHelper.GetStoryById(props.Stories, props.CurrentStoryId);
        if (!story) {
            return react_1.default.createElement("div", null);
        }
        var adminButton = react_1.default.createElement("div", null);
        if (props.IsAdmin) {
            adminButton = react_1.default.createElement("div", null,
                react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return changeCurrentStory(); } }, "\u0418\u0437\u043C\u0435\u043D\u0438\u0442\u044C"),
                react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return cancelChangeCurrentStory(); } }, "\u041E\u0442\u043C\u0435\u043D\u0438\u0442\u044C"),
                react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return tryMakeStoryComplete(); } }, "\u041E\u0442\u043C\u0435\u0442\u0438\u0442\u044C \u043A\u0430\u043A \u0432\u044B\u043F\u043E\u043B\u043D\u0435\u043D\u043D\u0443\u044E"));
        }
        var storyBodyRender = function () {
            if (props.IsAdmin) {
                return react_1.default.createElement("div", null,
                    react_1.default.createElement("input", { className: "persent-100-width form-control", placeholder: "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435", value: props.CurrentStoryNameChange, type: "text", onChange: function (e) {
                            props.CurrentStoryNameOnChange(e.target.value);
                        } }),
                    react_1.default.createElement("input", { className: "persent-100-width form-control", placeholder: "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435", value: props.CurrentStoryDescriptionChange, type: "text", onChange: function (e) {
                            props.CurrentStoryDescriptionOnChange(e.target.value);
                        } }));
            }
            else {
                return react_1.default.createElement("div", null,
                    react_1.default.createElement("p", null,
                        "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435: ",
                        story.Name),
                    react_1.default.createElement("p", null,
                        "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435: ",
                        story.Description));
            }
        };
        return react_1.default.createElement("div", { className: "planing-current-story-main planing-poker-left-one-section" },
            react_1.default.createElement("p", null, "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435 \u0442\u0435\u043A\u0443\u0449\u0435\u0439 \u0437\u0430\u0434\u0430\u0447\u0438"),
            react_1.default.createElement("div", null,
                react_1.default.createElement("p", null,
                    "Id: ",
                    story.Id),
                storyBodyRender()),
            adminButton);
    };
    var AddNewStory = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.AddNewStory, props.RoomName, storiesState.NameForAdd, storiesState.DescriptionForAdd);
    };
    var storiesListRender = function () {
        var adminButtonInList = function (id) {
            return react_1.default.createElement(react_1.Fragment, null);
        };
        //todo как то норм назвать
        var addNewForm = react_1.default.createElement("div", null);
        var adminButtonNotInList = react_1.default.createElement("div", null);
        if (props.IsAdmin && !storiesState.ShowOnlyCompleted) {
            adminButtonInList = function (id) {
                return react_1.default.createElement("div", null,
                    react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return props.MakeCurrentStory(id); } }, "\u0421\u0434\u0435\u043B\u0430\u0442\u044C \u0442\u0435\u043A\u0443\u0449\u0435\u0439"),
                    react_1.default.createElement("button", { className: "btn btn-danger", onClick: function () { return props.DeleteStory(id); } }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C"));
            };
            adminButtonNotInList = react_1.default.createElement("div", null,
                react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return AddNewStory(); } }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C"));
            addNewForm = react_1.default.createElement("div", null,
                react_1.default.createElement("p", null, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u043D\u043E\u0432\u0443\u044E:"),
                react_1.default.createElement("span", null, "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435:"),
                react_1.default.createElement("input", { className: "persent-100-width form-control", placeholder: "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435", value: storiesState.NameForAdd, type: "text", onChange: function (e) {
                        setStoriesState(function (prevState) {
                            var newState = __assign({}, prevState);
                            newState.NameForAdd = e.target.value;
                            return newState;
                        });
                    } }),
                react_1.default.createElement("span", null, "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435:"),
                react_1.default.createElement("textarea", { className: "persent-100-width form-control", placeholder: "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435", value: storiesState.DescriptionForAdd, onChange: function (e) {
                        setStoriesState(function (prevState) {
                            var newState = __assign({}, prevState);
                            newState.DescriptionForAdd = e.target.value;
                            return newState;
                        });
                    } }),
                adminButtonNotInList);
        }
        return react_1.default.createElement("div", { className: "planing-stories-list-main planing-poker-left-one-section" },
            react_1.default.createElement("p", null, "\u0418\u0441\u0442\u043E\u0440\u0438\u0438:"),
            react_1.default.createElement("span", null, "\u041F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u0432\u044B\u043F\u043E\u043B\u043D\u0435\u043D\u043D\u044B\u0435: "),
            react_1.default.createElement("input", { onClick: function () {
                    setStoriesState(function (prevState) {
                        var newState = __assign({}, prevState);
                        newState.ShowOnlyCompleted = !newState.ShowOnlyCompleted;
                        return newState;
                    });
                }, type: "checkbox" }),
            react_1.default.createElement("div", null,
                react_1.default.createElement("div", { className: "stories-data-list" }, props.Stories.filter(function (x) { return x.Completed === storiesState.ShowOnlyCompleted; }).map(function (x) { return react_1.default.createElement("div", { className: "planing-story-in-list " + (x.Completed ? "completed-story" : "not-completed-story"), key: x.Id },
                    react_1.default.createElement("p", null,
                        "Id: ",
                        x.Id),
                    react_1.default.createElement("p", null,
                        "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435: ",
                        x.Name),
                    react_1.default.createElement("p", null,
                        "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435: ",
                        x.Description),
                    completedStoryInfo(x),
                    adminButtonInList(x.Id),
                    react_1.default.createElement("hr", null)); })),
                react_1.default.createElement("div", null,
                    react_1.default.createElement("button", { className: "btn btn-primary", onClick: function () { return loadOldStories(); } }, "\u0417\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u043F\u0440\u043E\u0448\u043B\u044B\u0435"))),
            react_1.default.createElement("div", null, addNewForm));
    };
    return react_1.default.createElement("div", null,
        react_1.default.createElement("div", null, currentStoryDescriptionRender()),
        react_1.default.createElement("div", { className: "padding-10-top" }),
        react_1.default.createElement("div", null, storiesListRender()));
};
exports.default = StoriesSection;


/***/ }),

/***/ "./src/components/Body/PlaningPoker/UserInList.tsx":
/*!*********************************************************!*\
  !*** ./src/components/Body/PlaningPoker/UserInList.tsx ***!
  \*********************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
var react_1 = __importStar(__webpack_require__(/*! react */ "react"));
// import { BrowserRouter, Route, Link, Switch } from "react-router-dom";
var RoomInfo_1 = __webpack_require__(/*! ./Models/RoomInfo */ "./src/components/Body/PlaningPoker/Models/RoomInfo.ts");
var UserInListProp = /** @class */ (function () {
    function UserInListProp() {
    }
    return UserInListProp;
}());
var UserInList = function (props) {
    var _a = react_1.useState("-"), selectedEditRole = _a[0], changeSelectedEditRoleState = _a[1];
    // useEffect(() => {
    //     if (props.HubConnected) {
    //     }
    // }, [props.HubConnected]);
    var addNewRoleToUser = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.AddNewRoleToUser, props.RoomName, props.User.Id, selectedEditRole);
    };
    var removeRoleUser = function () {
        props.MyHubConnection.send(G_PlaningPokerController.EndPoints.EndpointsBack.RemoveRoleUser, props.RoomName, props.User.Id, selectedEditRole);
    };
    var delButton = react_1.default.createElement("div", null);
    var statusChange = react_1.default.createElement("div", null);
    if (props.RenderForAdmin) {
        delButton = react_1.default.createElement("div", null,
            react_1.default.createElement("button", { className: "btn btn-danger", onClick: function () { return props.TryToRemoveUserFromRoom(props.User.Id); } }, "\u0412\u044B\u0433\u043D\u0430\u0442\u044C"));
        statusChange = react_1.default.createElement("div", null,
            react_1.default.createElement("select", { className: "form-control", value: selectedEditRole, onChange: function (e) {
                    // changeSelectedEditRoleState(e.target.value)
                    changeSelectedEditRoleState(function (prevState) {
                        return e.target.value;
                    });
                } },
                react_1.default.createElement("option", { value: "-" }, "\u041D\u0435 \u0432\u044B\u0431\u0440\u0430\u043D\u043E"),
                react_1.default.createElement("option", { value: RoomInfo_1.UserRoles.User }, RoomInfo_1.UserRoles.User),
                react_1.default.createElement("option", { value: RoomInfo_1.UserRoles.Admin }, RoomInfo_1.UserRoles.Admin),
                react_1.default.createElement("option", { value: RoomInfo_1.UserRoles.Observer }, RoomInfo_1.UserRoles.Observer)),
            react_1.default.createElement("button", { className: "btn btn-success", onClick: function () { return addNewRoleToUser(); } }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u0440\u043E\u043B\u044C"),
            react_1.default.createElement("button", { className: "btn btn-danger", onClick: function () { return removeRoleUser(); } }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C \u0440\u043E\u043B\u044C"));
    }
    var vote = "отсутствует";
    if (props.User.Vote && !props.HideVote) {
        vote = props.User.Vote + "";
    }
    else if (props.HasVote) {
        vote = "скрыта";
    }
    if (!props.User.CanVote()) {
        vote = "без права голоса";
    }
    var classColorize = " planing-user-not-voted"; //что бы белый не перебивать
    if (props.RoomStatus === RoomInfo_1.RoomStatus.AllCanVote) {
        //подсвечиваем проголосовавших
        if (props.HasVote || !props.User.CanVote()) {
            classColorize = " planing-user-voted";
        }
        else {
            classColorize = " planing-user-not-voted";
        }
    }
    else if (props.RoomStatus === RoomInfo_1.RoomStatus.CloseVote) {
        //подсвечиваем min max
        if (props.User.Vote) {
            if (props.MinVote === props.User.Vote) {
                classColorize = " planing-user-voted-min";
            }
            else if (props.MaxVote === props.User.Vote) {
                classColorize = " planing-user-voted-max";
            }
        }
    }
    props.HasVote;
    return react_1.default.createElement("div", null,
        react_1.default.createElement("div", { className: "planing-user" + classColorize },
            react_1.default.createElement("p", null, props.User.Name),
            react_1.default.createElement("p", null,
                "\u043E\u0446\u0435\u043D\u043A\u0430: ",
                vote),
            delButton,
            react_1.default.createElement("p", null,
                "\u0420\u043E\u043B\u0438: ",
                props.User.Roles.join(',')),
            statusChange),
        react_1.default.createElement("div", { className: "padding-10-top" }));
};
exports.default = UserInList;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/ForceNew/OneCard.tsx":
/*!****************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/ForceNew/OneCard.tsx ***!
  \****************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneCard = exports.CreateCardEdit = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var CreateCardEdit = /** @class */ (function () {
    function CreateCardEdit() {
        this.Id = -1;
        this.Word = "";
        this.WordAnswer = "";
        this.Description = "";
        this.MainImageSave = null;
    }
    return CreateCardEdit;
}());
exports.CreateCardEdit = CreateCardEdit;
var OneCard = /** @class */ (function (_super) {
    __extends(OneCard, _super);
    function OneCard(props) {
        return _super.call(this, props) || this;
        // this.AddNewTemplate = this.AddNewTemplate.bind(this);
    }
    // RenderCard(x: CreateCardState): JSX.Element {
    //     return <div key={x.Id}>
    //         <input type="text" onChange={this.WordOnChange} value={x.Word}></input>
    //         <input type="text" onChange={this.WordAnswerOnChange} value={x.WordAnswer}></input>
    //         <input type="text" onChange={this.WordDescriptionOnChange} value={x.Description}></input>
    //     </div>
    // }
    OneCard.prototype.render = function () {
        var _this = this;
        return React.createElement("div", { className: "force-add-one-card padding-10-top col-sm-12" },
            React.createElement("div", { className: "force-add-one-card-inner", key: this.props.Card.Id },
                React.createElement("div", { className: "padding-10-top" },
                    React.createElement("input", { className: "form-control", type: "text", placeholder: "\u0441\u043B\u043E\u0432\u043E", onChange: function (e) { _this.props.WordOnChange(_this.props.Card.Id, e); }, value: this.props.Card.Word })),
                React.createElement("div", { className: "padding-10-top" },
                    React.createElement("input", { className: "form-control", type: "text", placeholder: "\u043E\u0442\u0432\u0435\u0442", onChange: function (e) { _this.props.WordAnswerOnChange(_this.props.Card.Id, e); }, value: this.props.Card.WordAnswer })),
                React.createElement("div", { className: "padding-10-top" },
                    React.createElement("input", { className: "form-control", type: "text", placeholder: "\u043E\u043F\u0438\u0441\u0430\u043D\u0438\u0435", onChange: function (e) { _this.props.WordDescriptionOnChange(_this.props.Card.Id, e); }, value: this.props.Card.Description }))));
    };
    return OneCard;
}(React.Component));
exports.OneCard = OneCard;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/ForceNew/WordsCardsForceAdd.tsx":
/*!***************************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/ForceNew/WordsCardsForceAdd.tsx ***!
  \***************************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsForceAdd = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var AlertData_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var OneWordList_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/WordsCardsApp/OneWordList */ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordList.ts");
var OneCard_1 = __webpack_require__(/*! ./OneCard */ "./src/components/Body/WordsCardsApp/ForceNew/OneCard.tsx");
// export class CreateCardState {
//     // Id: number;
//     Word: string;
//     WordAnswer: string;
//     Description: string;
//     MainImageSave?: File;//не хранится тут, проставляется при редактировании
// }
var WordsCardsForceAdd = /** @class */ (function (_super) {
    __extends(WordsCardsForceAdd, _super);
    function WordsCardsForceAdd(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            Cards: [],
            MaxId: 0,
            WordLists: [],
            ListsLoaded: false,
            SelectedList: -1,
        };
        _this.AddNewTemplate = _this.AddNewTemplate.bind(_this);
        _this.WordOnChange = _this.WordOnChange.bind(_this);
        _this.WordAnswerOnChange = _this.WordAnswerOnChange.bind(_this);
        _this.WordDescriptionOnChange = _this.WordDescriptionOnChange.bind(_this);
        _this.GetById = _this.GetById.bind(_this);
        _this.SaveAll = _this.SaveAll.bind(_this);
        _this.LoadAllWordLists = _this.LoadAllWordLists.bind(_this);
        _this.ListOnChange = _this.ListOnChange.bind(_this);
        return _this;
    }
    WordsCardsForceAdd.prototype.componentDidMount = function () {
        this.LoadAllWordLists();
    };
    WordsCardsForceAdd.prototype.AddNewTemplate = function () {
        var newState = __assign({}, this.state);
        var newCard = new OneCard_1.CreateCardEdit();
        newCard.Id = ++newState.MaxId;
        newState.Cards.push(newCard);
        this.setState(newState);
    };
    WordsCardsForceAdd.prototype.WordOnChange = function (id, e) {
        var newState = __assign({}, this.state);
        var item = this.GetById(newState, id);
        if (item == null) {
            return;
        }
        item.Word = e.target.value;
        this.setState(newState);
    };
    WordsCardsForceAdd.prototype.WordAnswerOnChange = function (id, e) {
        var newState = __assign({}, this.state);
        var item = this.GetById(newState, id);
        if (item == null) {
            return;
        }
        item.WordAnswer = e.target.value;
        this.setState(newState);
    };
    WordsCardsForceAdd.prototype.WordDescriptionOnChange = function (id, e) {
        var newState = __assign({}, this.state);
        var item = this.GetById(newState, id);
        if (item == null) {
            return;
        }
        item.Description = e.target.value;
        this.setState(newState);
    };
    WordsCardsForceAdd.prototype.SaveAll = function () {
        var _this = this;
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            newState.Cards = [];
            _this.setState(newState);
            var alertL = new AlertData_1.AlertData();
            alertL.Text = "Сохранено";
            alertL.Type = AlertData_1.AlertTypeEnum.Success;
            G_AddAbsoluteAlertToState(alertL);
        };
        G_WordsCardsController.CreateList(this.state.Cards, this.state.SelectedList + '', success);
        // let data = new FormData();
        // for (let i = 0; i < this.state.Cards.length; ++i) {
        //     data.append('newData[' + i + '].word', this.state.Cards[i].Word);
        //     data.append('newData[' + i + '].word_answer', this.state.Cards[i].WordAnswer);
        //     data.append('newData[' + i + '].description', this.state.Cards[i].Description);
        //     data.append('newData[' + i + '].list_id', this.state.SelectedList + '');
        //     // data.append('newData.word_answer', this.state.Cards[i].WordAnswer);
        //     // data.append('newData.description', this.state.Cards[i].Description);
        // }
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let res = xhr as IOneWordCardBack[];
        //             if (res.length > 0) {
        //                 let newState = { ...refThis.state };
        //                 newState.Cards = [];
        //                 this.setState(newState);
        //                 let alertL = new AlertData();
        //                 alertL.Text = "Сохранено";
        //                 alertL.Type = AlertTypeEnum.Success;
        //                 G_AddAbsoluteAlertToState(alertL);
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/create-list',
        // }, true);
    };
    WordsCardsForceAdd.prototype.LoadAllWordLists = function () {
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var dataFront = [];
            data.forEach(function (bk) {
                var nd = new OneWordList_1.OneWordList();
                nd.FillByBackModel(bk);
                dataFront.push(nd);
            });
            newState.ListsLoaded = true;
            newState.WordLists = dataFront;
            refThis.setState(newState);
        };
        G_WordsListController.GetAllForUser(success);
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: {},
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack[];
        //             if (dataBack.length > 0) {
        //                 let newState = { ...refThis.state };
        //                 let dataFront: OneWordList[] = [];
        //                 dataBack.forEach(bk => {
        //                     let nd = new OneWordList();
        //                     nd.FillByBackModel(bk);
        //                     dataFront.push(nd);
        //                 });
        //                 newState.ListsLoaded = true;
        //                 newState.WordLists = dataFront;
        //                 this.setState(newState);
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/get-all-for-user',
        // }, true);
    };
    WordsCardsForceAdd.prototype.ListOnChange = function (e) {
        // console.log(e);
        var newState = __assign({}, this.state);
        newState.SelectedList = +e.target.value;
        this.setState(newState);
    };
    WordsCardsForceAdd.prototype.render = function () {
        var _this = this;
        var listSelect = React.createElement("div", null);
        if (this.state.ListsLoaded) {
            listSelect = React.createElement("div", null,
                React.createElement("select", { value: this.state.SelectedList, onChange: this.ListOnChange },
                    React.createElement("option", { key: -1, value: -1 }, "\u0411\u0435\u0437 \u0441\u043F\u0438\u0441\u043A\u0430"),
                    this.state.WordLists.map(function (x) { return React.createElement("option", { key: x.Id, value: x.Id }, x.Title); })));
        }
        return React.createElement("div", { className: "container" },
            React.createElement("div", { className: "row" },
                listSelect,
                React.createElement("div", { className: "force-add-cards-list" }, this.state.Cards.map(function (x, index) {
                    return React.createElement(OneCard_1.OneCard, { Card: x, key: x.Id, WordOnChange: _this.WordOnChange, WordAnswerOnChange: _this.WordAnswerOnChange, WordDescriptionOnChange: _this.WordDescriptionOnChange });
                })),
                React.createElement("div", { className: "col-sm-12" },
                    React.createElement("button", { className: "btn btn-primary", onClick: this.AddNewTemplate }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u043D\u043E\u0432\u044B\u0439 \u0448\u0430\u0431\u043B\u043E\u043D"),
                    React.createElement("button", { className: "btn btn-primary", onClick: this.SaveAll }, "\u0421\u043E\u0445\u0440\u0430\u043D\u0438\u0442\u044C \u0432\u0441\u0435"),
                    React.createElement("div", { className: "padding-10-top" },
                        React.createElement(react_router_dom_1.Link, { to: "/words-cards-app" }, "\u043F\u0435\u0440\u0435\u0439\u0442\u0438 \u043A \u0441\u043B\u043E\u0432\u0430\u0440\u044E \u0431\u0435\u0437 \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F")))));
    };
    WordsCardsForceAdd.prototype.GetById = function (st, id) {
        for (var i = 0; i < st.Cards.length; ++i) {
            if (st.Cards[i].Id == id) {
                return st.Cards[i];
            }
        }
        return null;
    };
    return WordsCardsForceAdd;
}(React.Component));
exports.WordsCardsForceAdd = WordsCardsForceAdd;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/OneWordCard.tsx":
/*!***********************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/OneWordCard.tsx ***!
  \***********************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneWordCard = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var WordActions_1 = __webpack_require__(/*! ./WordActions */ "./src/components/Body/WordsCardsApp/WordActions.tsx");
var OneWordCard = /** @class */ (function (_super) {
    __extends(OneWordCard, _super);
    function OneWordCard(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            AlwaysShowWordAnswer: false,
            AlwaysShowWordImage: false,
            AlwaysShowWord: false,
            WriteMode: false,
            CurrentCardNewSelectedList: -1,
            ShowCrdsListInfo: false,
            // WordGoodWrited: false,
            // ShowCurrentWordAnswer: false,
            // ShowCurrentWordImage: false,
        };
        _this.ChangeAlwaysShowWordAnswer = _this.ChangeAlwaysShowWordAnswer.bind(_this);
        _this.ChangeAlwaysShowWordImage = _this.ChangeAlwaysShowWordImage.bind(_this);
        _this.ChangeAlwaysShowWord = _this.ChangeAlwaysShowWord.bind(_this);
        _this.RenderCardBody = _this.RenderCardBody.bind(_this);
        _this.WriteModeOnClick = _this.WriteModeOnClick.bind(_this);
        _this.NewAddListOnChange = _this.NewAddListOnChange.bind(_this);
        // this.WordInputCompare = this.WordInputCompare.bind(this);
        _this.AddCardToList = _this.AddCardToList.bind(_this);
        _this.RemoveFromList = _this.RemoveFromList.bind(_this);
        _this.ChangeShowCardLists = _this.ChangeShowCardLists.bind(_this);
        return _this;
    }
    OneWordCard.prototype.ChangeAlwaysShowWordImage = function () {
        var newState = __assign({}, this.state);
        newState.AlwaysShowWordImage = !newState.AlwaysShowWordImage;
        this.setState(newState);
    };
    OneWordCard.prototype.ChangeShowCardLists = function () {
        var newState = __assign({}, this.state);
        newState.ShowCrdsListInfo = !newState.ShowCrdsListInfo;
        this.setState(newState);
    };
    OneWordCard.prototype.ChangeAlwaysShowWordAnswer = function () {
        var newState = __assign({}, this.state);
        newState.AlwaysShowWordAnswer = !newState.AlwaysShowWordAnswer;
        this.setState(newState);
    };
    OneWordCard.prototype.ChangeAlwaysShowWord = function () {
        var newState = __assign({}, this.state);
        newState.AlwaysShowWord = !newState.AlwaysShowWord;
        this.setState(newState);
    };
    OneWordCard.prototype.WriteModeOnClick = function () {
        var newState = __assign({}, this.state);
        newState.WriteMode = !newState.WriteMode;
        this.setState(newState);
    };
    OneWordCard.prototype.NewAddListOnChange = function (e) {
        // console.log(e);
        var newState = __assign({}, this.state);
        newState.CurrentCardNewSelectedList = +e.target.value;
        this.setState(newState);
    };
    OneWordCard.prototype.AddCardToList = function () {
        this.props.AddCardToList(this.props.CurrentCard.Id, this.state.CurrentCardNewSelectedList);
    };
    OneWordCard.prototype.RemoveFromList = function (listId) {
        this.props.RemoveFromList(this.props.CurrentCard.Id, listId);
    };
    OneWordCard.prototype.RenderCardBody = function () {
        var _this = this;
        if (this.props.EditCurrentCard) {
            var editTitle = React.createElement("p", null, "\u0420\u0435\u0434\u0430\u043A\u0442\u0438\u0440\u043E\u0432\u0430\u043D\u0438\u0435");
            if (this.props.EditCurrentCard.Id < 1) {
                editTitle = React.createElement("p", null, "\u0421\u043E\u0437\u0434\u0430\u043D\u0438\u0435");
            }
            return React.createElement("div", null,
                editTitle,
                React.createElement("label", null, "\u0421\u043B\u043E\u0432\u043E"),
                React.createElement("input", { className: "form-control", onChange: this.props.WordOnChange, type: "text", value: this.props.EditCurrentCard.Word }),
                React.createElement("br", null),
                React.createElement("label", null, "\u041E\u0442\u0432\u0435\u0442"),
                React.createElement("input", { className: "form-control", onChange: this.props.WordAnswerOnChange, type: "text", value: this.props.EditCurrentCard.WordAnswer }),
                React.createElement("br", null),
                React.createElement("label", null, "\u041E\u043F\u0438\u0441\u0430\u043D\u0438\u0435"),
                React.createElement("input", { className: "form-control", onChange: this.props.WordDescriptionOnChange, type: "text", value: this.props.EditCurrentCard.Description }),
                React.createElement("br", null),
                React.createElement("input", { className: "form-control", id: "main_image_input", type: "file" }),
                React.createElement("br", null),
                React.createElement("label", null, "\u0443\u0434\u0430\u043B\u0438\u0442\u044C \u043A\u0430\u0440\u0442\u0438\u043D\u043A\u0443"),
                React.createElement("input", { onClick: this.props.DeleteMainImageOnClick, type: 'checkbox' }));
        }
        if (!this.props.CurrentCard) {
            return React.createElement("div", null);
        }
        var word = React.createElement("div", null, "\u0441\u043B\u043E\u0432\u043E \u0441\u043A\u0440\u044B\u0442\u043E");
        if (this.state.AlwaysShowWord || this.props.ShowCurrentWord) {
            word = React.createElement("div", null,
                React.createElement("p", null,
                    "\u0421\u043B\u043E\u0432\u043E - ",
                    this.props.CurrentCard.Word));
        }
        var wordAnswer = React.createElement("div", null, "\u043E\u0442\u0432\u0435\u0442 \u0441\u043A\u0440\u044B\u0442");
        if (this.state.AlwaysShowWordAnswer || this.props.ShowCurrentWordAnswer) {
            wordAnswer = React.createElement("div", null,
                React.createElement("p", null,
                    "\u0421\u043B\u043E\u0432\u043E \u043E\u0442\u0432\u0435\u0442 - ",
                    this.props.CurrentCard.WordAnswer),
                React.createElement("p", null,
                    "\u0421\u043B\u043E\u0432\u043E \u043E\u043F\u0438\u0441\u0430\u043D\u0438\u0435 - ",
                    this.props.CurrentCard.Description));
        }
        var imageRender = React.createElement("div", null);
        if (this.props.CurrentCard.ImagePath) {
            imageRender = React.createElement("div", null, "\u0438\u0437\u043E\u0431\u0440\u0430\u0436\u0435\u043D\u0438\u0435 \u0441\u043A\u0440\u044B\u0442\u043E");
            if (this.state.AlwaysShowWordImage || this.props.ShowCurrentWordImage) {
                imageRender = React.createElement("div", { className: "one-word-card-image-line" },
                    React.createElement("div", { className: "one-word-card-image" },
                        React.createElement("img", { className: "persent-100-width-height", src: this.props.CurrentCard.ImagePath, alt: "" })));
            }
        }
        var hiddenWord = React.createElement("div", null);
        if (this.props.CurrentCard.Hided) {
            hiddenWord = React.createElement("p", { className: "word-card-hidden-status" }, "\u0421\u043B\u043E\u0432\u043E \u0441\u043A\u0440\u044B\u0442\u043E");
        }
        var writeWord = React.createElement("div", null);
        var writeWordClass = "alert-danger";
        if (this.WordInputCompare(this.props.WriteTestString)) {
            writeWordClass = "alert-success";
        }
        if (this.state.WriteMode) {
            writeWord = React.createElement("div", { className: "" },
                React.createElement("input", { className: "" + writeWordClass, onChange: this.props.WriteTestChanged, placeholder: "\u0432\u0432\u0435\u0434\u0438\u0442\u0435 \u0441\u043B\u043E\u0432\u043E", type: "text", value: this.props.WriteTestString }));
        }
        var wordListActions = React.createElement("div", null,
            React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.ChangeShowCardLists }, "\u0421\u043F\u0438\u0441\u043A\u0438"));
        if (this.state.ShowCrdsListInfo) {
            //if (this.props.CurrentCard.Lists) {//TODO тут бы еще какую то кнопку мб, что бы не рисовать всегда
            var listSelect = React.createElement("div", null,
                React.createElement("select", { value: this.state.CurrentCardNewSelectedList, onChange: this.NewAddListOnChange },
                    React.createElement("option", { key: -1, value: -1 }, "\u0411\u0435\u0437 \u0441\u043F\u0438\u0441\u043A\u0430"),
                    this.props.WordLists.map(function (x) { return React.createElement("option", { key: x.Id, value: x.Id }, x.Title); })),
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.AddCardToList }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u043D\u043E\u0432\u044B\u0439"));
            wordListActions = React.createElement("div", null,
                React.createElement("p", null, "\u0421\u043F\u0438\u0441\u043A\u0438:"),
                this.props.CurrentCard.Lists.map(function (x) {
                    var lst = _this.props.WordLists.find(function (x1) { return x1.Id == x.IdList; });
                    if (lst) {
                        return React.createElement("div", { key: lst.Id },
                            lst.Title,
                            React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: function () { return _this.RemoveFromList(lst.Id); } }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C \u0438\u0437 \u0441\u043F\u0438\u0441\u043A\u0430"));
                    }
                }),
                listSelect,
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.ChangeShowCardLists }, "\u0421\u043F\u0438\u0441\u043A\u0438"));
            // }
        }
        return React.createElement("div", null,
            word,
            React.createElement("hr", null),
            wordAnswer,
            React.createElement("hr", null),
            imageRender,
            hiddenWord,
            writeWord,
            wordListActions);
    };
    OneWordCard.prototype.render = function () {
        return React.createElement("div", { className: "word-card-card-main col-md-8" },
            React.createElement("div", { className: "word-card-card-inner" }, this.RenderCardBody()),
            React.createElement("div", { className: "padding-10-top" }),
            React.createElement(WordActions_1.WordActions, { ChangeAlwaysShowWordImage: this.ChangeAlwaysShowWordImage, ChangeAlwaysShowWordAnswer: this.ChangeAlwaysShowWordAnswer, ChangeAlwaysShowWord: this.ChangeAlwaysShowWord, WriteModeOnClick: this.WriteModeOnClick, ChangeShowCurrentWordImage: this.props.ChangeShowCurrentWordImage, ChangeShowCurrentWordAnswer: this.props.ChangeShowCurrentWordAnswer, ChangeShowCurrentWord: this.props.ChangeShowCurrentWord, SearchStrChanged: this.props.SearchStrChanged, SearchedString: this.props.SearchedString, StartEditCard: this.props.StartEditCard, CancelEditCard: this.props.CancelEditCard, SaveCard: this.props.SaveCard, AddNewTemplate: this.props.AddNewTemplate, ShowNextCard: this.props.ShowNextCard, ShowHiddenCardsOnClick: this.props.ShowHiddenCardsOnClick, ChangeVisibilityCurrentCard: this.props.ChangeVisibilityCurrentCard, ShuffleCardsOnClick: this.props.ShuffleCardsOnClick, DeleteCurrentCard: this.props.DeleteCurrentCard, EditTemplateViewNow: this.props.EditTemplateViewNow, WordLists: this.props.WordLists, SelectedList: this.props.SelectedList, ListOnChange: this.props.ListOnChange }));
    };
    OneWordCard.prototype.WordInputCompare = function (str) {
        var wordForCompare = this.props.CurrentCard.Word;
        if (this.state.AlwaysShowWord) {
            wordForCompare = this.props.CurrentCard.WordAnswer;
        }
        if (str.toUpperCase() == wordForCompare.toUpperCase()) {
            return true;
        }
        else {
            return false;
        }
    };
    return OneWordCard;
}(React.Component));
exports.OneWordCard = OneWordCard;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/OneWordCardInList.tsx":
/*!*****************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/OneWordCardInList.tsx ***!
  \*****************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneWordCardInList = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneWordCardInList = /** @class */ (function (_super) {
    __extends(OneWordCardInList, _super);
    function OneWordCardInList(props) {
        return _super.call(this, props) || this;
    }
    OneWordCardInList.prototype.render = function () {
        var _this = this;
        var selectedClassName = "word-in-list";
        if (this.props.Selected) {
            selectedClassName += " word-in-list-selected";
        }
        return React.createElement("div", { onClick: function () { _this.props.OnSelectedCard(_this.props.Card.Id); }, className: selectedClassName }, this.props.Card.Word);
    };
    return OneWordCardInList;
}(React.Component));
exports.OneWordCardInList = OneWordCardInList;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/WordActions.tsx":
/*!***********************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/WordActions.tsx ***!
  \***********************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordActions = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var WordActions = /** @class */ (function (_super) {
    __extends(WordActions, _super);
    function WordActions(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            ShowMoreActions: false,
        };
        _this.ShowMoreAction = _this.ShowMoreAction.bind(_this);
        _this.DosnloadFile = _this.DosnloadFile.bind(_this);
        return _this;
    }
    WordActions.prototype.ShowMoreAction = function () {
        var newState = __assign({}, this.state);
        newState.ShowMoreActions = !newState.ShowMoreActions;
        this.setState(newState);
    };
    WordActions.prototype.DosnloadFile = function () {
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: {},
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/download-all-words-file',
        // }, true);
        G_WordsCardsController.DownloadAllWordsFile();
        // window.open("/api/wordscards/download-all-words-file");
    };
    WordActions.prototype.render = function () {
        // let buttons: JSX.Element;
        var buttons = React.createElement("div", null,
            React.createElement("button", { className: "btn btn-primary btn-sm", onClick: this.props.ShowNextCard }, "\u0441\u043B\u0435\u0434\u0443\u044E\u0449\u0435\u0435 \u0441\u043B\u043E\u0432\u043E"),
            React.createElement("button", { className: "btn btn-primary btn-sm", onClick: this.props.ChangeShowCurrentWordImage }, "\u043F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u043A\u0430\u0440\u0442\u0438\u043D\u043A\u0443"),
            React.createElement("button", { className: "btn btn-primary btn-sm", onClick: this.props.ChangeShowCurrentWord }, "\u043F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u0441\u043B\u043E\u0432\u043E"),
            React.createElement("button", { className: "btn btn-primary btn-sm", onClick: this.props.ChangeShowCurrentWordAnswer }, "\u043F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u043E\u0442\u0432\u0435\u0442"),
            React.createElement("button", { className: "btn btn-dark btn-sm", onClick: this.props.ChangeVisibilityCurrentCard }, "\u0438\u0437\u043C\u0435\u043D\u0438\u0442\u044C \u0432\u0438\u0434\u0438\u043C\u043E\u0441\u0442\u044C"),
            React.createElement("input", { className: "form-control", onChange: this.props.SearchStrChanged, type: "text", placeholder: "\u043F\u043E\u0438\u0441\u043A...", value: this.props.SearchedString }));
        var editsButtons = React.createElement("div", null);
        if (this.props.EditTemplateViewNow) {
            editsButtons = React.createElement("div", null,
                React.createElement("button", { className: "btn btn-success btn-sm", onClick: this.props.SaveCard }, "\u0441\u043E\u0445\u0440\u0430\u043D\u0438\u0442\u044C \u0438\u0437\u043C\u0435\u043D\u0435\u043D\u0438\u044F"),
                React.createElement("button", { className: "btn btn-danger btn-sm", onClick: this.props.CancelEditCard }, "\u043E\u0442\u043C\u0435\u043D\u0438\u0442\u044C \u0438\u0437\u043C\u0435\u043D\u0435\u043D\u0438\u044F"));
        }
        else {
            editsButtons = React.createElement("div", null,
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.props.StartEditCard }, "\u0440\u0435\u0434\u0430\u043A\u0442\u0438\u0440\u043E\u0432\u0430\u0442\u044C"));
        }
        var listSelect = React.createElement("div", null);
        if (this.props.WordLists) {
            listSelect = React.createElement("div", null,
                React.createElement("select", { value: this.props.SelectedList, onChange: this.props.ListOnChange },
                    React.createElement("option", { key: -1, value: -1 }, "\u0411\u0435\u0437 \u0441\u043F\u0438\u0441\u043A\u0430"),
                    this.props.WordLists.map(function (x) { return React.createElement("option", { key: x.Id, value: x.Id }, x.Title); })));
        }
        // let listsActions = <div></div>
        if (this.state.ShowMoreActions) {
            buttons = React.createElement("div", null,
                buttons,
                React.createElement("hr", null),
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.props.AddNewTemplate }, "\u041F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u043D\u043E\u0432\u044B\u0439 \u0448\u0430\u0431\u043B\u043E\u043D"),
                React.createElement("button", { className: "btn btn-secondary btn-sm" }, "\u0417\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044C \u0444\u0430\u0439\u043B"),
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.DosnloadFile }, "\u0421\u043A\u0430\u0447\u0430\u0442\u044C \u0444\u0430\u0439\u043B"),
                React.createElement("button", { className: "btn btn-secondary btn-sm", onClick: this.props.ShuffleCardsOnClick }, "\u043F\u0435\u0440\u0435\u043C\u0435\u0448\u0430\u0442\u044C"),
                editsButtons,
                React.createElement("button", { className: "btn btn-danger btn-sm", onClick: this.props.DeleteCurrentCard }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C"),
                React.createElement("label", null, "\u0412\u0441\u0435\u0433\u0434\u0430 \u043E\u0442\u043E\u0431\u0440\u0430\u0436\u0430\u0442\u044C \u0441\u043B\u043E\u0432\u043E"),
                React.createElement("input", { onClick: this.props.ChangeAlwaysShowWord, type: "checkbox" }),
                React.createElement("label", null, "\u0412\u0441\u0435\u0433\u0434\u0430 \u043E\u0442\u043E\u0431\u0440\u0430\u0436\u0430\u0442\u044C \u043E\u0442\u0432\u0435\u0442 \u043D\u0430 \u0441\u043B\u043E\u0432\u043E"),
                React.createElement("input", { onClick: this.props.ChangeAlwaysShowWordAnswer, type: "checkbox" }),
                React.createElement("label", null, "\u0412\u0441\u0435\u0433\u0434\u0430 \u043E\u0442\u043E\u0431\u0440\u0430\u0436\u0430\u0442\u044C \u0438\u0437\u043E\u0431\u0440\u0430\u0436\u0435\u043D\u0438\u0435"),
                React.createElement("input", { onClick: this.props.ChangeAlwaysShowWordImage, type: "checkbox" }),
                React.createElement("label", null, "\u041F\u043E\u043A\u0430\u0437\u0430\u0442\u044C \u0441\u043F\u0440\u044F\u0442\u0430\u043D\u043D\u044B\u0435"),
                React.createElement("input", { type: "checkbox", onClick: this.props.ShowHiddenCardsOnClick }),
                React.createElement("label", null, "\u0420\u0435\u0436\u0438\u043C \u043F\u0438\u0441\u044C\u043C\u0430"),
                React.createElement("input", { type: "checkbox", onClick: this.props.WriteModeOnClick }),
                React.createElement("hr", null),
                listSelect,
                React.createElement("hr", null),
                React.createElement(react_router_dom_1.Link, { to: "/words-cards-app/force-add" }, "\u043F\u0435\u0440\u0435\u0439\u0442\u0438 \u0432 \u0440\u0435\u0436\u0438\u043C \u0443\u0441\u043A\u043E\u0440\u0435\u043D\u043D\u043E\u0433\u043E \u0434\u043E\u0431\u0430\u0432\u043B\u0435\u043D\u0438\u044F"),
                React.createElement("br", null),
                React.createElement(react_router_dom_1.Link, { to: "/words-cards-app/word-list" }, "\u0440\u0430\u0431\u043E\u0442\u0430 \u0441 \u0441\u043F\u0438\u0441\u043A\u0430\u043C\u0438 \u0441\u043B\u043E\u0432"));
            // listsActions = <div>
            // </div>
        }
        else {
        }
        return React.createElement("div", { className: "words-cards-list-actions" },
            buttons,
            React.createElement("button", { className: "btn btn-info btn-sm", onClick: this.ShowMoreAction }, "\u0431\u043E\u043B\u044C\u0448\u0435 \u0434\u0435\u0439\u0441\u0442\u0432\u0438\u0439"));
    };
    return WordActions;
}(React.Component));
exports.WordActions = WordActions;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/WordsCardsAppMain.tsx":
/*!*****************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/WordsCardsAppMain.tsx ***!
  \*****************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsAppMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var WordsCardsForceAdd_1 = __webpack_require__(/*! ./ForceNew/WordsCardsForceAdd */ "./src/components/Body/WordsCardsApp/ForceNew/WordsCardsForceAdd.tsx");
var WordsCardsListMain_1 = __webpack_require__(/*! ./WordsCardsListMain */ "./src/components/Body/WordsCardsApp/WordsCardsListMain.tsx");
var WordsCardsListWork_1 = __webpack_require__(/*! ./WordsList/WordsCardsListWork */ "./src/components/Body/WordsCardsApp/WordsList/WordsCardsListWork.tsx");
var WordsCardsAppMain = /** @class */ (function (_super) {
    __extends(WordsCardsAppMain, _super);
    function WordsCardsAppMain(props) {
        return _super.call(this, props) || this;
    }
    WordsCardsAppMain.prototype.render = function () {
        // return <WordsCardsListMain/>
        return React.createElement(react_router_dom_1.Switch, null,
            React.createElement(react_router_dom_1.Route, { exact: true, path: "/words-cards-app", component: WordsCardsListMain_1.WordsCardsListMain }),
            React.createElement(react_router_dom_1.Route, { path: "/words-cards-app/force-add", component: WordsCardsForceAdd_1.WordsCardsForceAdd }),
            React.createElement(react_router_dom_1.Route, { path: "/words-cards-app/word-list", component: WordsCardsListWork_1.WordsCardsListWork }));
    };
    return WordsCardsAppMain;
}(React.Component));
exports.WordsCardsAppMain = WordsCardsAppMain;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/WordsCardsList.tsx":
/*!**************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/WordsCardsList.tsx ***!
  \**************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsList = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneWordCardInList_1 = __webpack_require__(/*! ./OneWordCardInList */ "./src/components/Body/WordsCardsApp/OneWordCardInList.tsx");
var WordsCardsList = /** @class */ (function (_super) {
    __extends(WordsCardsList, _super);
    function WordsCardsList(props) {
        var _this = _super.call(this, props) || this;
        _this.state = { SearchedString: null };
        return _this;
        // this.SearchStrChande = this.SearchStrChande.bind(this);
    }
    WordsCardsList.prototype.render = function () {
        var _this = this;
        //offset-md-1
        return React.createElement("div", { className: "word-card-cards-list-main col-md-4" },
            React.createElement("div", { className: "word-card-cards-list-inner" }, this.props.CardList.map(function (x) {
                var _a;
                return React.createElement(OneWordCardInList_1.OneWordCardInList, { OnSelectedCard: _this.props.OnSelectedCard, Card: x, key: x.Id, Selected: x.Id == ((_a = _this.props.CurrentCard) === null || _a === void 0 ? void 0 : _a.Id) });
            })));
    };
    return WordsCardsList;
}(React.Component));
exports.WordsCardsList = WordsCardsList;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/WordsCardsListMain.tsx":
/*!******************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/WordsCardsListMain.tsx ***!
  \******************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsListMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var OneWordCard_1 = __webpack_require__(/*! ./OneWordCard */ "./src/components/Body/WordsCardsApp/OneWordCard.tsx");
var WordsCardsList_1 = __webpack_require__(/*! ./WordsCardsList */ "./src/components/Body/WordsCardsApp/WordsCardsList.tsx");
var OneWordCard_2 = __webpack_require__(/*! ../../_ComponentsLink/Models/WordsCardsApp/OneWordCard */ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordCard.ts");
var AlertData_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var OneWordList_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/WordsCardsApp/OneWordList */ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordList.ts");
var WordCardWordList_1 = __webpack_require__(/*! ../../_ComponentsLink/Models/WordsCardsApp/WordCardWordList */ "./src/components/_ComponentsLink/Models/WordsCardsApp/WordCardWordList.ts");
var WordsCardsListMain = /** @class */ (function (_super) {
    __extends(WordsCardsListMain, _super);
    function WordsCardsListMain(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            Cards: [],
            CurrentCard: null,
            EditCurrentCard: null,
            ShowCurrentWordAnswer: false,
            ShowCurrentWordImage: false,
            ShowCurrentWord: false,
            SearchedString: "",
            CardsLoaded: false,
            ShowHidenCard: false,
            WriteTestString: "",
            WordLists: [],
            WordListSelected: -1,
            // WordListsLoaded:false,
        };
        // for (let i = 0; i < 20; ++i) {
        //     let card1 = new OneWordCardModel();
        //     card1.Description = "description" + i;
        //     card1.Id = i;
        //     card1.Word = "words" + i;
        //     card1.WordAnswer = "WordAnswer" + i;
        //     this.state.Cards.push(card1);
        // }
        //действия над словами с бэком:
        //добавить, удалить, получить список, обновить, спрятать, загрузить файл, скачать файл
        //без бэка
        //показать\спрятать: описание слово кратинку, перемешать, показать спрятанные, поиск
        _this.ChangeShowCurrentWordAnswer = _this.ChangeShowCurrentWordAnswer.bind(_this);
        _this.ChangeShowCurrentWord = _this.ChangeShowCurrentWord.bind(_this);
        _this.ChangeShowCurrentWordImage = _this.ChangeShowCurrentWordImage.bind(_this);
        _this.OnSelectedCard = _this.OnSelectedCard.bind(_this);
        _this.SearchStrChanged = _this.SearchStrChanged.bind(_this);
        _this.StartEditCard = _this.StartEditCard.bind(_this);
        _this.CancelEditCard = _this.CancelEditCard.bind(_this);
        _this.SaveCard = _this.SaveCard.bind(_this);
        _this.AddNewTemplate = _this.AddNewTemplate.bind(_this);
        _this.WordOnChange = _this.WordOnChange.bind(_this);
        _this.WordAnswerOnChange = _this.WordAnswerOnChange.bind(_this);
        _this.WordDescriptionOnChange = _this.WordDescriptionOnChange.bind(_this);
        _this.ShowNextCard = _this.ShowNextCard.bind(_this);
        _this.DeleteMainImageOnClick = _this.DeleteMainImageOnClick.bind(_this);
        _this.ShowHiddenCardsOnClick = _this.ShowHiddenCardsOnClick.bind(_this);
        _this.ChangeVisibilityCurrentCard = _this.ChangeVisibilityCurrentCard.bind(_this);
        _this.ShuffleCardsOnClick = _this.ShuffleCardsOnClick.bind(_this);
        _this.DeleteCurrentCard = _this.DeleteCurrentCard.bind(_this);
        _this.WriteTestChanged = _this.WriteTestChanged.bind(_this);
        _this.LoadAllWordLists = _this.LoadAllWordLists.bind(_this);
        _this.ListOnChange = _this.ListOnChange.bind(_this);
        _this.AddCardToList = _this.AddCardToList.bind(_this);
        _this.RemoveFromList = _this.RemoveFromList.bind(_this);
        return _this;
    }
    WordsCardsListMain.prototype.componentDidMount = function () {
        this.LoadAllWordLists();
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var dataFront = [];
            data.forEach(function (bk) {
                var nd = new OneWordCard_2.OneWordCard();
                nd.FillByBackModel(bk);
                dataFront.push(nd);
            });
            refThis.setState({
                Cards: dataFront,
                CardsLoaded: true,
                // FollowedCards: followed,
                // NotFollowedCards: notFollowed,
            });
        };
        G_WordsCardsController.GetAllForUser(success);
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: {},
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             // console.log(xhr);
        //             let dataBack = xhr as IOneWordCardBack[];
        //             let dataFront: OneWordCardModel[] = [];
        //             dataBack.forEach(bk => {
        //                 let nd = new OneWordCardModel();
        //                 nd.FillByBackModel(bk);
        //                 dataFront.push(nd);
        //             });
        //             this.setState({//смержит?????
        //                 Cards: dataFront,
        //                 CardsLoaded: true,
        //                 // FollowedCards: followed,
        //                 // NotFollowedCards: notFollowed,
        //             });
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/get-all-for-user',
        // }, true);
    };
    WordsCardsListMain.prototype.WordOnChange = function (event) {
        var newState = __assign({}, this.state);
        newState.EditCurrentCard.Word = event.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.WordAnswerOnChange = function (event) {
        var newState = __assign({}, this.state);
        newState.EditCurrentCard.WordAnswer = event.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.WordDescriptionOnChange = function (event) {
        var newState = __assign({}, this.state);
        newState.EditCurrentCard.Description = event.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.DeleteMainImageOnClick = function () {
        var newState = __assign({}, this.state);
        newState.EditCurrentCard.NeedDeleteMainImage = !newState.EditCurrentCard.NeedDeleteMainImage;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.StartEditCard = function () {
        if (!this.state.CurrentCard) {
            var alert_1 = new AlertData_1.AlertData();
            alert_1.Text = 'Не выбрано слово';
            alert_1.Type = AlertData_1.AlertTypeEnum.Error;
            G_AddAbsoluteAlertToState(alert_1);
            return;
        }
        var newState = __assign({}, this.state);
        newState.EditCurrentCard = {
            Id: newState.CurrentCard.Id,
            Word: newState.CurrentCard.Word,
            WordAnswer: newState.CurrentCard.WordAnswer,
            Description: newState.CurrentCard.Description,
            ImagePath: newState.CurrentCard.ImagePath,
            NeedDeleteMainImage: false,
        };
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ShowNextCard = function () {
        var thisRef = this;
        var funcSearch = function (arr, startIndex) {
            for (var i = startIndex; i < arr.length; ++i) {
                if (thisRef.FilterWordNeedShowInList(arr[i])) {
                    return arr[i];
                }
            }
            for (var i = 0; i < startIndex && i < arr.length; ++i) {
                if (thisRef.FilterWordNeedShowInList(arr[i])) {
                    return arr[i];
                }
            }
            return null;
        };
        // console.log('===');
        if (!this.state.CurrentCard) {
            var newState = __assign({}, this.state);
            this.ChangeCurrentCard(newState);
            newState.CurrentCard = funcSearch(this.state.Cards, 0);
            this.setState(newState);
            return;
            // if (this.state.Cards.length > 0) {
            //     let newState = { ...this.state };
            //     this.ChangeCurrentCard(newState);
            //     newState.CurrentCard = this.state.Cards[0];
            //     this.setState(newState);
            //     return;
            // }
            // else {
            //     return;
            // }
        }
        for (var i = 0; i < this.state.Cards.length; ++i) {
            if (this.state.Cards[i].Id === this.state.CurrentCard.Id) {
                var newState = __assign({}, this.state);
                this.ChangeCurrentCard(newState);
                newState.CurrentCard = funcSearch(this.state.Cards, i + 1);
                // if (i + 1 < this.state.Cards.length) {
                //     newState.CurrentCard = this.state.Cards[i + 1];
                // }
                // else {
                //     newState.CurrentCard = this.state.Cards[0];
                // }
                this.setState(newState);
                return;
            }
        }
    };
    WordsCardsListMain.prototype.AddNewTemplate = function () {
        var newState = __assign({}, this.state);
        this.ChangeCurrentCard(newState);
        newState.EditCurrentCard = {
            Id: -1,
            Word: "",
            WordAnswer: "",
            Description: "",
            NeedDeleteMainImage: false,
        };
        this.setState(newState);
    };
    WordsCardsListMain.prototype.CancelEditCard = function () {
        if (!this.state.EditCurrentCard) {
            // let alert = new AlertData();
            // alert.Text = 'Не выбрано слово';
            // G_AddAbsoluteAlertToState(alert);
            return;
        }
        var newState = __assign({}, this.state);
        newState.EditCurrentCard = null;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.SaveCard = function () {
        if (!this.state.EditCurrentCard) {
            var alert_2 = new AlertData_1.AlertData();
            alert_2.Text = 'Активируйте режим редактирования';
            alert_2.Type = AlertData_1.AlertTypeEnum.Error;
            G_AddAbsoluteAlertToState(alert_2);
            return;
        }
        var refThis = this;
        var editCurrantCard = this.state.EditCurrentCard;
        this.state.EditCurrentCard.MainImageSave;
        editCurrantCard.MainImageSave = $('#main_image_input')[0].files[0];
        var currentCardId = editCurrantCard.Id;
        if (currentCardId < 1) {
            // создаем
            this.AddNewCardInListRequest(editCurrantCard, function (fromBack) {
                // let newCardData = new OneCardInListData(cardForUpdate);
                var newState = __assign({}, refThis.state);
                var newCard = new OneWordCard_2.OneWordCard();
                newCard.FillByBackModel(fromBack);
                newState.Cards.push(newCard);
                newState.EditCurrentCard = null;
                refThis.setState(newState);
            });
        }
        else {
            //редачим
            this.EditCardInListRequest(editCurrantCard, function (fromBack) {
                // let newCardData = new OneCardInListData(cardForUpdate);
                var newState = __assign({}, refThis.state);
                var actualCard = refThis.GetFromStateCardsById(newState, currentCardId);
                if (actualCard) {
                    actualCard.FillByBackModel(fromBack);
                    newState.EditCurrentCard = null;
                    refThis.setState(newState);
                }
                // newState.CurrentCard.FillByBackModel(fromBack);
                // newState.Card.Title=newState.NewCardData.Title;
                // newState.Card.Title=newState.NewCardData.body;
                // newState.EditCurrentCard = null;
                // refThis.setState(newState);
            });
        }
    };
    WordsCardsListMain.prototype.ShuffleCardsOnClick = function () {
        // console.log('=-===');
        var newState = __assign({}, this.state);
        newState.Cards.sort(function () { return Math.random() - 0.5; });
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ChangeVisibilityCurrentCard = function () {
        if (!this.state.CurrentCard) {
            return;
        }
        var refThis = this;
        var cardId = this.state.CurrentCard.Id;
        var success = function (error, data) {
            var _a;
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var card = refThis.GetFromStateCardsById(newState, cardId);
            if (card) {
                card.Hided = data.result;
                if ((!newState.ShowHidenCard) && card.Hided && ((_a = newState.CurrentCard) === null || _a === void 0 ? void 0 : _a.Id) == cardId) {
                    refThis.ChangeCurrentCard(newState);
                }
                refThis.setState(newState);
            }
        };
        G_WordsCardsController.Hide(cardId, success);
        // let data = new FormData();
        // let cardId = this.state.CurrentCard.Id;
        // data.append('id', cardId + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
        //             let res = xhr as BoolResultBack;
        //             let newState = { ...refThis.state };
        //             let card = refThis.GetFromStateCardsById(newState, cardId);
        //             if (card) {
        //                 card.Hided = res.result;
        //                 if ((!newState.ShowHidenCard) && card.Hided && newState.CurrentCard?.Id == cardId) {
        //                     this.ChangeCurrentCard(newState);
        //                 }
        //                 refThis.setState(newState);
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/hide',
        // }, true);
    };
    WordsCardsListMain.prototype.AddCardToList = function (cardId, listId) {
        var _this = this;
        if (cardId < 1 || listId < 1) {
            return;
        }
        var card = this.GetFromStateCardsById(this.state, cardId);
        if (card.Lists.find(function (x) { return x.IdList == listId; })) {
            return;
        }
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            card = _this.GetFromStateCardsById(newState, cardId);
            if (card) {
                var newRec = new WordCardWordList_1.WordCardWordList();
                newRec.FillByBackModel(data);
                card.Lists.push(newRec);
                refThis.setState(newState);
            }
        };
        G_WordsListController.AddToList(cardId, listId, success);
        // let data = new FormData();
        // data.append('card_id', cardId + '');
        // data.append('list_id', listId + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
        //             let res = xhr as IWordCardWordList;
        //             if (res.id_list) {
        //                 let newState = { ...refThis.state };
        //                 card = this.GetFromStateCardsById(newState, cardId);
        //                 if (card) {
        //                     let newRec = new WordCardWordList();
        //                     newRec.FillByBackModel(res);
        //                     card.Lists.push(newRec);
        //                     refThis.setState(newState);
        //                 }
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/add-to-list',
        // }, true);
    };
    WordsCardsListMain.prototype.RemoveFromList = function (cardId, listId) {
        var _this = this;
        if (cardId < 1 || listId < 1) {
            return;
        }
        var card = this.GetFromStateCardsById(this.state, cardId);
        if (!card.Lists.find(function (x) { return x.IdList == listId; })) {
            return;
        }
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            if (data.result) {
                var newState = __assign({}, refThis.state);
                card = _this.GetFromStateCardsById(newState, cardId);
                for (var i = 0; i < card.Lists.length; ++i) {
                    if (card.Lists[i].IdList == listId) {
                        card.Lists.splice(i, 1);
                        refThis.setState(newState);
                        return;
                    }
                }
            }
        };
        G_WordsListController.RemoveFromList(cardId, listId, success);
        // let data = new FormData();
        // data.append('card_id', cardId + '');
        // data.append('list_id', listId + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "DELETE",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
        //             let res = xhr as BoolResultBack;
        //             if (res.result) {
        //                 let newState = { ...refThis.state };
        //                 card = this.GetFromStateCardsById(newState, cardId);
        //                 for (let i = 0; i < card.Lists.length; ++i) {
        //                     if (card.Lists[i].IdList == listId) {
        //                         card.Lists.splice(i, 1)
        //                         refThis.setState(newState);
        //                         return;
        //                     }
        //                 }
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/remove-from-list',
        // }, true);
    };
    WordsCardsListMain.prototype.DeleteCurrentCard = function () {
        if (!this.state.CurrentCard) {
            return;
        }
        var refThis = this;
        var cardId = this.state.CurrentCard.Id;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var card = refThis.TryRemoveFromStateCardsById(newState, cardId);
            if (card) {
                refThis.ChangeCurrentCard(newState);
                refThis.setState(newState);
            }
        };
        G_WordsCardsController.Delete(cardId, success);
        // let data = new FormData();
        // let cardId = this.state.CurrentCard.Id;
        // data.append('id', cardId + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "DELETE",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
        //             let res = xhr as IOneWordCardBack;
        //             if (res?.id && res.id > 0) {
        //                 let newState = { ...refThis.state };
        //                 let card = refThis.TryRemoveFromStateCardsById(newState, cardId);
        //                 if (card) {
        //                     this.ChangeCurrentCard(newState);
        //                     refThis.setState(newState);
        //                 }
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/delete',
        // }, true);
    };
    WordsCardsListMain.prototype.ShowHiddenCardsOnClick = function () {
        var _a;
        var newState = __assign({}, this.state);
        newState.ShowHidenCard = !newState.ShowHidenCard;
        if ((!newState.ShowHidenCard) && ((_a = newState.CurrentCard) === null || _a === void 0 ? void 0 : _a.Hided)) {
            this.ChangeCurrentCard(newState);
        }
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ChangeShowCurrentWordImage = function () {
        var newState = __assign({}, this.state);
        newState.ShowCurrentWordImage = !newState.ShowCurrentWordImage;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ChangeShowCurrentWordAnswer = function () {
        var newState = __assign({}, this.state);
        newState.ShowCurrentWordAnswer = !newState.ShowCurrentWordAnswer;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ChangeShowCurrentWord = function () {
        var newState = __assign({}, this.state);
        newState.ShowCurrentWord = !newState.ShowCurrentWord;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.OnSelectedCard = function (id) {
        var _a;
        if (((_a = this.state.CurrentCard) === null || _a === void 0 ? void 0 : _a.Id) == id) {
            return;
        }
        var newState = __assign({}, this.state);
        this.ChangeCurrentCard(newState);
        newState.CurrentCard = newState.Cards.find(function (x) { return x.Id == id; });
        this.setState(newState);
    };
    WordsCardsListMain.prototype.SearchStrChanged = function (event) {
        var newState = __assign({}, this.state);
        newState.SearchedString = event.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.WriteTestChanged = function (event) {
        var newState = __assign({}, this.state);
        newState.WriteTestString = event.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.ListOnChange = function (e) {
        // console.log(e);
        var newState = __assign({}, this.state);
        newState.WordListSelected = +e.target.value;
        this.setState(newState);
    };
    WordsCardsListMain.prototype.render = function () {
        var _this = this;
        if (!this.state.CardsLoaded) { //TODO нужен кастомный
            return React.createElement("div", { className: 'card-list-preloader' },
                React.createElement("div", { className: "spinner-border persent-100-width-height", role: "status" },
                    React.createElement("span", { className: "sr-only" }, "Loading...")));
        }
        return React.createElement("div", { className: "main-body container" },
            React.createElement("div", { className: "row" },
                React.createElement(OneWordCard_1.OneWordCard, { CurrentCard: this.state.CurrentCard, EditCurrentCard: this.state.EditCurrentCard, ChangeShowCurrentWordAnswer: this.ChangeShowCurrentWordAnswer, ChangeShowCurrentWordImage: this.ChangeShowCurrentWordImage, ChangeShowCurrentWord: this.ChangeShowCurrentWord, ShowCurrentWordAnswer: this.state.ShowCurrentWordAnswer, ShowCurrentWordImage: this.state.ShowCurrentWordImage, ShowCurrentWord: this.state.ShowCurrentWord, SearchStrChanged: this.SearchStrChanged, SearchedString: this.state.SearchedString, StartEditCard: this.StartEditCard, CancelEditCard: this.CancelEditCard, SaveCard: this.SaveCard, AddNewTemplate: this.AddNewTemplate, WordOnChange: this.WordOnChange, WordAnswerOnChange: this.WordAnswerOnChange, WordDescriptionOnChange: this.WordDescriptionOnChange, ShowNextCard: this.ShowNextCard, DeleteMainImageOnClick: this.DeleteMainImageOnClick, ShowHiddenCardsOnClick: this.ShowHiddenCardsOnClick, ChangeVisibilityCurrentCard: this.ChangeVisibilityCurrentCard, ShuffleCardsOnClick: this.ShuffleCardsOnClick, EditTemplateViewNow: this.state.EditCurrentCard != null, DeleteCurrentCard: this.DeleteCurrentCard, WriteTestChanged: this.WriteTestChanged, WriteTestString: this.state.WriteTestString, WordLists: this.state.WordLists, SelectedList: this.state.WordListSelected, ListOnChange: this.ListOnChange, AddCardToList: this.AddCardToList, RemoveFromList: this.RemoveFromList }),
                React.createElement(WordsCardsList_1.WordsCardsList, { CardList: this.state.Cards.filter(function (x) { return _this.FilterWordNeedShowInList(x); }), CurrentCard: this.state.CurrentCard, OnSelectedCard: this.OnSelectedCard })));
    };
    // ---------------------------------------------------------PRIVATE
    WordsCardsListMain.prototype.EditCardInListRequest = function (newElement, callBack) {
        // let refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            callBack(data);
        };
        G_WordsCardsController.Update(newElement, success);
        // let data = new FormData();
        // data.append('id', newElement.Id + '');
        // data.append('word', newElement.Word);
        // data.append('word_answer', newElement.WordAnswer);
        // data.append('description', newElement.Description);
        // data.append('delete_main_image', JSON.stringify(newElement.NeedDeleteMainImage));
        // if (newElement.MainImageSave) {
        //     data.append('main_image_new', newElement.MainImageSave);
        // }
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let res = xhr as IOneWordCardBack;
        //             if (res.id && res.id > 0) {
        //                 callBack(res);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/update',
        // }, true);
    };
    WordsCardsListMain.prototype.AddNewCardInListRequest = function (newElement, callBack) {
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            callBack(data);
        };
        G_WordsCardsController.Create(newElement, success);
        // let data = new FormData();
        // data.append('word', newElement.Word);
        // data.append('word_answer', newElement.WordAnswer);
        // data.append('description', newElement.Description);
        // if (newElement.MainImageSave) {
        //     data.append('main_image_new', newElement.MainImageSave);
        // }
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let res = xhr as IOneWordCardBack;
        //             if (res.id && res.id > 0) {
        //                 callBack(res);
        //             }
        //             else {
        //                 //что то не то вернулось
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordscards/create',
        // }, true);
    };
    WordsCardsListMain.prototype.ChangeCurrentCard = function (stateCopy) {
        stateCopy.CurrentCard = null;
        stateCopy.EditCurrentCard = null;
        stateCopy.ShowCurrentWord = false;
        stateCopy.ShowCurrentWordAnswer = false;
        stateCopy.ShowCurrentWordImage = false;
        stateCopy.WriteTestString = "";
    };
    WordsCardsListMain.prototype.FilterWordNeedShowInList = function (card) {
        var _this = this;
        var res = card.Word.indexOf(this.state.SearchedString) >= 0
            && (this.state.ShowHidenCard || !card.Hided);
        if (!res) {
            return false;
        }
        if (this.state.WordListSelected > 0) {
            res = card.Lists.some(function (x) { return x.IdList == _this.state.WordListSelected; });
        }
        return res;
    };
    WordsCardsListMain.prototype.GetFromStateCardsById = function (state, id) {
        for (var i = 0; i < state.Cards.length; ++i) {
            if (state.Cards[i].Id == id) {
                return state.Cards[i];
            }
        }
        return null;
    };
    WordsCardsListMain.prototype.TryRemoveFromStateCardsById = function (state, id) {
        for (var i = 0; i < state.Cards.length; ++i) {
            if (state.Cards[i].Id == id) {
                return state.Cards.splice(i, 1);
            }
        }
        return null;
    };
    WordsCardsListMain.prototype.LoadAllWordLists = function () {
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var dataFront = [];
            data.forEach(function (bk) {
                var nd = new OneWordList_1.OneWordList();
                nd.FillByBackModel(bk);
                dataFront.push(nd);
            });
            // newState.WordListsLoaded = true;
            newState.WordLists = dataFront;
            refThis.setState(newState);
        };
        G_WordsListController.GetAllForUser(success);
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: {},
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack[];
        //             if (dataBack.length == 0) {
        //                 return;//todo
        //             }
        //             let newState = { ...refThis.state };
        //             let dataFront: OneWordList[] = [];
        //             dataBack.forEach(bk => {
        //                 let nd = new OneWordList();
        //                 nd.FillByBackModel(bk);
        //                 dataFront.push(nd);
        //             });
        //             // newState.WordListsLoaded = true;
        //             newState.WordLists = dataFront;
        //             this.setState(newState);
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/get-all-for-user',
        // }, true);
    };
    return WordsCardsListMain;
}(React.Component));
exports.WordsCardsListMain = WordsCardsListMain;


/***/ }),

/***/ "./src/components/Body/WordsCardsApp/WordsList/WordsCardsListWork.tsx":
/*!****************************************************************************!*\
  !*** ./src/components/Body/WordsCardsApp/WordsList/WordsCardsListWork.tsx ***!
  \****************************************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsListWork = exports.OneWordListState = exports.OneWordListEdit = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var OneWordList_1 = __webpack_require__(/*! ../../../_ComponentsLink/Models/WordsCardsApp/OneWordList */ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordList.ts");
var OneWordListEdit = /** @class */ (function () {
    function OneWordListEdit() {
        this.Id = null;
        this.Title = "";
    }
    return OneWordListEdit;
}());
exports.OneWordListEdit = OneWordListEdit;
var OneWordListState = /** @class */ (function () {
    function OneWordListState() {
    }
    return OneWordListState;
}());
exports.OneWordListState = OneWordListState;
//это страница работы с списками
var WordsCardsListWork = /** @class */ (function (_super) {
    __extends(WordsCardsListWork, _super);
    function WordsCardsListWork(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            ListsLoaded: false,
            WordLists: [],
            EditModel: null,
        };
        _this.LoadAllWordLists = _this.LoadAllWordLists.bind(_this);
        _this.AddNewTemplate = _this.AddNewTemplate.bind(_this);
        _this.CancelChange = _this.CancelChange.bind(_this);
        _this.SaveChange = _this.SaveChange.bind(_this);
        _this.SaveNew = _this.SaveNew.bind(_this);
        _this.NewTemplateTitleOnChange = _this.NewTemplateTitleOnChange.bind(_this);
        _this.RenderOneCard = _this.RenderOneCard.bind(_this);
        _this.StartEdit = _this.StartEdit.bind(_this);
        _this.EditTemplateTitleOnChange = _this.EditTemplateTitleOnChange.bind(_this);
        _this.Delete = _this.Delete.bind(_this);
        return _this;
    }
    WordsCardsListWork.prototype.componentDidMount = function () {
        this.LoadAllWordLists();
    };
    WordsCardsListWork.prototype.SaveChange = function (id) {
        var rec = this.GetByIdFromState(this.state, id);
        if (!rec || !rec.EditModel) {
            return;
        }
        rec.EditModel.Id = rec.Record.Id;
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var recActual = refThis.GetByIdFromState(newState, id);
            recActual.Record.FillByBackModel(data);
            recActual.EditModel = null;
            refThis.setState(newState);
        };
        G_WordsListController.Update(rec.EditModel, success);
        // let data = new FormData();
        // data.append('title', rec.EditModel.Title);
        // data.append('id', rec.Record.Id + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PATCH",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack;
        //             if (dataBack.id < 1) {
        //                 return;
        //             }
        //             let newState = { ...refThis.state };
        //             let recActual = this.GetByIdFromState(newState, id);
        //             recActual.Record.FillByBackModel(dataBack);
        //             recActual.EditModel = null;
        //             this.setState(newState);
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/update',
        // }, true);
    };
    WordsCardsListWork.prototype.StartEdit = function (id) {
        var newState = __assign({}, this.state);
        var rec = this.GetByIdFromState(newState, id);
        if (!rec) {
            return;
        }
        rec.EditModel = new OneWordListEdit();
        rec.EditModel.Title = rec.Record.Title;
        this.setState(newState);
    };
    WordsCardsListWork.prototype.SaveNew = function () {
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var nd = new OneWordList_1.OneWordList();
            nd.FillByBackModel(data);
            newState.WordLists.push({
                Record: nd,
                EditModel: null,
            });
            newState.EditModel = null;
            refThis.setState(newState);
        };
        G_WordsListController.Create(this.state.EditModel.Title, success);
        // let data = new FormData();
        // data.append('title', this.state.EditModel.Title);
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "PUT",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack;
        //             if (dataBack.id < 1) {
        //                 return;
        //             }
        //             let newState = { ...refThis.state };
        //             let nd = new OneWordList();
        //             nd.FillByBackModel(dataBack);
        //             newState.WordLists.push(
        //                 {
        //                     Record: nd,
        //                     EditModel: null,
        //                 }
        //             );
        //             newState.EditModel = null;
        //             this.setState(newState);
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/create',
        // }, true);
    };
    WordsCardsListWork.prototype.Delete = function (id) {
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            for (var i = 0; i < newState.WordLists.length; ++i) {
                if (newState.WordLists[i].Record.Id == id) {
                    newState.WordLists.splice(i, 1);
                    refThis.setState(newState);
                    return;
                }
            }
        };
        G_WordsListController.Delete(id, success);
        // let data = new FormData();
        // data.append('id', id + '');
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: data,
        //     Type: "DELETE",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack;
        //             if (dataBack.id < 1) {
        //                 return;
        //             }
        //             let newState = { ...refThis.state };
        //             for (let i = 0; i < newState.WordLists.length; ++i) {
        //                 if (newState.WordLists[i].Record.Id == id) {
        //                     newState.WordLists.splice(i, 1);
        //                     this.setState(newState);
        //                     return;
        //                 }
        //             }
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/delete',
        // }, true);
    };
    WordsCardsListWork.prototype.CancelChange = function (id) {
        if (id < 1) {
            var newState_1 = __assign({}, this.state);
            newState_1.EditModel = null;
            this.setState(newState_1);
            return;
        }
        var newState = __assign({}, this.state);
        var recordFromState = this.GetByIdFromState(newState, id);
        recordFromState.EditModel = null;
        this.setState(newState);
    };
    WordsCardsListWork.prototype.NewTemplateTitleOnChange = function (e) {
        var newState = __assign({}, this.state);
        newState.EditModel.Title = e.target.value;
        this.setState(newState);
    };
    WordsCardsListWork.prototype.EditTemplateTitleOnChange = function (id, e) {
        var newState = __assign({}, this.state);
        var rec = this.GetByIdFromState(newState, id);
        if (!rec || !rec.EditModel) {
            return;
        }
        rec.EditModel.Title = e.target.value;
        this.setState(newState);
    };
    WordsCardsListWork.prototype.LoadAllWordLists = function () {
        //копия src\components\Body\WordsCardsApp\ForceNew\WordsCardsForceAdd.tsx
        var refThis = this;
        var success = function (error, data) {
            if (error || !data) {
                return;
            }
            var newState = __assign({}, refThis.state);
            var dataFront = [];
            data.forEach(function (bk) {
                var nd = new OneWordList_1.OneWordList();
                nd.FillByBackModel(bk);
                dataFront.push({
                    Record: nd,
                    EditModel: null,
                });
            });
            newState.ListsLoaded = true;
            newState.WordLists = dataFront;
            refThis.setState(newState);
        };
        G_WordsListController.GetAllForUser(success);
        // let refThis = this;
        // G_AjaxHelper.GoAjaxRequest({
        //     Data: {},
        //     Type: "GET",
        //     FuncSuccess: (xhr, status, jqXHR) => {
        //         let resp: MainErrorObjectBack = xhr as MainErrorObjectBack;
        //         if (resp.errors) {
        //             //TODO ошибка
        //         }
        //         else {
        //             let dataBack = xhr as IWordListBack[];
        //             if (dataBack.length == 0) {
        //                 return;//todo
        //             }
        //             let newState = { ...refThis.state };
        //             let dataFront: OneWordListState[] = [];
        //             dataBack.forEach(bk => {
        //                 let nd = new OneWordList();
        //                 nd.FillByBackModel(bk);
        //                 dataFront.push(
        //                     {
        //                         Record: nd,
        //                         EditModel: null,
        //                     }
        //                 );
        //             });
        //             newState.ListsLoaded = true;
        //             newState.WordLists = dataFront;
        //             this.setState(newState);
        //         }
        //     },
        //     FuncError: (xhr, status, error) => { },
        //     Url: G_PathToServer + 'api/wordslist/get-all-for-user',
        // }, true);
    };
    WordsCardsListWork.prototype.AddNewTemplate = function () {
        var newState = __assign({}, this.state);
        newState.EditModel = new OneWordListEdit();
        this.setState(newState);
    };
    WordsCardsListWork.prototype.RenderOneCard = function (data) {
        var _this = this;
        if (!data.EditModel) {
            return React.createElement("div", { className: "work-words-one-list padding-10-top col-sm-12", key: data.Record.Id },
                React.createElement("div", { className: "work-words-one-list-inner" },
                    React.createElement("p", null,
                        data.Record.Id,
                        " - ",
                        data.Record.Title),
                    React.createElement("button", { onClick: function () { _this.StartEdit(data.Record.Id); }, className: "btn btn-primary" }, "\u0420\u0435\u0434\u0430\u043A\u0442\u0438\u0440\u043E\u0432\u0430\u0442\u044C"),
                    React.createElement("button", { onClick: function () { _this.Delete(data.Record.Id); }, className: "btn btn-primary" }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C")));
        }
        return React.createElement("div", { className: "work-words-one-list padding-10-top col-sm-12", key: data.Record.Id },
            React.createElement("div", { className: "work-words-one-list-inner" },
                React.createElement("input", { className: "form-control", onChange: function (e) { return _this.EditTemplateTitleOnChange(data.Record.Id, e); }, value: data.EditModel.Title }),
                React.createElement("button", { onClick: function () { _this.CancelChange(data.Record.Id); }, className: "btn btn-primary" }, "\u041E\u0442\u043C\u0435\u043D\u0438\u0442\u044C"),
                React.createElement("button", { onClick: function () { _this.SaveChange(data.Record.Id); }, className: "btn btn-primary" }, "\u0421\u043E\u0445\u0440\u0430\u043D\u0438\u0442\u044C")));
    };
    WordsCardsListWork.prototype.render = function () {
        var _this = this;
        var lists = React.createElement("div", null);
        if (this.state.ListsLoaded) {
            lists = React.createElement("div", null, this.state.WordLists.map(function (x) { return _this.RenderOneCard(x); }));
        }
        var actions = React.createElement("div", null,
            React.createElement("button", { onClick: this.AddNewTemplate, className: "btn btn-primary" }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u043D\u043E\u0432\u044B\u0439 \u0448\u0430\u0431\u043B\u043E\u043D"),
            React.createElement("div", { className: "padding-10-top" },
                React.createElement(react_router_dom_1.Link, { to: "/words-cards-app" }, "\u043F\u0435\u0440\u0435\u0439\u0442\u0438 \u043A \u0441\u043B\u043E\u0432\u0430\u0440\u044E \u0431\u0435\u0437 \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F")));
        var editTemplate = React.createElement("div", null);
        if (this.state.EditModel != null) {
            editTemplate = React.createElement("div", null,
                React.createElement("input", { className: "form-control", type: "text", placeholder: "\u043D\u0430\u0437\u0432\u0430\u043D\u0438\u0435 \u0441\u043F\u0438\u0441\u043A\u0430", onChange: this.NewTemplateTitleOnChange, value: this.state.EditModel.Title }),
                React.createElement("button", { onClick: function () { _this.CancelChange(-1); }, className: "btn btn-primary" }, "\u041E\u0442\u043C\u0435\u043D\u0438\u0442\u044C"),
                React.createElement("button", { onClick: this.SaveNew, className: "btn btn-primary" }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C"));
        }
        return React.createElement("div", { className: "container" },
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "work-words-lists-main" },
                    actions,
                    editTemplate,
                    lists)));
    };
    WordsCardsListWork.prototype.GetByIdFromState = function (state, id) {
        for (var i = 0; i < state.WordLists.length; ++i) {
            if (state.WordLists[i].Record.Id == id) {
                return state.WordLists[i];
            }
        }
        return null;
    };
    return WordsCardsListWork;
}(React.Component));
exports.WordsCardsListWork = WordsCardsListWork;


/***/ }),

/***/ "./src/components/Footer/FooterMain.tsx":
/*!**********************************************!*\
  !*** ./src/components/Footer/FooterMain.tsx ***!
  \**********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.FooterMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var PostFooter_1 = __webpack_require__(/*! ./PostFooter */ "./src/components/Footer/PostFooter.tsx");
var SocialLinkGroup_1 = __webpack_require__(/*! ./SocialLinkGroup */ "./src/components/Footer/SocialLinkGroup.tsx");
// export interface IHeaderLogoProps {
// }
var FooterMain = /** @class */ (function (_super) {
    __extends(FooterMain, _super);
    function FooterMain(props) {
        return _super.call(this, props) || this;
    }
    FooterMain.prototype.render = function () {
        // return <input placeholder="Поиск" onChange={this.onTextChanged} />;
        return React.createElement("div", { className: 'main-footer' },
            React.createElement("div", { className: 'main-footer-inner container' },
                React.createElement("div", { className: 'row' },
                    React.createElement("div", { className: 'col-md-4' },
                        React.createElement("ul", null,
                            React.createElement("li", null,
                                React.createElement("a", { href: '#' }, "\u041E \u043F\u0440\u043E\u0435\u043A\u0442\u0435")),
                            React.createElement("li", null,
                                " ",
                                React.createElement("a", { href: '#' }, "\u041F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044C\u0441\u043A\u043E\u0435 \u0441\u043E\u0433\u043B\u0430\u0448\u0435\u043D\u0438\u0435")),
                            React.createElement("li", null,
                                React.createElement("a", { href: '#' }, "\u041F\u043E\u043B\u0438\u0442\u0438\u043A\u0430 \u043E\u0431\u0440\u0430\u0431\u043E\u0442\u043A\u0438 \u043F\u0435\u0440\u0441\u043E\u043D\u0430\u043B\u044C\u043D\u044B\u0445 \u0434\u0430\u043D\u043D\u044B\u0445")))),
                    React.createElement(SocialLinkGroup_1.SocialLinkGroup, null),
                    React.createElement("div", { className: 'col-md-4 footer-contacts' },
                        React.createElement("p", { className: 'contacts-head' }, "\u041A\u043E\u043D\u0442\u0430\u043A\u0442\u044B"),
                        React.createElement("ul", null,
                            React.createElement("li", null, "\u041F\u043E\u0447\u0442\u0430"),
                            React.createElement("li", null, "\u041D\u043E\u043C\u0435\u0440 \u0442\u0435\u043B\u0435\u0444\u043E\u043D\u0430"))))),
            React.createElement(PostFooter_1.PostFooter, null));
    };
    return FooterMain;
}(React.Component));
exports.FooterMain = FooterMain;


/***/ }),

/***/ "./src/components/Footer/PostFooter.tsx":
/*!**********************************************!*\
  !*** ./src/components/Footer/PostFooter.tsx ***!
  \**********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.PostFooter = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
// export interface IHeaderLogoProps {
// }
var PostFooter = /** @class */ (function (_super) {
    __extends(PostFooter, _super);
    function PostFooter(props) {
        return _super.call(this, props) || this;
    }
    PostFooter.prototype.render = function () {
        return React.createElement("div", { className: 'sub-footer-under' });
    };
    return PostFooter;
}(React.Component));
exports.PostFooter = PostFooter;


/***/ }),

/***/ "./src/components/Footer/SocialLinkGroup.tsx":
/*!***************************************************!*\
  !*** ./src/components/Footer/SocialLinkGroup.tsx ***!
  \***************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.SocialLinkGroup = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var SocialLinkGroup = /** @class */ (function (_super) {
    __extends(SocialLinkGroup, _super);
    function SocialLinkGroup(props) {
        return _super.call(this, props) || this;
    }
    SocialLinkGroup.prototype.render = function () {
        return React.createElement("div", { className: 'col-md-4' },
            React.createElement("p", { className: 'social-link-head' }, "\u0421\u043E\u0446\u0438\u0430\u043B\u044C\u043D\u044B\u0435 \u0441\u0435\u0442\u0438 "),
            React.createElement("div", { className: 'row' },
                React.createElement("div", { className: 'col-3' },
                    React.createElement("div", { className: 'footer-social-link' })),
                React.createElement("div", { className: 'col-3' },
                    React.createElement("div", { className: 'footer-social-link' })),
                React.createElement("div", { className: 'col-3' },
                    React.createElement("div", { className: 'footer-social-link' })),
                React.createElement("div", { className: 'col-3' },
                    React.createElement("div", { className: 'footer-social-link' }))));
    };
    return SocialLinkGroup;
}(React.Component));
exports.SocialLinkGroup = SocialLinkGroup;


/***/ }),

/***/ "./src/components/Header/HeaderLogo.tsx":
/*!**********************************************!*\
  !*** ./src/components/Header/HeaderLogo.tsx ***!
  \**********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.HeaderLogo = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var HeaderLogo = /** @class */ (function (_super) {
    __extends(HeaderLogo, _super);
    function HeaderLogo(props) {
        return _super.call(this, props) || this;
    }
    HeaderLogo.prototype.render = function () {
        return React.createElement("div", { className: 'main-header-logo nopadding col-4  col-md-2' },
            React.createElement("a", { href: "/menu" },
                React.createElement("img", { className: 'main-header-logo-img', src: G_PathToBaseImages + "Header_logo.png", alt: "menu" })));
    };
    return HeaderLogo;
}(React.Component));
exports.HeaderLogo = HeaderLogo;


/***/ }),

/***/ "./src/components/Header/HeaderMain.tsx":
/*!**********************************************!*\
  !*** ./src/components/Header/HeaderMain.tsx ***!
  \**********************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.HeaderMain = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var HeaderLogo_1 = __webpack_require__(/*! ./HeaderLogo */ "./src/components/Header/HeaderLogo.tsx");
var HeaderUserMenu_1 = __webpack_require__(/*! ./HeaderUserMenu */ "./src/components/Header/HeaderUserMenu.tsx");
var HeaderMain = /** @class */ (function (_super) {
    __extends(HeaderMain, _super);
    function HeaderMain(props) {
        return _super.call(this, props) || this;
    }
    HeaderMain.prototype.render = function () {
        return React.createElement("div", { className: 'main-header' },
            React.createElement("div", { className: 'main-header-inner container' },
                React.createElement("div", { className: 'main-header-row row' },
                    React.createElement(HeaderLogo_1.HeaderLogo, null),
                    React.createElement("div", { className: 'd-none d-md-inline-block col-md-7' }),
                    React.createElement(HeaderUserMenu_1.HeaderUserMenu, { AuthInfo: this.props.AuthInfo }))));
    };
    return HeaderMain;
}(React.Component));
exports.HeaderMain = HeaderMain;


/***/ }),

/***/ "./src/components/Header/HeaderUserMenu.tsx":
/*!**************************************************!*\
  !*** ./src/components/Header/HeaderUserMenu.tsx ***!
  \**************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.HeaderUserMenu = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var HeaderUserMenu = /** @class */ (function (_super) {
    __extends(HeaderUserMenu, _super);
    function HeaderUserMenu(props) {
        var _this = _super.call(this, props) || this;
        _this.LogginedUserRender = _this.LogginedUserRender.bind(_this);
        return _this;
    }
    HeaderUserMenu.prototype.componentDidMount = function () {
        //TODO стучимся в апи, устанавливаем стейт
    };
    HeaderUserMenu.prototype.UserImageRender = function (imgPath) {
        var path = imgPath;
        if (!path) {
            path = G_PathToBaseImages + 'user_empty_image.png';
        }
        return React.createElement("img", { className: 'header-user-img', src: path });
    };
    HeaderUserMenu.prototype.LogginedOrNotRender = function (loggined) {
        if (loggined) {
            return this.LogginedUserRender();
        }
        else {
            return this.NotLogginedUserRender();
        }
    };
    HeaderUserMenu.prototype.LogginedUserRender = function () {
        return React.createElement("div", { className: 'header-user-block-inner' },
            React.createElement("div", { className: 'dropdown-toggle header-user-dropdown', "data-toggle": "dropdown", "aria-haspopup": "true", "aria-expanded": "false" },
                React.createElement("span", { className: 'header-user-name-text d-inline-block' }, this.props.AuthInfo.User.Name),
                React.createElement("span", { className: 'd-inline-block header-user-img' }, this.UserImageRender(this.props.AuthInfo.User.Image))),
            React.createElement("div", { className: "dropdown-menu", style: { backgroundColor: 'greenyellow' } },
                React.createElement(react_router_dom_1.Link, { className: "dropdown-item", to: "/menu/auth/login/" }, "\u0412\u043E\u0439\u0442\u0438"),
                React.createElement(react_router_dom_1.Link, { className: "dropdown-item", to: "/menu/auth/register/" }, "\u0417\u0430\u0440\u0435\u0433\u0438\u0441\u0442\u0440\u0438\u0440\u043E\u0432\u0430\u0442\u044C\u0441\u044F"),
                React.createElement("a", { className: "dropdown-item", href: "#" }, "Action"),
                React.createElement("a", { className: "dropdown-item", href: "#" }, "Another action"),
                React.createElement("a", { className: "dropdown-item", href: "#" }, "Something else here"),
                React.createElement("div", { className: "dropdown-divider" }),
                React.createElement("a", { className: "dropdown-item", href: "#" }, "Separated link")));
    };
    HeaderUserMenu.prototype.NotLogginedUserRender = function () {
        return React.createElement("div", { className: 'header-user-block-inner' },
            React.createElement("div", { className: 'dropdown-toggle header-auth-dropdown', "data-toggle": "dropdown", "aria-haspopup": "true", "aria-expanded": "false" }, "\u0410\u0432\u0442\u043E\u0440\u0438\u0437\u0430\u0446\u0438\u044F"),
            React.createElement("div", { className: "dropdown-menu", style: { backgroundColor: 'greenyellow' } },
                React.createElement(react_router_dom_1.Link, { className: "dropdown-item", to: "/menu/auth/login/" }, "\u0412\u043E\u0439\u0442\u0438"),
                React.createElement(react_router_dom_1.Link, { className: "dropdown-item", to: "/menu/auth/register/" }, "\u0417\u0430\u0440\u0435\u0433\u0438\u0441\u0442\u0440\u0438\u0440\u043E\u0432\u0430\u0442\u044C\u0441\u044F")));
    };
    HeaderUserMenu.prototype.render = function () {
        return React.createElement("div", { className: 'header-user-block col-8 col-md-3 nopadding ' }, this.LogginedOrNotRender(this.props.AuthInfo.AuthSuccess));
    };
    return HeaderUserMenu;
}(React.Component));
exports.HeaderUserMenu = HeaderUserMenu;


/***/ }),

/***/ "./src/components/MainComponent.tsx":
/*!******************************************!*\
  !*** ./src/components/MainComponent.tsx ***!
  \******************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../../typings/globals.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MainComponent = void 0;
var React = __importStar(__webpack_require__(/*! react */ "react"));
var HeaderMain_1 = __webpack_require__(/*! ./Header/HeaderMain */ "./src/components/Header/HeaderMain.tsx");
// import { BodyMain } from './Body/BodyMain';
var FooterMain_1 = __webpack_require__(/*! ./Footer/FooterMain */ "./src/components/Footer/FooterMain.tsx");
var MainAlertAbsolute_1 = __webpack_require__(/*! ./Alerts/MainAlertAbsolute */ "./src/components/Alerts/MainAlertAbsolute.tsx");
var react_router_dom_1 = __webpack_require__(/*! react-router-dom */ "./node_modules/react-router-dom/esm/react-router-dom.js");
var AlertData_1 = __webpack_require__(/*! ./_ComponentsLink/Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var AppRouter_1 = __webpack_require__(/*! ./AppRouter */ "./src/components/AppRouter.tsx");
var MainComponent = /** @class */ (function (_super) {
    __extends(MainComponent, _super);
    function MainComponent(props) {
        var _this = _super.call(this, props) || this;
        var localState = {
            Auth: {
                AuthSuccess: false,
                User: null,
            },
            AbsoluteAlerts: [],
            MaxIdMainAlert: 0,
        };
        _this.state = localState;
        _this.AddMainALert = _this.AddMainALert.bind(_this);
        _this.RemoveMainALert = _this.RemoveMainALert.bind(_this);
        _this.LogOutHandler = _this.LogOutHandler.bind(_this);
        window.G_AddAbsoluteAlertToState = _this.AddMainALert;
        var thisRef = _this;
        return _this;
        // window.addEventListener("logout", function (e) { thisRef.LogOutHandler() });
    }
    MainComponent.prototype.LogOutHandler = function () {
        var auth = {
            AuthSuccess: false,
            User: null,
        };
        // localStorage.setItem('header_auth', JSON.stringify(auth));
        localStorage.removeItem("header_auth");
        this.setState({
            Auth: auth,
            AbsoluteAlerts: [],
        });
    };
    MainComponent.prototype.AddMainALert = function (alert) {
        var _this = this;
        // console.log('1-AddMainALert');
        var storedAlert = new AlertData_1.AlertDataStored();
        storedAlert.FillByAlertData(alert);
        this.setState(function (prevState) {
            var newState = __assign({}, prevState);
            storedAlert.Key = ++newState.MaxIdMainAlert;
            newState.AbsoluteAlerts.push(storedAlert);
            if (alert.Timeout) {
                setTimeout(function () {
                    _this.RemoveMainALert(storedAlert.Key);
                }, alert.Timeout);
            }
            return newState;
        });
    };
    MainComponent.prototype.RemoveMainALert = function (alertId) {
        this.setState(function (prevState) {
            var newState = __assign({}, prevState);
            for (var i = 0; i < newState.AbsoluteAlerts.length; ++i) {
                if (newState.AbsoluteAlerts[i].Key == alertId) {
                    newState.AbsoluteAlerts.splice(i, 1);
                    if (newState.AbsoluteAlerts.length == 0) {
                        newState.MaxIdMainAlert = 0;
                    }
                    // this.setState(newState);
                    break;
                }
            }
            return newState;
        });
    };
    MainComponent.prototype.componentDidMount = function () {
        return __awaiter(this, void 0, void 0, function () {
            var authStoredJson, authStored, auth, refThis, success;
            return __generator(this, function (_a) {
                authStoredJson = localStorage.getItem('header_auth');
                authStored = JSON.parse(authStoredJson);
                if (authStoredJson && authStored) {
                    auth = {
                        AuthSuccess: true,
                        User: {
                            Name: authStored.Name,
                            Image: authStored.Image,
                        }
                    };
                    this.setState({
                        Auth: auth,
                        AbsoluteAlerts: [],
                    });
                    return [2 /*return*/];
                }
                if (window.location.pathname.startsWith('/menu/auth/')) {
                    return [2 /*return*/];
                }
                refThis = this;
                success = function (error, data) {
                    if (error) {
                        return;
                    }
                    var auth = {
                        AuthSuccess: true,
                        User: {
                            Name: data.name,
                            Image: data.main_image_path
                        }
                    };
                    localStorage.setItem('header_auth', JSON.stringify(auth.User));
                    refThis.setState({
                        Auth: auth,
                        AbsoluteAlerts: [],
                    });
                };
                window.G_UsersController.GetShortestUSerInfo(success);
                return [2 /*return*/];
            });
        });
    };
    MainComponent.prototype.render = function () {
        return React.createElement("div", null,
            React.createElement(react_router_dom_1.BrowserRouter, null,
                React.createElement(HeaderMain_1.HeaderMain, { AuthInfo: this.state.Auth }),
                React.createElement(AppRouter_1.AppRouter, null),
                React.createElement(FooterMain_1.FooterMain, null),
                React.createElement(MainAlertAbsolute_1.MainAlertAbsolute, { Data: this.state.AbsoluteAlerts, RemoveALert: this.RemoveMainALert })));
    };
    return MainComponent;
}(React.Component));
exports.MainComponent = MainComponent;


/***/ }),

/***/ "./src/components/_ComponentsLink/AjaxLogic.ts":
/*!*****************************************************!*\
  !*** ./src/components/_ComponentsLink/AjaxLogic.ts ***!
  \*****************************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AjaxHelper = void 0;
var AlertData_1 = __webpack_require__(/*! ./Models/AlertData */ "./src/components/_ComponentsLink/Models/AlertData.ts");
var AjaxHelper = /** @class */ (function () {
    function AjaxHelper() {
    }
    AjaxHelper.prototype.TryRefreshToken = function (notRedirectWhenNotAuth, callBack) {
        this.GoAjaxRequest({
            Data: {},
            Type: "POST",
            NeedTryRefreshToken: false,
            NotRedirectWhenNotAuth: notRedirectWhenNotAuth,
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    localStorage.removeItem("header_auth");
                    var eventLogOut = new CustomEvent("logout", {});
                    window.dispatchEvent(eventLogOut);
                    if (!notRedirectWhenNotAuth) {
                        location.href = '/menu/auth/login/';
                    }
                }
                else {
                    //TODO записываем полученные токены
                    if (callBack) {
                        callBack();
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/authenticate/refresh-access-token',
        });
    };
    AjaxHelper.prototype.GoAjaxRequest = function (obj, fileLoad) {
        if (fileLoad === void 0) { fileLoad = false; }
        return __awaiter(this, void 0, void 0, function () {
            var thisRef, ajaxObj, haders, _a;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        thisRef = this;
                        if (!obj.Type)
                            obj.Type = 'GET';
                        if (!obj.DataType)
                            obj.DataType = 'json'; //html
                        if (obj.NeedTryRefreshToken !== false) {
                            obj.NeedTryRefreshToken = true;
                        }
                        ajaxObj = {
                            type: obj.Type,
                            data: obj.Data,
                            url: obj.Url,
                            //processData: false, // Не обрабатываем файлы
                            //contentType: false, // Так jQuery скажет серверу что это строковой запрос
                            success: function (xhr, status, jqXHR) {
                                //if(jqXHR.status==200){//EXAMPLE STATUS
                                //DO SOMETHING
                                //}
                                if (obj.FuncSuccess) {
                                    try {
                                        obj.FuncSuccess(xhr, status, jqXHR);
                                    }
                                    catch (e) {
                                        console.log('Ошибка ' + e.name + ":" + e.message + "\n" + e.stack);
                                    }
                                }
                            },
                            error: function (xhr, status, error) {
                                //alert("ошибка загрузки");
                                if (obj.FuncError)
                                    obj.FuncError(xhr, status, error);
                            },
                            // shows the loader element before sending.
                            beforeSend: function () {
                                if (obj.FuncBeforeSend)
                                    obj.FuncBeforeSend();
                                //  PreloaderShowChange(true);
                            },
                            // hides the loader after completion of request, whether successfull or failor.
                            complete: function (jqXHR, status) {
                                if (jqXHR.status == 401) {
                                    if (obj.NeedTryRefreshToken) {
                                        thisRef.TryRefreshToken(obj.NotRedirectWhenNotAuth, function () {
                                            obj.NeedTryRefreshToken = false;
                                            thisRef.GoAjaxRequest(obj, fileLoad);
                                        }); //TODO await или что то такое
                                    }
                                }
                                else {
                                    var resp = jqXHR.responseJSON;
                                    if (resp.errors && Array.isArray(resp.errors)) {
                                        //TODO ошибка
                                        if (!obj.NotGlobalError && G_AddAbsoluteAlertToState) {
                                            var alertLogic_1 = new AlertData_1.AlertData();
                                            resp.errors.forEach(function (error) {
                                                var errArr = alertLogic_1.GetByErrorBack(error);
                                                errArr.forEach(function (alertForShow) {
                                                    G_AddAbsoluteAlertToState(alertForShow);
                                                });
                                            });
                                        }
                                    }
                                }
                                if (obj.FuncComplete) {
                                    try {
                                        obj.FuncComplete(jqXHR, status);
                                    }
                                    catch (e) {
                                        console.log('Ошибка ' + e.name + ":" + e.message + "\n" + e.stack);
                                    }
                                }
                                //PreloaderShowChange(false);
                                // console.log("ajax complete");
                            },
                            dataType: obj.DataType //'html'
                        };
                        // if(obj.dataType){
                        //     ajaxObj.dataType=obj.dataType
                        // }
                        if (fileLoad) {
                            //processData: false, // Не обрабатываем файлы
                            //contentType: false,
                            ajaxObj.processData = false;
                            ajaxObj.contentType = false;
                        }
                        haders = { 'Authorization_Access_Token': localStorage.getItem('access_token') };
                        if (obj.NeedTryRefreshToken) {
                            haders['Authorization_Refresh_Token'] = localStorage.getItem('refresh_token');
                        }
                        ajaxObj.headers = haders;
                        _b.label = 1;
                    case 1:
                        _b.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this.TrySend(ajaxObj)];
                    case 2:
                        _b.sent();
                        return [3 /*break*/, 4];
                    case 3:
                        _a = _b.sent();
                        return [3 /*break*/, 4];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    AjaxHelper.prototype.TrySend = function (ajaxObj) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: //async       : Promise<any>
                    return [4 /*yield*/, $.ajax(ajaxObj)];
                    case 1:
                        _a.sent(); //await
                        return [2 /*return*/];
                }
            });
        });
    };
    return AjaxHelper;
}());
exports.AjaxHelper = AjaxHelper;


/***/ }),

/***/ "./src/components/_ComponentsLink/BackModel/MenuApp/OneCardInListDataBack.ts":
/*!***********************************************************************************!*\
  !*** ./src/components/_ComponentsLink/BackModel/MenuApp/OneCardInListDataBack.ts ***!
  \***********************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneCardInListDataBack = void 0;
var OneCardInListDataBack = /** @class */ (function () {
    function OneCardInListDataBack() {
    }
    OneCardInListDataBack.prototype.FillByFullMode = function (data) {
        this.id = data.id;
        this.title = data.title;
        this.body = data.body;
        this.image = data.main_image_path;
        this.followed = data.followed;
    };
    return OneCardInListDataBack;
}());
exports.OneCardInListDataBack = OneCardInListDataBack;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/AuthenticateController.ts":
/*!******************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/AuthenticateController.ts ***!
  \******************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AuthenticateController = exports.RegisterModel = exports.LoginModel = void 0;
var LoginModel = /** @class */ (function () {
    function LoginModel() {
    }
    return LoginModel;
}());
exports.LoginModel = LoginModel;
var RegisterModel = /** @class */ (function () {
    function RegisterModel() {
    }
    return RegisterModel;
}());
exports.RegisterModel = RegisterModel;
var AuthenticateController = /** @class */ (function () {
    function AuthenticateController() {
    }
    AuthenticateController.prototype.Login = function (model, onSuccess) {
        var data = {
            'email': model.Email,
            'password': model.Password,
        };
        // let ajx: AjaxHelper.IAjaxHelper = new AjaxHelper.AjaxHelper();
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "POST",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    onSuccess(resp);
                }
                else {
                    onSuccess(null);
                    //TODO записываем полученные токены
                    // document.location.href = "/menu";
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/authenticate/login',
        });
    };
    AuthenticateController.prototype.Register = function (model, onSuccess) {
        var data = {
            'email': model.Email,
            'password': model.Password,
            "password_confirm": model.ConfirmPassword,
        };
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    onSuccess(resp);
                }
                else {
                    onSuccess(null);
                    //TODO записываем полученные токены
                    // document.location.href = "/menu";
                }
            },
            Url: G_PathToServer + 'api/authenticate/register',
        });
    };
    AuthenticateController.prototype.Logout = function () {
        alert('not inplemented');
    };
    AuthenticateController.prototype.RefreshAccessToken = function (notRedirectWhenNotAuth, callBack) {
        G_AjaxHelper.TryRefreshToken(notRedirectWhenNotAuth, callBack);
    };
    return AuthenticateController;
}());
exports.AuthenticateController = AuthenticateController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/MenuApp/ArticleController.ts":
/*!*********************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/MenuApp/ArticleController.ts ***!
  \*********************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.ArticleController = exports.IdInput = void 0;
var IdInput = /** @class */ (function () {
    function IdInput() {
    }
    return IdInput;
}());
exports.IdInput = IdInput;
var ArticleController = /** @class */ (function () {
    function ArticleController() {
    }
    ArticleController.prototype.GetAllShortForUser = function (onSuccess) {
        G_AjaxHelper.GoAjaxRequest({
            Data: {},
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/article/get-all-short-for-user',
        });
    };
    ArticleController.prototype.GetAllForUser = function (nSuccess) {
        alert('not implemented');
    };
    ArticleController.prototype.Detail = function (model, onSuccess) {
        var data = {
            "id": model.Id,
        };
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (dataBack.id && dataBack.id > 0) {
                        onSuccess(null, dataBack);
                    }
                    else {
                        //ошибка
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/article/detail',
        });
    };
    ArticleController.prototype.Follow = function (model, onSuccess) {
        var data = {
            "id": model.Id,
        };
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PATCH",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var boolRes = xhr;
                    if (boolRes.result === true || boolRes.result === false) {
                        onSuccess(null, boolRes);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/article/follow',
        });
    };
    ArticleController.prototype.Create = function (model, success) {
        var data = {
            "title": model.Title,
            "body": model.Body,
            // "main_image_new":newElement.Image,
        };
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    success(resp, null);
                }
                else {
                    var resBack = xhr;
                    if (Number.isInteger(resBack.id) && resBack.id > 0) {
                        success(null, resBack);
                        // callBack(resBack);
                    }
                    else {
                        //что то не то вернулось
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/article/create',
        });
    };
    ArticleController.prototype.Edit = function (model, success) {
        var data = new FormData();
        data.append('id', model.Id + '');
        data.append('title', model.Title);
        data.append('body', model.Body);
        data.append('delete_main_image', JSON.stringify(model.NeedDeleteMainImage));
        if (model.MainImageSave) {
            data.append('main_image_new', model.MainImageSave);
        }
        if (model.AdditionalImagesSave) {
            model.AdditionalImagesSave.forEach(function (addImage, index) {
                data.append('additional_images', addImage); //' + index + '
            });
        }
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PATCH",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    success(resp, null);
                }
                else {
                    var res = xhr;
                    if (res.id && res.id > 0) {
                        success(null, res);
                        // callBack(res);
                    }
                    else {
                        //какая то ошибка
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/article/edit',
        }, true);
    };
    return ArticleController;
}());
exports.ArticleController = ArticleController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/PlaningPoker/PlaningPokerController.ts":
/*!*******************************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/PlaningPoker/PlaningPokerController.ts ***!
  \*******************************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.PlaningPokerController = exports.HubEndpoints = exports.HubEndpointsBack = exports.HubEndpointsFront = void 0;
var HubEndpointsFront = /** @class */ (function () {
    function HubEndpointsFront() {
        this.MovedStoryToComplete = "MovedStoryToComplete";
        this.DeletedStory = "DeletedStory";
        this.CurrentStoryChanged = "CurrentStoryChanged";
        this.NewCurrentStory = "NewCurrentStory";
        this.AddedNewStory = "AddedNewStory";
        this.VoteEnd = "VoteEnd";
        this.VoteStart = "VoteStart";
        this.UserRoleChanged = "UserRoleChanged";
        this.VoteChanged = "VoteChanged";
        this.UserLeaved = "UserLeaved";
        this.UserNameChanged = "UserNameChanged";
        this.NewUserInRoom = "NewUserInRoom";
        this.RoomNotCreated = "RoomNotCreated";
        this.ConnectedToRoomError = "ConnectedToRoomError";
        this.EnteredInRoom = "EnteredInRoom";
        this.PlaningNotifyFromServer = "PlaningNotifyFromServer";
        this.NeedRefreshTokens = "NeedRefreshTokens";
    }
    return HubEndpointsFront;
}());
exports.HubEndpointsFront = HubEndpointsFront;
var HubEndpointsBack = /** @class */ (function () {
    function HubEndpointsBack() {
        this.GetConnectionId = "GetConnectionId";
        this.CreateRoom = "CreateRoom";
        this.EnterInRoom = "EnterInRoom";
        this.AddNewStory = "AddNewStory";
        this.MakeStoryComplete = "MakeStoryComplete";
        this.ChangeCurrentStory = "ChangeCurrentStory";
        this.KickUser = "KickUser";
        this.StartVote = "StartVote";
        this.EndVote = "EndVote";
        this.MakeCurrentStory = "MakeCurrentStory";
        this.DeleteStory = "DeleteStory";
        this.SaveRoom = "SaveRoom";
        this.DeleteRoom = "DeleteRoom";
        this.UserNameChange = "UserNameChange";
        this.AddNewRoleToUser = "AddNewRoleToUser";
        this.RemoveRoleUser = "RemoveRoleUser";
        this.LoadNotActualStories = "LoadNotActualStories";
    }
    return HubEndpointsBack;
}());
exports.HubEndpointsBack = HubEndpointsBack;
var HubEndpoints = /** @class */ (function () {
    function HubEndpoints() {
        this.EndpointsFront = new HubEndpointsFront();
        this.EndpointsBack = new HubEndpointsBack();
    }
    return HubEndpoints;
}());
exports.HubEndpoints = HubEndpoints;
var PlaningPokerController = /** @class */ (function () {
    function PlaningPokerController() {
        this.EndPoints = new HubEndpoints();
    }
    PlaningPokerController.prototype.GetUsersIsRoom = function (roomname, userId, onSuccess) {
        G_AjaxHelper.GoAjaxRequest({
            Data: {
                'roomname': roomname,
                'userConnectionId': userId
            },
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/PlanitPoker/get-users-in-room',
        });
    };
    PlaningPokerController.prototype.GetRoomInfo = function (roomname, userId, onSuccess) {
        G_AjaxHelper.GoAjaxRequest({
            Data: {
                'roomname': roomname,
                'userConnectionId': userId
            },
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/PlanitPoker/get-room-info',
        });
    };
    return PlaningPokerController;
}());
exports.PlaningPokerController = PlaningPokerController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/UsersController.ts":
/*!***********************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/UsersController.ts ***!
  \***********************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.UsersController = void 0;
var UsersController = /** @class */ (function () {
    function UsersController() {
    }
    UsersController.prototype.GetShortestUSerInfo = function (onSuccess) {
        G_AjaxHelper.GoAjaxRequest({
            Data: {},
            Type: "GET",
            NotRedirectWhenNotAuth: true,
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (!dataBack.id) {
                        //TODO какая то ошибка
                        alert('что то сломалось-1');
                        return;
                    }
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/users/get-shortest-user-info',
        });
    };
    return UsersController;
}());
exports.UsersController = UsersController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsCardsController.ts":
/*!******************************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsCardsController.ts ***!
  \******************************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsCardsController = void 0;
var WordsCardsController = /** @class */ (function () {
    function WordsCardsController() {
    }
    WordsCardsController.prototype.GetAllForUser = function (onSuccess) {
        G_AjaxHelper.GoAjaxRequest({
            Data: {},
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    // console.log(xhr);
                    var dataBack = xhr;
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/get-all-for-user',
        }, true);
    };
    WordsCardsController.prototype.Create = function (model, onSuccess) {
        var data = new FormData();
        data.append('word', model.Word);
        data.append('word_answer', model.WordAnswer);
        data.append('description', model.Description);
        if (model.MainImageSave) {
            data.append('main_image_new', model.MainImageSave);
        }
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var res = xhr;
                    if (res.id && res.id > 0) {
                        onSuccess(null, res);
                    }
                    else {
                        //что то не то вернулось
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/create',
        }, true);
    };
    WordsCardsController.prototype.CreateList = function (model, listForCards, success) {
        var data = new FormData();
        for (var i = 0; i < model.length; ++i) {
            data.append('newData[' + i + '].word', model[i].Word);
            data.append('newData[' + i + '].word_answer', model[i].WordAnswer);
            data.append('newData[' + i + '].description', model[i].Description);
            data.append('newData[' + i + '].list_id', listForCards);
            // data.append('newData.word_answer', this.state.Cards[i].WordAnswer);
            // data.append('newData.description', this.state.Cards[i].Description);
        }
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    success(resp, null);
                }
                else {
                    var res = xhr;
                    if (res.length > 0) {
                        success(null, res);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/create-list',
        }, true);
    };
    WordsCardsController.prototype.Delete = function (cardId, success) {
        var data = new FormData();
        data.append('id', cardId + '');
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "DELETE",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    success(resp, null);
                }
                else {
                    //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
                    var res = xhr;
                    if ((res === null || res === void 0 ? void 0 : res.id) && res.id > 0) {
                        success(null, res);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/delete',
        }, true);
    };
    WordsCardsController.prototype.Update = function (model, onSuccess) {
        var data = new FormData();
        data.append('id', model.Id + '');
        data.append('word', model.Word);
        data.append('word_answer', model.WordAnswer);
        data.append('description', model.Description);
        data.append('delete_main_image', JSON.stringify(model.NeedDeleteMainImage));
        if (model.MainImageSave) {
            data.append('main_image_new', model.MainImageSave);
        }
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PATCH",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var res = xhr;
                    if (res.id && res.id > 0) {
                        onSuccess(null, res);
                    }
                    else {
                        //что то не то вернулось
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/update',
        }, true);
    };
    WordsCardsController.prototype.Hide = function (cardId, onSuccess) {
        var data = new FormData();
        data.append('id', cardId + '');
        var refThis = this;
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PATCH",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
                    var res = xhr;
                    if (res.result === true || res.result === false) {
                        onSuccess(null, res);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordscards/hide',
        }, true);
    };
    WordsCardsController.prototype.CreateFromFile = function () {
        alert('not implemented');
    };
    WordsCardsController.prototype.DownloadAllWordsFile = function () {
        window.open("/api/wordscards/download-all-words-file");
    };
    return WordsCardsController;
}());
exports.WordsCardsController = WordsCardsController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsListController.ts":
/*!*****************************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsListController.ts ***!
  \*****************************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordsListController = void 0;
var WordsListController = /** @class */ (function () {
    function WordsListController() {
    }
    WordsListController.prototype.GetAllForUser = function (onSuccess) {
        var refThis = this;
        G_AjaxHelper.GoAjaxRequest({
            Data: {},
            Type: "GET",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (dataBack.length > 0) {
                        onSuccess(null, dataBack);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/get-all-for-user',
        }, true);
    };
    WordsListController.prototype.Create = function (title, onSuccess) {
        var data = new FormData();
        data.append('title', title);
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (dataBack.id < 1) {
                        return;
                    }
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/create',
        }, true);
    };
    WordsListController.prototype.RemoveFromList = function (cardId, listId, onSuccess) {
        var data = new FormData();
        data.append('card_id', cardId + '');
        data.append('list_id', listId + '');
        var refThis = this;
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "DELETE",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
                    var res = xhr;
                    if (res.result === true || res.result === false) {
                        onSuccess(null, res);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/remove-from-list',
        }, true);
    };
    WordsListController.prototype.AddToList = function (cardId, listId, onSuccess) {
        var data = new FormData();
        data.append('card_id', cardId + '');
        data.append('list_id', listId + '');
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PUT",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    //TODO тут может быть ошибка, что мы не дождались ответа серва а выбранная картинка уже изменилась
                    var res = xhr;
                    if (res.id_list) {
                        onSuccess(null, res);
                    }
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/add-to-list',
        }, true);
    };
    WordsListController.prototype.Delete = function (id, onSuccess) {
        var data = new FormData();
        data.append('id', id + '');
        var refThis = this;
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "DELETE",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (dataBack.id < 1) {
                        return;
                    }
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/delete',
        }, true);
    };
    WordsListController.prototype.Update = function (model, onSuccess) {
        var data = new FormData();
        data.append('title', model.Title);
        data.append('id', model.Id + '');
        G_AjaxHelper.GoAjaxRequest({
            Data: data,
            Type: "PATCH",
            FuncSuccess: function (xhr, status, jqXHR) {
                var resp = xhr;
                if (resp.errors) {
                    //TODO ошибка
                    onSuccess(resp, null);
                }
                else {
                    var dataBack = xhr;
                    if (dataBack.id < 1) {
                        return;
                    }
                    onSuccess(null, dataBack);
                }
            },
            FuncError: function (xhr, status, error) { },
            Url: G_PathToServer + 'api/wordslist/update',
        }, true);
    };
    return WordsListController;
}());
exports.WordsListController = WordsListController;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/AlertData.ts":
/*!************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/AlertData.ts ***!
  \************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AlertData = exports.AlertDataStored = exports.AlertTypeEnum = void 0;
var AlertTypeEnum;
(function (AlertTypeEnum) {
    AlertTypeEnum[AlertTypeEnum["Error"] = 1] = "Error";
    AlertTypeEnum[AlertTypeEnum["Success"] = 2] = "Success";
})(AlertTypeEnum = exports.AlertTypeEnum || (exports.AlertTypeEnum = {}));
;
var AlertDataStored = /** @class */ (function () {
    function AlertDataStored() {
    }
    AlertDataStored.prototype.FillByAlertData = function (data) {
        this.Key = data.Key;
        this.Text = data.Text;
        this.Type = data.Type;
    };
    return AlertDataStored;
}());
exports.AlertDataStored = AlertDataStored;
var AlertData = /** @class */ (function () {
    function AlertData() {
        this.Timeout = null;
    }
    AlertData.prototype.GetByErrorBack = function (data) {
        var res = [];
        data.errors.forEach(function (errBackText) {
            var newAlert = new AlertData();
            newAlert.Text = errBackText;
            newAlert.Type = AlertTypeEnum.Error;
            // newAlert.Key = data.key;
            res.push(newAlert);
        });
        return res;
    };
    return AlertData;
}());
exports.AlertData = AlertData;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/CustomImage.ts":
/*!**************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/CustomImage.ts ***!
  \**************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.CustomImage = void 0;
var CustomImage = /** @class */ (function () {
    function CustomImage() {
    }
    CustomImage.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.Path = newData.path;
        this.ArticleId = newData.article_id;
    };
    return CustomImage;
}());
exports.CustomImage = CustomImage;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/ErrorHandleLogic.ts":
/*!*******************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/ErrorHandleLogic.ts ***!
  \*******************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.MainErrorHandler = void 0;
var MainErrorHandler = /** @class */ (function () {
    function MainErrorHandler() {
    }
    MainErrorHandler.prototype.NotAuth = function () {
        document.location.href = "/menu/auth/login/";
    };
    return MainErrorHandler;
}());
exports.MainErrorHandler = MainErrorHandler;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/MenuApp/OneCardInListData.ts":
/*!****************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/MenuApp/OneCardInListData.ts ***!
  \****************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneCardInListData = void 0;
var OneCardInListData = /** @class */ (function () {
    function OneCardInListData(backModel) {
        if (backModel) {
            this.FillByBackModel(backModel);
        }
    }
    OneCardInListData.prototype.FillByFullModel = function (newData) {
        this.Id = newData.Id;
        this.Title = newData.Title;
        this.Body = newData.Body;
        this.Image = newData.Image;
        this.Followed = newData.Followed;
    };
    OneCardInListData.prototype.FillByBackModel = function (backModel) {
        this.Id = backModel.id;
        this.Title = backModel.title;
        this.Body = backModel.body;
        this.Image = backModel.image;
        this.Followed = backModel.followed;
    };
    return OneCardInListData;
}());
exports.OneCardInListData = OneCardInListData;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/MenuApp/Poco/IOneCardFullDataEdit.ts":
/*!************************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/MenuApp/Poco/IOneCardFullDataEdit.ts ***!
  \************************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneCardFullDataEdit = void 0;
var OneCardFullDataEdit = /** @class */ (function () {
    function OneCardFullDataEdit() {
        this.AdditionalImagesSave = [];
        // this.AdditionalImagesForRemove = [];
        // this.AdditionalImagesEdit = [];
    }
    return OneCardFullDataEdit;
}());
exports.OneCardFullDataEdit = OneCardFullDataEdit;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/Poco/AppItem.ts":
/*!***************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/Poco/AppItem.ts ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.AppItem = void 0;
var AppItem = /** @class */ (function () {
    function AppItem(init) {
        Object.assign(this, init);
    }
    return AppItem;
}());
exports.AppItem = AppItem;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordCard.ts":
/*!****************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordCard.ts ***!
  \****************************************************************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneWordCard = void 0;
var WordCardWordList_1 = __webpack_require__(/*! ./WordCardWordList */ "./src/components/_ComponentsLink/Models/WordsCardsApp/WordCardWordList.ts");
var OneWordCard = /** @class */ (function () {
    function OneWordCard() {
    }
    OneWordCard.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.ImagePath = newData.image_path;
        this.Word = newData.word;
        this.WordAnswer = newData.word_answer;
        this.Hided = newData.hided;
        this.Description = newData.description;
        this.UserId = newData.user_id;
        this.Lists = newData.lists.map(function (x) {
            var lst = new WordCardWordList_1.WordCardWordList();
            lst.FillByBackModel(x);
            return lst;
        });
    };
    return OneWordCard;
}());
exports.OneWordCard = OneWordCard;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordList.ts":
/*!****************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/WordsCardsApp/OneWordList.ts ***!
  \****************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.OneWordList = void 0;
var OneWordList = /** @class */ (function () {
    function OneWordList() {
    }
    OneWordList.prototype.FillByBackModel = function (newData) {
        this.Id = newData.id;
        this.Title = newData.title;
    };
    return OneWordList;
}());
exports.OneWordList = OneWordList;


/***/ }),

/***/ "./src/components/_ComponentsLink/Models/WordsCardsApp/WordCardWordList.ts":
/*!*********************************************************************************!*\
  !*** ./src/components/_ComponentsLink/Models/WordsCardsApp/WordCardWordList.ts ***!
  \*********************************************************************************/
/***/ ((__unused_webpack_module, exports) => {

"use strict";

Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.WordCardWordList = void 0;
var WordCardWordList = /** @class */ (function () {
    function WordCardWordList() {
    }
    WordCardWordList.prototype.FillByBackModel = function (newData) {
        this.IdList = newData.id_list;
        this.IdWord = newData.id_word;
    };
    return WordCardWordList;
}());
exports.WordCardWordList = WordCardWordList;


/***/ }),

/***/ "./src/index.tsx":
/*!***********************!*\
  !*** ./src/index.tsx ***!
  \***********************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";

/// <reference path="../typings/globals.d.ts" />
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
var React = __importStar(__webpack_require__(/*! react */ "react"));
var ReactDOM = __importStar(__webpack_require__(/*! react-dom */ "react-dom"));
var MainComponent_1 = __webpack_require__(/*! ./components/MainComponent */ "./src/components/MainComponent.tsx");
var AjaxLogic_1 = __webpack_require__(/*! ./components/_ComponentsLink/AjaxLogic */ "./src/components/_ComponentsLink/AjaxLogic.ts");
var AuthenticateController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/AuthenticateController */ "./src/components/_ComponentsLink/Controllers/AuthenticateController.ts");
var ArticleController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/MenuApp/ArticleController */ "./src/components/_ComponentsLink/Controllers/MenuApp/ArticleController.ts");
var PlaningPokerController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/PlaningPoker/PlaningPokerController */ "./src/components/_ComponentsLink/Controllers/PlaningPoker/PlaningPokerController.ts");
var UsersController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/UsersController */ "./src/components/_ComponentsLink/Controllers/UsersController.ts");
var WordsCardsController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/WordsCardsApp/WordsCardsController */ "./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsCardsController.ts");
var WordsListController_1 = __webpack_require__(/*! ./components/_ComponentsLink/Controllers/WordsCardsApp/WordsListController */ "./src/components/_ComponentsLink/Controllers/WordsCardsApp/WordsListController.ts");
var ErrorHandleLogic_1 = __webpack_require__(/*! ./components/_ComponentsLink/Models/ErrorHandleLogic */ "./src/components/_ComponentsLink/Models/ErrorHandleLogic.ts");
__webpack_require__(/*! ../style/main.css */ "./style/main.css");
__webpack_require__(/*! ../style/auth.css */ "./style/auth.css");
__webpack_require__(/*! ../style/body.css */ "./style/body.css");
__webpack_require__(/*! ../style/footer.css */ "./style/footer.css");
__webpack_require__(/*! ../style/header.css */ "./style/header.css");
__webpack_require__(/*! ../style/alerts.css */ "./style/alerts.css");
__webpack_require__(/*! ../style/menu.css */ "./style/menu.css");
__webpack_require__(/*! ../style/menu_app.css */ "./style/menu_app.css");
__webpack_require__(/*! ../style/menu_app_one_card.css */ "./style/menu_app_one_card.css");
__webpack_require__(/*! ../style/word_cards.css */ "./style/word_cards.css");
//localstorage
//header_auth
//INIT CONST
// window.G_PathToBaseImages = "../../images/";
window.G_PathToBaseImages = "/images/";
window.G_EmptyImagePath = G_PathToBaseImages + "user_empty_image.png";
window.G_PreloaderPath = G_PathToBaseImages + "loading.gif";
window.G_PathToServer = "/"; //"http://localhost:8000/";
window.G_PathToServerMenu = G_PathToServer + "menu/";
window.G_AjaxHelper = new AjaxLogic_1.AjaxHelper();
window.G_ErrorHandleLogic = new ErrorHandleLogic_1.MainErrorHandler();
//controllers
window.G_AuthenticateController = new AuthenticateController_1.AuthenticateController();
window.G_UsersController = new UsersController_1.UsersController();
window.G_ArticleController = new ArticleController_1.ArticleController();
window.G_WordsCardsController = new WordsCardsController_1.WordsCardsController();
window.G_WordsListController = new WordsListController_1.WordsListController();
window.G_PlaningPokerController = new PlaningPokerController_1.PlaningPokerController();
//
// G_AddAbsoluteAlertToState -->MainComponent
//
ReactDOM.render(React.createElement(MainComponent_1.MainComponent, null), 
// <div>test</div>,
document.getElementById("menu_all_main_content_start"));


/***/ }),

/***/ "./node_modules/value-equal/esm/value-equal.js":
/*!*****************************************************!*\
  !*** ./node_modules/value-equal/esm/value-equal.js ***!
  \*****************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
function valueOf(obj) {
  return obj.valueOf ? obj.valueOf() : Object.prototype.valueOf.call(obj);
}

function valueEqual(a, b) {
  // Test for strict equality first.
  if (a === b) return true;

  // Otherwise, if either of them == null they are not equal.
  if (a == null || b == null) return false;

  if (Array.isArray(a)) {
    return (
      Array.isArray(b) &&
      a.length === b.length &&
      a.every(function(item, index) {
        return valueEqual(item, b[index]);
      })
    );
  }

  if (typeof a === 'object' || typeof b === 'object') {
    var aValue = valueOf(a);
    var bValue = valueOf(b);

    if (aValue !== a || bValue !== b) return valueEqual(aValue, bValue);

    return Object.keys(Object.assign({}, a, b)).every(function(key) {
      return valueEqual(a[key], b[key]);
    });
  }

  return false;
}

/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (valueEqual);


/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "React" ***!
  \************************/
/***/ ((module) => {

"use strict";
module.exports = React;

/***/ }),

/***/ "react-dom":
/*!***************************!*\
  !*** external "ReactDOM" ***!
  \***************************/
/***/ ((module) => {

"use strict";
module.exports = ReactDOM;

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			id: moduleId,
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/global */
/******/ 	(() => {
/******/ 		__webpack_require__.g = (function() {
/******/ 			if (typeof globalThis === 'object') return globalThis;
/******/ 			try {
/******/ 				return this || new Function('return this')();
/******/ 			} catch (e) {
/******/ 				if (typeof window === 'object') return window;
/******/ 			}
/******/ 		})();
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module is referenced by other modules so it can't be inlined
/******/ 	var __webpack_exports__ = __webpack_require__("./src/index.tsx");
/******/ 	
/******/ })()
;
//# sourceMappingURL=bundle.js.map