/*-------------------------------------------------------------------------------------------------------------------------------*/
/*This is main JS file that contains custom scripts used in this template*/
/*-------------------------------------------------------------------------------------------------------------------------------*/
/* Template Name: Site Title*/
/* Version: 1.0 Initial Release*/
/* Build Date: 22-04-2015*/
/* Author: Unbranded*/
/* Website: http://
/* Copyright: (C) 2015 */
/*-------------------------------------------------------------------------------------------------------------------------------*/

/*--------------------------------------------------------*/
/* TABLE OF CONTENTS: */
/*--------------------------------------------------------*/
/* 01 - VARIABLES */
/* 02 - page calculations */
/* 03 - function on document ready */
/* 04 - function on page load */
/* 05 - function on page resize */
/* 06 - function on page scroll */
/* 07 - swiper sliders */
/* 08 - buttons, clicks, hovers */
/*-------------------------------------------------------------------------------------------------------------------------------*/


/*================*/
/* 01 - VARIABLES */
/*================*/
var swipers = [], winW, winH, winScr, _isresponsive, intPoint = 500, smPoint = 768, mdPoint = 992, lgPoint = 1200, addPoint = 1600;
var _ismobile = navigator.userAgent.match(/Android/i) || navigator.userAgent.match(/webOS/i) || navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPad/i) || navigator.userAgent.match(/iPod/i);

/*========================*/
/* 02 - page calculations */
/*========================*/
function pageCalculations() {
    winW = $(window).width();
    winH = $(window).height();
    if ($('.menu-button').is(':visible')) _isresponsive = true;
    else _isresponsive = false;

    $('.fixed-header-margin').css({ 'padding-top': $('header').outerHeight(true) });
    $('.parallax-slide').css({ 'height': winH });
}


$(document).ready(function () {
    /*=================================*/
    /* 03 - function on document ready */
    /*=================================*/
    pageCalculations();
    if ($('.search-drop-down .overflow').length && !_ismobile) {
        $('.search-drop-down').addClass('active');
        $('.search-drop-down .overflow').jScrollPane();
        $('.search-drop-down').removeClass('active');
    }
    if (_ismobile) $('body').addClass('mobile');

    /*============================*/
    /* 04 - function on page load */
    /*============================*/

    pageCalculations();
    $('#loader-wrapper').fadeOut();
    $('body').addClass('loaded');
    initSwiper();

    //if (window.innerHeight > window.innerWidth) {

    //    $('.portrate-landscape-setting').each(function (index, element) {

    //        $(element).removeClass("navigation-banner-wrapper").addClass('navigation-banner-wrapper-portrait');


    //    });

    //}
    //else {

    //    $('.portrate-landscape-setting').each(function (index, element) {

    //        $(element).removeClass("navigation-banner-wrapper-portrait").addClass('navigation-banner-wrapper');

    //    });

    //}

});

/*==============================*/
/* 05 - function on page resize */
/*==============================*/
function resizeCall() {
    pageCalculations();

    $('.navigation:not(.disable-animation)').addClass('disable-animation');

    $('.swiper-container.initialized[data-slides-per-view="responsive"]').each(function () {
        var thisSwiper = swipers['swiper-' + $(this).attr('id')], $t = $(this), slidesPerViewVar = updateSlidesPerView($t), centerVar = thisSwiper.params.centeredSlides;
        thisSwiper.params.slidesPerView = slidesPerViewVar;
        thisSwiper.reInit();
        if (!centerVar) {
            var paginationSpan = $t.find('.pagination span');
            var paginationSlice = paginationSpan.hide().slice(0, (paginationSpan.length + 1 - slidesPerViewVar));
            if (paginationSlice.length <= 1 || slidesPerViewVar >= $t.find('.swiper-slide').length) $t.addClass('pagination-hidden');
            else $t.removeClass('pagination-hidden');
            paginationSlice.show();
        }
    });
}
if (!_ismobile) {
    $(window).resize(function () {
        resizeCall();
    });
} else {
    window.addEventListener("orientationchange", function () {
        //orientationChangeSetting();
        resizeCall();
    }, false);
}

