mergeInto(LibraryManager.library, {
  
  UploadImage: function () {
    var input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/png, image/jpeg, image/gif';
    input.onchange = function(event) {
        var file = event.target.files[0];
        if (!file) return;
        var reader = new FileReader();
        reader.onload = function(event) {
            var dataUrl = event.target.result;
            SendMessage('ImageUploader', 'OnImageSelected', dataUrl);
        };
        reader.readAsDataURL(file);
    };
    input.click();
    }
});