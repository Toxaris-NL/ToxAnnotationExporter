var save = function(){};
var discard = function(){};
(function(){
  var edited = false;

  var editableCells = document.querySelectorAll('td[contenteditable="true"]');
  for (var i = 0; i < editableCells.length; i++) {
      var cell = editableCells[i];
      addEvent(cell, 'input', function(){
      annotationsEdited();
      });
  }

  function annotationsEdited() {
    if (edited === true) { return; }
    edited = true;
    showSaveLayer();
  }

  function showSaveLayer() {
    document.querySelectorAll('#saveLayer')[0].style.display = 'block';
  }

  function hideSaveLayer() {
    document.querySelectorAll('#saveLayer')[0].style.display = null;
  }

  save = function() {
    edited = false;
    // It is important to hide the save dialog before reading the source of the page, or the
    // newly generated file will show the save dialog on load
    hideSaveLayer();
    generateDownload(getFilenameFromUrl(), generateHtmlFromSourceCode());
    }
    discard = function() {
    edited = false;
    window.location.reload();
  }

  function getFilenameFromUrl() {
    var url = window.location.pathname;
    var filename = decodeURIComponent(url.substring(url.lastIndexOf('/')+1));

    // Strip, if present, the automatic numbering some browsers do when saving a file with a filename
    // that already exists, like "file (1)", "file (2)", "file (3)". This will leave the
    // "original" file name, so the browser can resume numbering from there.
    filename = filename.replace(/ \(([0-9]+)\)\./, '.', filename);

    return filename;
  }

  function generateHtmlFromSourceCode() {
    var html = '<!doctype html>\n';
    html    += '<html>\n';
    html    += '<head>\n';
    html    += document.head.innerHTML.trim() + '\n';
    html    += '</head>\n';
    html    += '<body>\n';
    html    += document.body.innerHTML.trim() + '\n';
    html    += '</body>\n';
    html    += '</html>';
    return html;
  }

  function generateDownload(fileName, fileContent) {
    var a = document.createElement('a');
    a.href = 'data:text/html;charset=utf-8;base64,' + b64EncodeUnicode(fileContent);
    a.setAttribute('download', fileName);
    a.style.display = 'none';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  }

  function b64EncodeUnicode(str) {
    return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function(match, p1) {
    return String.fromCharCode('0x' + p1);
    }));
  }

  function b64DecodeUnicode(str) {
    return decodeURIComponent(Array.prototype.map.call(atob(str), function(c) {
    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
  }

  function addEvent(element, eventName, handler) {
    if (element.addEventListener) {
    element.addEventListener(eventName, handler, false);
    } else if (element.attachEvent) {
    element.attachEvent('on' + eventName, handler);
    } else {
    console.log ('Warning! No handling possible for element');
    }
  }

  addEvent(window, 'beforeunload', function(e){
    // Notice that to avoid scamming, chromium (and hence chrome) has disabled the possibility to set
    // a custom message in the dialog. The user will be presented with a predefined dialog that is
    // easy to understand. Firefox has adopted the same policy.
    // This may or may not display the custom message on other browsers.
    if (edited === true) {
    var msg = 'There are unsaved changes. Do you wish to exit without saving the changes?\n';
    msg    += 'If you click accept, all changes will be lost. If you click cancel, you will be allowed ';
    msg    += 'to save your changes before exiting.';

    (e || window.event).returnValue = msg; // Some browsers use this
    return msg; // And others use this
    }
    });
}());