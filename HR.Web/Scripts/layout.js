﻿$(function () {
    $("input ,select, textarea,hidden, a").not($(":button,a")).keypress(function (evt) {
        if (evt.keyCode == 13) {
            evt.preventDefault();
            return false;
        }
    });

    $("#txtSearch").keypress(function (evt) {
        if (evt.keyCode == 13) {
            $("#btnSearch").click();
            window.location = $('#btnSearch').attr('href');
        }
    });

    /* JS Utility Functions */
    //$(document).on('keypress', '.numOnlyCss', function (evt) {
    //    var text = $('#txtChequeNo').val();
    //    evt = (evt) ? evt : window.event;
    //    var charCode = (evt.which) ? evt.which : evt.keyCode;
    //    if (text.length >= 10) {
    //        if (charCode == 8 || charCode == 9)
    //            return true;
    //        return false;
    //    }

    //    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
    //        return false;
    //    }
    //    return true;
    //});

    $(document).on('keypress', '.dtCss', function (evt) {        
        return false;
    });

    $(document).on('keydown', '.dtCss', function (evt) {
       return false;
    });

    $(document).on('keypress', '.alphaNumCss', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = parseInt((evt.which) ? evt.which : evt.keyCode);
        if ((charCode >= 48 && charCode <= 57) || (charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122))
            return true;
        else
            return false;
    });

    //$(document).on('keypress', '.decimalCss', function (evt) {
        
        $(".decimalCss").on("input", function (evt) {
            var self = $(this);
            self.val(self.val().replace(/[^0-9\.]/g, ''));
            if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
                evt.preventDefault();
            }
        });
        //
        //alert(evt.keyCode);
        //alert(evt.charCode);

        //var charCode = (evt.which) ? evt.which : (evt.charCode);
        //if (charCode == 8 || charCode == 0) {

        //}
        //else if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        //    alert(charCode);
        //    return false;
        //}

        //
        //var character = String.fromCharCode(evt.charCode)
        //var newValue = this.value + character;                
        //if (isNaN(newValue) || parseFloat(newValue) * 10000 % 1 > 0) {
        //    evt.preventDefault();
        //    return false;
        //}

        //return true;
        /*
       var charCode = (evt.which) ? evt.which : event.keyCode;
       if (charCode != 46 && charCode > 31
         && (charCode < 48 || charCode > 57))
           return false;

       return true;*/
    //    debugger;
        //if (navigator.appName == "Netscape")
        //    Key = evt.charCode; //or e.which; (standard method)
        //else
        //    Key = evt.keyCode;
        //var character = String.fromCharCode(Key);
        ////console.log(Key);

        //if (Key == 0)
        //    return true;

        //var newValue = this.value + character;
        //if (isNaN(newValue) || parseFloat(newValue) * 10000 % 1 > 0) {
        //    evt.preventDefault();
        //    return false;
        //}

    //});

    $(document).on('keypress', '.numCss', function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if ((charCode >= 48 && charCode <= 57) || charCode == 8)
            return true;
        else
            return false;
    });
    $(document).on('keypress', '.numCssPlus', function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if ((charCode >= 48 && charCode <= 57) || charCode == 8 || charCode == 43)
            return true;
        else
            return false;
    });
    $('.validatename').keypress(function (e) {
        if (window.event)
                    code = e.keyCode;
                else
                    code = e.which;
                if (code == 32 || (code >= 97 && code <= 122) || (code >= 65 && code <= 90))
                    return true;
                else
                    return false;
    });
    $('.validatenumber').keypress(function (e) {

        if (window.event)
            code = e.keyCode;
        else
            code = e.which;
        if ((code >= 48 && code <= 57)||code==43)
            return true;
        else
            return false;

    });
    $('.number').keypress(function (e) {
        if (window.event)
            code = e.keyCode;
        else
            code = e.which;
        if (code >= 48 && code <= 57 )
            return true;
        else
            return false;
    })

   
    $.ajaxSetup({
        error: function (x, e) {
            if (x.status == 401) {
                location.href = ErrorUrl;
            }
        }
    });

    $('input').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue',
        increaseArea: '20%' // optional
    });

    $('.modal .modal-dialog').attr('class', 'modal-dialog  bounceIn  animated');

    $('.ajaxCss').click(function () {
        LoadingAnimation();
    });

    $(".dtCss input.form-control").keypress(function (evt) {        
        evt.preventDefault();
        return false;
    });
    //$('.form-control').addClass('input-sm');
    //$('.control-label').css('margin-bottom','10px');
    //function testAnim(x) {
    //    $('.modal .modal-dialog').attr('class', 'modal-dialog  ' + x + '  animated');
    //};
    //$('.modal-dialog').on('show.bs.modal', function (e) {
        
    //    testAnim('bounceIn');
    //})
    //$('.modal .modal-dialog').on('hide.bs.modal', function (e) {
    //    //var anim = $('#exit').val();
    //    testAnim('bounceOut');
    //})
    /* JS Utility Functions */
});

function LoadingAnimation() {
    $.loader({
        className: "blue-with-image-2",
        content: '',
    });
}

function StopLoadingAnimation() {
    $('#jquery-loader-background').remove();
    $('#jquery-loader').remove();
}

function createCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function onNavBarClick() {
    var cookieVal = readCookie('NavBarCookie');
    if (cookieVal != null) {
        var isOpen = cookieVal == 'open' ? true : false;
        if (isOpen) {
            createCookie('NavBarCookie', 'close', 300);
        } else {
            createCookie('NavBarCookie', 'open', 300);
        }
    } else {
        createCookie('NavBarCookie', 'close', 300);
    }    
}
//$('.maxlength').keypress(function (e) {
//    debugger;
//    var maxLength = $('.maxlength').attr('maxlength');
    
//    var Value = e.target.value;
//    var length = Value.length;
//    if (length >= 30) {
//        e.preventDefault();
//        return false;
//    }
//    else {
//        return true;
//    }
//});
