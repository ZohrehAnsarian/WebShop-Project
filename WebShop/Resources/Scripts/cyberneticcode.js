

var deviceIsTouchable = false;

if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    deviceIsTouchable = true;
}

//Methods of Search start
function searchBoxKeypress(e, searchBoxId, routeAddress, waitingMessage) {

    if (e.keyCode == 13) {

        var searchBoxText = $("#" + searchBoxId).val();

        if (searchBoxText == '') {
            return;
        }

        searchButtonClick(searchBoxId, routeAddress, waitingMessage);
    }

}

function searchButtonClick(searchBoxId, routeAddress, waitingMessage) {

    var searchBoxText = $("#" + searchBoxId);

    if (searchBoxText.val() == '') {

        return;
    }

    HoldOn.open({
        theme: 'sk-bounce',
        message: "<h4>" + waitingMessage + "</h4>"
    });

    location.href = routeAddress + "?searchText=" + searchBoxText.val();
}

//Methods of Search end

function setHtmlContent(id, elementId, routeAddress) {

    $.ajax({
        type: "Post",
        url: routeAddress,
        data: { id: id },
        dataType: "json",
        success: function (response) {

            $('#' + elementId).summernote('destroy');

            $('#' + elementId).html(response);


            $('#' + elementId).summernote({
                height: 300, minHeight: null, maxHeight: null, focus: false,
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']], ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['height', ['height']],
                    ['table', ['table']],
                    ['insert', ['link', 'picture', 'hr', 'uploadfile']],
                    ['view', ['fullscreen', 'codeview']],
                    ['help', ['help']]
                ],
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

function resetFileInput(id, multiple, callBackFunction) {

    //$("#" + id).replaceWith($("#" + id).val('').clone(true));

    var oldInput = document.getElementById(id);
    var newInput = document.createElement("input");

    newInput.type = "file";
    newInput.id = oldInput.id;
    newInput.name = oldInput.name;
    newInput.className = oldInput.className;
    newInput.style.cssText = oldInput.style.cssText;
    newInput.multiple = multiple;
    newInput.onchange = callBackFunction;
    oldInput.parentNode.replaceChild(newInput, oldInput);

}

function resetFileInputByContainerAndClassName(containerId, className, multiple, callBackFunction) {

    var oldInput = document.getElementById(containerId).getElementsByClassName(className)[0];
    var newInput = document.createElement("input");

    newInput.type = "file";
    newInput.id = oldInput.id;
    newInput.name = oldInput.name;
    newInput.className = oldInput.className;
    newInput.style.cssText = oldInput.style.cssText;
    newInput.multiple = multiple;

    oldInput.parentNode.replaceChild(newInput, oldInput);

    var callFunction = function () {

        callBackFunction(newInput);
    }
    newInput.onchange = callFunction;


}

//Confirm dialog start

function openConfirmDialog(confirmDialogObject) {

    if (confirmDialogObject.width == "") {
        screenWidth = $(window).width();
        if (screenWidth < 400) {
            confirmDialogObject.width = "100%";
        }
        else
            if (screenWidth >= 400 && screenWidth < 768) {
                confirmDialogObject.width = "50%";
            }
            else
                if (screenWidth >= 768 && screenWidth < 992) {
                    confirmDialogObject.width = "40%";
                }
                else if (screenWidth >= 992 && screenWidth < 1200) {
                    confirmDialogObject.width = "30%";
                }
                else {
                    confirmDialogObject.width = "20%";
                }
    }

    $("#" + confirmDialogObject.dialogConfirmId).dialog({
        resizable: false,
        height: confirmDialogObject.height,
        width: confirmDialogObject.width,
        modal: true,
        dialogClass: "no-close", //removes X from dialog
        buttons: {
            'YesButton': {
                click: function () {
                    $(this).dialog("close");
                    confirmDialogObject.acceptCallbak(confirmDialogObject.sender);
                },
                text: confirmDialogObject.yesButton,
                class: "btn btn-primary"
            },
            'NoButton': {
                click: function () {
                    confirmDialogObject.rejectCallbak(confirmDialogObject.sender);
                    $(this).dialog("close");
                },
                text: confirmDialogObject.noButton,
                class: "btn btn-primary"
            }
        },
        create: function () {
            var buttons = $('.ui-dialog-buttonset').children('button');
            buttons.removeClass("ui-button ui-widget ui-state-default ui-state-active ui-state-focus");
        },
        open: function () {
            $('.ui-widget-overlay').addClass('custom-overlay');
        }
    });
}


//Confirm dialog end

// Validation mthods start

function validateEmail(sEmail) {

    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(sEmail)) {
        return true;
    }
    return false;
}

// Validation mthods end


function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}


function showConfirmModal(title, message, okText, cancelText, callBackFunction) {
    event.stopImmediatePropagation();

    $.confirm({
        title: title,
        content: '' +
            '<form action="" class="formName">' +
            '<hr/>' +
            '<span>' + message + '</span>' +
            '</form>',
        buttons: {
            formSubmit: {
                text: '' + okText + '',
                btnClass: 'btn-blue',
                action: function () {

                    callBackFunction();
                }
            },
            cancel: {
                text: cancelText,
                btnClass: 'btn-default',
                action: function () {
                    //close
                }
            },
        },
        onContentReady: function () {
            // bind to events
            var jc = this;
            this.$content.find('form').on('submit', function (e) {
                // if the user submits the form by pressing enter in the field.
                e.preventDefault();
                jc.$$formSubmit.trigger('click'); // reference the button and click it
            });
        }
    });

}
function getBackgroundImageSize(element) {

    var imageSize = {
        width: 0,
        height: 0
    };
    var imageSrc = $(element).css('background-image').replace(/url\((['"])?(.*?)\1\)/gi, '$2').split(',')[0];

    var image = new Image();
    image.src = imageSrc;

    imageSize.width = image.width,
        imageSize.height = image.height;

    return imageSize;
}

function setSwiperImagesAcpetRatio() {

    $(".navigation-banner-wrapper").each(function (index, element) {
        var percentChange = 0;
        var deviceWidth = $(window).width();
        var imageSize = getBackgroundImageSize(element);
        var imageWidth = imageSize.width;
        var imageheight = imageSize.height;

        if (deviceWidth >= imageWidth) {
            percentChange = Math.round((imageWidth * 100) / deviceWidth);

            element.width = Math.round((imageWidth * percentChange) / 100);
            element.height = Math.round((imageheight * percentChange) / 100);
        }
        else {
            percentChange = Math.round((deviceWidth * 100) / imageWidth);

            element.width = Math.round((imageWidth * percentChange) / 100);
            element.height = Math.round((imageheight * percentChange) / 100);
        }
    });
}

function reverseString(str) {
    if (str === "")
        return "";
    else
        return reverseString(str.substr(1)) + str.charAt(0);
}