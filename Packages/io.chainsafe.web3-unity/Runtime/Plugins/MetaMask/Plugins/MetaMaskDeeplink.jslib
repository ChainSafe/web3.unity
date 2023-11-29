var LibraryMetaMaskDeeplink = {

OpenMetaMaskDeeplink: function (url) {

  var urlStr = UTF8ToString(url);

  // Function to hide the popup
  function hidePopup() {
    popup.style.display = "none";
  }

  // Function to show the popup
  function showPopup() {
    popup.style.display = "flex";
  }

  // Function to detect iOS, iPad, or Safari
  function isIOSOrSafari() {
    var userAgent = window.navigator.userAgent;
    var isIOS = /iPad|iPhone|iPod/.test(userAgent) && !window.MSStream;
    var isSafari = /^((?!chrome|android).)*safari/i.test(userAgent);
    return isIOS || isSafari;
  }

  if (isIOSOrSafari()) {
    // Check if the popup already exists, if not create one
    var popup = document.getElementById("metamask-popup");
    if (!popup) {
      // create a div to contain the popup
      popup = document.createElement("div");
      popup.id = "metamask-popup";
      popup.style.display = "flex";
      popup.style.flexDirection = "column";
      popup.style.justifyContent = "center";
      popup.style.alignItems = "center";
      popup.style.position = "fixed";
      popup.style.top = "0";
      popup.style.left = "0";
      popup.style.width = "100%";
      popup.style.height = "100%";
      popup.style.backgroundColor = "rgba(255, 255, 255, 0.9)";
      popup.style.zIndex = "1000";
      popup.style.fontFamily = "Myriad Pro, Arial, sans-serif";

      // create the inner content of the popup
      var logo = document.createElement("div");
      logo.style.width = "200px";
      logo.style.height = "200px";
      logo.style.backgroundColor = "rgba(0, 0, 0, 0)";
      logo.style.borderRadius = "50%";
      logo.style.marginBottom = "20px";
      logo.style.display = "flex";
      logo.style.justifyContent = "center";
      logo.style.alignItems = "center";

      var img = document.createElement("img");
      var metamaskFoxSvg = 'data:image/svg+xml;base64,PHN2ZyBmaWxsPSJub25lIiBoZWlnaHQ9IjMzIiB2aWV3Qm94PSIwIDAgMzUgMzMiIHdpZHRoPSIzNSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiIHN0cm9rZS13aWR0aD0iLjI1Ij48cGF0aCBkPSJtMzIuOTU4MiAxLTEzLjEzNDEgOS43MTgzIDIuNDQyNC01LjcyNzMxeiIgZmlsbD0iI2UxNzcyNiIgc3Ryb2tlPSIjZTE3NzI2Ii8+PGcgZmlsbD0iI2UyNzYyNSIgc3Ryb2tlPSIjZTI3NjI1Ij48cGF0aCBkPSJtMi42NjI5NiAxIDEzLjAxNzE0IDkuODA5LTIuMzI1NC01LjgxODAyeiIvPjxwYXRoIGQ9Im0yOC4yMjk1IDIzLjUzMzUtMy40OTQ3IDUuMzM4NiA3LjQ4MjkgMi4wNjAzIDIuMTQzNi03LjI4MjN6Ii8+PHBhdGggZD0ibTEuMjcyODEgMjMuNjUwMSAyLjEzMDU1IDcuMjgyMyA3LjQ2OTk0LTIuMDYwMy0zLjQ4MTY2LTUuMzM4NnoiLz48cGF0aCBkPSJtMTAuNDcwNiAxNC41MTQ5LTIuMDc4NiAzLjEzNTggNy40MDUuMzM2OS0uMjQ2OS03Ljk2OXoiLz48cGF0aCBkPSJtMjUuMTUwNSAxNC41MTQ5LTUuMTU3NS00LjU4NzA0LS4xNjg4IDguMDU5NzQgNy40MDQ5LS4zMzY5eiIvPjxwYXRoIGQ9Im0xMC44NzMzIDI4Ljg3MjEgNC40ODE5LTIuMTYzOS0zLjg1ODMtMy4wMDYyeiIvPjxwYXRoIGQ9Im0yMC4yNjU5IDI2LjcwODIgNC40Njg5IDIuMTYzOS0uNjEwNS01LjE3MDF6Ii8+PC9nPjxwYXRoIGQ9Im0yNC43MzQ4IDI4Ljg3MjEtNC40NjktMi4xNjM5LjM2MzggMi45MDI1LS4wMzkgMS4yMzF6IiBmaWxsPSIjZDViZmIyIiBzdHJva2U9IiNkNWJmYjIiLz48cGF0aCBkPSJtMTAuODczMiAyOC44NzIxIDQuMTU3MiAxLjk2OTYtLjAyNi0xLjIzMS4zNTA4LTIuOTAyNXoiIGZpbGw9IiNkNWJmYjIiIHN0cm9rZT0iI2Q1YmZiMiIvPjxwYXRoIGQ9Im0xNS4xMDg0IDIxLjc4NDItMy43MTU1LTEuMDg4NCAyLjYyNDMtMS4yMDUxeiIgZmlsbD0iIzIzMzQ0NyIgc3Ryb2tlPSIjMjMzNDQ3Ii8+PHBhdGggZD0ibTIwLjUxMjYgMjEuNzg0MiAxLjA5MTMtMi4yOTM1IDIuNjM3MiAxLjIwNTF6IiBmaWxsPSIjMjMzNDQ3IiBzdHJva2U9IiMyMzM0NDciLz48cGF0aCBkPSJtMTAuODczMyAyOC44NzIxLjY0OTUtNS4zMzg2LTQuMTMxMTcuMTE2N3oiIGZpbGw9IiNjYzYyMjgiIHN0cm9rZT0iI2NjNjIyOCIvPjxwYXRoIGQ9Im0yNC4wOTgyIDIzLjUzMzUuNjM2NiA1LjMzODYgMy40OTQ2LTUuMjIxOXoiIGZpbGw9IiNjYzYyMjgiIHN0cm9rZT0iI2NjNjIyOCIvPjxwYXRoIGQ9Im0yNy4yMjkxIDE3LjY1MDctNy40MDUuMzM2OS42ODg1IDMuNzk2NiAxLjA5MTMtMi4yOTM1IDIuNjM3MiAxLjIwNTF6IiBmaWxsPSIjY2M2MjI4IiBzdHJva2U9IiNjYzYyMjgiLz48cGF0aCBkPSJtMTEuMzkyOSAyMC42OTU4IDIuNjI0Mi0xLjIwNTEgMS4wOTEzIDIuMjkzNS42ODg1LTMuNzk2Ni03LjQwNDk1LS4zMzY5eiIgZmlsbD0iI2NjNjIyOCIgc3Ryb2tlPSIjY2M2MjI4Ii8+PHBhdGggZD0ibTguMzkyIDE3LjY1MDcgMy4xMDQ5IDYuMDUxMy0uMTAzOS0zLjAwNjJ6IiBmaWxsPSIjZTI3NTI1IiBzdHJva2U9IiNlMjc1MjUiLz48cGF0aCBkPSJtMjQuMjQxMiAyMC42OTU4LS4xMTY5IDMuMDA2MiAzLjEwNDktNi4wNTEzeiIgZmlsbD0iI2UyNzUyNSIgc3Ryb2tlPSIjZTI3NTI1Ii8+PHBhdGggZD0ibTE1Ljc5NyAxNy45ODc2LS42ODg2IDMuNzk2Ny44NzA0IDQuNDgzMy4xOTQ5LTUuOTA4N3oiIGZpbGw9IiNlMjc1MjUiIHN0cm9rZT0iI2UyNzUyNSIvPjxwYXRoIGQ9Im0xOS44MjQyIDE3Ljk4NzYtLjM2MzggMi4zNTg0LjE4MTkgNS45MjE2Ljg3MDQtNC40ODMzeiIgZmlsbD0iI2UyNzUyNSIgc3Ryb2tlPSIjZTI3NTI1Ii8+PHBhdGggZD0ibTIwLjUxMjcgMjEuNzg0Mi0uODcwNCA0LjQ4MzQuNjIzNi40NDA2IDMuODU4NC0zLjAwNjIuMTE2OS0zLjAwNjJ6IiBmaWxsPSIjZjU4NDFmIiBzdHJva2U9IiNmNTg0MWYiLz48cGF0aCBkPSJtMTEuMzkyOSAyMC42OTU4LjEwNCAzLjAwNjIgMy44NTgzIDMuMDA2Mi42MjM2LS40NDA2LS44NzA0LTQuNDgzNHoiIGZpbGw9IiNmNTg0MWYiIHN0cm9rZT0iI2Y1ODQxZiIvPjxwYXRoIGQ9Im0yMC41OTA2IDMwLjg0MTcuMDM5LTEuMjMxLS4zMzc4LS4yODUxaC00Ljk2MjZsLS4zMjQ4LjI4NTEuMDI2IDEuMjMxLTQuMTU3Mi0xLjk2OTYgMS40NTUxIDEuMTkyMSAyLjk0ODkgMi4wMzQ0aDUuMDUzNmwyLjk2Mi0yLjAzNDQgMS40NDItMS4xOTIxeiIgZmlsbD0iI2MwYWM5ZCIgc3Ryb2tlPSIjYzBhYzlkIi8+PHBhdGggZD0ibTIwLjI2NTkgMjYuNzA4Mi0uNjIzNi0uNDQwNmgtMy42NjM1bC0uNjIzNi40NDA2LS4zNTA4IDIuOTAyNS4zMjQ4LS4yODUxaDQuOTYyNmwuMzM3OC4yODUxeiIgZmlsbD0iIzE2MTYxNiIgc3Ryb2tlPSIjMTYxNjE2Ii8+PHBhdGggZD0ibTMzLjUxNjggMTEuMzUzMiAxLjEwNDMtNS4zNjQ0Ny0xLjY2MjktNC45ODg3My0xMi42OTIzIDkuMzk0NCA0Ljg4NDYgNC4xMjA1IDYuODk4MyAyLjAwODUgMS41Mi0xLjc3NTItLjY2MjYtLjQ3OTUgMS4wNTIzLS45NTg4LS44MDU0LS42MjIgMS4wNTIzLS44MDM0eiIgZmlsbD0iIzc2M2UxYSIgc3Ryb2tlPSIjNzYzZTFhIi8+PHBhdGggZD0ibTEgNS45ODg3MyAxLjExNzI0IDUuMzY0NDctLjcxNDUxLjUzMTMgMS4wNjUyNy44MDM0LS44MDU0NS42MjIgMS4wNTIyOC45NTg4LS42NjI1NS40Nzk1IDEuNTE5OTcgMS43NzUyIDYuODk4MzUtMi4wMDg1IDQuODg0Ni00LjEyMDUtMTIuNjkyMzMtOS4zOTQ0eiIgZmlsbD0iIzc2M2UxYSIgc3Ryb2tlPSIjNzYzZTFhIi8+PHBhdGggZD0ibTMyLjA0ODkgMTYuNTIzNC02Ljg5ODMtMi4wMDg1IDIuMDc4NiAzLjEzNTgtMy4xMDQ5IDYuMDUxMyA0LjEwNTItLjA1MTloNi4xMzE4eiIgZmlsbD0iI2Y1ODQxZiIgc3Ryb2tlPSIjZjU4NDFmIi8+PHBhdGggZD0ibTEwLjQ3MDUgMTQuNTE0OS02Ljg5ODI4IDIuMDA4NS0yLjI5OTQ0IDcuMTI2N2g2LjExODgzbDQuMTA1MTkuMDUxOS0zLjEwNDg3LTYuMDUxM3oiIGZpbGw9IiNmNTg0MWYiIHN0cm9rZT0iI2Y1ODQxZiIvPjxwYXRoIGQ9Im0xOS44MjQxIDE3Ljk4NzYuNDQxNy03LjU5MzIgMi4wMDA3LTUuNDAzNGgtOC45MTE5bDIuMDAwNiA1LjQwMzQuNDQxNyA3LjU5MzIuMTY4OSAyLjM4NDIuMDEzIDUuODk1OGgzLjY2MzVsLjAxMy01Ljg5NTh6IiBmaWxsPSIjZjU4NDFmIiBzdHJva2U9IiNmNTg0MWYiLz48L2c+PC9zdmc+'
      img.src = metamaskFoxSvg;
      img.alt = "Metamask logo";
      img.width = "150";
      img.height = "150";
      logo.appendChild(img);

      var header = document.createElement("div");
      header.style.fontSize = "24px";
      header.style.fontWeight = "bold";
      header.style.color = "#333333";
      header.style.marginBottom = "20px";
      header.style.fontFamily = "Myriad Pro, Arial, sans-serif";
      header.innerText = "Confirm Deeplink?";

      var button = document.createElement("a");
      button.href = urlStr;
      button.style.fontSize = "18px";
      button.style.fontWeight = "bold";
      button.style.color = "#ffffff";
      button.style.backgroundColor = "#1565C0";
      button.style.padding = "10px 20px";
      button.style.borderRadius = "5px";
      button.style.cursor = "pointer";
      button.style.textDecoration = "none";
      button.style.fontFamily = "Myriad Pro, Arial, sans-serif";
      button.innerText = "Yes Proceed";

      // append the inner content to the popup
      popup.appendChild(logo);
      popup.appendChild(header);
      popup.appendChild(button);

      // add the popup to the document body
      document.body.appendChild(popup);

       // Update the button's href attribute
    button.href = urlStr;

    // Show the popup
    showPopup();

    // Add a click event listener to the button
    button.addEventListener("click", function(event) {
      hidePopup(); // Hide the popup
    });

    // Log a message to the console
    console.log("Added MetaMask popup to page.");

  }else{
    showPopup();

  }
  }else {
    // If not iOS, iPad, or Safari, open the URL using window.open()
    window.open(urlStr, "_blank");
     console.log("MetaMask deeplink URL mobile: " + urlStr);
  }
},

WebGLIsMobile: function () {
  return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent)
},