function orientationChangeSetting() {
    $('.portrate-landscape-setting').each(function (index, element) {

        $(element).toggleClass('navigation-banner-wrapper-portrait');
        $(element).toggleClass("navigation-banner-wrapper");

    });

}
/*==============================*/
/* 06 - function on page scroll */
/*==============================*/
function scrollCalculations() {
    winScr = $(window).scrollTop();
    var headerComp = ($('header').outerHeight() <= 200) ? $('header').outerHeight() : 200;
    if (winScr >= headerComp && !$('.header-demo').length) {
        //if (!$('header').hasClass('fixed-header')) {
        //    $('header').addClass('fixed-header');
        //    if (!_ismobile) closePopups();
        //}

        if (!$('header').hasClass('position-fixed-header')) {
            $('header').addClass('position-fixed-header');
            if (!_ismobile) closePopups();
        }
    }
    else {
        //if ($('header').hasClass('fixed-header')) {
        //    $('header').removeClass('fixed-header');
        //    if (!_ismobile) closePopups();
        //}

        if ($('header').hasClass('position-fixed-header')) {
            $('header').removeClass('position-fixed-header');
            if (!_ismobile) closePopups();
        }
    }
    $('nav').addClass('disable-animation');
}

scrollCalculations();
$(window).scroll(function () {
    scrollCalculations();
});

/*=====================*/
/* 07 - swiper sliders */
/*=====================*/
var initIterator = 0;
function initSwiper() {

    $('.swiper-container:not(.initialized)').each(function () {
        var $t = $(this);

        var index = 'swiper-unique-id-' + initIterator;

        $t.addClass('swiper-' + index + ' initialized').attr('id', index);
        $t.find('.pagination').addClass('pagination-' + index);

        var autoPlayVar = parseInt($t.attr('data-autoplay'), 10);
        if (_ismobile) autoPlayVar = 0;
        var centerVar = parseInt($t.attr('data-center'), 10);
        var simVar = ($t.closest('.circle-description-slide-box').length) ? false : true;

        var slidesPerViewVar = $t.attr('data-slides-per-view');
        if (slidesPerViewVar == 'responsive') {
            slidesPerViewVar = updateSlidesPerView($t);
        }
        else slidesPerViewVar = parseInt(slidesPerViewVar, 10);

        var loopVar = parseInt($t.attr('data-loop'), 10);
        var speedVar = parseInt($t.attr('data-speed'), 10);

        swipers['swiper-' + index] = new Swiper('.swiper-' + index, {

            speed: speedVar,
            pagination: '.pagination-' + index,
            loop: loopVar,
            paginationClickable: true,
            autoplay: autoPlayVar,
            slidesPerView: slidesPerViewVar,
            keyboardControl: true,
            calculateHeight: false,
            simulateTouch: simVar,
            centeredSlides: centerVar,
            roundLengths: true,
            onSlideChangeEnd: function (swiper) {
                var activeIndex = (loopVar === true) ? swiper.activeIndex : swiper.activeLoopIndex;
                if ($t.closest('.navigation-banner-swiper').length || $t.closest('.parallax-slide').length) {
                    var qVal = $t.find('.swiper-slide-active').attr('data-val');
                    $t.find('.swiper-slide[data-val="' + qVal + '"]').addClass('active');
                }

                easyZoomer(swiper);
            },
            onSlideChangeStart: function (swiper) {
                var activeIndex = (loopVar === true) ? swiper.activeIndex : swiper.activeLoopIndex;
                if ($t.hasClass('product-preview-swiper')) {
                    swipers['swiper-' + $t.parent().find('.product-thumbnails-swiper').attr('id')].swipeTo(activeIndex);
                    $t.parent().find('.product-thumbnails-swiper .swiper-slide.selected').removeClass('selected');
                    $t.parent().find('.product-thumbnails-swiper .swiper-slide').eq(activeIndex).addClass('selected');
                }
                else $t.find('.swiper-slide.active').removeClass('active');
            },
            onSlideClick: function (swiper) {
                if ($t.hasClass('product-preview-swiper')) {
                    $t.find('.default-image').attr('src', $t.find('.swiper-slide-active img').attr('src'));
                    $t.find('.zoomed-image').attr('src', $t.find('.swiper-slide-active img').data('zoom'));
                    $t.find('.product-zoom-container').addClass('visible').animate({ 'opacity': '1' });
                }
                else if ($t.hasClass('product-thumbnails-swiper')) {
                    swipers['swiper-' + $t.parent().parent().find('.product-preview-swiper').attr('id')].swipeTo(swiper.clickedSlideIndex);
                    $t.find('.active').removeClass('active');
                    $(swiper.clickedSlide).addClass('active');
                }
            }
        });
        swipers['swiper-' + index].reInit();
        if (!centerVar) {
            if ($t.attr('data-slides-per-view') == 'responsive') {
                var paginationSpan = $t.find('.pagination span');
                var paginationSlice = paginationSpan.hide().slice(0, (paginationSpan.length + 1 - slidesPerViewVar));
                if (paginationSlice.length <= 1 || slidesPerViewVar >= $t.find('.swiper-slide').length) $t.addClass('pagination-hidden');
                else $t.removeClass('pagination-hidden');
                paginationSlice.show();
            }
        }
        initIterator++;
    });

}

