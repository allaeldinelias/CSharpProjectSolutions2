jQuery(document).ready(function () {
    jQuery.event.props.push("dataTransfer");
    new Upload();
});

var Upload = Toolbox.Base.extend({
    constructor: function () {
        jQuery("#uploadForm").submit(jQuery.proxy(this.uploadForm, this));
        jQuery("#dropTarget").bind("dragover", jQuery.proxy(this.dragOver, this));
        jQuery("body").bind("dragover", jQuery.proxy(this.dragOverBody, this));
        jQuery("#dropTarget").bind("drop", jQuery.proxy(this.drop, this));
    },

    sImageUrl: null,
    uploadForm: function () {
        var sData = new FormData(jQuery("#uploadForm")[0]);
        sData.append("bImageOnly", false);
        if (this.sImageUrl != null) {
            sData.append("imageurl", this.sImageUrl);
        }
        jQuery.ajax({
            url: "upload.cshtml", data: sData, dataType: "json", type: "post", contentType: false,
            processData: false
        }).
            done(jQuery.proxy(this.clearForm, this)).
            fail(jQuery.proxy(this.errorReturn, this));
        return false;
    },

    clearForm: function (oResults) {
        jQuery("#placeholder").attr("src", "images/drop-target.png");
        jQuery("#results").html(oResults.result);
        jQuery("input").val("");
        jQuery("textarea").val("");
        jQuery("#title").focus();
    },

    dragOver: function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        evt.dataTransfer.dropEffect = "copy";
    },
    dragOverBody: function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        evt.dataTransfer.dropEffect = "none";
    },

    drop: function(evt) {
        evt.stopPropagation();
        evt.preventDefault();
        // step 2 get the file that was dropped into a formdata object
        if (evt.dataTransfer.files === undefined) {
            alert("Internet Explorer doesn't support file drop yet ... Please use the Upload mechanism, below");
            return;
        }

        var files = evt.dataTransfer.files;
        // files is a FileList of File objects. We are only uploading the first one here.
        var myFileObject = files[0];
        var formData = new FormData();
        // Append our file to the formData object
        // Notice the first argument "file" and keep it in mind
        formData.append('fileToUpload', myFileObject);
        formData.append('bImageOnly', true);
        jQuery.ajax({
            url: "upload.cshtml", data: formData, dataType: "json", type: "post", contentType: false,
            processData: false
        }).
            done(jQuery.proxy(this.displayDroppedImage, this)).
            fail(jQuery.proxy(this.errorReturn, this));

    },

    displayDroppedImage: function (oResults) {
        console.log(oResults);
        jQuery("#placeholder").attr("src", oResults.imageurl);
        this.sImageUrl = oResults.imageurl;
    },
    errorReturn: function (err) {
        console.log(err);
    }
});