LSExists: function (key) {
  return localStorage.getItem(UTF8ToString(key)) !== null
},
  
LSWrite: function(key, data) {
  localStorage.setItem(UTF8ToString(key), UTF8ToString(data))
},

LSRead: function(key) {
  var data = localStorage.getItem(UTF8ToString(key))
  
  // required to return string data
  var bufferSize = lengthBytesUTF8(data) + 1;
  var buffer = _malloc(bufferSize)
  stringToUTF8(data, buffer, bufferSize);
  return buffer;
},

LSDelete: function(key) {
  localStorage.removeItem(UTF8ToString(key))
}, 
  
_SendRequestFetch: function(idUtf8, objectNameUtf8, methodUtf8, urlUtf8, parUtf8, isGet, authHeaderKeyUtf8, authHeaderValueUtf8) {
  const id = UTF8ToString(idUtf8)
  const objectName = UTF8ToString(objectNameUtf8)
  const method = UTF8ToString(methodUtf8)
  const url = UTF8ToString(urlUtf8)
  const par = UTF8ToString(parUtf8)
  const authHeaderKey = UTF8ToString(authHeaderKeyUtf8)
  const authHeaderValue = UTF8ToString(authHeaderValueUtf8)
  
  let headers = {}
  if (authHeaderKey) {
    headers = {
      [authHeaderKey]: authHeaderValue
    }
  }
  
  const data = {
    method,
    body: par,
    headers
  }
  
  fetch(url, data).then(function (resp) {
    return resp.json()
  }).then(function (result) {
    const resultData = {
      responseJson: JSON.stringify(result),
      errorMessage: null,
      id,
    }
    
    window.unityInstance.SendMessage(objectName, "OnFetchResponseCallback", JSON.stringify(resultData))
  }).catch(function(e) {
    const resultData = {
      responseJson: null,
      errorMessage: e.toString(),
      id,
    }
    
    window.unityInstance.SendMessage(objectName, "OnFetchResponseCallback", JSON.stringify(resultData))
  })
},
  
};

mergeInto(LibraryManager.library, LibraryMetaMaskDeeplink);