function updateSlidesPerView(swiperContainer) {
    if (winW >= 1920 && swiperContainer.parent().hasClass('full-width-product-slider')) return 6;
    if (winW >= addPoint) return parseInt(swiperContainer.attr('data-add-slides'), 10);
    else if (winW >= lgPoint) return parseInt(swiperContainer.attr('data-lg-slides'), 10);
    else if (winW >= mdPoint) return parseInt(swiperContainer.attr('data-md-slides'), 10);
    else if (winW >= smPoint) return parseInt(swiperContainer.attr('data-sm-slides'), 10);
    else if (winW >= intPoint) return parseInt(swiperContainer.attr('data-int-slides'), 10);
    else return parseInt(swiperContainer.attr('data-xs-slides'), 10);
}

//swiper arrows
$('.swiper-arrow-left').click(function () {
    swipers['swiper-' + $(this).parent().attr('id')].swipePrev();
});

$('.swiper-arrow-right').click(function () {
    swipers['swiper-' + $(this).parent().attr('id')].swipeNext();
});


/*==============================*/
/* 08 - buttons, clicks, hovers */
/*==============================*/

//desktop menu
$('nav>ul>li').on('mouseover', function () {
    if (!_isresponsive) {
        $(this).find('.submenu').stop().fadeIn(300);
    }
});

$('nav>ul>li').on('mouseleave', function () {
    if (!_isresponsive) {
        $(this).find('.submenu').stop().fadeOut(300);
    }
});

//responsive menu
$('nav li .fa').on('click', function () {
    if (_isresponsive) {
        $(this).next('.submenu').slideToggle();
        $(this).parent().toggleClass('opened');
    }
});

$('.submenu-list-title .toggle-list-button').on('click', function () {
    if (_isresponsive) {
        $(this).parent().next('.toggle-list-container').slideToggle();
        $(this).parent().toggleClass('opened');
    }
});

$('.menu-button').on('click', function () {
    $('.navigation.disable-animation').removeClass('disable-animation');
    $('body').addClass('opened-menu');
    $(this).closest('header').addClass('opened');
    $('.opened .close-header-layer').fadeIn(300);
    closePopups();
    return false;
});

$('.close-header-layer, .close-menu').on('click', function () {
    $('.navigation.disable-animation').removeClass('disable-animation');
    $('body').removeClass('opened-menu');
    $('header.opened').removeClass('opened');
    $('.close-header-layer:visible').fadeOut(300);
});

//toggle menu block for "everything" template
$('.toggle-desktop-menu').on('click', function () {
    $('.navigation').toggleClass('active');
    $('nav').removeClass('disable-animation');
    $('.search-drop-down').removeClass('active');
});

/*tabs*/
var tabsFinish = 0;
$('.tab-switcher').on('click', function () {
    if ($(this).hasClass('active') || tabsFinish) return false;
    tabsFinish = 1;
    var thisIndex = $(this).parent().find('.tab-switcher').index(this);
    $(this).parent().find('.active').removeClass('active');
    $(this).addClass('active');

    $(this).closest('.tabs-container').find('.tabs-entry:visible').animate({ 'opacity': '0' }, 300, function () {
        $(this).hide();
        var showTab = $(this).parent().find('.tabs-entry').eq(thisIndex);
        showTab.show().css({ 'opacity': '0' });
        if (showTab.find('.swiper-container').length) {
            swipers['swiper-' + showTab.find('.swiper-container').attr('id')].resizeFix();
            if (!showTab.find('.swiper-active-switch').length) showTab.find('.swiper-pagination-switch:first').addClass('swiper-active-switch');
        }
        showTab.animate({ 'opacity': '1' }, function () { tabsFinish = 0; });
    });

});

