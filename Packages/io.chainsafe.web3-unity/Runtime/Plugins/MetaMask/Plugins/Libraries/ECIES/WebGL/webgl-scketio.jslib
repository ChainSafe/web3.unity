var SocketPlugin = {
  $socketState: {
    SocketGameObjectName: "",
    sockets: new Map(),

    /* Event listeners */
    onConnected: null,
    onDisconnected: null,

    //send to unity stuff
    CallUnityEvent: function (id, event, data) {
      var JsonData = null;
      if (data != null) {
        JsonData = data;
      }
      unityInstance.SendMessage(
        socketState.SocketGameObjectName,
        "callSocketEvent",
        JSON.stringify({
          EventName: event,
          SocketId: id,
          JsonData: JSON.stringify(JsonData),
        })
      );
    },
  },

  /**
   * Set onAvailable callback
   *
   * @param callback Reference to C# static function
   */
  SocketIOSetOnAvailable: function (callback) {
    socketState.onAvailable = callback;
  },

  /**
   * Set onConnected callback
   *
   * @param callback Reference to C# static function
   */
  SocketIOSetOnConnected: function (callback) {
    socketState.onConnected = callback;
  },

  SetupGameObjectName: function (str) {
    socketState.SocketGameObjectName = UTF8ToString(str);
    socketState.sockets = new Map();
  },

  GetProtocol: function () {
    if (io != undefined) return io.getProtocol;
    else {
      console.error(
        "SocketIO io object not found! Did you forget to include Reference in header?"
      );
      throw new Error(
        "SocketIO object not found! Did you forget to include Reference in header?"
      );
    }
  },

  EstablishSocket: function (url_raw, options_raw) {
    if (io != undefined) {
      const url = UTF8ToString(url_raw);
      const options = UTF8ToString(options_raw); //string of user options selected

      var soc;
      if (options.length > 0) soc = io(url, JSON.parse(options));
      else soc = io(url);

      var id = 0;
      do {
        //generate an id between 1 and 10000
        id = Math.floor(Math.random() * 10000) + 1;
      } while (socketState.sockets.has(id));

      socketState.sockets.set(id, soc);

      var cur = this;

      soc.onAny(function (event, args) {
        socketState.CallUnityEvent(id, event, args);
      });
      soc.on("connect", (args) => {
        socketState.CallUnityEvent(id, "connect", args);
      });
      soc.on("disconnect", (args) => {
        socketState.CallUnityEvent(id, "disconnect", args);
      });
      soc.on("error", function (evData) {
        console.error("Connection Error:", evData);
      });

      return id;
    } else {
      console.error(
        "SocketIO io object not found! Did you forget to include Reference in header?"
      );
      throw new Error(
        "SocketIO object not found! Did you forget to include Reference in header?"
      );
    }
  },

  //Socket Object stuff

  Socket_IsConnected: function (id) {
    return socketState.sockets.get(id).connected;
  },

  Socket_Connect: function (id) {
    socketState.sockets.get(id).connect();
  },

  Socket_Disconnect: function (id) {
    socketState.sockets.get(id).disconnect();
  },

  // Socket_Send: function(id, data_raw) {
  //     if(data_raw != null)
  //         socketState.sockets.get(id).send(JSON.parse(UTF8ToString(data_raw)));
  //     else
  //         socketState.sockets.get(id).send(null);
  // },

  Socket_Emit: function (id, event_raw, data_raw) {
    if (UTF8ToString(data_raw).length == 0) {
      socketState.sockets.get(id).emit(UTF8ToString(event_raw), null);
    } else {
      socketState.sockets
        .get(id)
        .emit(UTF8ToString(event_raw), JSON.parse(UTF8ToString(data_raw)));
    }
  },

  Socket_Get_Conn_Id: function (id) {
    var result = socketState.sockets.get(id).id;
    if (result != undefined) {
      var buffersize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(buffersize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } else {
      return null;
    }
  },
};
autoAddDeps(SocketPlugin, "$socketState");
mergeInto(LibraryManager.library, SocketPlugin);