$('.swiper-tabs .title, .links-drop-down .title').on('click', function () {
    $(this).toggleClass('active');
    $(this).next().slideToggle(300);
});

/*sidebar menu*/
$('.sidebar-navigation .title').on('click', function () {
    if ($('.sidebar-navigation .title .fa').is(':visible')) {
        $(this).parent().find('.list').slideToggle(300);
        $(this).parent().toggleClass('active');
    }
});

/*search drop down*/
$('.search-drop-down .title').on('click', function () {
    $(this).parent().toggleClass('active');
});

$('.search-drop-down .category-entry').on('click', function () {
    var thisDropDown = $(this).closest('.search-drop-down');
    thisDropDown.removeClass('active');
    thisDropDown.find('.title span').text($(this).text());
});

/*search popup*/
$('.open-search-popup').on('click', function (e) {
    if ($('.search-box.popup').hasClass('active') === false) {
        $(this).find('#faFaSearch').hide();
        $(this).find('#faFaSearchMobile').hide();
        $(this).find('#faFaTimes').remove();
        $(this).append("<i id='faFaTimes' class='fa fa-times'>");

        $('#faFaTimes').append("<img id='searchPopupImage'"
            + "class='embeded-search-popup-image'" + "style='position: absolute;top: 25px;width:34px;height:19px;right:0;content:\"\"'"
            + " src='/Resources/Images/search-angle.png'>");

        clearTimeout(closecartTimeout);
        $('.cart-box.active').animate({ 'opacity': '0' }, 300, function () { $(this).removeClass('active'); });
        $('.search-box.popup').addClass('active').css({ 'right': winW - $(this).offset().left - $(this).outerWidth() * 0.5 - 45, 'top': $(this).offset().top - winScr + 0.5 * $(this).height() + 20, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300, function () {
            $('.search-box.popup input').focus();
        });
    }
    else {
        closeSearchPopups();
    }

    if (e.pageY - winScr > winH - 100) $('.search-box.popup').addClass('bottom-align');
    else $('.search-box.popup').removeClass('bottom-align');
    return false;
});

function closeSearchPopups() {
    $('.search-box.popup.active').animate({ 'opacity': '0' }, 50, function () {
        $(this).removeClass('active');

    });

    $('.open-search-popup').find('#faFaTimes').remove();
    $('#faFaSearch').show();
    $('#faFaSearchMobile').show();

}

/*cart popup*/
$('.open-cart-popup').on('mouseover', function (e) {

    openCartPopups();
});

function openCartPopups() {

    clearTimeout(closecartTimeout);

    if (!$('.cart-box.popup').hasClass('active')) {
        if (_ismobile) {
            self = $('.header-functionality-entry.open-cart-popup');
        }
        else {
            self = $('.open-cart-popup');

        }
        closePopups();

        if (self.length > 0 && $(self).offset().left > winW * 0.5) {

            $('.cart-box.popup').addClass('active cart-right').css({ 'left': 'auto', 'right': winW - $(self).offset().left - $(self).outerWidth() * 0.5 - 45, 'top': $(self).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
        else if (self.length > 0) {

            $('.cart-box.popup').addClass('active cart-left').css({ 'right': 'auto', 'left': $(self).offset().left, 'top': $(self).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
    }
}

$('.open-cart-popup').on('mouseleave', function () {
    closecartTimeout = setTimeout(function () { closeCartPopups(); }, 100);
});

$('.open-cart-popup').on('focusout', function () {
    closecartTimeout = setTimeout(function () { closeCartPopups(); }, 100);
});

var closecartTimeout = 0;
$('.cart-box.popup').on('mouseover', function () {
    clearTimeout(closecartTimeout);
});
$('.cart-box.popup').on('mouseleave', function () {
    closecartTimeout = setTimeout(function () { closeCartPopups(); }, 100);
});

function closeCartPopups() {
    $('.cart-box.popup.active').animate({ 'opacity': '0' }, 50, function () {
        $(this).removeClass('active');
        $('.cart-box').removeClass('cart-left cart-right');
    });
}

function closePopups() {
    $('.cart-box.popup.active').animate({ 'opacity': '0' }, 50, function () {
        $(this).removeClass('active');
        $('.cart-box').removeClass('cart-left cart-right');
    });
}


///////////////////////////////////////// account popup start ////////////////////////////////////////////////

$('.open-account-popup').on('mouseover', function (e) {
    clearTimeout(closeaccountTimeout);

    if (!$('.account-box.popup').hasClass('active')) {
        closeAccountPopups();
        if ($(this).offset().left > winW * 0.5) {
            $('.account-box.popup').addClass('active account-right').css({ 'left': 'auto', 'right': winW - $(this).offset().left - $(this).outerWidth() * 0.5 - 45, 'top': $(this).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
        else {
            $('.account-box.popup').addClass('active account-left').css({ 'right': 'auto', 'left': $(this).offset().left, 'top': $(this).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
    }

});

$('.open-account-popup').on('mouseleave', function () {
    closeaccountTimeout = setTimeout(function () { closeAccountPopups(); }, 100);
});
$('.open-account-popup').on('focusout', function () {
    closeaccountTimeout = setTimeout(function () { closeAccountPopups(); }, 100);
});

var closeaccountTimeout = 0;
$('.account-box.popup').on('mouseover', function () {
    clearTimeout(closeaccountTimeout);
});

$('.account-box.popup').on('mouseleave', function () {
    closeaccountTimeout = setTimeout(function () { closeAccountPopups(); }, 100);
});

function closeAccountPopups() {
    $('.account-box.popup.active').animate({ 'opacity': '0' }, 50, function () {
        $(this).removeClass('active');
        $('.account-box').removeClass('account-left account-right');
    });
}
///////////////////////////////////////// account popup end ////////////////////////////////////////////////

///////////////////////////////////////// language popup start ////////////////////////////////////////////////

$('.open-language-popup').on('mouseover', function (e) {
    clearTimeout(closelanguageTimeout);

    if (!$('.language-box.popup').hasClass('active')) {
        closelanguagePopups();
        if ($(this).offset().left > winW * 0.5) {
            $('.language-box.popup').addClass('active language-right').css({ 'left': 'auto', 'right': winW - $(this).offset().left - $(this).outerWidth() * 0.5 - 45, 'top': $(this).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
        else {
            $('.language-box.popup').addClass('active language-left').css({ 'right': 'auto', 'left': $(this).offset().left, 'top': $(this).offset().top - winScr + 30, 'opacity': '0' }).stop().animate({ 'opacity': '1' }, 300);
        }
    }

});

$('.open-language-popup').on('mouseleave', function () {
    closelanguageTimeout = setTimeout(function () { closelanguagePopups(); }, 100);
});
$('.open-language-popup').on('focusout', function () {
    closelanguageTimeout = setTimeout(function () { closelanguagePopups(); }, 100);
});

var closelanguageTimeout = 0;
$('.language-box.popup').on('mouseover', function () {
    clearTimeout(closelanguageTimeout);
});

$('.language-box.popup').on('mouseleave', function () {
    closelanguageTimeout = setTimeout(function () { closelanguagePopups(); }, 100);
});

function closelanguagePopups() {
    $('.language-box.popup.active').animate({ 'opacity': '0' }, 50, function () {
        $(this).removeClass('active');
        $('.language-box').removeClass('language-left language-right');
    });
}
///////////////////////////////////////// language popup end ////////////////////////////////////////////////

/*main menu mouseover calculations*/
// $('nav>ul>li').on('mouseover', function(){
// 	var subFoo = $(this).find('.submenu');
// 	if(subFoo.length) closePopups();
// 	if(subFoo.length){
// 		subFoo.removeClass('left-align right-align');
// 		if(subFoo.offset().left<0) subFoo.addClass('left-align');
// 		else if(subFoo.offset().left+subFoo.outerWidth()>winW) subFoo.addClass('right-align');
// 	}
// });

/*departments dropdown (template "fullwidthheader")*/
$('.departmets-drop-down .title').on('click', function () {
    $(this).parent().find('.list').slideToggle(300);
    $(this).toggleClass('active');
});

$('.departmets-drop-down').on('mouseleave', function () {
    $(this).find('.list').slideUp(300);
    $(this).find('.title').removeClass('active');
});

/*simple arrows slider*/
var finishBannerSlider = 0;
function leftClick(obj_clone, arrow) {
    var obj = arrow.parent().parent().find(obj_clone);
    if (finishBannerSlider) return false;
    finishBannerSlider = 1;
    obj.last().clone(true).insertBefore(obj.first());
    obj.last().remove();
    var item_width = obj.outerWidth(true);
    obj.parent().css('left', '-' + item_width + 'px');
    obj.parent().animate({ 'left': '0px' }, 300, function () { finishBannerSlider = 0; });
    return false;
}

function rightClick(obj_clone, arrow) {
    var obj = arrow.parent().parent().find(obj_clone);
    if (finishBannerSlider) return false;
    finishBannerSlider = 1;
    obj.first().clone(true).insertAfter(obj.last());
    var item_width = obj.outerWidth(true);
    obj.parent().animate({ 'left': '-' + item_width + 'px' }, 300, function () {
        obj.first().remove();
        obj.parent().css('left', '0px');
        finishBannerSlider = 0;
    });
    return false;
}

$('.menu-slider-arrows .left').on('click', function () {
    leftClick('.menu-slider-entry', $(this));
});

$('.menu-slider-arrows .right').on('click', function () {
    rightClick('.menu-slider-entry', $(this));
});

//product page - zooming image
var imageObject = {};
$('.product-zoom-container').on('mouseover', function (e) {
    var $t = $(this);
    imageObject.thisW = $t.width();
    imageObject.thisH = $t.height();
    imageObject.zoomW = $t.find('.zoom-area').outerWidth();
    imageObject.zoomH = $t.find('.zoom-area').outerHeight();
    imageObject.thisOf = $t.offset();
    zoomMousemove($(this), e);
});

function zoomMousemove(foo, e) {
    var $t = foo,
        x = e.pageX - imageObject.thisOf.left,
        y = e.pageY - imageObject.thisOf.top,
        zoomX = x - imageObject.zoomW * 0.5,
        zoomY = y - imageObject.zoomH * 0.5;
    if (zoomX < 0) zoomX = 0;
    else if (zoomX + imageObject.zoomW > imageObject.thisW) zoomX = imageObject.thisW - imageObject.zoomW;
    if (zoomY < 0) zoomY = 0;
    else if (zoomY + imageObject.zoomH > imageObject.thisH) zoomY = imageObject.thisH - imageObject.zoomH;
    $t.find('.move-box').css({ 'left': x * (-2), 'top': y * (-2) });
    $t.find('.zoom-area').css({ 'left': zoomX, 'top': zoomY });
}

$('.product-zoom-container').on('mousemove', function (e) {
    zoomMousemove($(this), e);
});

$('.product-zoom-container').on('click', function () {
    $(this).animate({ 'opacity': '0' }, function () { $(this).removeClass('visible'); });
});

$('.product-zoom-container').on('mouseleave', function () {
    $(this).click();
});

$('.color-selector .entry').on('click', function () {
    $(this).parent().find('.active').removeClass('active');
    $(this).addClass('active');
});

$
function increaseQuantity(self) {
    var divUpd = $(self).parent().find('.number'), newVal = parseInt(divUpd.val(), 10) + 1;
    divUpd.val(newVal);
    $(divUpd).trigger('change');
}

function decreaseQuantity(self) {
    var divUpd = $(self).parent().find('.number'), newVal = parseInt(divUpd.val(), 10) - 1;
    if (newVal >= 1) {
        divUpd.val(newVal);
        $(divUpd).trigger('change');
    }
}
var dict = []; // create an empty array


function quantityInputChange(self) {

    if ($(self).val() == 0) {
        $(self).val(1);
        alert("Zero is not allowed minimum value is 1!");
    }


    var updateId = $(self).parent().find('.fa-refresh').attr('id');

    if (dict[updateId] == undefined) {

        var blinkTimer = setInterval(function () {
            $(self).parent().find('.fa-refresh').fadeTo('slow', 0.3).fadeTo('slow', 1.0);

        }, 1500);

        dict[updateId] = blinkTimer;
    }


}

//accordeon
$('.accordeon-title').on('click', function () {
    $(this).toggleClass('active');
    $(this).next().slideToggle();
});

//open image popup
$('.open-image').on('click', function () {
    showPopup($('#image-popup'));
    return false;
});

//open product popup
$('.open-product').on('click', function (e) {
    e.preventDefault();
    showPopup($('#product-popup'));
    initSwiper();

    return false;
});


function findSwiper(swiperId) {
    var i = 0;
    var doIt = true;
    while (doIt) {

        var tempId = 'swiper-swiper-unique-id-' + i;
        var tempSwiper = swipers[tempId];

        if (tempSwiper.wrapper.id === swiperId) {

            return tempSwiper;

        }

        i++;
    }

}

function easyZoomer(swipper) {

    if (swipper.wrapper.id === 'productPreviewSwiperWrapper') {
        //
        //$('#jqZoom_' + swipper.activeIndex).zoom();

    }

}

//open subscribe popup
$('.open-subscribe').on('click', function () {
    showPopup($('#subscribe-popup'));
    $('#subscribe-popup .styled-form .field-wrapper input').focus();
    return false;
});

$('.close-popup, .overlay-popup .close-layer').on('click', function () {
    $('.overlay-popup.visible').removeClass('active');
    setTimeout(function () { $('.overlay-popup.visible').removeClass('visible'); }, 500);
});

function showPopup(id) {
    id.addClass('visible active');
}

//shop - sort arrow
$('.sort-button').click(function () {
    $(this).toggleClass('active');
});

//shop - view button
$('.view-button.grid').click(function () {
    if ($(this).hasClass('active')) return false;
    $('.shop-grid').fadeOut(function () {
        $('.shop-grid').removeClass('list-view').addClass('grid-view');
        $(this).fadeIn();
    });
    $(this).parent().find('.active').removeClass('active');
    $(this).addClass('active');
});

$('.view-button.list').click(function () {
    if ($(this).hasClass('active')) return false;
    $('.shop-grid').fadeOut(function () {
        $('.shop-grid').removeClass('grid-view').addClass('list-view');
        $(this).fadeIn();
    });
    $(this).parent().find('.active').removeClass('active');
    $(this).addClass('active');
});

//close message
$('.message-close').on('click', function () {
    $(this).parent().hide();
});

//portfolio
$('.portfolio-entry').on('mouseover', function () {
    $(this).addClass('active');
});

$('.portfolio-entry').on('mouseleave', function () {
    $(this).removeClass('active');
});

//simple search form focus
$('.simple-search-form input').on('focus', function () {
    $(this).closest('.simple-search-form').addClass('active');
});

$('.simple-search-form input').on('blur', function () {
    $(this).closest('.simple-search-form').removeClass('active');
});

function createShopCartItems(data) {
    var cartItem = '';
    debugger
    var languageDictionary = JSON.parse(data.JsonLanguageDictionary);

    data.OrderList.forEach(function (item, index) {

        var html =

            "<div class='cart-entry' id='product_" + item.ProductFeatureId + "'>" +
            "<a class='image'><img src='" + item.ImageUrl + "' alt='' /></a>" +
            "<div class='content'>" +
            "<a class='title' href='#'>" + item.ProductName + "</a>" +

            "<div class='d-flex quantity-selector detail-info-entry'>" +
            "   <div class='entry number-minus' onclick='decreaseQuantity(this)' style='height:20px;width:20px'>&nbsp;</div>" +
            "   <input type='number' id='quantityInput'  onchange='quantityInputChange(this)' class='persian-font entry number no-arrows' value='" + item.Quantity + "' style='height: 20px;' />" +
            "   <div class='entry number-plus' onclick='increaseQuantity(this)' style='height:20px;width:20px'>&nbsp;</div>" +
            "   <div class='quantity-referesh' style='height: 20px; width: 20px'><i id='updateQuantity_" + item.ProductFeatureId.replaceAll('-', '_') + "' class='fa fa-refresh' title='Update' " + "onclick=\"updateQuantity(this, '" + item.Id + "', " + item.CategoryId + " )\"></i></div>" +
            "</div>" +
            "<div class='d-flex'>" +
            "    <div>" +
            "       <span> <small> " + languageDictionary['Dollar'] + " </small> </span>" +
            "    </div>" +
            "    <div> <span class='persian-font pl-2' id='priceId'> " + item.Price + " </span> </div>" +
            "</div>" +
            "</div>" +
            "<div class='button-x' data-id='" + item.Id + "'><i class='fa fa-trash'></i></div>" +
            "</div>";

        cartItem += html;

    });
    localStorage.setItem("ShopCartItems", cartItem);
    return cartItem;
}

function updateQuantity(self, id, categoryId) {
    var quantity = $(self).parent().parent().find('.number').val();
    if (quantity <= 0) {
        quantity = 1
        $(self).parent().parent().find('.number').val(1);

        alert("Zero is not allowed minimum value is 1!");
    }

    manageShopCartItem('update', id, categoryId, quantity, '', '');

    var updateId = $(self).parent().parent().find('.fa-refresh').attr('id');


    clearInterval(dict[updateId]);
    delete dict[updateId];
}

function manageShopCartItem(action, id, categortyId, quantity, caller, callerUrl) {

    var cartId = localStorage.getItem("CartId");

    var output = $("#orderDiv");
    var requestOrder = '';

    if (action == "add") {
        var path = '';
        $('.feature-detail-selector').each(function (index, element) {

            var detailId = $(element).find('.active-feature-type-detail-leaf').data('feature-type-detail-id');
            if (detailId == undefined) {
                detailId = $(element).find('.entry.active').data('feature-type-detail-id');
            }
            path += detailId + '/';
        });

        if (path.length > 0) {

            path = path.substring(0, path.length - 1);
        }

        path = path.split("/").reverse().join("/");

        var orderItem = {
            id: '',
            tempCartId: cartId,
            productFeatureGuid: '',
            price: $("#priceDiv").text(),
            quantity: $("#quantityInput").val(),
            userId: '',
            path: path,
            categoryId: categortyId
        }
        requestOrder = $.ajax(
            {
                type: 'post',
                dataType: 'json',
                url: '/order/ato',
                data: orderItem
            }
        );
    }
    else if (action == "delete") {
        requestOrder = $.ajax(
            {
                type: 'post',
                dataType: 'json',
                url: '/order/dfo',
                data: {
                    tempCartId: cartId,
                    cartItemId: id,
                }
            }
        );
    }
    else if (action == "update") {

        requestOrder = $.ajax(
            {
                type: 'post',
                dataType: 'json',
                url: '/order/uo',
                data: {
                    quantity: quantity,
                    tempCartId: cartId,
                    cartItemId: id,
                }
            }
        );
    }
    else if (cartId != '' && cartId != null && cartId != undefined) {
        requestOrder = $.ajax(
            {
                type: 'post',
                dataType: 'json',
                url: '/order/gci',
                data: { tempCartId: cartId }
            }
        );
    }

    if (requestOrder != '') {
        requestOrder.done(function (data) {
            if (cartId == '' || cartId == null || cartId == undefined) {
                localStorage.setItem("CartId", data.TempCartId);
                cartId = data.TempCartId;
            }

            localStorage.setItem("ShopCartItems", '');

            output.html("");
            $('#orderCartButtonsDiv').html("");

            var html = createShopCartItems(data)

            output.append(html);

            var languageDictionary = JSON.parse(data.JsonLanguageDictionary);

            $('#orderCartButtonsDiv').html("<div class='summary'>" +
                "<div class='grandtotal mt-3'>" + languageDictionary["Grand Total"] + ": <span class='persian-font' id='grandTotalId'></span></div>" +
                "</div>" +
                "<div class='cart-buttons'>" +
                "<div class=''>" +
                "   <a class='button style-4' href='/Order/lsc?tempCartId=" + cartId + "'>" + languageDictionary["Checkout"] + "</a>" +
                "<div class='clear'></div>" +
                "</div>");


            if (data.TotalQuantity == 0)
                data.TotalQuantity = '';

            $(".shopping-cart-count").text(data.TotalQuantity);
            $(".reponsive-shopping-cart-count").text(data.TotalQuantity);

            $("#grandTotalId").text(data.TotalPrice + " " + languageDictionary["Dollar"]);

            var shoptCartGrandTotal = $("#shoptCartGrandTotal");

            if (shoptCartGrandTotal.length > 0) {
                $(shoptCartGrandTotal).text(data.TotalPrice + " " + languageDictionary["Dollar"] );
            }

            if (action == 'add' || action == 'delete') {
                openCartPopups($('.open-cart-popup'));
                closecartTimeout = setTimeout(function () { closeCartPopups(); }, 3000);

            }


            if (action == 'delete' && data.OrderList.length == 0) {

                localStorage.setItem("CartId", '');
                localStorage.setItem("ShopCartItems", '');
            }
            if (action == 'delete' && caller == 'refreshOrderItem') {

                var refreshRequest = $.ajax({
                    type: 'post',
                    url: callerUrl,
                    datatype: 'html',
                });

                refreshRequest.done(function (data) {
                    $('#shopCartContainer').html(data);
                });
            }


        });

        requestOrder.fail(function (jqXHR, textStatus, errorThrown) {

        });
    }
}

$(document).on('click', '.button-x', function () {
    let id = $(this).data('id');
    manageShopCartItem('delete', id, '', 0, '', '');
});