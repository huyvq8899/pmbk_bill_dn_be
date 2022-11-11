MAX_WIDTH_LOGO = 250;
/*
* enum vị trí logo
*/
POSITION_LOGO = {
    Left: 1,
    Right: 2,
}

/*
* enum vị trí hình nền
*/
POSITION_BACKGROUND = {
    FULL: 1,
    CENTER: 2,
}

TEMPLATE_LANGUAGE_VN = "LANGUAGE-VN";
TEMPLATE_LANGUAGE_EN = "LANGUAGE-EN";

MAX_WIDTH_BG = 865;
MAX_HEIGHT_BG = 1224.0840336134454;
MAX_WIDTH_QR = 160;
MAX_HEIGHT_QR = 160;
MIN_WIDTH_QR = 75;
MIN_HEIGHT_QR = 75;
MERGE_FIELD = "MergeFied_";
CUSTOM_SELLER = "CustomSeller_";
CUSTOM_FIELD_BUYER = "CustomBuyer_";
CUSTOM_BUYER = "CustomField";
CUSTOM_TABLE = "CustomTableDetail";
CUSTOM_INFOR = "CustomInvoiceInfor_";
CUSTOM_SIGN = "CustomSign";
var _regexTableDetail = /^CustomField([1-9]{1}\d?)Detail$/;
$(document).ready(function () {
    init();
});

/**
 * khởi tạo ban đầu
 * */
function init() {
    if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
        $("html").addClass("scroll");
    }
    $(".discountAmount").addClass("display-none");
    $(".converterRegion").addClass("display-none");
    $(".cls-convert").addClass("display-none");
    //$(".curency-block").addClass("display-none");
    $(".description-invoice-client").removeClass("display-none");
    $(".description-invoice").addClass("display-none");
    $('.template-table').addClass("display-none");
    if (!configCallback.isBLPHaveFee) {
        resizeElementInfor();
    }
    initResizeableTables();
    if (!configCallback.isTVT() && !configCallback.isBLPHasFeeK80()) {
        initResizeDragForESign();
    }
    else if (configCallback.isTVT()) {
        initResizeDragForSellerSignTicket();
        initResizeDragForTaxCodeSignTicket();
    }
    removeResizeDragable();
    configCallback.setupControlByTemplate();
    $(document).click(function () {
        configCallback.showHideModifyInfomationInvoice();
    })
    initByEditMode();
    customFieldClick();

    if (configCallback.isTVT()) {
        configCallback.chooseMethodShowNumber(checkMethodShowNumber());

    }

    //Sự kiện click vào mẫu
    //vmthanh - 30/07/2020
    $(document).mouseup(function (e) {
        highlightElementByStep(configCallback.getCurrentStep());
    });

    setTypeTemplateByMode(configCallback.modeTemplate, configCallback.templateDiscount, configCallback.templateCurrency);
    setTypeCurrency(false);
    setupToResizeColspan();
};

function setupToResizeColspan() {
    if ($("#tbDetail .tr-col-span").length > 0) {
        var fieldParentEle = $("#tbDetail .tr-col-span [field-parent]");
        for (var i = 0; i < fieldParentEle.length; i++) {
            var elementParent = $(fieldParentEle[i]);
            var fieldParent = elementParent.attr("field-parent");
            var dataField = elementParent.attr("data-field");
            var divFake = "<div fake-parent=" + fieldParent + " class='display-flex fake-parent'></div>";

            if ($("[fake-parent=" + fieldParent + "]").length == 0) {
                $("#tbDetail").before(divFake);
            }

            var divFakeElement = "<div class='fake-child' fake-child=" + dataField + "></div>";
            if ($("[fake-child=" + dataField + "]").length == 0) {
                $("[fake-parent=" + fieldParent + "]").append(divFakeElement);
            }
            $("[fake-child=" + dataField + "]").css("width", $("#tbDetail td[data-field=" + dataField + "]").width() + "px");
            $("[fake-child=" + dataField + "]").css("height", $("#tbDetail td[data-field=" + dataField + "]").height() + "px");
        }
    }
}

function resetSizePositionFakeColspan() {
    var fakeParent = $("[fake-parent]");
    for (var i = 0; i < fakeParent.length; i++) {
        var fakeParentElement = $(fakeParent[i]);
        var fieldParent = fakeParentElement.attr("fake-parent");
        fakeParentElement.css("width", $("#tbDetail .tr-header [data-field=" + fieldParent + "]").width() + "px");
        fakeParentElement.css("top", $("#tbDetail .tr-header [field-parent=" + fieldParent + "]").first().offset().top + "px");
        fakeParentElement.css("left", $("#tbDetail .tr-header [field-parent=" + fieldParent + "]").first().offset().left + "px");
    }
}

function resizeableColspanTable() {
    var fakeParent = $("[fake-parent]");
    for (var i = 0; i < fakeParent.length; i++) {
        var fakeParentElement = $(fakeParent[i]);
        var childFake = fakeParentElement.find("[fake-child]");
        fakeParentElement.find("[fake-child]").last().css("flex", "1");
        for (var j = 0; j < childFake.length - 1; j++) {
            $(childFake[j]).resizable({
                handles: 'e',
                resize: function (event, ui) {
                    var $element = $(event.target);
                    var $parent = $element.parents("[fake-parent]");
                    var fieldParent = $parent.attr("fake-parent");
                    var tdParent = $("#tbDetail .tr-header td[field-parent=" + fieldParent + "]");
                    for (var i = 0; i < tdParent.length; i++) {
                        var $item = $(tdParent[i]);
                        $item.css("width", $("[fake-child=" + $item.attr("data-field") + "]").width() + "px");
                    }
                }
            });
        }
    }
}

/**
 * Khởi tạo resize table
 * ndanh1 29/7/2020
 * */
function initResizeableTables() {
    resizeTable("#tbDetail");
    resizeTable("#tbDetail", true);

    resizeTable("#signXml");
    resizeTable("#signXml", true);

    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    resizeTableFirstInvoice();
    resizeTable("#firstInvoice", true);

    resizeTable("#invoiceInfor");
    resizeTable("#invoiceInfor", true);
    resizeTableFooter();
    resizeTable("#tbFooter", true);
}

/**
 * khởi tạo resize table footer
 * ndanh1 30/7/2020
 * */
function resizeTableFooter() {
    $("#tbFooter").colResizable({
        liveDrag: true,
        gripInnerHtml: "",
        draggingClass: "dragging",
        resizeMode: 'fit',
        onDrag: reloadNewWidthTable($("#tbFooter td:visible")),
    }, $("#tbFooter tr:visible:first td"));


}

/**
 * khởi tạo các tham số theo edit mode
 * ndanh1 29/7/2020
 * */
function initByEditMode() {
    if (configCallback.editMode == 1) {
        if (!configCallback.isBLP() && !configCallback.isSignTemplate) {
            $(".header-content-invoice").css("width", "100%")
            $(".logo-template-content").parents("td").css("width", "0px");
        }
    }
    if (configCallback.editMode == 2 || configCallback.editMode == 5 || configCallback.addFromOldTemplate) {
        addNewRedutionTax43();
        configCallback.setupBackgroundDefault();
        if ($(".LANGUAGE-TEMPLATE").text() == TEMPLATE_LANGUAGE_VN) {
            configCallback.isVN = true;
        } else {
            configCallback.isVN = false;
        }
        configCallback.currentTemplateName = configCallback.data.ReportName;
        configCallback.currentInvSeri = configCallback.data.InvSeries;
        if (configCallback.dataAdvanced) {
            configCallback.totalAmountInWordEN = configCallback.dataAdvanced.IsAmountInWordEN;
        }
        configCallback.setupDataAfterLoadToEdit();
        if (configCallback.editMode == 2 && !configCallback.isDuplicateTemplate) {
            if ($("[data-field=TotalAmountWithVAT8]").length > 0) {
                configCallback.useTemplateMR = true;
                if ($("[data-field=TotalAmountWithVAT8]").css("display") != "none") {
                    configCallback.currentDisplay8 = true;
                }
            }

            if (configCallback.useTemplateMR) {
                if ($("[data-field=TotalAmountWithVAT0] td:first-child .edit-label-en").length == 0) {
                    addBonusLabelENMR();
                }
            }
        }

        if (configCallback.typeOfTemplate == "1" || configCallback.typeOfTemplate == "4") {
            if (configCallback.data.InvoiceType == 6) {
                configCallback.setInvoiceKPTQ();
            }
        }

        if (configCallback.isTVT()) {
            var $invSeri = $($("[data-field=InvoiceSeries]")[0]);
            if (configCallback.isVN) {
                $invSeri.parent().attr("typeShow", "row");
            }
            else {
                $invSeri.parent().attr("typeShow", "column");
            }
        }
    }
    var dMoney = $(".TicketPrice").text();
    var vat = $(".VatRateTicket").text();
    var serviceName = $("[data-field=ServiceName] div").text();
    var unitName = $(".unit-name-ticket").first().text().trim();
    var unitShow = $(".unit-name-ticket").length > 0 && !$(".unit-name-ticket").hasClass("display-none");
    var objReduceTax43 = {
        AmountWithoutReduceVAT: $(".AmountWithoutReduceVAT").text(),
        TaxReduction43Amount: $(".TaxReduction43Amount").text(),
        TaxRate: $(".TaxRate").text(),
        IsTaxReduction43: $(".IsTaxReduction43").text(),
    }
    var unit = {
        value: unitName,
        isShow: unitShow,
    }
    configCallback.initForTVTAfterLoadTemplate(dMoney, vat, unit, serviceName);
    configCallback.initForReceiptAfterLoadTemplate();
    if (configCallback.isTVT()) {
        configCallback.initForReduceTax43(objReduceTax43);
        if (configCallback.isTVTTransport()) {
            enableHiddenSomeField();
        }
        addLabelENForTicket();
        addQRCodeForTicket();
        addStyleForConverter();
        appendBlockTaxSign();
        changeLabelMisaInfo();
        setCurrentStyleOfTicketPrice();
        removeInteralStyleTicket();
    }
    configCallback.initFeeInfor();
    configCallback.isSignTemplate = false;

    if (configCallback.isBLP()) {
        $("[data-field=InvoiceCode]").remove();
    }

    if (configCallback.isBLPHasFeeK80()) {
        addQRCodeForTicket();
    }

    if (configCallback.isBLPHaveFee() && configCallback.editMode != 1) {
        var totalAmount = $(".TotalAmountTemplate").html();
        totalAmount = configCallback.formatNumberByCurrency(totalAmount);
        $("[data-field=TotalAmountWithVAT] .edit-value").html(totalAmount);
    }

    if (!configCallback.isBLP() && !configCallback.isTVT()) {
        appendBlockTaxSignForInvoice();
        if (configCallback.editMode == 1) {
            handleChangeTypeInvoiceForTaxSign(configCallback.isTemplateWithCode);
        }
    }

    $("[data-field=SellerAddress]").addClass("disable-hiden");

    if (configCallback.isDeliveryND123()) {
        changeStyleForSellerSignOfDelivery123();
    }
    //check mẫu HĐ NĐ51 xem có thông tin mới lúc sưar chưa?nếu chưa thì insert 
    if (configCallback.isDeliveryND51()) {
        handleAppendHtmlOutward();
    }
}

/**
 * get thông tin số tiền
 * */
function getInforMoney(isBeforeTax) {
    var dMoney = $(".TicketPrice").text();
    if (isBeforeTax) {
        dMoney = $(".TicketPriceBeforeVAT").text();
    }

    var vat = $(".VatRateTicket").text();

    return item = {
        total: dMoney,
        vat: vat,
    };
}

/**
 * Thực hiện disable sự kiện resizeable của table
 * ndanh1 29/7/2020
 * */
function disableResizeableTables() {
    resizeTable("#tbDetail", true);
    resizeTable("#signXml", true);
    resizeTable("#firstInvoice", true);
    resizeTable("#invoiceInfor", true);
    resizeTable("#tbFooter", true);

    removeResizeDragable();
}

/**
 * Sự kiện click vào các khối
 * 29/6/2020
 * */
function reloadAllWidthTable() {
    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    reloadNewWidthTable($("#tbDetail .tr-header td:visible"));
    reloadNewWidthTable($("#invoiceInfor .tr-invoice-infor .item-invoice-infor:visible"));
    reloadNewWidthTable($("#tbFooter td:visible"));
}

/**
 * Sự kiện click vào các khối
 * 29/6/2020
 * */
function customFieldClick() {
    $('[group-field]').off("click").on('click', function (event) {
        event.preventDefault();
        if (configCallback.tabActive == "config-detail") {
            var oldCustomField = configCallback.customField;
            var customField = $(this).attr('group-field');

            var firstInvoice = $(this).parents("#firstInvoice").length > 0;
            var invoiceInfor = $(this).parents("#invoiceInfor").length > 0;

            reloadAllWidthTable();
            $('.bg-picked-by-user').removeClass("bg-picked-by-user");
            if (configCallback.isTVT()) {
                $(this).addClass("bg-picked-by-user");
            }
            else if (configCallback.isBLPHasFeeK80()) {
                if (firstInvoice) {
                    $(this).parents(".highlight-block").last().addClass("bg-picked-by-user");
                }
                else {
                    $(this).addClass("bg-picked-by-user");
                }
            }
            else {
                if (!$(this).hasClass("highlight-block") || invoiceInfor || firstInvoice) {
                    $(this).parents(".highlight-block").last().addClass("bg-picked-by-user");
                } else {
                    $(this).addClass("bg-picked-by-user");
                }
            }

            if (configCallback.isTVT() && customField == "sign-xml") {
                disableHiddenSellerSign(configCallback.isTemplateWithCode);
            }

            configCallback.htmlBlock(customField);
            if (oldCustomField != customField) {
                disableResizeableTables();
                var lstMege = $(this).find(".merge-row-infor");
                removeResizeDragable();

                if (!(configCallback.isTVT() && customField == "search-block")) {
                    for (var i = 0; i < lstMege.length; i++) {
                        resizeElementInfor($(lstMege[i]).attr("merge-field"));
                    }
                }
                if (customField == "table-detail") {
                    resizeTable("#tbDetail", false, true);
                    resetSizePositionFakeColspan();
                    resizeableColspanTable();
                } else if (customField == "sign-xml") {
                    if (!configCallback.isTVT() && !configCallback.isBLPHasFeeK80()) {
                        initResizeDragForESign();
                        if (checkShowDisplayTaxCodeSign()) {
                            initResizeDragForTaxESign();
                        }
                        resizeTable("#signXml", false, true);
                    }
                    else {
                        if ($("[data-field=QRCodeField]").css("display") != "none") {
                            resizableQRCodeDown();
                        }
                        if (configCallback.isTVT()) {
                            initResizeDragForSellerSignTicket();
                            initResizeDragForTaxCodeSignTicket();
                        }
                    }
                } else if (customField == "buyer-infor") {
                    initResizeableForBuyerInfor();
                } else if (customField == "table-footer") {
                    resizeTableFooter();
                } else if (customField == "seller-infor") {
                    if ($(".seller-infor").parents("#invoiceInfor")) {
                        resizeTable("#invoiceInfor", false, true);
                    }
                }
                else if (customField == "invoice-infor") {
                    if ($("[data-field=DescriptionInvoiceClient]").css("display") != "none") {
                        configCallback.setCheckDescription(true);
                    }
                    else {
                        configCallback.setCheckDescription(false);
                    }
                }
                else if (customField == "other-invoice") {
                    if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
                        if ($("[data-field=QRCodeFieldTop]").css("display") != "none") {
                            resizableQRCodeTop();
                        }
                    }
                }

                if (firstInvoice) {
                    resizeTable("#firstInvoice", false, true);
                    if (checkPositionQRTicket() == 0) {
                        $("#firstInvoice tr td.qrcodetop").css("width", "60px");
                        var $logo = $(".logo-template-content");
                        var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
                        if (hasLogo) {
                            $("#firstInvoice tr td.header-content-invoice").css("width", "160px");
                        }
                        else {
                            $("#firstInvoice tr td.header-content-invoice").css("width", "210px");
                        }

                    }
                }

                if (invoiceInfor) {
                    resizeTable("#invoiceInfor", false, true);
                }
            }

            if ($(this).parents("#firstInvoice").length > 0) {
                resizeTableFirstInvoice();
            }
        }
    })
}

function removeResizeDragable() {
    $(".seller-infor .ui-resizable-handle.ui-resizable-e").remove();
    $(".buyer-infor .ui-resizable-handle.ui-resizable-e").remove();
    $(".table-detail .ui-resizable-handle.ui-resizable-e").remove();

    if (!configCallback.isTVT() && !configCallback.isBLPHasFeeK80()) {
        $("[data-field=SellerSignContent] .content-sign").draggable({ disabled: true });
        $("[data-field=SellerSignContent] .content-sign").resizable('disable');

        if ($("[data-field=TaxCodeSignContent] .content-sign").is('.ui-resizable')) {
            $("[data-field=TaxCodeSignContent] .content-sign").resizable('disable');
            $("[data-field=TaxCodeSignContent] .content-sign").draggable({ disabled: true });
        }
    }
    else {
        removeResizeQR();
    }

    if (configCallback.isTVT()) {
        $("[data-field=SellerSignContent] .content-sign").draggable({ disabled: true });
        $("[data-field=SellerSignContent] .content-sign").resizable('disable');
        $("[data-field=SellerSignContent]").css("cursor", "");

        //Do trạng thái không mã thì không có init nên nếu remove sẽ lỗi
        if ($("[data-field=TaxCodeSignContent] .content-sign").hasClass("ui-resizable")) {
            $("[data-field=TaxCodeSignContent] .content-sign").draggable({ disabled: true });
            $("[data-field=TaxCodeSignContent] .content-sign").resizable('disable');
            $("[data-field=TaxCodeSignContent]").css("cursor", "");
        }
    }

}

/**
 * Get width của table trước khi disable
 * @param {any} oldCustomField
 * ndanh1 30/7/2020
 */
function getWidthTableBeforeChange(oldCustomField) {
    var $element = [],
        widths = [];
    switch (oldCustomField) {
        case 'table-detail':
            $element = $("#tbDetail .tr-header td:visible");
            break;
        case 'table-footer':
            $element = $("#tbFooter tr:visible:first td")
    }

    for (var i = 0; i < $element.length; i++) {
        widths.push($element[i].style.width);
    }

    return widths;
}

/**
 * set lại width cho table after disable resize
 * @param {any} oldCustomField
 * @param {any} widths
 * ndanh1 30/7/20250
 */
function setWidthTableAfterChange(oldCustomField, widths) {
    var $element = [];
    switch (oldCustomField) {
        case 'table-detail':
            $element = $("#tbDetail .tr-header td:visible");
            break;
        case 'table-footer':
            $element = $("#tbFooter tr:visible:first td");
    }

    for (var i = 0; i < $element.length; i++) {
        $($element[i]).css("width", widths[i]);
    }
}

/**
 * khởi tạo để resize buyer
 * 23/7/2020
 * */
function initResizeableForBuyerInfor() {
    if (!configCallback.isBLPHaveFee()) {
        var maxWidth = $(".buyer-infor").parent().width();
        $(".buyer-infor").resizable({
            maxWidth: maxWidth,
            handles: 'e',
            resize: function (event, ui) {
            }
        });
    }

}

/**
 * init resize table
 * @param {any} element
 * @param {any} disable
 * ndanh1 9/7/2020
 */
function resizeTable(element, disable, reinit, callback) {
    if (disable) {
        $(element).colResizable({ disable: true });
    } else if (reinit) {
        $(element).colResizable({ liveDrag: true });
    } else {
        $(element).colResizable({
            liveDrag: true,
            gripInnerHtml: "",
            draggingClass: "dragging",
            resizeMode: 'fit',
        });
    }
}

/**
 * init resize table first invoice
 * @param {any} element
 * @param {any} disable
 * ndanh1 9/7/2020
 */
function resizeTableFirstInvoice() {
    $("#firstInvoice").colResizable({
        liveDrag: true,
        gripInnerHtml: "",
        draggingClass: "dragging",
        resizeMode: 'fit',
        onDrag: function () {
            resetWidthOfTableInvoiceInfor();
        },
        onResize: function () {
            resetWidthOfTableInvoiceInfor();
        }
    });
}

/**
 * reset width của table InvoiceInfor
 * ndanh1 29/7/2020
 * */
function resetWidthOfTableInvoiceInfor() {
    if ($("#firstInvoice #invoiceInfor").length > 0) {
        resizeTable("#invoiceInfor", true);
        var rates = getRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"));
        resetWidthByRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"), rates, $("#firstInvoice .header-content-invoice").width());
        resizeTable("#invoiceInfor", false, true);
    }
}

/**
 * lấy item config tương ứng theo custom-field
 * @param {any} customField
 */
function getItemConfig(customField) {
    var me = this;
    arrConfig = [];
    switch (customField) {
        case "table-detail":
            arrConfig = me.getTableColumnConfig();
            break;
        case "table-footer":
            arrConfig = me.getConfigData(customField);
            break;
        case "sign-xml":
            arrConfig = me.getConfigData(customField);
            break;
        case "other-invoice":
            arrConfig = me.getConfigData(customField);
            break;
        default:
            arrConfig = me.getConfigData(customField);
            break;
    }
    return arrConfig;
}

/**
 * Lấy list item của thằng table-detail
 * ndanh1 29/6/2020
 * */
function getTableColumnConfig() {
    var elements = $("[group-field='table-detail'] .tr-header:first-child td[data-field]");
    var arr = [];
    for (var i = 0; i < elements.length; i++) {
        var $ele = $(elements[i]);
        var item = getColumnConfig($ele);
        var elemenChildren = $("[group-field='table-detail'] .tr-header td[field-parent={0}]".format(item.dataField));
        if (elemenChildren.length > 0) {
            item.canMove = false;
            item.value = null;
            arr.push(item);
            for (var j = 0; j < elemenChildren.length; j++) {
                var $eleChild = $(elemenChildren[j]);
                var itemChild = getColumnConfig($eleChild);
                itemChild.canMove = false;
                arr.push(itemChild);
            }
        } else {
            arr.push(item);
        }
    }

    arr = getTableColumnOfFooter(arr);
    return arr
}

function getColumnConfig($ele) {
    var symbol = null,
        dataField = $ele.attr("data-field"),
        isShow = !$ele.hasClass("display-none"),
        canHide = !$ele.hasClass("disable-hiden"),
        $label = $ele.find(".edit-label"),
        $labelEN = $ele.find(".edit-label-en"),
        isNumber = $("[group-field=table-detail] [data-field=" + dataField + "]").find(".edit-value").css("text-align") == "right";

    var label = {
        value: $label.text() ? $label.text().trim().replace(":", "") : "",
        isShow: true,
        canEdit: !$label.hasClass("none-edit"),
    }

    var labelEN = {
        value: $labelEN.text() ? $labelEN.text().trim().replace(":", "") : "",
        isShow: $labelEN.length > 0 && !$labelEN.hasClass("display-none"),
        canEdit: !$labelEN.hasClass("none-edit"),
    }

    var value = {
        value: "",
        isShow: true,
        canEdit: false,
        placeholder: dataField.includes("CustomField") ? "" : "[" + label.value + "]",
    }

    symbol = getItemConfigSymbol(dataField);

    return item = {
        dataField: dataField,
        isShow: isShow,
        canHide: canHide,
        canMove: true,
        label: label,
        value: value,
        labelEN: labelEN,
        symbol: symbol,
        isCustomField: dataField.includes("CustomField"),
        isNumber: isNumber,
    }
}

/**
 * get data của dòng tr-of-footer
 * ndanh1 28/1/2021
 * */
function getTableColumnOfFooter(arr) {
    var trOfFooter = $(".tr-of-footer");
    for (var i = 0; i < trOfFooter.length; i++) {
        var $element = $(trOfFooter[i]).find("[data-field]").first(),
            dataField = $(trOfFooter[i]).find("[data-field]").first().attr("data-field"),
            $label = $element.find(".edit-label"),
            $labelEN = $element.find(".edit-label-en");
        var label = {
            value: $label.text(),
            isShow: true,
            canEdit: true,
        }

        var labelEN = {
            value: $labelEN.text(),
            isShow: !$labelEN.hasClass("display-none"),
            canEdit: true,
        }

        var value = {
            value: $labelEN.text(),
            isShow: false,
            canEdit: true,
        }

        var item = {
            dataField: dataField,
            isShow: true,
            canHide: false,
            canMove: false,
            label: label,
            value: value,
            labelEN: labelEN,
            symbol: null,
            isCustomField: false,
        }

        arr.push(item);
    }

    return arr;
}

/**
 * lấy dữ liệu để đổ sang phần configdetail
 * @param {any} customField
 * ndanh1 29/6/2020
 */
function getConfigData(customField, lstDataFieldConfig) {
    var lstDataField = [];

    if (lstDataFieldConfig) {
        lstDataField = lstDataFieldConfig;
    } else {
        if (customField == "invoice-infor") {
            proccessInvoiceInfor();
        }
        var elements = $("[group-field='" + customField + "'] [data-field]");
        if (customField == "other-invoice") {
            if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
                elements = $("[group-field='" + customField + "'] [data-field], [data-field=QRCodeFieldTop]");
            }
        }
        for (var i = 0; i < elements.length; i++) {
            if ($(elements[i]).hasClass("sign-content")) {
                continue;
            }
            var field = $(elements[i]).attr("data-field");
            if (field == "AdjustmentInvoiceInfoChange" && customField == "buyer-infor") {
                continue;
            }
            lstDataField.push(field);
        }
        lstDataField = lstDataField.filter(onlyUnique);
    }


    var arr = [];
    for (var i = 0; i < lstDataField.length; i++) {
        var $ele = $("[data-field=" + lstDataField[i] + "]"),
            dataField = $ele.attr("data-field"),
            isShow = !$ele.hasClass("display-none"),
            canHide = !$ele.hasClass("disable-hiden"),
            canMove = true,
            $label = $ele.find('.edit-label'),
            $value = $ele.find('.edit-value'),
            $labelEN = $ele.find('.edit-label-en'),
            isMerge = null,
            type = 1;

        if ($ele.hasClass("not-show-preview")) {
            continue;
        }

        var label = {
            value: $label.text() ? $label.text().trim().replace(":", "") : "",
            isShow: $label.length > 0 && !$label.hasClass("display-none"),
            canEdit: !$label.hasClass("none-edit"),
            placeholder: "[Tiêu đề]",
        }

        var labelEN = {
            value: $labelEN.text() ? $labelEN.text().trim().replace(":", "") : "",
            isShow: $labelEN.length > 0 && !$labelEN.hasClass("display-none"),
            canEdit: !$labelEN.hasClass("none-edit"),
        }

        var value = {
            value: $value.text() ? $value.text().trim().replace(":", "") : "",
            isShow: ($value.length > 0 && !$value.hasClass("display-none")),
            canEdit: !$value.hasClass("none-edit"),
            placeholder: "",
            hasValue: $value.length > 0,
        }

        if (!value.value) {
            if (dataField.includes("CustomField")) {
                value.placeholder = "[Nội dung]";
            } else if (dataField == "TemplateNote") {
                value.placeholder = "Ghi chú";
            } else if (dataField == "SubTitleInvoice") {
                if (configCallback.isTVT()) {
                    value.placeholder = "Tên khác";
                } else {
                    value.placeholder = "Tên khác (VD: Phiếu bảo hành,...)";
                }
            } else if (dataField.includes("CustomSign")) {
                value.placeholder = "Chức danh người ký";
            } else if (!label.value) {
                value.placeholder = "[Nội dung]";
            }
            else {
                value.placeholder = "[" + label.value + "]";
            }
        }

        if ((dataField == "SubTitleInvoice" || dataField == "TemplateNote") && $value.length > 0) {
            value.value = $value.html().replaceAll("&nbsp;", " ").replaceAll("<br>", "\n");
        }

        if (customField == "information-footer") {
            label.isShow = false;
        }

        if (customField == "other-invoice" || customField == "information-footer" ||
            customField == "search-block" || customField == "table-footer" || customField == "sign-xml") {
            canMove = false;
        }

        if ($ele.parents(".merge-row-infor").length > 0) {
            isMerge = $ele.parents(".merge-row-infor").attr("merge-field");
        }

        if ($ele.parents("[sign-region]").length > 0) {
            isMerge = $ele.parents("[sign-region]").attr("sign-region");
        }

        if (customField == "sign-xml" && !(dataField.includes("Full") || dataField.includes("FullEN") || $ele.hasClass("seller-sign-content"))) {
            canMove = true;
        }

        if (dataField == "DateInvoice" && customField == "invoice-infor") {
            if (configCallback.isVN) {
                value.value = $ele.find(".date-text").text() + " " + $ele.find(".month-text").text() + " " + $ele.find(".year-text").text();
            } else {
                value.value = $ele.find(".date-text").text() + "(Date) " + $ele.find(".month-text").text() + "(month) " + $ele.find(".year-text").text() + "(year)";
            }

            labelEN.isShow = false;
        }

        if (dataField == "DeliveryOrderDate") {
            if (configCallback.isVN) {
                value.value = $ele.find(".date-text").text() + " " + $ele.find(".month-text").text() + " " + $ele.find(".year-text").text();
            } else {
                value.value = $ele.find(".date-text").text() + "(date) " + $ele.find(".month-text").text() + "(month) " + $ele.find(".year-text").text() + "(year)";
            }

            labelEN.isShow = false;
        }

        if ($ele.hasClass("type-2")) {
            type = 2;
        } else if ($ele.hasClass("type-3")) {
            type = 3;
        }

        var isCustomField = false;
        if (dataField.includes("CustomField") || dataField == "DueDate" || dataField == "ShippingAddress") {
            isCustomField = true;
        } else if ($ele.hasClass("custom-field-buyer") || $ele.hasClass("custom-invoice-infor") || $ele.hasClass("custom-field-ticket")) {
            isCustomField = true;
        } else if ($ele.hasClass("custom-seller") || dataField.includes("CustomSeller_")) {
            isCustomField = true;
        }

        if ((dataField == "TicketPriceBeforeVAT" || dataField == "TicketPriceBeforeVATOther")) {
            if ($(".VatRateTicket").text()) {
                value.canEdit = false;
            } else {
                value.canEdit = true;
            }
        }

        var unit = null;
        if ($ele.find(".unit-name-ticket").length > 0) {
            unit = {
                value: $ele.find(".unit-name-ticket").text(),
                isShow: !$ele.find(".unit-name-ticket").hasClass("display-none"),
                canEdit: false,
            }
        }

        if (dataField == "QRCodeField") {
            if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
                if ($("[data-field=QRCodeField]").hasClass("display-none")) {
                    isShow = false;
                }
                else {
                    if ($("[data-field=QRCodeField]").find(".qrcode-parent").hasClass("display-none")) {
                        isShow = false;
                    }
                    else {
                        isShow = true;
                    }
                }
            }
            else {
                isShow = !$(".qrcode-parent").hasClass("display-none");
            }
        }

        if (dataField == "TaxCodeSign" && configCallback.isDeliveryND51()) {
            canMove = false;
        }

        if (configCallback.isTVT() && lstDataField[i] == "TotalAmountInWords") {
            if ($("#en-amount-inword").css("display") != "none") {
                let amountEN = $("#en-amount-inword").text();
                value.value = value.value + " " + amountEN;
            }
        }

        var item = {
            dataField: dataField,
            isShow: isShow,
            canHide: canHide,
            canMove: canMove,
            label: label,
            value: value,
            labelEN: labelEN,
            symbol: null,
            unit: unit,
            merge: isMerge,
            isCustomField: isCustomField,
            type: type,
        }
        arr.push(item);
    }

    return arr;
}

/**
 * Kiểm tra xem nếu có subtitle invoice chưa
 * nếu chưa thì thực hiện thêm vào
 * ndanh1 28/1/2021
 * */
function proccessInvoiceInfor() {
    if ($("[data-field='SubTitleInvoice'] .edit-value").length == 0) {
        $("[data-field='SubTitleInvoice']").empty();
        $("[data-field='SubTitleInvoice']").append("<span class='edit-value'></span>")
    }
}

/**
 * Lấy dữ liệu của dòng symbol
 * ndanh1 29/6/2020
 * */
function getItemConfigSymbol(dataField) {
    var symbol = {
        value: "",
        isShow: false,
    }
    $symbolEle = $(".tr-symbol");
    if ($symbolEle.length > 0) {
        symbol = {
            value: $(".tr-symbol").find("[data-field=" + dataField + "] .edit-symbol").text(),
            isShow: true,
        }
    }

    return symbol;
}

/**
 * Lấy style của thằng đang được focus
 * @param {any} dataField
 * @param {any} typeData
 * 29/6/2020
 */
function getStyleOfElement(customField, dataField, typeData, isTicket) {
    var type = getTypeData(typeData);
    var $element = $("[data-field=" + dataField + "] ." + type);
    if ($element.length > 0) {
        var $thisElement = $("[data-field=" + dataField + "]");
        var color = $element.css("color");
        var size = $element.css("font-size").replace("px", "");
        var italic = $element.css("font-style") == "italic" ? true : false;
        var bold = Number($element.css("font-weight")) > 600 ? true : false;
        var bgColor = customField == "table-detail" ? $element.parents("td").css("background-color") : $element.css("background-color");
        var $value = $("td[data-field=" + dataField + "] ." + type).first();
        var customSellerAlign = 1;
        var align = $element.css("text-align"),
            mstBorder = false;
        var merge = $thisElement.parents(".merge-row-infor").length > 0 ? $thisElement.parents(".merge-row-infor").attr("merge-field") : "";
        var typeTextTransform = "";

        if (dataField == "SellerTaxCode" || dataField == "BuyerTaxCode") {
            mstBorder = $("[data-field=" + dataField + "]").hasClass("border-active");
        }

        if (customField == "seller-infor") {
            if ($thisElement.hasClass("type-2")) {
                customSellerAlign = 2;
            } else if ($thisElement.hasClass("type-3")) {
                customSellerAlign = 3;
            }
            if (isTicket) {
                let position = $("[data-field=" + dataField + "]").css("justify-content");
                if (position == 'center') {
                    align = 'center';
                }
                else if (position == 'flex-start') {
                    align = 'left';
                }
                else if (position == 'flex-end') {
                    align = 'right';
                }
                else {
                    align = 'left';
                }
            }
        } else if (customField == "ticket-price") {
            align = $thisElement.css("text-align");
        } else if (dataField == "SearchInfo" || dataField == "LinkSearch" || dataField == "TransactionID" || dataField == "MeinvoiceNote") {
            if (dataField == "MeinvoiceNote") {
                align = $("[data-field=" + dataField + "]").css("text-align");
            } else {
                align = $("[data-field=" + dataField + "]").parent().css("text-align");
            }
        }

        if (configCallback.isTVT()) {
            customSellerAlign = ($thisElement.find(".edit-label").length > 0 && !$thisElement.find(".edit-label").hasClass("display-none")) ? 1 : 2;
        }

        var isWrap = false;

        if (customField == "seller-infor" || customField == "buyer-infor") {
            if ($element.css("white-space") == "nowrap") {
                isWrap = false;
            }
            else {
                isWrap = true;
            }
        }
        else if (customField == "table-detail") {
            if ($element.css("word-break") == "break-all") {
                isWrap = true;
            }
            else {
                isWrap = false;
            }
        }

        if (configCallback.isBLP()) {
            if ((!configCallback.isBLPHaveFee() && customField == "invoice-infor") || (configCallback.isBLPHaveFee() && customField == "buyer-infor")) {
                if (type == "edit-value") {
                    var feeName = $("[data-field=FeeName] .edit-value");
                    typeTextTransform = feeName.css("text-transform");
                }
                
            }
        }

        var item = {
            dataField: dataField,
            color: color ? color : "#000",
            size: size,
            italic: italic,
            bold: bold,
            bgColor: bgColor,
            align: align,
            mstBorder: mstBorder,
            customSellerAlign: customSellerAlign,
            merge: merge,
            isWrap: isWrap,
            typeTextTransform: typeTextTransform,
            customField: customField,
            type: type,
        }
        return item;
    }
}

/**
 * Lấy style của ô ký với vé
 * tqha (14/9/2022)
 * @param {any} dataField
 */
function getStyleForBlockSignTicket(customField, dataField) {
    var $element = $("[data-field=" + dataField + "] .esign-label");
    if ($element.length > 0) {
        var $thisElement = $("[data-field=" + dataField + "]");
        var color = $element.css("color");
        var size = $element.css("font-size").replace("px", "");
        var italic = $element.css("font-style") == "italic" ? true : false;
        var bold = Number($element.css("font-weight")) > 600 ? true : false;
        var bgColor = customField == "table-detail" ? $element.parents("td").css("background-color") : $element.css("background-color");
        var customSellerAlign = 1;
        var align = $element.css("text-align"),
            mstBorder = false;
        var merge = $thisElement.parents(".merge-row-infor").length > 0 ? $thisElement.parents(".merge-row-infor").attr("merge-field") : "";
        var typeTextTransform = "";
        var isWrap = false;

        var item = {
            dataField: dataField,
            color: color ? color : "#000",
            size: size,
            italic: italic,
            bold: bold,
            bgColor: bgColor,
            align: align,
            mstBorder: mstBorder,
            customSellerAlign: customSellerAlign,
            merge: merge,
            isWrap: isWrap,
            typeTextTransform: typeTextTransform,
            customField: customField,
            type: null,
        }
        return item;
    }
}


/**
 * set data field active
 * @param {any} dataField
 * ndanh1 3/7/2020
 */
function setActiveDataField(dataField) {
    $(".element-active").removeClass("element-active");
    $("[data-field=" + dataField + "]").addClass("element-active");
}

/**
 * Group lại các giá trị
 * @param {any} value
 * @param {any} index
 * @param {any} self
 */
function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

/**
 * Thay đổi dữ liệu bên preview
 * @param {any} itemConfig
 * @param {any} dataField
 * @param {any} typeData
 * ndanh1 26/9/2020
 */
function changeDataItem(customField, itemConfig, dataField, typeData) {
    var type = getTypeData(typeData);
    var isVN = configCallback.isVN;
    var $thisElement = $("[data-field=" + dataField + "]");
    var $element = $("[data-field=" + dataField + "] ." + type);
    var $value = $("[data-field=" + dataField + "] .edit-value");
    var $label = $("[data-field=" + dataField + "] .edit-label");
    var $twoDot = $("[data-field=" + dataField + "] .two-dot");

    if (customField == "table-detail") {
        if (typeData != "value" && !$element.hasClass("none-edit")) {
            $element.text(itemConfig.value);
        }
    } else {
        var value = itemConfig.value;
        // thỏa mãn điều kiện trên mới cho thay đổi nội dung edit-value
        if (conditionToEdit(typeData, dataField) && !$element.hasClass("none-edit")) {
            if (dataField == "TemplateNote" || dataField == "SubTitleInvoice") {
                $element.html(value);
            } else {
                $element.text(value);
            }
        }

        if (!(configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent"))) {
            // Nếu label nó có text và label của nó hiển thị thì bỏ padding-none
            if ($label.text() && !$label.hasClass("display-none")) {
                $value.removeClass("padding-none");
            } else {
                // Nếu nó đang chứa 1 empty-div để căn trái thì cũng bỏ padding-none
                if ($thisElement.find(".empty-div").length > 0 && getWidth($thisElement.find(".empty-div")) > 0) {
                    $value.removeClass("padding-none");
                } else {
                    $value.addClass("padding-none");
                }
            }
        }
    }

    if (configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent")) {
        changeStyleSignBlock($element, itemConfig, dataField);
    }
    else {
        changeStyleElement($element, itemConfig, typeData, dataField);
    }

    if (itemConfig.alginSeller != 0) {
        if (configCallback.customField == "seller-infor") {
            setAlignmentSellerInfor(itemConfig.alginSeller);
        } else {
            setAlignmentBuyerInfor(itemConfig.alginSeller);
        }
    }

    if (configCallback.checkFieldNoteCustomBuyer(dataField)) {
        if (!$value.hasClass('display-none') && $value.text() && !$label.hasClass('display-none') && $label.text()) {
            $twoDot.removeClass("display-none");
        } else {
            $twoDot.addClass("display-none");
        }
    }

    if (dataField == "SubTitleInvoice" || dataField == "SellerSignByClient" || dataField == "SellerSignDateClient" || dataField == "SellerSignBy" || dataField == "SellerSignDate" || dataField == "TaxCodeSign" || dataField == "TaxCodeSignDate") {
        if ($('[data-field=' + dataField + '] .edit-label-en').text()) {
            $('[data-field=' + dataField + '] .edit-label-en').css("padding-left", "4px");
        } else {
            $('[data-field=' + dataField + '] .edit-label-en').css("padding-left", "0");
        }
    }
}

/**
 * Một số điều kiện để thay đổi value
 * @param {any} typeData
 * @param {any} dataField
 */
function conditionToEdit(typeData, dataField) {
    if (!(typeData == "value" && (dataField == "InforWebsiteSearch" || dataField == "SellerTaxCode" || dataField == "ByerTaxCode" || dataField == "DateInvoice" || dataField == "SellerSignContent" || dataField == "TaxCodeSignContent"))) {
        return true;
    }

    return false;
}

/**
 * Lấy về loại dữ liệu
 * @param {any} typeData
 */
function getTypeData(typeData) {
    var type = "edit-value";
    switch (typeData) {
        case "label-en":
            type = "edit-label-en"
            break;
        case "label":
            type = "edit-label";
            break;
        case "symbol":
            type = "edit-symbol";
            break;
        case "unit-name":
            type = "unit-name-ticket";
            break;
        default:
            type = "edit-value";
    }

    return type;
}

/**
 * Thay đổi style của element
 * @param {any} $element
 * @param {any} itemConfig
 * ndanh1 29/6/2020
 */
function changeStyleElement($element, itemConfig, typeData, dataField) {
    var fontBold = "400",
        fontItalic = "normal",
        fontColor = "#000000",
        fontSize = "14px";

    if (itemConfig.bold) {
        fontBold = "bold";
    }

    if (itemConfig.italic) {
        fontItalic = "italic";
    }

    if (itemConfig.size) {
        fontSize = itemConfig.size;
        fontSize = fontSize.replaceAll("px", "") + "px";
    }

    if (itemConfig.color) {
        fontColor = itemConfig.color;
    }

    $element.css("font-weight", fontBold);
    $element.css("color", fontColor);
    $element.css("font-style", fontItalic);
    $element.css("font-size", fontSize);

    if ((typeData == "label" && configCallback.isVN) || (typeData == "label-en" && !configCallback.isVN)) {
        $element.parent().find(".two-dot").css("font-weight", fontBold);
        $element.parent().find(".two-dot").css("color", fontColor);
        $element.parent().find(".two-dot").css("font-style", fontItalic);
        $element.parent().find(".two-dot").css("font-size", fontSize);
    }

    if (dataField && dataField == "DateInvoice" && typeData == "value") {
        var fontEN = Number(itemConfig.size.replace("px", "")) - 1;
        $element.find(".edit-label-en").css("font-size", fontEN + "px");
    }

    if (configCallback.isTVT() && dataField && dataField == "TotalAmountInWords" && typeData == "value") {
        var fontSizeEN = Number(itemConfig.size.replace("px", "")) - 1 + "px";
        $element.parent().find("#en-amount-inword").css("font-weight", fontBold);
        $element.parent().find("#en-amount-inword").css("color", fontColor);
        $element.parent().find("#en-amount-inword").css("font-style", fontItalic);
        $element.parent().find("#en-amount-inword").css("font-size", fontSizeEN);
    }

    if (typeData == "label") {
        if (itemConfig.size <= 10) {
            $element.closest("[data-field]").css("padding", "1px 0");
        }
        else {
            $element.closest("[data-field]").css("padding", "4px 0");
        }

        $element.closest("[data-field]").css("font-size", fontsize + "px");
    }
    else if (typeData == "value") {
        if ($element.parent().find(".edit-label").css("display") != "none") {
            if (Number.parseInt($element.parent().find(".edit-label").css("font-size")) <= 10) {
                $element.closest("[data-field]").css("padding", "1px 0");
            }
            else {
                if (itemConfig.size <= 10) {
                    $element.closest("[data-field]").css("padding", "1px 0");
                }
                else {
                    $element.closest("[data-field]").css("padding", "4px 0");
                }
            }
        }
        else {
            if (itemConfig.size <= 10) {
                $element.closest("[data-field]").css("padding", "1px 0");
            }
            else {
                $element.closest("[data-field]").css("padding", "4px 0");
            }
        }
    }
}

/**
 * reset style cho phần chữ ký
 * tqha (22/9/2022)
 * @param {any} dataField
 */
function resetStyleSignBlock(dataField) {
    var $signBy = $("[data-field=SellerSignByClient]");
    var $signDate = $("[data-field=SellerSignDateClient]");

    if (dataField == "TaxCodeSignContent") {
        $signBy = $("[data-field=TaxCodeSign]");
        $signDate = $("[data-field=TaxCodeSignDate]");
    }

    //Phần ký bởi
    var $editSignByLabel = $signBy.find(".esign-label");
    var $editSignByLabelEN = $signBy.find(".esign-label-en");
    var $editSignByTwoDot = $signBy.find(".two-dot");
    var $editSignByValue = $signBy.find(".esign-value");

    //Phần ký ngày
    var $editSignDateLabel = $signDate.find(".esign-label");
    var $editSignDateLabelEN = $signDate.find(".esign-label-en");
    var $editSignDateTwoDot = $signDate.find(".two-dot");
    var $editSignDateValue = $signDate.find(".esign-value");

    //Update style cho phần ký bởi
    //--------------------
    $editSignByLabel.removeAttr("style");
    $editSignByLabelEN.removeAttr("style");
    $editSignByTwoDot.removeAttr("style");
    $editSignByValue.removeAttr("style");
    //--------------------

    //Update style cho phần ký ngày
    //--------------------
    $editSignDateLabel.removeAttr("style");
    $editSignDateLabelEN.removeAttr("style");
    $editSignDateTwoDot.removeAttr("style");
    $editSignDateValue.removeAttr("style");
    //--------------------

    //Update style cho từ Signature Valid
    var $element = $signBy.prev();
    $element.removeAttr("style");
}

/**
 * Thay đổi style cho phần ký với vé
 * @param {any} $element
 * @param {any} itemConfig
 * @param {any} dataField
 * tqha (14/9/2022)
 */
function changeStyleSignBlock($element, itemConfig, dataField) {
    var fontBold = "400",
        fontItalic = "normal";

    if (itemConfig.bold) {
        fontBold = "bold";
    }

    if (itemConfig.italic) {
        fontItalic = "italic";
    }
    handleChangeStyleSignBlock(itemConfig.size, dataField, null, fontBold, fontItalic);
}

/**
 * Xử lý thay đổi style cho phần ký với vé
 * @param {any} $element
 * @param {any} itemConfig
 * @param {any} dataField
 * tqha (14/9/2022)
 */
function handleChangeStyleSignBlock(size, dataField, lineHeight, fontBold, fontItalic) {
    var fontSize = "14px",
        fontSizeEN = "13px";

    if (size) {
        size = Number.parseInt(size);
        fontSize = size;
        fontSizeEN = size - 1;
        fontSize = fontSize + "px";
        fontSizeEN = fontSizeEN + "px";
    }

    var $signBy = $("[data-field=SellerSignByClient]");
    var $signDate = $("[data-field=SellerSignDateClient]");

    if (dataField == "TaxCodeSignContent") {
        $signBy = $("[data-field=TaxCodeSign]");
        $signDate = $("[data-field=TaxCodeSignDate]");
    }

    //Phần ký bởi
    var $editSignByLabel = $signBy.find(".esign-label");
    var $editSignByLabelEN = $signBy.find(".esign-label-en");
    var $editSignByTwoDot = $signBy.find(".two-dot");
    var $editSignByValue = $signBy.find(".esign-value");

    //Phần ký ngày
    var $editSignDateLabel = $signDate.find(".esign-label");
    var $editSignDateLabelEN = $signDate.find(".esign-label-en");
    var $editSignDateTwoDot = $signDate.find(".two-dot");
    var $editSignDateValue = $signDate.find(".esign-value");

    //Update style cho phần ký bởi
    //--------------------
    $editSignByLabel.css("font-size", fontSize);
    $editSignByLabelEN.css("font-size", fontSizeEN);
    $editSignByTwoDot.css("font-size", fontSize);
    $editSignByValue.css("font-size", fontSize);
    //--------------------

    //Update style cho phần ký ngày
    //--------------------
    $editSignDateLabel.css("font-size", fontSize);
    $editSignDateLabelEN.css("font-size", fontSizeEN);
    $editSignDateTwoDot.css("font-size", fontSize);
    $editSignDateValue.css("font-size", fontSize);
    //--------------------

    //Update style cho từ Signature Valid
    var $element = $signBy.prev();
    $element.css("font-size", fontSize);

    if (size <= 10) {
        $("[data-field=" + dataField + "]").css("padding", "1px 0");
        $signBy.css("padding", "1px 0");
        $signDate.css("padding", "1px 0");
        $element.css("padding", "1px 0");
    }
    else {
        $("[data-field=" + dataField + "]").css("padding", "4px 0");
        $signBy.css("padding", "4px 0");
        $signDate.css("padding", "4px 0");
        $element.css("padding", "4px 0");
    }

    if (size <= 10) {
        $("[data-field=" + dataField + "] .background-sign").css("background-size", "contain");
    }
    else {
        $("[data-field=" + dataField + "] .background-sign").css("background-size", "");
    }

    //Nếu truyền cả line height thì update tiếp line height
    if (lineHeight) {
        lineHeight = lineHeight + "px";
        //Update style cho phần ký bởi
        //--------------------
        $editSignByLabel.css("line-height", lineHeight);
        $editSignByLabelEN.css("line-height", lineHeight);
        $editSignByTwoDot.css("line-height", lineHeight);
        $editSignByValue.css("line-height", lineHeight);
        //--------------------

        //Update style cho phần ký ngày
        //--------------------
        $editSignDateLabel.css("line-height", lineHeight);
        $editSignDateLabelEN.css("line-height", lineHeight);
        $editSignDateTwoDot.css("line-height", lineHeight);
        $editSignDateValue.css("line-height", lineHeight);
        //--------------------

        //Update style cho từ Signature Valid
        var $element = $signBy.prev();
        $element.css("line-height", lineHeight);
    }

    //Nếu truyền cả fontBold và fontItalic thì update tiếp
    if (fontBold && fontItalic) {
        //Update style cho phần ký bởi
        //--------------------
        $editSignByLabel.css("font-weight", fontBold);
        $editSignByLabelEN.css("font-weight", fontBold);
        $editSignByTwoDot.css("font-weight", fontBold);

        $editSignByLabel.css("font-style", fontItalic);
        $editSignByLabelEN.css("font-style", fontItalic);
        $editSignByTwoDot.css("font-style", fontItalic);
        //--------------------

        //Update style cho phần ký ngày
        //--------------------
        $editSignDateLabel.css("font-weight", fontBold);
        $editSignDateLabelEN.css("font-weight", fontBold);
        $editSignDateTwoDot.css("font-weight", fontBold);

        $editSignDateLabel.css("font-style", fontItalic);
        $editSignDateLabelEN.css("font-style", fontItalic);
        $editSignDateTwoDot.css("font-style", fontItalic);
        //--------------------

    }
}

/**
 * Set text align cho preview
 * @param {any} dataField
 * @param {any} align
 * @param {any} typeData
 * ndanh1 29/6/2020
 */
function setTextAlign(dataField, align, typeData) {
    if (dataField == "SearchInfo" || dataField == "LinkSearch" || dataField == "TransactionID" || dataField == "MeinvoiceNote") {
        if (dataField == "MeinvoiceNote") {
            $("[data-field=" + dataField + "]").css("text-align", align);
        } else {
            $("[data-field=" + dataField + "]").parent().css("text-align", align);
        }
    } else if (configCallback.customField == "table-footer" && !isTableFooterMultiRates(dataField) && (typeData == "label" || typeData == "label-en")) {
        if (align == "right") {
            $("[data-field=" + dataField + "] .edit-label").css("width", "100%");
        } else {
            $("[data-field=" + dataField + "] .edit-label").css("width", "unset");
        }
        $("[data-field=" + dataField + "] .edit-label").css("text-align", align);
    } else {
        var type = getTypeData(typeData);
        $("[data-field=" + dataField + "] ." + type).css("text-align", align);
    }
}

/**
 * Ẩn hiện Item block
 * @param {any} customField
 * @param {any} dataField
 * @param {any} isShow
 * danh1 29/6/2020
 */
function showHideItemBlock(customField, dataField, isShow) {
    var $element = $("[data-field=" + dataField + "]");

    if ($element.length > 0) {
        showHideItemBlockData(dataField, isShow);
        if (customField == "seller-infor") {
            reloadAlignSeller();
        } else if (customField == "buyer-infor") {
            reloadAlignBuyer();
            changePositionQRCodeCashRegister();
        }

        if ((dataField == "DiscountRateOther" || dataField == "TotalAmountWithVAT") && configCallback.isTemplateWater()) {
            if (isShow) {
                $element.parent().find("[data-field*=NotShow]").show();
            } else {
                $element.parent().find("[data-field*=NotShow]").hide();
            }
        }
    } else {
        if (customField == "buyer-infor") {
            addNewCustomBuyer(configCallback.getDataCustomField(dataField));
        } else if (customField == "table-detail") {
            addNewColumnInTable(configCallback.getDataCustomField(dataField));
        }
    }
    checkStepToExecute();
}

/**
 * load lại kiểu hiển thị của seller-infor
 * ndanh1 2/7/2020
 * */
function reloadAlignSeller() {
    var typeSeller = 1;
    if ($("[group-field=seller-infor]").hasClass("type-2")) {
        typeSeller = 2;
    } else if ($("[group-field=seller-infor]").hasClass("type-3")) {
        typeSeller = 3;
    }
    setAlignmentSellerInfor(typeSeller);

    // load lại kiểu hiển thị của custom seller
    reloadTypeCustomSeller();

}

function reloadAlignBuyer() {
    var typeSeller = 1;
    if ($("[group-field=buyer-infor]").hasClass("type-2")) {
        typeSeller = 2;
    } else if ($("[group-field=buyer-infor]").hasClass("type-3")) {
        typeSeller = 3;
    }
    setAlignmentBuyerInfor(typeSeller);

}


function getTypeSellerData($customSeller) {
    var type = 1;
    if ($customSeller.hasClass('type-2')) {
        type = 2;
    } else if ($customSeller.hasClass('type-3')) {
        type = 3;
    }

    return type;
}

/**
 * load lại kiểu hiển thị của custom seller
 * ndanh1
 * */
function reloadTypeCustomSeller() {
    // load lại thêm cả thằng thông tin mở rộng nếu có
    var $customSeller = $('[group-field=seller-infor] [data-field]');
    if ($customSeller.length > 0) {
        for (var i = 0; i < $customSeller.length; i++) {
            var $item = $($customSeller[i]);
            if ($item.parent('.merge-row-infor').length > 0) {
                if ($item.parent('.merge-row-infor').find('[data-field]').first().attr('data-field') != $item.attr('data-field')) {
                    continue;
                }
            }

            var dataFieldCustom = $($customSeller[i]).attr('data-field');
            var type = getTypeSellerData($($customSeller[i]));
            changeTypeCustomData(dataFieldCustom, type);
        }
    }
}

/**
 * Ẩn hiện Item data table
 * @param {any} customField
 * @param {any} dataField
 * @param {any} isShow
 * danh1 29/6/2020
 */
function showHideItemBlockData(dataField, isShow) {
    if (configCallback.customField == "table-detail") {
        resizeTable("#tbDetail", true);
    }
    var $element = $("[data-field=" + dataField + "]");
    if (isShow) {
        if (dataField == "DescriptionInvoiceClient") {
            $("[data-field=DescriptionInvoiceClient]").removeClass("display-cannot-show");
            $("[data-field=DescriptionInvoiceClient]").removeClass("display-none");
        }
        else if (dataField == "ServiceFeeRate") {
            $element.removeClass("display-none");
            $element.find(".edit-label").parent().css("border-style", "none none solid solid");
            $('[data-field="NotShow5"]').hide();
            $('[data-field="NotShow6"]').hide();
        }
        else {
            $element.removeClass("display-none");
        }

        if (configCallback.customField == "table-detail" && $(".tr-symbol").length > 0) {
            $(".tr-symbol [data-field=" + dataField + "]").removeClass("display-none");
        }
    } else {
        if (dataField.includes("CustomField")) {
            $element.remove();
        } else {
            if (dataField == "DescriptionInvoiceClient") {
                $("[data-field=DescriptionInvoiceClient]").addClass("display-cannot-show");
                $("[data-field=DescriptionInvoiceClient]").addClass("display-none");
            }
            else if (dataField == "ServiceFeeRate") {
                if ($('div[attr-field="ServiceFee"]').checked) {
                    $element.removeClass("display-none");
                    $element.find(".edit-label").parent().css("border-style", "none none solid solid");
                    $('[data-field="NotShow5"]').hide();
                    $('[data-field="NotShow6"]').hide();
                }
                else {
                    $element.addClass("display-none");
                    $('[data-field="NotShow5"]').show();
                    $('[data-field="NotShow6"]').show();
                }
            }
            else {
                $element.addClass("display-none");
            }
        }


        if (configCallback.customField == "table-detail" && $(".tr-symbol").length > 0) {
            $(".tr-symbol [data-field=" + dataField + "]").addClass("display-none");
        }
    }

    if (dataField == "MeinvoiceNote") {
        var maxHeight = configCallback.isA5() ? 543 : 1143;
        var minHeight = maxHeight - $(".search-block").height();
        $(".content-detail").css("min-height", minHeight + "px");
    }

    if (configCallback.customField == "table-detail") {
        resizeTable("#tbDetail", true);
        reloadNewWidthTable($("#tbDetail .tr-header td:visible"));
        resizeTable("#tbDetail", false, true);
    }
}

/**
 * Đặt lại width của table
 * ndanh1 15/7/2020
 * */
function resetWidthTable($element) {
    var headerTD = $element;
    for (var i = 0; i < headerTD.length; i++) {
        $(headerTD[i]).css("width", $(headerTD[i]).width() + "px")
    }
}

/**
 * Vẽ dòng ký hiệu cột
 * ndanh1 29/6/2020
 * */
function addRowSymbolColumn() {
    $symbolEle = $(".tr-symbol");
    if ($symbolEle.length > 0) {
        $symbolEle.remove();
    } else {
        var htmlSymbol = "";
        var $trHeaderLast = $(".tr-header").last();
        var $tds = $trHeaderLast.find("td[data-field]");
        if ($(".tr-header").length > 1) {
            $tds = $(".tr-data-detail:visible").first().find("td[data-field]");
        }
        var symbolValue = 1;
        var dataFieldActive = $(".element-active").attr("data-field");
        for (var i = 0; i < $tds.length; i++) {
            var $item = $($tds[i]);
            var displayNone = $item.hasClass("display-none") ? "display-none" : "";
            var dataField = $item.attr("data-field");
            var activeElement = dataFieldActive == dataField ? "element-active" : "";
            var tdSymbol = "<td class='text-center font-bold {0} {3}' data-field='{1}'><div class='edit-symbol'>{2}</div></td>";
            if (!displayNone) {
                tdSymbol = tdSymbol.format(displayNone, dataField, symbolValue, activeElement);
                symbolValue += 1;
            } else {
                tdSymbol = tdSymbol.format(displayNone, dataField, "", "");
            }

            htmlSymbol += tdSymbol;
        }

        htmlSymbol = "<tr class='tr-symbol'>" + htmlSymbol + "</tr>";

        $trHeaderLast.after(htmlSymbol);
    }
}

/**
 * Lấy list dữ liệu symbol
 * ndanh1 8/7/2020
 * */
function getColumnSymbolData() {
    var listSymbol = [];
    var $eleSymbol = $(".tr-symbol .edit-symbol");
    for (var i = 0; i < $eleSymbol.length; i++) {
        var $element = $($eleSymbol[i]);
        var item = {
            value: $element.text(),
            dataField: $($element.parents("[data-field]")[0]).attr("data-field")
        }
        listSymbol.push(item);
    }
    return listSymbol;
}

/**
 * Tăng hoặc giảm kích thước của element xác định
 * @param {any} dataField
 * @param {any} typeData
 * @param {any} isDesc
 * ndanh1 30/6/2020
 */
function descOrInscFontSize(dataField, typeData, isDesc) {
    if (configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent")) {
        var fontSize = 16;
        if (dataField == "SellerSignContent") {
            fontSize = Number.parseInt($("[data-field=SellerSignByClient] .edit-label").css("font-size").replace("px", ""));
        }
        else {
            fontSize = Number.parseInt($("[data-field=TaxCodeSignDate] .edit-label").css("font-size").replace("px", ""));
        }

        if (isDesc == "true") {
            fontSize -= 1;
        } else {
            fontSize += 1;
        }

        if (fontSize < 8) {
            fontSize = 8;
        }
        else if (fontSize > 30) {
            fontSize = 30;
        }

        if (!configCallback.isVN) {
            fontSize = fontSize > 20 ? 20 : fontSize;
        }
        handleChangeStyleSignBlock(fontSize, dataField, fontSize);
        return fontSize + "px";
    }
    else {
        var type = getTypeData(typeData);
        var $element = $("[data-field=" + dataField + "] ." + type);
        var $thisElementDataField = $("[data-field=" + dataField + "]");
        var fontSize = Number($element.css("font-size").replace("px", ""));
        if (isDesc == "true") {
            fontSize -= 1;
        } else {
            fontSize += 1;
        }

        if (fontSize < 8) {
            fontSize = 8;
        }
        else if (fontSize > 30) {
            fontSize = 30;
        }

        $element.css("font-size", fontSize + "px");

        if ((typeData == "label" && configCallback.isVN) || (typeData == "label-en" && !configCallback.isVN)) {
            $thisElementDataField.find(".two-dot").css("font-size", fontSize + "px");
        }

        if (dataField == "DateInvoice" && typeData == "value") {
            var fontSizeEN = fontSize - 1;
            $thisElementDataField.find(".edit-label-en").css("font-size", fontSizeEN + "px")
        }

        if (configCallback.isTVT() && dataField && dataField == "TotalAmountInWords" && typeData == "value") {
            var fontSizeEN = fontSize - 1;
            $element.parent().find("#en-amount-inword").css("font-size", fontSizeEN + "px");
        }

        if (typeData == "label") {
            if (fontSize <= 10) {
                $thisElementDataField.css("padding", "1px 0");
            }
            else {
                $thisElementDataField.css("padding", "4px 0");
            }
            $thisElementDataField.css("font-size", fontSize + "px");
        }
        else if (typeData == "value") {
            if ($thisElementDataField.find(".edit-label").css("display") != "none") {
                if (Number.parseInt($thisElementDataField.find(".edit-label").css("font-size")) <= 10) {
                    $thisElementDataField.css("padding", "1px 0");
                }
                else {
                    if (fontSize <= 10) {
                        $thisElementDataField.css("padding", "1px 0");
                    }
                    else {
                        $thisElementDataField.css("padding", "4px 0");
                    }
                }
            }
            else {
                if (fontSize <= 10) {
                    $thisElementDataField.css("padding", "1px 0");
                }
                else {
                    $thisElementDataField.css("padding", "4px 0");
                }
            }
        }

        return fontSize + "px";
    }
    
}


/**
 * Tăng hoặc giảm line height của element xác định
 * @param {any} dataField
 * @param {any} typeData
 * @param {any} isDesc
 * tqha(22/9/2022)
 */
function descOrInscLineHeight(dataField, typeData, isDesc) {
    if (configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent")) {
        var fontSize = 16;
        var lineHeight = 16;
        if (dataField == "SellerSignContent") {
            fontSize = Number.parseInt($("[data-field=SellerSignByClient] .edit-label").css("font-size").replace("px", ""));
            lineHeight = Number.parseInt($("[data-field=SellerSignByClient] .edit-label").css("line-height").replace("px", ""));
            if (isNaN(lineHeight)) {
                lineHeight = fontSize;
            }
        }
        else {
            fontSize = Number.parseInt($("[data-field=TaxCodeSignDate] .edit-label").css("font-size").replace("px", ""));
            lineHeight = Number.parseInt($("[data-field=TaxCodeSignDate] .edit-label").css("line-height").replace("px", ""));
            if (isNaN(lineHeight)) {
                lineHeight = fontSize;
            }
        }

        if (isDesc == "true") {
            lineHeight -= 1;
        } else {
            lineHeight += 1;
        }

        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }

        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }

        handleChangeStyleSignBlock(fontSize, dataField, lineHeight);
    }
    else {
        var type = getTypeData(typeData);
        var $element = $("[data-field=" + dataField + "] ." + type);
        var $thisElementDataField = $("[data-field=" + dataField + "]");
        var fontSize = Number($element.css("font-size").replace("px", ""));
        var lineHeight = Number($element.css("line-height").replace("px", ""));
        if (isNaN(lineHeight)) {
            lineHeight = fontSize;
        }
        if (isDesc == "true") {
            lineHeight -= 1;
        } else {
            lineHeight += 1;
        }

        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }

        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }

        $element.css("line-height", lineHeight + "px");
        if (!configCallback.isTVT()) {
            if ($element.is("span") && typeData == "label") {
                if ($element.parent().attr("data-field") == "SearchInfo") {
                    $element.parent().parent().css("line-height", lineHeight + "px");
                }
                if ($element.parent().attr("data-field") == "MeinvoiceNote") {
                    $element.parent().css("line-height", lineHeight + "px");
                }
            }
        }

        if ((typeData == "label" && configCallback.isVN) || (typeData == "label-en" && !configCallback.isVN)) {
            $thisElementDataField.find(".two-dot").css("line-height", lineHeight + "px");
        }

        if (dataField == "DateInvoice" && typeData == "value") {
            var lineHeightEN = lineHeight - 1;
            $thisElementDataField.find(".edit-label-en").css("line-height", lineHeightEN + "px")
        }

        if (configCallback.isTVT() && dataField && dataField == "TotalAmountInWords" && typeData == "value") {
            var lineHeightEN = lineHeight - 1;
            $element.parent().find("#en-amount-inword").css("line-height", lineHeightEN + "px");
        }
    }

}


/**
 * Tăng giảm kích thước đi 1
 * @param {any} isDesc
 * ndanh1 30/6/2020
 */
function descOrInscFontSizeForTemplate(isDesc) {
    var $value = $(".edit-value");
    var $label = $(".edit-label");
    var $labelEn = $(".edit-label-en");
    var $symbol = $(".edit-symbol");
    var $twoDot = $(".two-dot");

    descInsc($value, isDesc);
    descInsc($label, isDesc);
    descInsc($labelEn, isDesc);
    descInsc($symbol, isDesc);
    descInsc($twoDot, isDesc);
}

/**
 * Tăng giảm font size theo list element
 * @param {any} $element
 * @param {any} isDesc
 * ndanh1 30/6/2020
 */
function descInsc($element, isDesc) {
    for (var i = 0; i < $element.length; i++) {
        var $item = $($element[i]);
        var fontSize = Number($item.css("font-size").replace("px", ""));
        if (isDesc == "true" || isDesc == true) {
            fontSize -= 1;
        } else {
            fontSize += 1;
        }

        $item.css("font-size", fontSize + "px");
    }
}

/*
* Update lại màu chữ của mẫu
* ndanh1 30/6/2020
*/
function updateColorForTemplate(color) {
    $("[data-field] span").css("color", color);
    $("[data-field] div").css("color", color);
    $("td").css("color", color);
    $("body").css("color", color);
    $("table").css("color", color);
    $(".container").css("color", color);
}

/*
* chỉnh sửa lại style cho 2 thằng mã số thuế người mua và bán có ô vuông
* ndanh1 29/6/2020
*/
function styleBorderTaxCode(field, isBorder) {
    var $parent = $("[data-field='" + field + "']"),
        $marginTaxCode = $("[data-field='" + field + "'] .margin-e-tax-code"),
        $eTaxCode = $("[data-field='" + field + "'] .e-tax-code");
    if (isBorder) {
        $eTaxCode.addClass("block-tax-code");
        $marginTaxCode.addClass("width-4");
        $parent.addClass("border-active");
    } else {
        $eTaxCode.removeClass("block-tax-code");
        $marginTaxCode.removeClass("width-4");
        $parent.removeClass("border-active");
    }
}

function setAlignmentForSellerBuyerInfor(type) {
    if (configCallback.customField == "buyer-infor") {
        setAlignmentBuyerInfor(type)
    } else {
        setAlignmentSellerInfor(type);
    }
}

/**
 * Get kiểu type alignment
 * ndanh1 25/8/2020
 * @param {any} customField
 */
function getTypeAlignment(customField) {
    var type = 1;
    var $element = $("[group-field=" + customField + "]");
    if ($element.hasClass("type-2")) {
        type = 2;
    } else if ($element.hasClass("type-3")) {
        type = 3;
    }

    return type;
}

/**
 * set vị trí ở thông tin người mua
 * @param {any} type
 * ndanh1 7/2/2020
 */
function setAlignmentBuyerInfor(type) {
    var $itemBuyer = $('[group-field=buyer-infor] [data-field]');
    var maxWidth = 0;
    resetAlignmentBuyerInfor($itemBuyer, type);
    $('[group-field=buyer-infor]').addClass('type-' + type);
    if (type == 2 || type == 3) {
        maxWidth = getMaxWidthForAlignment($itemBuyer);

        if (type == 2) {
            for (var i = 0; i < $itemBuyer.length; i++) {
                var $item = $($itemBuyer[i]);
                if ($item.parent('.merge-row-infor').length > 0) {
                    if ($item.parent('.merge-row-infor').find('[data-field]').first().attr('data-field') != $item.attr('data-field')) {
                        continue;
                    }
                }

                if ($item.find('.edit-label').text()) {
                    var width = maxWidth - getWidth($item.find('.edit-label'));
                    if (!configCallback.isVN) {
                        width = width - getWidth($item.find('.edit-label-en'));
                    }

                    $item.find('.two-dot').css('min-width', width + 'px');
                }

                $item.find('.edit-label').css('min-width', getWidth($item.find('.edit-label')) + 4 + 'px');
                $item.find('.edit-value').css('min-width', getWidth($item.find('.edit-value')) + 4 + 'px');
                $item.find('.edit-label-en').css('min-width', getWidth($item.find('.edit-label-en')) + 4 + 'px');
            }
        } else {
            for (var i = 0; i < $itemBuyer.length; i++) {
                var $item = $($itemBuyer[i]);
                if ($item.parent('.merge-row-infor').length > 0) {
                    if ($item.parent('.merge-row-infor').find('[data-field]').first().attr('data-field') != $item.attr('data-field')) {
                        continue;
                    }
                }
                var $label = $item.find('.edit-label');
                var $labelEN = $item.find('.edit-label-en');
                var $editVal = $item.find('.edit-value');
                var $twoDot = $item.find('.two-dot');

                if (configCallback.isVN) {
                    if ($label.text()) {
                        $label.css('min-width', maxWidth + 'px');
                    }
                } else {
                    // cái này cho thằng Tiếng Anh
                    if ($label.text() && $labelEN.length > 0) {
                        $label.css('min-width', getWidth($item.find('.edit-label')) + 4 + 'px');
                        $labelEN.css('min-width', maxWidth - getWidth($item.find('.edit-label')) + 4 + 'px');
                        $editVal.css('min-width', getWidth($item.find('.edit-value')) + 'px');
                        $twoDot.css('min-width', '4px');
                    }
                }
            }
        }
    }
}

/**
 * set vị trí ở thông tin người bán
 * @param {any} type
 * ndanh1 7/2/2020
 */
function setAlignmentSellerInfor(type) {
    var $itemsSeller = $('[group-field=seller-infor] [data-field]');
    var maxWidth = 0;
    resetAlignmentSellerInfor($itemsSeller, type);
    $('[group-field=seller-infor]').addClass('type-' + type);

    if (type == 2 || type == 3) {
        maxWidth = getMaxWidthForAlignment($itemsSeller);

        if (type == 2) {
            for (var i = 0; i < $itemsSeller.length; i++) {
                var $item = $($itemsSeller[i]);
                if ($item.parent('.merge-row-infor').length > 0) {
                    if ($item.parent('.merge-row-infor').find('[data-field]').first().attr('data-field') != $item.attr('data-field')) {
                        continue;
                    }
                }

                if ($item.find('.edit-label').text()) {
                    var width = maxWidth - getWidth($item.find('.edit-label'));
                    if (!configCallback.isVN) {
                        width = width - getWidth($item.find('.edit-label-en'));
                    }

                    $item.find('.two-dot').css('min-width', width + 'px');
                }

                $item.find('.edit-label').css('min-width', getWidth($item.find('.edit-label')) + 4 + 'px');
                $item.find('.edit-value').css('min-width', getWidth($item.find('.edit-value')) + 4 + 'px');
                $item.find('.edit-label-en').css('min-width', getWidth($item.find('.edit-label-en')) + 4 + 'px');
            }
        } else {
            for (var i = 0; i < $itemsSeller.length; i++) {
                var $item = $($itemsSeller[i]);
                if ($item.parent('.merge-row-infor').length > 0) {
                    if ($item.parent('.merge-row-infor').find('[data-field]').first().attr('data-field') != $item.attr('data-field')) {
                        continue;
                    }
                }
                var $label = $item.find('.edit-label');
                var $labelEN = $item.find('.edit-label-en');
                var $editVal = $item.find('.edit-value');
                var $twoDot = $item.find('.two-dot');

                if (configCallback.isVN) {
                    if ($label.text()) {
                        $label.css('min-width', maxWidth + 'px');
                    }
                } else {
                    // cái này cho thằng Tiếng Anh
                    if ($label.text() && $labelEN.length > 0) {
                        $label.css('min-width', getWidth($item.find('.edit-label')) + 4 + 'px');
                        $labelEN.css('min-width', maxWidth - getWidth($item.find('.edit-label')) + 4 + 'px');
                        $editVal.css('min-width', getWidth($item.find('.edit-value')) + 'px');
                        $twoDot.css('min-width', '4px');
                    }
                }
            }
        }
    }

    // reload lại custom seller
    reloadTypeCustomSeller();
}

/**
 * Get max width label
 * @param {any} $itemField
 * ndanh1 25/8/2020
 */
function getMaxWidthForAlignment($itemField) {
    var maxWidth = 0;
    for (var i = 0; i < $itemField.length; i++) {
        var $item = $($itemField[i]);
        if ($item.parent(".merge-row-infor").length > 0) {
            if ($item.parent(".merge-row-infor").find("[data-field]").first().attr("data-field") != $item.attr("data-field")) {
                continue;
            }
        }

        var widthLabel = getWidth($item.find(".edit-label"));
        if (!configCallback.isVN) {
            widthLabel = widthLabel + getWidth($item.find(".edit-label-en"));
        }

        if (widthLabel > maxWidth) {
            maxWidth = widthLabel;
        }
    }

    if (getPositionSeller() == 2) {
        var maxSeller = getMaxWidthOfBuyerAndSeller(maxWidth, "seller-infor");
        var maxBuyer = getMaxWidthOfBuyerAndSeller(maxWidth, "buyer-infor");
        if (!$("[group-field=" + "buyer-infor" + "]").hasClass("type-3") && !$("[group-field=" + "buyer-infor" + "]").hasClass("type-2")) {
            maxSeller += 12;
        }
        if (!$("[group-field=" + "seller-infor" + "]").hasClass("type-3") && !$("[group-field=" + "seller-infor" + "]").hasClass("type-2")) {
            maxBuyer += 12;
        }
        if ($("[group-field=" + "seller-infor" + "]").hasClass("type-3") && $("[group-field=" + "buyer-infor" + "]").hasClass("type-3")) {
            maxBuyer -= 8;
            maxSeller -= 8;
        }
        if ($("[group-field=" + "seller-infor" + "]").hasClass("type-2") && $("[group-field=" + "buyer-infor" + "]").hasClass("type-2")) {
            maxBuyer -= 8;
            maxSeller -= 8;
        }
        if ($("[group-field=" + "seller-infor" + "]").hasClass("type-2") && $("[group-field=" + "buyer-infor" + "]").hasClass("type-3")) {
            maxBuyer -= 8;
            maxSeller -= 8;
        }
        if ($("[group-field=" + "seller-infor" + "]").hasClass("type-3") && $("[group-field=" + "buyer-infor" + "]").hasClass("type-2")) {
            maxBuyer -= 8;
            maxSeller -= 8;
        }
        maxWidth = maxSeller > maxBuyer ? maxSeller : maxBuyer;
    }

    return maxWidth;
}

/**
 * add 2 chấm cho label
 * @param {any} customField
 */
function addTwoDotForLabelInfor(customField) {
    var field = "seller-infor";
    if (customField == "seller-infor") {
        field = "buyer-infor";
    }

    var $itemField = $("[group-field=" + field + "] [data-field]");
    for (var i = 0; i < $itemField.length; i++) {
        var $item = $($itemField[i]);
        if (configCallback.isVN) {
            if ($item.find(".edit-label").text()) {
                $item.find(".edit-label").text($item.find(".edit-label").text() + ":")
            }
        } else {
            if ($item.find(".edit-label").text()) {
                $item.find(".edit-label-en").text($item.find(".edit-label-en").text() + ":")
            }
        }
    }
}

/**
 * get cái max width khi làm thông tin người bán, người mua thẳng hàng
 * @param {any} customField
 * ndanh1 25/8/2020
 */
function getMaxWidthOfBuyerAndSeller(maxWidth, customField) {
    var realField = "seller-infor";
    if (customField == "seller-infor") {
        realField = "buyer-infor";
    }

    var $itemField = $("[group-field=" + realField + "] [data-field]");

    for (var i = 0; i < $itemField.length; i++) {
        var $item = $($itemField[i]);
        //$item.find(".edit-label").text($item.find(".edit-label").text().replace(":", ""));
        var widthLabel = getWidth($item.find(".edit-label"));
        if (!configCallback.isVN) {
            //$item.find(".edit-label-en").text($item.find(".edit-label-en").text().replace(":", ""))
            widthLabel = widthLabel + getWidth($item.find(".edit-label-en"));
            widthLabel = widthLabel + getWidth($item.find(".two-dot"));
        }

        if (widthLabel > maxWidth) {
            maxWidth = widthLabel;
        }
    }

    return Math.round(maxWidth);
}

/**
 * reset thằng align của buyer infor
 * @param {any} $itemBuyer
 * 7/1/2020
 */
function resetAlignmentBuyerInfor($itemBuyer, type, config) {
    if (!config) {
        $('[group-field=buyer-infor]').removeClass('type-1');
        $('[group-field=buyer-infor]').removeClass('type-2');
        $('[group-field=buyer-infor]').removeClass('type-3');
    }

    for (var i = 0; i < $itemBuyer.length; i++) {
        var $item = $($itemBuyer[i]);
        $item.find('.two-dot').css('min-width', 'unset');
        $item.find('.edit-label').css('min-width', '0px');
        $item.find('.edit-label-en').css('min-width', '0px');
    }
}

/**
 * reset thằng align của seller infor
 * @param {any} $itemsSeller
 * 7/1/2020
 */
function resetAlignmentSellerInfor($itemsSeller, type, config) {
    if (!config) {
        $('[group-field=seller-infor]').removeClass('type-1');
        $('[group-field=seller-infor]').removeClass('type-2');
        $('[group-field=seller-infor]').removeClass('type-3');
    }

    for (var i = 0; i < $itemsSeller.length; i++) {
        var $item = $($itemsSeller[i]);
        $item.find('.two-dot').css('min-width', 'unset');
        $item.find('.edit-label').css('min-width', '0px');
        $item.find('.edit-label-en').css('min-width', '0px');
    }
}

/**
 * Lấy width của phần tử
 * @param {any} $item
 * ndanh1 7/1/2020
 */
function getWidth($item) {
    if (!$item) {
        return 0;
    } else if ($item.css("display") == "none") {
        return 0;
    } else if ($item.length == 0) {
        return 0;
    }
    return Math.round($item.innerWidth());
}

//Set opcity cho thằng ảnh
//vmthanh 09/07/2020
function setStyleBackgroundByUser(opacity) {
    $('.bg-template-parent').css('height', '50%');
    $('.bg-template-parent').css('width', '85%');
    $('.bg-template-parent .bg-template').css('opacity', parseInt(opacity) / 100);
}

/**
 * set background cho mẫu
 * @param {any} image
 * ndanh1 7/1/2020
 */
function setBackgroundForTemplate(image, sizeImg) {
    if (!configCallback.isTVT()) {
        if (image) {
            $(".bg-template").css("background-image", "url(" + image + ")");
        } else {
            $(".bg-template").css("background-image", "none");
        }
        if (image == '') {
            $('.bg-template-parent').removeClass('highlight-logo');
        }
        else {
            resizeableBackground(false);
            initEventResizeDraggableBackground();
        }
        if (sizeImg) {
            var sizeImgArray = JSON.parse(sizeImg);
            configCallback.cur_WIDTH_BG = sizeImgArray[0];
            configCallback.cur_HEIGHT_BG = sizeImgArray[1];
            $('.bg-template-parent').css('width', sizeImgArray[0]);
            $('.bg-template-parent').css('height', sizeImgArray[1]);
        }
    }

}

//Set style cho thằng ảnh nền có sẵn
//vmthanh - 22/07/2020
function setStyleForBackgroundAvailabel() {
    $('.bg-template-default').css('width', '500px');
    $('.bg-template-default').css('height', '500px');
    $('.bg-template-default').css('left', '185px');
    if (configCallback.isA5()) {
        $('.bg-template-default').css('top', '48px');
        $('.bg-template-default').css('left', '175px');
    }
    else {
        $('.bg-template-default').css('top', '368px');
        $('.bg-template-default').css('left', '185px');
    }
    //$('.bg-template-default .bg-default').css('opacity', '1');
}
/**
 * set khung hình cho mẫu
 * @param {any} image
 * ndanh1 7/1/2020
 */
function setFrameForTemplate(image) {
    $(".bg-template-default").css("background-image", "url(" + image + ")");
}

/**
 * Set kiểu hiển thị trường mở rộng seller
 * @param {any} dataField
 * @param {any} type
 * ndanh1 1/7/2020
 */
function changeTypeCustomData(dataField, type) {
    var $element = $("[data-field=" + dataField + "]"),
        $value = $element.find(".edit-value"),
        $twoDot = $element.find(".two-dot"),
        $label = $element.find(".edit-label");
    $labelEN = $element.find(".edit-label-en");

    //reset về ban đầu
    resetTypeCustomData($element, $value, $label, $labelEN, $twoDot);

    // add class theo type tương ứng
    $element.addClass("type-" + type);

    var typeSeller = 1;
    if ($("[group-field=seller-infor]").hasClass("type-2")) {
        typeSeller = 2;
    } else if ($("[group-field=seller-infor]").hasClass("type-3")) {
        typeSeller = 3;
    }

    if (type != 1) {
        $label.addClass("display-none");
        $value.addClass("padding-none");
        $twoDot.addClass("display-none");
        if (!configCallback.isVN) {
            $labelEN.addClass("display-none");
        }
        if (type == 3) {
            var width = 0;
            var $prevElement = $element.prev();

            // tìm element gần nó nhất và đang hiển thị để căn width theo nó
            while ($prevElement.prev()) {
                if (!$prevElement.hasClass("display-none")) {
                    width = getWidth($prevElement.find(".edit-label")) + getWidth($prevElement.find(".empty-div"))
                        + getWidth($prevElement.find(".edit-label-en")) + getWidth($prevElement.find(".two-dot"));
                    if (!configCallback.isVN && $prevElement.length > 0 && !$prevElement.find(".edit-label").hasClass("display-none")) {
                        width = width + 4;
                    }
                    break;
                }

                $prevElement = $prevElement.prev()
            }

            var emptyDiv = "<div class='empty-div display-table-cell white-space-nowrap'></div>";
            $value.before(emptyDiv);
            $element.find(".empty-div").css("width", width + "px");
            if (width > 0) {
                $value.removeClass("padding-none");
            } else {
                $value.addClass("padding-none");
            }
        }
    } else {
        if ($label.text()) {
            $value.removeClass('padding-none');
        } else {
            $value.addClass('padding-none');
        }
    }
}

/**
 * reload lại width của table invoice infor
 * ndanh1 29/7/2020
 * */
function reloadWidthTableInvoiceInfor() {
    resizeTable("#invoiceInfor", true);
    var rates = getRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"));
    resetWidthByRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"), rates);
    resizeTable("#invoiceInfor", false, true);
}

/**
 * Thay đổi vị trí của thông tin đơn vị
 * ndanh1 9/7/2020
 * */
function changePositionSellerInfor(position) {
    var positionNow = getPositionSeller();
    var widthLogo = $(".item-logo:not(.display-none)").css("width");
    $("#invoiceInfor td").first().removeClass("display-none");

    var isBLP = configCallback.isBLP();
    if (position == 1) {
        $(".header-invoice").append($(".title-invoice-seires"));
        $(".header-content-invoice").append($(".seller-infor"));
        $(".header-invoice").css("border-bottom", "");

        if (isBLP) {
            $(".receipt-title-container").append($("[group-field='receipt-title']"));
            $(".logo-right").parents("td").first().before($("[group-field='other-invoice']").parents("td").first());
            $("#invoiceInfor td").css("width", "unset");
            $("#firstInvoice td").css("width", "unset");
            $("[group-field='receipt-title']").parents("td").first().css("width", "310px");
        }

    } else {
        $(".header-content-invoice").append($(".title-invoice-seires"));
        $(".header-invoice").prepend($(".seller-infor"));

        if (positionNow != 2 && positionNow != 3) {
            if (isBLP) {
                $("#invoiceInfor td").last().append($("[group-field='receipt-title']").first());
                $("#invoiceInfor tr").last().append($("[group-field='other-invoice']").parents("td").first());
                $(".receipt-title-container").css("width", "0px");
                $("#invoiceInfor td").css("width", "unset");
            }
        }

        if (position == 3) {
            $("#invoiceInfor td").first().append($(".seller-infor"));
        } else {
            if (isBLP) {
                $("#invoiceInfor td").first().addClass("display-none");
                //if (positionNow == 3) {
                //    $(".header-invoice").append($(".seller-infor"));
                //}
            }
        }

        if (position == 3) {
            $(".header-invoice").css("border-bottom", "none");
        }
        else {
            $(".header-invoice").css("border-bottom", "");
        }

        resetWidthOfTableInvoiceInfor();
    }

    if (!isHasLogo()) {
        $(".item-logo").css("width", "0px");
    } else {
        $(".item-logo:not(.display-none)").css("width", widthLogo);
        $(".item-logo.display-none").css("width", "0px");
    }

    if (getPositionSeller() == 2) {
        if (getTypeAlignment("buyer-infor") == getTypeAlignment("seller-infor")) {
            setAlignmentBuyerInfor(getTypeAlignment("buyer-infor"));
            setAlignmentSellerInfor(getTypeAlignment("buyer-infor"));
        }
    } else {
        setAlignmentBuyerInfor(getTypeAlignment("buyer-infor"));
        setAlignmentSellerInfor(getTypeAlignment("seller-infor"));
    }

    $("#firstInvoice").addClass("bg-picked-by-user");
    $("#invoiceInfor").addClass("bg-picked-by-user");
}

function addEventResizeAfterChangePositionSeller() {
    resizeTable("#invoiceInfor", false, true);
    resizeTable("#firstInvoice", false, true);
}

/**
 * lấy tỷ lệ width từng cột so với cả bảng
 * ndanh1 29/7/2020
 * */
function getRateColumnWidth($table, columns) {
    var rates = [];
    var widthTable = $table.width();
    for (var i = 0; i < columns.length; i++) {
        var $column = $(columns[i]);
        var rate = ($column.width()) / widthTable;
        rates.push(rate);
    }
    return rates;
}

/**
 * Tính lại width theo tỷ lệ
 * ndanh1 29/7/2020
 * */
function resetWidthByRateColumnWidth($table, columns, rates, newWidth) {
    var widthTable = $table.width();
    if (newWidth) {
        widthTable = newWidth;
    }
    for (var i = 0; i < columns.length; i++) {
        var $column = $(columns[i]);
        $column.css("width", (widthTable * rates[i]) + "px");
    }
}

/*
* hiển thị logo theo cấu hình nsd
* ttanh(06/11/2019)
*/
function UpLoadLogoForPreview(imgLogo, position) {
    if (configCallback.isTVT()) {
        sortaleByField("InvoiceNumber", "InvoiceSeries", true, false, false);
    }
    resizeTable("#invoiceInfor", true);
    var rates = getRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"));
    $(".logo-template-content").css("background-image", "url(" + imgLogo + ")");
    $(".logo-template-content.logo-left").parents("td").css("width", ($(".logo-template-content").parents("td").width() + 10) + "px");
    ShowLogoByPosition(position);
    resizeableLogo();
    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    resetWidthByRateColumnWidth($("#invoiceInfor"), $("#invoiceInfor").find("tr:first-child .item-invoice-infor"), rates);
    resizeTable("#invoiceInfor", false, true);
    resizeTable("#invoiceInfor", true);
    initEventResizeDraggableLogo();
    if (configCallback.isTVT()) {
        resetWithInvoiceSeries();
    }
    //Chỉnh lại độ rộng với máy tính tiền
    changePositionQRCodeCashRegister();

}

/*
* chỉnh lại chiều cao rộng của logo
* ttanh(06/11/2019)
*/
function ShowLogoByPosition(position) {
    //$(".logo-template-content").css("display", "none");
    var myLogo = $($(".logo-template-content")[0]),
        width = 150, height = width / configCallback.rateLogo;
    if (height > 100) {
        height = 100;
        width = height * configCallback.rateLogo;
    }
    if (position == POSITION_LOGO.Right) {
        myLogo = $($(".logo-template-content")[1]);
    }

    if (configCallback.isTVT()) {
        var positionQR = checkPositionQRTicket();
        if (positionQR == 0) {
            width = 65
            height = 65
        }
    }

    if (configCallback.isBLPHasFeeK80()) {
        changeStyleForInvoiceSeries(position);
    }

    parentMyLogo = myLogo.parents('.table-header-item');
    myLogo.css("width", width);
    myLogo.css("height", height);
    myLogo.css("display", "block");
    myLogo.addClass('highlight-logo');
    $('.item-logo').addClass("display-none");
    parentMyLogo.removeClass("display-none");
    parentMyLogo.css("width", width);
}

// Hàm chuyển đổi svg sang base64
// vmthanh - 02/07/2020
function getBase64FrameBackground() {
    var svgElement = $('svg');
    var svgString = new XMLSerializer().serializeToString(svgElement);
    var decoded = unescape(encodeURIComponent(svgString));
    var base64 = btoa(decoded);
    var imgSource = `data:image/svg+xml;base64,${base64}`;
    return imgSource;
}

// Hàm thêm svg để preview trên mẫu
// vmthanh - 03/07/2020
function addFrameTemplate(svgContent, tag, isAvailabelImage) {
    if (tag == 'frame') {
        $('.frame-template svg').remove();
        $('.frame-template').css("background-image", "none");
        if (svgContent != '') {
            $('.frame-template').append(svgContent);
        }
    } else if (tag == "background-default") {
        $('.bg-template-default .bg-default svg').remove();
        $('.bg-template-default .bg-default').css("background-image", "none");
        if (svgContent != '') {
            $('#delete-bg').trigger('click');
            $('.bg-template-default .bg-default').append(svgContent);
            if (isAvailabelImage) {
                $('.bg-template-default .bg-default svg').css('height', 'inherit');
                $('.bg-template-default .bg-default svg').css('width', 'inherit');
            }
        }
    }
}

// Thay đổi màu viền
// vmthanh - 03/07/2020
function setColorFrameTemplate(color, tag) {
    if (tag == 'frame') {
        $('.frame-template svg').css('fill', color);
    }
    else {
        $('.bg-template-default .bg-default svg').css('fill', color);
    }
}

//Lấy content svg để bind vào thằng bên detail
//vmthanh - 06/07/2020
function getContentSvg(tag) {
    var content = '';
    if (tag == 'frame') {
        content = $('.frame-template').html();
    }
    else {
        content = $('.bg-template-default .bg-default').html();
    }
    configCallback.previewImagePickedByUser(content, tag);
}

//Set size và position cho hình nền
//vmthanh - 06/07/2020
function ShowBackgroundByPosition(position) {
    var mybg = $(".bg-template-parent"),
        heightBG = configCallback.cur_HEIGHT_BG,
        widthBG = configCallback.cur_WIDTH_BG,
        rateTemplateA5 = 1.405607476635514;
    var rateBG = configCallback.rateBackground;
    if (configCallback.isA4N()) {
        MAX_HEIGHT_BG = 865;
        MAX_WIDTH_BG = 1224.0840336134454;
        rateBG = MAX_WIDTH_BG / MAX_HEIGHT_BG;
    }
    if (configCallback.isA5()) {
        MAX_HEIGHT_BG = 585;
        MAX_WIDTH_BG = 830;
        rateBG = MAX_WIDTH_BG / MAX_HEIGHT_BG;
    }
    switch (position) {
        case POSITION_BACKGROUND.FULL://toàn màn hình
            mybg.css("width", "100%");
            widthBG = mybg.width();
            heightBG = widthBG / rateBG;
            if (heightBG > MAX_HEIGHT_BG) {
                heightBG = MAX_HEIGHT_BG;
                //set up lại chiều rộng ảnh
                widthBG = heightBG * rateBG;
                mybg.css("width", widthBG);
            }
            mybg.css("height", heightBG);
            //chỉnh lại pos
            var top = "calc(50% - " + heightBG / 2 + "px)";
            mybg.css("top", top);
            mybg.css("left", "0");
            if (configCallback.isA5()) {
                $(".bg-template-parent").css("top", "0px")
            } else if (configCallback.isA4N()) {
                $(".bg-template-parent").css("top", "0px");
            }
            break;
        case POSITION_BACKGROUND.CENTER://giữa hóa đơn
            if (widthBG > MAX_WIDTH_BG) {
                widthBG = MAX_WIDTH_BG;
            }
            //set up lại chiều cao ảnh
            heightBG = widthBG / rateBG;
            if (heightBG > MAX_HEIGHT_BG) {
                heightBG = MAX_HEIGHT_BG;
                //set up lại chiều rộng ảnh
                widthBG = heightBG * rateBG;
            }
            mybg.css("width", widthBG);
            mybg.css("height", heightBG);
            mybg.css("top", ((MAX_HEIGHT_BG - heightBG) / 2) + "px");
            mybg.css("left", ((MAX_WIDTH_BG - widthBG) / 2) + "px");
            if (configCallback.isA5() || configCallback.isA4N()) {
                $(".bg-template-parent").css("top", "calc(50% - " + (heightBG / 2) + "px)");
            }
            break;
    };
    if (configCallback.isA5()) {
        if ($(".bg-template-parent").height() > 597) {
            $(".bg-template-parent").height(597);
        }
    }
    mybg.addClass('highlight-logo');
}

//Sự kiện resize ảnh logo và di chuyển ảnh mẫu
//created by vmthanh 07/07/2020
function resizeableBackground(isDefault) {
    if (!configCallback.isTVT()) {
        initDraggableAndResizableBackground(isDefault);
        var itemResize = $(".bg-template-parent .ui-icon-gripsmall-diagonal-se"),
            itemResizeParent = $(".bg-template-parent");
        if ($('.bg-template-parent .bg-template').css('background-image').includes('base64')) {
            itemResize.addClass('resizable');
            itemResizeParent.addClass('highlight-logo');
        }
        if (!isDefault) {
            if ($('.bg-template-default svg').length > 0) {
                $(".bg-template-default .ui-icon-gripsmall-diagonal-se").addClass('resizable');
                $(".bg-template-default").addClass('highlight-logo');
            }
        }
    }

}

//Sự kiện resize ảnh logo và di chuyển ảnh mẫu
//vmthanh(08/07/2020)
function resizeableLogo() {
    initDraggableAndResizableLogo();
    var itemResize = $(".item-logo .ui-icon-gripsmall-diagonal-se"),
        itemResizeParent = $('.logo-template-content');
    itemResize.addClass('resizable');
    itemResizeParent.addClass('highlight-logo');
}

/*
* add sự kiện kéo size và chuyển vị trí cho logo
* vmthanh(08/07/2020)
*/
initDraggableAndResizableLogo = function () {
    $(".logo-template-content").resizable({
        maxWidth: MAX_WIDTH_LOGO,
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
            setTimeout(function () {
                $(event.target).data("dragging", false);
            }, 1);
        }
    });
    $(".logo-template-content").draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target),
                $parentTemp = $target.parents(".logo-template"),
                $parent = $target.parents("td"),
                maxBottom = $parent.offset().top + $parent.height(),
                //Vi tri thực của bottom so với thẻ td phải cộng thêm top của thẻ div cha của logo vì vị trí của logo theo thẻ div template
                nowBottom = $parentTemp.offset().top + ui.position.top + $target.height(),
                positionLogo = configCallback.getPositionLogo(), widthTD;
            //nếu là góc trái
            if (positionLogo == 1) {
                widthTD = $($(".item-logo")[0]).width();
            } else {
                widthTD = $($(".item-logo")[1]).width();
            }
            if ($target.width() + ui.position.left >= widthTD) {
                ui.position.left = widthTD - $target.width();
            }
            if (ui.position.left < 0) {
                ui.position.left = 0;
            }

            if (($parentTemp.offset().top + ui.position.top) < $parent.offset().top) {
                //ui.position.top = (($parentTemp.offset().top + ui.position.top) - $parent.offset().top);
                ui.position.top = ($parent.offset().top - $parentTemp.offset().top);
            } else if (nowBottom > maxBottom) {
                ui.position.top = maxBottom - $target.height() - $parentTemp.offset().top;
            }



            //if ((Math.abs(ui.position.top) + ($target.height() / 2)) > 80) {
            //    //if (ui.position.top <= 0) {
            //    //    //ui.position.top = ($target.height() / 2) - 80;
            //    //    ui.position.top = 0;
            //    //} else {
            //    //    ui.position.top = 80 - ($target.height() / 2);
            //    //}

            //    if (ui.position.top < 0) {
            //        ui.position.top = ($target.height() / 2) - 80;
            //    } else {
            //        ui.position.top = 80 - ($target.height() / 2);
            //    }
            //}

        },
    });
}

//Sự kiện resize ảnh logo và di chuyển ảnh mẫu
//created by vmthanh 07/07/2020
function UpLoadBackgroundForPreview(imgBackground, position) {
    $(".bg-template").css("background-image", "url(" + imgBackground + ")");
    $(".bg-template-parent").removeClass("display-none");
    $(".bg-template-parent").css("transform", "none");
    $("#rotate").show();
    ShowBackgroundByPosition(position);
    resizeableBackground(false);
    initEventResizeDraggableBackground('parent');
    $(".bg-template").click();

}


//Sự kiện resize ảnh nền và di chuyển ảnh mẫu
//created by vmthanh 07/07/2020
function initDraggableAndResizableBackground(isDefault) {
    var $elementResize = $(".bg-template-parent");
    if (isDefault) {
        $elementResize = $(".bg-template-default");
    }
    else {
        $elementResizeOther = $(".bg-template-default");
        $elementResizeOther.resizable({
            maxWidth: MAX_WIDTH_BG,
        });
        $elementResizeOther.draggable({
            start: function (event, ui) {
                $(this).data("dragging", true);
            },
            stop: function (event, ui) {
                setTimeout(function () {
                    $(event.target).data("dragging", false);
                }, 1);
            },
            drag: function (event, ui) {
                var $target = $(event.target), // cái ảnh nền
                    $ifr = $('.container').parent().parent(),
                    bodyIfr = $("body"),
                    matrix = /matrix\((-?\d*\.?\d+),\s*0,\s*0,\s*(-?\d*\.?\d+),\s*0,\s*0\)/,
                    maxWidth = 0, maxHeight = 0;

                lstScale = bodyIfr.css("transform").match(matrix);
                if (lstScale.length > 1) {
                    curScale = parseFloat(lstScale[1])//0.87
                    maxWidth = $ifr.width() / curScale;
                    maxHeight = $ifr.height() / curScale;
                }

                if (maxWidth > 0 && maxHeight > 0) {
                    //kéo ngang
                    if (ui.position.left < 0) {
                        ui.position.left = 0;
                    }
                    if ($target.width() + ui.position.left >= maxWidth) {
                        ui.position.left = maxWidth - $target.width();
                    }
                    //kéo dọc
                    if (ui.position.top < 0) {
                        ui.position.top = 0;
                    }
                    if ($target.height() + ui.position.top >= maxHeight) {
                        ui.position.top = maxHeight - $target.height();
                    }
                }
            },
        });
        $elementResizeOther.css('z-index', '999');
    }
    $elementResize.resizable({
        maxWidth: MAX_WIDTH_BG,
    });
    $elementResize.draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
            setTimeout(function () {
                $(event.target).data("dragging", false);
            }, 1);
        },
        drag: function (event, ui) {
            var $target = $(event.target), // cái ảnh nền
                $ifr = $('.container').parent().parent(),
                bodyIfr = $("body"),
                matrix = /matrix\((-?\d*\.?\d+),\s*0,\s*0,\s*(-?\d*\.?\d+),\s*0,\s*0\)/,
                maxWidth = 0, maxHeight = 0;

            lstScale = bodyIfr.css("transform").match(matrix);
            if (lstScale.length > 1) {
                curScale = parseFloat(lstScale[1])//0.87
                maxWidth = $ifr.width() / curScale;
                maxHeight = $ifr.height() / curScale;
            }

            if (maxWidth > 0 && maxHeight > 0) {
                //kéo ngang
                if (ui.position.left < 0) {
                    ui.position.left = 0;
                }
                if ($target.width() + ui.position.left >= maxWidth) {
                    ui.position.left = maxWidth - $target.width();
                }
                //kéo dọc
                if (ui.position.top < 0) {
                    ui.position.top = 0;
                }
                if ($target.height() + ui.position.top >= maxHeight) {
                    ui.position.top = maxHeight - $target.height();
                }
            }
        },
    });
    $elementResize.css('z-index', '999');
}

/*
* event click delete logo
* vmthanh(09/07/2020)
*/
function DeleteLogo() {
    var myLogo = $(".logo-template-content");
    myLogo.css("width", 0);
    myLogo.css("height", 0);
    myLogo.css("background-image", "none");
    myLogo.css("display", "none");
    $(".header-content-invoice").css("width", "100%")
    $(".logo-template-content").parents("td").css("width", "0px");
    // Xử lý cho tem vé thẻ

    if (configCallback.isTVT() && $("[data-field=TemplateCode]").parents(".merge-row-infor").length == 0) {
        $("[merge-field=MergeFied_0]").attr('style', '');
        var itemMerge = {
            listItem: ["InvoiceNumber"],
            listMerge: [],
        }
        //mergeRowInfor(itemMerge);
        // Nếu là song ngữ thì hiển thị theo chiều dọc
        var $invSeri = $($("[data-field=InvoiceSeries]")[0]);
        if (!configCallback.isVN) {
            $invSeri.parent().css("flex-direction", "column");
            $invSeri.parent().attr("typeShow", "column");
        }
        else {
            if (checkPositionQRTicket() == 0) {
                $invSeri.parent().css("flex-direction", "column");
                $invSeri.parent().attr("typeShow", "column");
            }
            else {
                $invSeri.parent().attr("typeShow", "row");
            }

        }
    }

    if (configCallback.isBLPHasFeeK80()) {
        changeStyleForInvoiceSeries(POSITION_LOGO.Left);
    }
    //Chỉnh lại độ rộng với máy tính tiền
    changePositionQRCodeCashRegister();
}

//Set opacity cho thằng bg
//created by vmthanh 07/07/2020
function setOpacityForBackground(opacity, bgDefault = false) {
    if (bgDefault) {
        $(".bg-template-default .bg-default").css("opacity", opacity);
    } else {
        $(".bg-template-parent .bg-template").css("opacity", opacity);
    }
}

/**
 * reset về ban đầu
 * @param {any} $element
 * @param {any} $value
 * @param {any} $label
 * ndanh1 2/7/2020
 */
function resetTypeCustomData($element, $value, $label, $labelEN, $twoDot) {
    $label.removeClass("display-none");
    if (!configCallback.isVN) {
        $labelEN.removeClass("display-none");
    }
    $value.removeClass("padding-none");
    $twoDot.removeClass("display-none");
    $value.css("margin-left", "0px");
    $element.removeClass("type-1");
    $element.removeClass("type-2");
    $element.removeClass("type-3");
    if ($element.find(".empty-div")) {
        $element.find(".empty-div").remove();
    }
}

/**
 * set background color cho header
 * @param {any} color
 * @param {any} type
 * ndanh1 7/2/2020
 */
function updateBackgroundForHeaderTable(color, dataField, type) {
    if (!color) {
        color = "transparent";
    }
    if ($("[data-field=" + dataField + "]").parent().hasClass("tr-footer-header")) {
        $(".tr-footer-header td").css("background-color", color)
    }
    else if (type == "symbol") {
        $(".tr-symbol td").css("background-color", color)
    } else {
        $(".tr-header td").css("background-color", color)
    }
}

/**
 * resize element
 * ndanh1 16/7/2020
 * */
function resizeElementInfor(mergeField) {
    var $lstElement = $(".merge-row-infor[merge-field=" + mergeField + "] [data-field]");
    $(".merge-row-infor[merge-field=" + mergeField + "] [data-field].flex-1").removeClass("flex-1");

    if (!configCallback.isBLPHaveFee()) {
        $(".merge-row-infor[merge-field=" + mergeField + "] [data-field]").last().addClass("flex-1");
    }
    var widthParent = $(".merge-row-infor[merge-field=" + mergeField + "]").width();
    for (var i = 0; i < $lstElement.length - 1; i++) {
        var $ele = $($lstElement[i]);
        var $prev = $ele.prev();
        var minWidth = getTotalWidthChild($ele.children());
        var maxWidth = getMaxWidth($lstElement, $ele.attr("data-field"), widthParent);
        $ele.resizable({
            handles: 'e',
            maxWidth: maxWidth,
            start: function (event, ui) {
                $prev = $(event.target).prev();
                if ($prev.length > 0) {
                    widthPrev = $prev.width();
                }
            },
            stop: function (event, ui) {
                var listElement = $(".merge-row-infor[merge-field=" + mergeField + "] [data-field]");
                var totalwidth = 0;
                //BXHIEU 15/08/2022 không dùng width thẳng như cũ lúc gen ra bị lỗi 567362
                for (var i = 0; i < listElement.length - 1; i++) {
                    totalwidth += $($(".merge-row-infor[merge-field=" + mergeField + "] [data-field]")[i]).width();
                }
                $(".merge-row-infor[merge-field=" + mergeField + "] [data-field]").last().css("width", "calc(100% - " + totalwidth + "px)");
            },
            resize: function (event, ui) {

            }
        });
    }
}

function getMaxWidth($lstElement, dataField, maxWidth) {
    var max = maxWidth;
    for (var i = 0; i < $lstElement.length; i++) {
        var $element = $($lstElement[i]);
        if ($element.attr("data-field") == dataField) {
            continue;
        }
        if ($element.attr("data-field") == "DeliveryOrderDate") {
            if ($element.children().length > 0) {
                max = max - getTotalWidthChild($element.children().children());
            }
            else {
                max = max - getTotalWidthChild($element.children());
            }
        }
        else {
            max = max - getTotalWidthChild($element.children());
        }
    }

    return max;
}

function getTotalWidthChild($element) {
    var children = $element;
    var totalWidth = 0;
    for (var i = 0; i < children.length; i++) {
        totalWidth += children[i].offsetWidth;
    }

    return totalWidth;
}

/**
 * Thêm cột mới cho column
 * @param {any} color
 * @param {any} type
 * ndanh1 7/2/2020
 */
function addNewColumnInTable(customField, isShow) {
    var curDiv = $("[data-field=" + customField.dataField + "]");
    resizeTable("#tbDetail", true);
    var widthTableNow = $("#tbDetail").width();
    if (!isShow) {
        curDiv.remove();
    } else {
        var htmlCustomTableLabel = `<td class="text-center custom-table" data-field="{0}" style="width: 100px;" rowspan="{3}">
                                    <div class="edit-label none-edit">{1}</div>
                                    <div class="edit-label-en none-edit display-none">{2}</div>
                                </td>`;
        var htmlCustomTableValue = `<td class="text-left" data-field="{0}">
                                    <div class="edit-value">{1}</div>
                                </td>`;
        if (customField.isNumber) {
            htmlCustomTableValue = `<td class="text-left" data-field="{0}">
                                    <div class="edit-value" style="text-align: right;">{1}</div>
                                </td>`;
        }
        var htmlCustomTableSymbol = `<td class="text-center" data-field="{0}">
                                    <div class="edit-symbol font-bold">{1}</div>
                                </td>`;
        var htmlCustomTableFooter = `<td class="text-center" data-field="{0}"></td>`;
        var dataValue = customField.value;
        var dataLabel = customField.label;
        var dataField = customField.dataField;
        var dataLabelEn = customField.labelEN;

        htmlCustomTableLabel = htmlCustomTableLabel.format(dataField, dataLabel, dataLabelEn, $(".tr-header").length);
        htmlCustomTableValue = htmlCustomTableValue.format(dataField, dataValue);
        htmlCustomTableSymbol = htmlCustomTableSymbol.format(dataField, customField.symbol);
        htmlCustomTableFooter = htmlCustomTableFooter.format(dataField);
        if ($(".tr-header").find("[data-field=" + customField.elementAppend + "]").length > 0) {
            if (customField.isDown) {
                $(".tr-header").first().find("[data-field=" + customField.elementAppend + "]").after(htmlCustomTableLabel);
                $(".tr-data-detail").find("[data-field=" + customField.elementAppend + "]").after(htmlCustomTableValue);
                $(".tr-symbol").find("[data-field=" + customField.elementAppend + "]").after(htmlCustomTableSymbol);
                $(".tr-of-footer").find("[data-field=" + customField.elementAppend + "]").after(htmlCustomTableFooter);
            } else {
                $(".tr-header").find("[data-field=" + customField.elementAppend + "]").before(htmlCustomTableLabel);
                $(".tr-data-detail").find("[data-field=" + customField.elementAppend + "]").before(htmlCustomTableValue);
                $(".tr-symbol").find("[data-field=" + customField.elementAppend + "]").before(htmlCustomTableSymbol);
                $(".tr-of-footer").find("[data-field=" + customField.elementAppend + "]").before(htmlCustomTableFooter);
            }
        } else {
            $(".tr-header").first().append(htmlCustomTableLabel);
            $(".tr-data-detail").append(htmlCustomTableValue);
            $(".tr-symbol").append(htmlCustomTableSymbol);
            $(".tr-of-footer").append(htmlCustomTableFooter);
        }

        $(".tr-header [data-field=" + dataField + "] .edit-label").parent().css("background-color", $("#tbDetail .tr-header [data-field]:visible:not(.element-active) .edit-label").first().parent().css("background-color"));
        $(".tr-symbol [data-field=" + dataField + "]").css("background-color", $("#tbDetail .tr-symbol [data-field]:visible:not(.element-active)").first().css("background-color"));
        if (!configCallback.isVN) {
            $("[data-field=" + dataField + "] .edit-label-en").removeClass("display-none");
        }

        var widthTableAfter = widthTableNow - 101;
        var rate = widthTableAfter / widthTableNow;
        var headerTD = $("#tbDetail .tr-header td:visible");
        for (var i = 0; i < headerTD.length; i++) {
            if ($(headerTD[i]).attr("data-field") != dataField) {
                $(headerTD[i]).css("width", ($(headerTD[i]).width() * rate) + "px");
            }
        }
    }

    reloadNewWidthTable($("#tbDetail .tr-header td:visible"));
    resizeTable("#tbDetail", false, true);
}

/**
 * Thêm dòng seller trên mẫu
 * ndanh1 30/6/2020
 * */
function addNewCustomSeller() {
    var dataField = generateDataField(".custom-seller", CUSTOM_SELLER);

    var htmlCustomSeller = `<div class="edit-item float-left width-full custom-seller" data-field="{0}">
                                <div class="edit-label display-table-cell white-space-nowrap">{1}</div>
                                <div class="edit-label-en display-table-cell white-space-nowrap {2}"></div>
                                <div class="two-dot display-table-cell white-space-nowrap">:</div>
                                <div class="edit-value display-table-cell"></div>
                            </div>`;
    var isShowEN = configCallback.isVN ? "display-none" : "";
    htmlCustomSeller = htmlCustomSeller.format(dataField, "Trường mở rộng", isShowEN);

    $("[group-field=seller-infor] > .clear").before(htmlCustomSeller);

    var item = [{
        dataField: dataField,
        isShow: true,
        canHide: true,
        canMove: true,
        label: {
            isShow: true,
            canEdit: true,
            value: "Trường mở rộng",
            placeholder: "[Tiêu đề]"
        },
        labelEN: {
            isShow: !configCallback.isVN,
            canEdit: true,
            value: "",
        },
        value: {
            isShow: true,
            value: "",
            canEdit: true,
            placeholder: "[Nội dung]",
        },
        isCustomField: true,
        symbol: null,
        merge: null
    }];

    return item;
}

function addNewRowInfor() {
    var dataField = generateDataField(".custom-invoice-infor", CUSTOM_INFOR);

    var htmlCustomSeller = `<div class="custom-invoice-infor" data-field="{0}">
                                <div class="edit-value"></div>
                            </div>`;
    htmlCustomSeller = htmlCustomSeller.format(dataField);

    $("[group-field=invoice-infor]").append(htmlCustomSeller);

    var item = [{
        dataField: dataField,
        isShow: true,
        canHide: true,
        canMove: true,
        label: {
            isShow: false,
            value: "",
            canEdit: true,
        },
        labelEN: {
            isShow: false,
            value: "",
            canEdit: true,
        },
        value: {
            isShow: true,
            value: "",
            canEdit: true,
            placeholder: "[Nội dung]",
        },
        symbol: null,
        merge: null,
        isCustomField: true,
    }];

    return item;
}

/**
 * Thêm cột mới cho column
 * @param {any} color
 * @param {any} type
 * ndanh1 7/2/2020
 */
function addNewCustomBuyer(customField, isShow, editLabel) {
    var curDiv = $("[data-field=" + customField.dataField + "]");
    if (!isShow) {
        curDiv.remove();
    } else {
        var dataValue = customField.value;
        var dataLabel = customField.label;
        var dataLabelEN = customField.labelEN;
        var dataField = customField.dataField;
        var canEditLabel = editLabel ? "" : "none-edit";
        var htmlCustomBuyer = `<div class="edit-item float-left width-full custom-buyer" data-field="{0}">
                                <div class="edit-label display-table-cell white-space-nowrap {5}">{1}</div>
                                <div class="edit-label-en display-table-cell white-space-nowrap {5} {2}">{3}</div>
                                <div class="two-dot display-table-cell white-space-nowrap">:</div>
                                <div class="edit-value display-table-cell none-edit">{4}</div>
                            </div>`;
        var isShowEN = configCallback.isVN ? "display-none" : "";
        htmlCustomBuyer = htmlCustomBuyer.format(dataField, dataLabel, isShowEN, dataLabelEN, dataValue, canEditLabel);

        var $element = $("[data-field=" + customField.elementAppend + "]");

        if (customField.isAppendMerge) {
            $element = $("[merge-field=" + customField.elementAppend + "]")
        }

        if ($element.length > 0) {
            if (customField.isDown) {
                $element.after(htmlCustomBuyer);
            } else {
                $element.before(htmlCustomBuyer);
            }
        } else {
            $("[group-field=buyer-infor] > .clear").before(htmlCustomBuyer);
        }
    }

    var lstMege = $("[data-field=buyer-infor]").find(".merge-row-infor");
    var $parentGroupField = $("[group-field=buyer-infor]");
    var lstMege = $parentGroupField.find(".merge-row-infor");

    if (lstMege.length > 0) {
        removeResizeDragable();
        for (var i = 0; i < lstMege.length; i++) {
            resizeElementInfor($(lstMege[i]).attr("merge-field"));
        }
    }

    checkStepToExecute();
    changePositionQRCodeCashRegister();
}

/**
 * add những trường nhập liệu
 * @param {any} item
 */
function addNewRowInBuyerForTicket(item) {
    var curDiv = $("[data-field=" + item.DataField + "]");
    if (curDiv.length == 0) {
        var dataLabel = item.Label;
        var dataLabelEN = item.LabelEN;
        var dataField = item.DataField;

        var htmlCustomBuyer = `<div class="float-left width-full custom-field-ticket" data-field="{0}">
                                <div class="edit-label display-table-cell white-space-nowrap">{1}</div>
                                <div class="edit-label-en display-table-cell white-space-nowrap {3}">{2}</div>
                                <div class="two-dot display-table-cell white-space-nowrap">:</div>
                                <div class="edit-value display-table-cell none-edit"></div>
                            </div>`;
        var isShowEN = configCallback.isVN ? "display-none" : "";
        htmlCustomBuyer = htmlCustomBuyer.format(dataField, dataLabel, dataLabelEN, isShowEN);
        if ($("[group-field=buyer-infor] > .ui-resizable-handle").length > 0) {
            if ($("[group-field=buyer-infor] > .clear").length > 0) {
                $("[group-field=buyer-infor] > .clear").before(htmlCustomBuyer);
            }
            else {
                $("[group-field=buyer-infor] > .ui-resizable-handle").before(htmlCustomBuyer);
            }
        }
        else {
            $("[group-field=buyer-infor]").append(htmlCustomBuyer);
        }

        configCallback.htmlBlock("buyer-infor", true);
    }

    checkStepToExecute();
}

/**
 * add các trường ghi chú
 * */
function addNewNoteForTicket() {
    var dataField = generateDataField(".custom-field-buyer", CUSTOM_FIELD_BUYER);
    var htmlCustomBuyer = `<div class="float-left width-full custom-field-buyer" data-field="{0}">
                                <div class="edit-label display-table-cell white-space-nowrap"></div>
                                <div class="edit-label-en display-table-cell white-space-nowrap {1}"></div>
                                <div class="two-dot display-table-cell white-space-nowrap display-none">:</div>
                                <div class="edit-value display-table-cell"></div>
                            </div>`;
    var isShowEN = configCallback.isVN ? "display-none" : "";
    htmlCustomBuyer = htmlCustomBuyer.format(dataField, isShowEN);
    if ($("[group-field=buyer-infor] > .ui-resizable-handle").length > 0) {
        if ($("[group-field=buyer-infor] > .clear").length > 0) {
            $("[group-field=buyer-infor] > .clear").before(htmlCustomBuyer);
        }
        else {
            $("[group-field=buyer-infor] > .ui-resizable-handle").before(htmlCustomBuyer);
        }
    }
    else {
        $("[group-field=buyer-infor]").append(htmlCustomBuyer);
    }
    configCallback.htmlBlock("buyer-infor", true);
    checkStepToExecute();
    return dataField;
}

/**
 * Merge 2 dòng thông tin lại với nhau
 * ndanh1 6/7/2020
 * */
function mergeRowInfor(itemMerge, customField) {
    if (itemMerge.listMerge.length == 0) {
        var mergeField = generateDataField(".merge-row-infor", MERGE_FIELD, "merge-field");
        var newHtmlDivMerge = "<div class='float-left width-full merge-row-infor display-flex' merge-field='" + mergeField + "'></div>";
        var resultCustom = [];

        if (!(configCallback.isTVT() && itemMerge.listItem.length == 1 && customField == "search-block")) {
            for (var i = 0; i < itemMerge.listItem.length; i++) {
                var dataField = itemMerge.listItem[i];
                var $element = $("[data-field=" + dataField + "]");
                var outerHtml = $element[0].outerHTML;
                if (i == 0) {
                    $element.after(newHtmlDivMerge);
                }

                var $merge = $("[merge-field=" + mergeField + "]");
                $merge.append(outerHtml);
                $element.remove();
                var configReturn = { "dataField": dataField, "merge": mergeField };
                resultCustom.push(configReturn);
            }
        }

        var divClear = "<div class='clear'></div>"
        $("[merge-field=" + mergeField + "]").append(divClear);

        var lstDataFieldNow = $("[merge-field=" + mergeField + "] [data-field]");
        
        if (!(configCallback.isTVT() && customField == "search-block")) {
            var width = 100 / lstDataFieldNow.length;
            for (var i = 0; i < lstDataFieldNow.length; i++) {
                $(lstDataFieldNow[i]).css("width", width + "%")
            }
            resizeElementInfor(mergeField);
        }
        else {
            //Fix cứng cho phần thông tin Misa với mẫu vé
            $(lstDataFieldNow[0]).css("width", "127.61px");
            $(lstDataFieldNow[1]).css("width", "");
            $("[merge-field=" + mergeField + "]").css("justify-content", "center");
            $("[group-field=search-block]").css("border-top", "2px solid rgba(0, 0, 0, 0.46)");
            //Bỏ dấu - khi merge lại
            var publicContent = $(lstDataFieldNow[0]).find("span").html();
            if (itemMerge.listItem.length > 1) {
                publicContent = publicContent.trim() + " -";
                $(lstDataFieldNow[0]).find("span").html(publicContent);
            }
        }

        return resultCustom;
    } else {
        var $eleMerge = $("[merge-field=" + itemMerge.listMerge[0] + "]");
        var resultCustom = [];
        for (var i = 1; i < itemMerge.listMerge.length; i++) {
            var $lstDataField = $("[merge-field=" + itemMerge.listMerge[i] + "] [data-field]")
            for (var j = 0; j < $lstDataField.length; j++) {
                itemMerge.listItem.push($($lstDataField[j]).attr("data-field"));
            }
        }

        for (var i = 0; i < itemMerge.listItem.length; i++) {
            var $element = $("[data-field=" + itemMerge.listItem[i] + "]");
            $($eleMerge[0]).find(".clear").before($element[0]);
            var configReturn = { "dataField": itemMerge.listItem[i], "merge": itemMerge.listMerge[0] };
            resultCustom.push(configReturn);
        }

        for (var i = 1; i < itemMerge.listMerge.length; i++) {
            $("[merge-field=" + itemMerge.listMerge[i] + "]").remove();
        }

        var lstDataFieldNow = $("[merge-field=" + itemMerge.listMerge[0] + "] [data-field]");
        
        if (!(configCallback.isTVT() && customField == "search-block")) {
            var width = 100 / lstDataFieldNow.length;
            for (var i = 0; i < lstDataFieldNow.length; i++) {
                $(lstDataFieldNow[i]).css("width", width + "%")
            }
            resizeElementInfor(itemMerge.listMerge[0]);
        }
        else {
            //Fix cứng cho phần thông tin Misa với mẫu vé
            $(lstDataFieldNow[0]).css("width", "123.61px");
            $(lstDataFieldNow[1]).css("width", "145.78px");
            $("[merge-field=" + mergeField + "]").css("justify-content", "center");
            $("[group-field=search-block]").css("border-top", "2px solid rgba(0, 0, 0, 0.46)");
            //Bỏ dấu - khi merge lại
            var publicContent = $(lstDataFieldNow[0]).find("span").html();
            publicContent = publicContent.trim() + " -";
            $(lstDataFieldNow[0]).find("span").html(publicContent);
        }

        return resultCustom;
    }
}

/**
 * làm lại data để return về sau khi vẽ xong
 * ndanh1 6/7/2020
 * */
function returnToCustomAfterMerge() {
    if (itemMerge.listMerge.length == 0) {

    }
}

/**
 * Bỏ chọn mer dòng
 * ndanh1 6/7/2020
 * */
function reMergeRowInfor(mergeField, customField) {
    var $mergeField = $("[merge-field=" + mergeField + "]");
    $mergeField.find(".ui-resizable-handle.ui-resizable-e").remove();
    var count = $mergeField[0].children.length;
    for (var i = 0; i < count; i++) {
        if ($($mergeField[0].children[0]).hasClass("clear")) {
            $($mergeField[0].children[0]).remove();
            continue;
        }
        $($mergeField[0].children[0]).css("width", "100%");
        $mergeField[0].before($mergeField[0].children[0]);
    }

    if (configCallback.isTVT() && customField == "search-block") {
        var misaInfo = $("[data-field=MISAINFO] .edit-label");
        var label = misaInfo.html().replace("-", "").trim();
        misaInfo.html(label);
        $("[group-field=search-block]").css("border-top", "2px solid rgba(0, 0, 0, 0.46)");
    }

    $mergeField.remove();
}

/**
 * Sinh field theo công thức
 * ndanh1 6/7/2020
 * */
function generateDataField(className, strField, field) {
    var $customField = $(className);
    if (!field) {
        field = "data-field";
    }
    var check = 0;
    for (var i = 0; i < $customField.length; i++) {
        if ($("[" + field + "=" + strField + i + "]").length == 0) {
            dataField = strField + i;
            check = 1;
            break;
        }
    }

    if (check == 0) {
        dataField = strField + $customField.length;
    }

    return dataField;
}

/**
 * update lại đống công customfield
 * @param {any} listCustom
 */
function updateCustomField(listCustom) {
    for (var i = 0; i < listCustom.length; i++) {
        var item = listCustom[i];
        if (item.EntityState == 2) {
            $("[data-field=" + item.FieldName + "] .edit-label").text(item.DisplayText);
        } else if (item.EntityState == 3) {
            $("[data-field=" + item.FieldName + "]").remove();
        }
    }
}

/**
 * Delete customfield
 * @param {any} dataField
 */
function deleteCustomField(dataField) {
    $("[data-field=" + dataField + "]").remove();
    checkStepToExecute();
}

/**
 * sắp xếp lại khi di chuyển
 * @param {any} field: cục muốn di chuyển
 * @param {any} apendToField: cục muốn append vào
 * @param {any} isDown: append dưới hay trên
 * @param {any} isParent: cục append vào có phải là merge-field không
 * @param {any} isMergeField: cục di chuyển có phải merge-field k
 * ndanh1 8/7/2020
 */
function sortaleByField(field, apendToField, isDown, isParent, isMergeField) {
    var $current = $($("[data-field=" + field + "]")[0]),
        $next = $($("[data-field=" + apendToField + "]")[0]);

    //dvkhanh check xem cot tiep tuc co phai cot con khong, neu dung lay cot cha
    var fieldParent = $next.attr('field-parent');
    if (!!fieldParent) $next = $($("[data-field=" + fieldParent + "]")[0]);

    if (isMergeField) {
        $current = $($("[merge-field=" + field + "]")[0])
    }
    if (isParent) {
        $next = $($("[merge-field=" + apendToField + "]")[0]);
    }

    if (field == "DescriptionInvoiceClient" || field == "DescriptionInvoicePaperClient") {
        $current = $current.parent();
    }

    if (apendToField == "DescriptionInvoiceClient" || apendToField == "DescriptionInvoicePaperClient") {
        $next = $next.parent();
    }

    if (isDown) {
        $current.insertAfter($next);
    } else {
        $current.insertBefore($next);
    }

    if (configCallback.isTVT()) {
        $current.parent().css("flex-direction", "column");
    }

    //kiểm tra nếu nó là table thì xếp lại toàn bộ họ hàng đi theo
    if ($current[0] && $current[0].tagName == "TD") {
        var lstTD = $("#tbDetail [data-field=" + field + "]"),
            lstTDNext = $("#tbDetail [data-field=" + apendToField + "]");
        for (var i = 1; i < lstTD.length; i++) {
            var $curTD = $(lstTD[i]),
                $curTDNext = $(lstTDNext[i]);
            if (isDown) {
                $curTD.insertAfter($curTDNext);
            } else {
                $curTD.insertBefore($curTDNext);
            }
        }
    }

    if (configCallback.customField == "seller-infor") {
        var type = 1
        if ($('[group-field="seller-infor"]').hasClass("type-2")) {
            type = 2;
        } else if ($('[group-field="seller-infor"]').hasClass("type-3")) {
            type = 3;
        }

        setAlignmentSellerInfor(type);
    }
    if (configCallback.customField == "buyer-infor") {
        var type = 1
        if ($('[group-field="buyer-infor"]').hasClass("type-2")) {
            type = 2;
        } else if ($('[group-field="buyer-infor"]').hasClass("type-3")) {
            type = 3;
        }

        setAlignmentBuyerInfor(type);
    }

    // reload lại event resize nếu nó là merge field
    if (configCallback.customField == "seller-infor" || configCallback.customField == "buyer-infor") {
        var $parentGroupField = $current.parents("[group-field]").first();
        var lstMege = $parentGroupField.find(".merge-row-infor");

        if (lstMege.length > 0) {
            removeResizeDragable();
            for (var i = 0; i < lstMege.length; i++) {
                resizeElementInfor($(lstMege[i]).attr("merge-field"));
            }
        }
    } else if (configCallback.customField == "table-detail") {
        resizeTable("#tbDetail", true);
        resizeTable("#tbDetail", false, true);
    }
}

/**
 * Sắp xếp di chuyển chữ ký
 * ndanh1 4/8/2020
 * */
function sortAbleSignXml(signRegion, apendToField, isDown) {
    resizeTable("#signXml", true);
    var $current = $($("[sign-region=" + signRegion + "]")[0]),
        $next = $($("[sign-region=" + apendToField + "]")[0]);

    if (isDown) {
        $current.insertAfter($next);
    } else {
        $current.insertBefore($next);
    }

    reloadNewWidthTable($("#signXml tr:first-child td:visible"));
    resizeTable("#signXml", false, true);
}

/**
 * Sắp xếp di chuyển chữ ký của biên lai K80
 * @param {any} signRegion
 * @param {any} apendToField
 * @param {any} isDown
 * tqha (12/8/2022)
 */
function sortAbleSignForBLK80(field, apendToField, isDown) {
    var $current = $($("[sign-region=" + field + "]")[0]),
        $next = $($("[sign-region=" + apendToField + "]")[0]);
    if ($current.length == 0) {
        $current = $($("[data-field=" + field + "]")[0]);
    }
    if ($next.length == 0) {
        $next = $($("[data-field=" + apendToField + "]")[0]);
    }

    if (isDown) {
        $current.insertAfter($next);
    } else {
        $current.insertBefore($next);
    }
}

/**
 * Get all data config của cả mẫu
 * ndanh1 13/7/2020
 * */
function updateDataBeforeSave() {
    $(".bg-picked-by-user").removeClass("bg-picked-by-user");
    $(".element-active").removeClass("element-active");
    var data = {
        DataMaster: getDataMaster(),
        DataDetail: getDataDetail(),
    }

    if (configCallback.isTVT()) {
        data.DataMaster.Fonts = $(".container").css("font-family");
        data.DataMaster.FontSize = $(".container").css("font-size").replace("px", "");
        data.DataMaster.Color = "rgb(0, 0, 0)";

        if ($("[data-field=InvoiceNumber]").hasClass("show-only-number")) {
            data.DataDetail[0].ConfigItemTemplates[1].ConfigBLP = 1;
        }
    }

    return data;
}

/**
 * Get data config của cả mẫu
 * ndanh1 13/7/2020
 * */
function getDataDetail() {
    var arrDataConfig = [];
    var QRCode = getPositionQRCode() == "seller-infor" ? true : false;
    var QRCodeTicket;
    var positionTicket;
    if (configCallback.isTVT()) {
        var positionTicket = checkPositionQRTicket()
        if (positionTicket == 2) {
            QRCodeTicket = false;
        }
        else {
            QRCodeTicket = true;
        }
    }

    var $listRegion = $("[group-field]");
    for (var i = 0; i < $listRegion.length; i++) {
        var $region = $($listRegion[i]);
        var nameRegion = $region.attr("group-field");
        var style = $("[group-field=" + nameRegion + "]").attr("style");
        var configItems;
        switch (nameRegion) {
            case "table-footer":
                configItems = getDataConfigTableFooter();
                break;
            case "table-detail":
                configItems = getDataConfigTableDetail();
                break;
            case "sign-xml":
                configItems = getDataConfigSignXml();
                break;
            case "ticket-price":
                configItems = getDataConfigTicketPrice();
                break;
            case "buyer-infor":
            case "seller-infor":
            case "other-invoice":
            case "curency-block":
            case "search-block":
            case "invoice-infor":
            default:
                configItems = getDataConfigGeneral(nameRegion);
                break;
        }

        var item = { NameRegion: nameRegion, ConfigItemTemplates: configItems, StyleOfRegion: style };
        if (nameRegion == "buyer-infor") {
            item.HasQRCode = !QRCode;
        } else if (nameRegion == "seller-infor") {
            item.HasQRCode = QRCode;
        }
        else if (nameRegion == "other-invoice" && positionTicket == 0 && configCallback.isTVT()) {
            item.HasQRCode = QRCodeTicket
        }
        else if (nameRegion == "sign-xml" && positionTicket == 1 && configCallback.isTVT()) {
            item.HasQRCode = QRCodeTicket
        }


        item.IsShow = !$region.hasClass("display-none");
        arrDataConfig.push(item);
    }
    return arrDataConfig;
}

/**
 * Get data chung
 * ndanh1 13/7/2020
 * */
function getDataMaster() {
    var lstTable = [];
    lstTable.push(getDataStyleTable($(".item-invoice-infor")));
    lstTable.push(getDataStyleTable($(".tr-invoice-first > td")));
    lstTable.push(getDataStyleTable($("#signXml td")));

    var lineHeight = $("#tbOther").css("line-height") ? $("#tbOther").css("line-height") : null;

    return data = {
        PosiSellerInfor: getPositionSeller(),
        ListTableConfig: lstTable,
        IsShowQRCode: checkQRIsShow(),
        StyleQRCode: "",
        IndentTemplate: [15, 15, 15, 15],
        HasSymbol: $(".edit-symbol").length > 0,
        Fonts: $("body").css("font-family"),
        FontSize: $("body").css("font-size").replace("px", ""),
        Color: $("body").css("color"),
        StyleContentDetail: $(".content-detail").attr("style"),
        UserTemplateDefineV3: {},
        IsShowUnit: $(".unit-name-ticket").length > 0 && !$(".unit-name-ticket").first().hasClass("display-none"),
        StyleUnitName: getStyleUnitName(),
        LineHeight: lineHeight,
    }
}

/**
 * Lấy các thông tin chung của mẫu
 * ndanh1 28/7/2020
 * */
function getInforGeneralTemplate() {
    var item = null;
    if (configCallback.isTVT()) {
        if ($(".container").length > 0) {
            item = {
                Fonts: $(".container").css("font-family"),
                Color: $(".container").css("color"),
            }
        }
        else {
            item = {
                Fonts: $("body").css("font-family"),
                Color: $("body").css("color"),
            }
        }
    }
    else {
        item = {
            Fonts: $("body").css("font-family"),
            Color: $("body").css("color"),
        }
    }
    

    return item;
}

/**
 * Kiểm tra qr code có show không ?
 * ndanh1 17/7/2020
 * */
function checkQRIsShow() {
    if (configCallback.isTVT()) {
        checkShow = true;
        checkShowQRTop = true;
        checkShowQRBottom = true;
        var qrbottom = $("[data-field=QRCodeField]")
        if (qrbottom.length > 0) {
            if (qrbottom.find(".qrcode-parent").hasClass("display-none")) {
                qrbottom.addClass("display-none");
                qrbottom.find(".qrcode-parent").removeClass("display-none")
            }
            if ($("[data-field=QRCodeField]").hasClass("display-none")) {
                checkShowQRBottom = false;
            }
        }
        else {
            checkShowQRBottom = false;
        }

        var qrTop = $("[data-field=QRCodeFieldTop]")
        if (qrTop.length > 0) {
            if ($("[data-field=QRCodeFieldTop]").hasClass("display-none")) {
                checkShowQRTop = false;
            }
        }
        else {
            checkShowQRTop = false;
        }
        checkShow = checkShowQRTop || checkShowQRBottom;
        return checkShow;
    }
    return !$(".qrcode-parent").hasClass("display-none");
}

/**
 * Get data config table detail
 * ndanh1 13/7/2020
 * */
function getDataConfigTableDetail() {
    var listData = [];
    var listDataField = $("[group-field=table-detail] .tr-header [data-field]");
    for (var i = 0; i < listDataField.length; i++) {
        var $element = $(listDataField[i]);
        var dataField = $element.attr("data-field")
        var $value = $("[group-field=table-detail] [data-field=" + dataField + "]").find(".edit-value");
        var $label = $("[group-field=table-detail] [data-field=" + dataField + "]").find(".edit-label");
        var $labelen = $("[group-field=table-detail] [data-field=" + dataField + "]").find(".edit-label-en");
        var $symbol = $("[group-field=table-detail] [data-field=" + dataField + "]").find(".edit-symbol");

        var value = {
            Value: $value.text(),
            Style: $value.attr("style"),
            IsShow: !$value.hasClass("display-none"),
        }

        var label = {
            Value: $label.text().replace(":", ""),
            Style: $label.attr("style"),
            IsShow: !$label.hasClass("display-none"),
            ParentStyle: $label.parent().attr("style"),
        }

        var labelEN = {
            Value: $labelen.text().replace(":", ""),
            Style: $labelen.attr("style"),
            IsShow: !$labelen.hasClass("display-none"),
        }

        var symbol = {
            Value: $symbol.text(),
            Style: $symbol.attr("style"),
            IsShow: !$symbol.hasClass("display-none"),
            ParentStyle: $symbol.parent().attr("style"),
        }

        var item = {
            DataField: dataField,
            IsShow: !$element.hasClass("display-none"),
            Style: $element.attr("style"),
            Value: value,
            Label: label,
            LabelEN: labelEN,
            Symbol: symbol,
        }

        listData.push(item);
    }

    return listData;
}

/**
 * Get data config table footer
 * ndanh1 13/7/2020
 * */
function getDataConfigTableFooter() {
    var listData = [];
    var listDataField = $("[group-field=table-footer] [data-field]");
    for (var i = 0; i < listDataField.length; i++) {
        var $element = $(listDataField[i]);
        var dataField = $element.attr("data-field")
        var $value = $element.find(".edit-value");
        var $label = $element.find(".edit-label");
        var $labelen = $element.find(".edit-label-en");
        var IsLabel = false;
        if ($label.length > 0) {
            IsLabel = true;
        }

        var value = $value.text().trim();
        if (dataField == "TemplateNote") {
            value = $value.html();
            value = value ? value.replaceAll("&nbsp;", "&#160;").replaceAll("<br>", "<br/>") : value;
        }

        var value = {
            Value: value,
            Style: $value.attr("style"),
            IsShow: $value.length > 0 && !$value.hasClass("display-none"),
        }

        var label = {
            Value: $label.text().trim().replace(":", ""),
            Style: $label.attr("style"),
            IsShow: $label.length > 0 && !$label.hasClass("display-none"),
        }

        var labelEN = {
            Value: $labelen.text().trim().replace(":", ""),
            Style: $labelen.attr("style"),
            IsShow: $labelen.length > 0 && !$labelen.hasClass("display-none"),
        }

        var twoDot = {
            Value: ":",
            Style: "",
            IsShow: $element.find(".two-dot").length > 0 && !$element.find(".two-dot").hasClass("display-none"),
        }

        if (twoDot.IsShow) {
            twoDot.Style = $element.find(".two-dot").attr("style");
        }

        if (twoDot.IsShow) {
            twoDot.Style = $element.find(".two-dot").attr("style");
        }

        var item = {
            DataField: dataField,
            IsShow: !$element.hasClass("display-none"),
            Style: $element.attr("style"),
            Value: value,
            Label: label,
            LabelEN: labelEN,
            IsLabel: IsLabel,
            TwoDot: twoDot,
        }

        listData.push(item);
    }

    return listData;
}

/**
 * Get data config theo group field
 * @param {any} groupField
 * ndanh1 13/7/2020
 */
function getDataConfigGeneral(groupField) {
    var listData = [];
    var listDataField = $("[group-field=" + groupField + "] [data-field]");
    if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
        if (groupField == "other-invoice") {
            listDataField = $("[group-field=" + groupField + "] [data-field], [data-field=QRCodeFieldTop]");
        }

        if (configCallback.isBLPHasFeeK80()) {
            if (groupField == "buyer-infor") {
                listDataField = $("[group-field=" + groupField + "] [data-field], [mark=UnitName]");
            }
        }
    }
    for (var i = 0; i < listDataField.length; i++) {
        var $element = $(listDataField[i]);
        var $value = $element.find(".edit-value");
        var $label = $element.find(".edit-label");
        var $labelen = $element.find(".edit-label-en");
        var typeShow = 1;
        var mergeField = $element.parents(".merge-row-infor").length > 0 ? $element.parents(".merge-row-infor").attr("merge-field") : "";
        var dataField = $element.attr("data-field");

        if (!dataField) {
            dataField = $element.attr("mark")
        }

        if ($element.hasClass("not-show-preview") && dataField != "InforWebsiteSearch") {
            continue;
        }

        if ($element.hasClass("type-2")) {
            typeShow = 2;
        } else if ($element.hasClass("type-3")) {
            typeShow = 3;
        }

        var value = {
            Value: $value.text(),
            Style: $value.attr("style"),
            IsShow: !$value.hasClass("display-none"),
        }

        var label = {
            Value: $label.text().trim().replace(":", ""),
            Style: $label.attr("style"),
            IsShow: !$label.hasClass("display-none"),
        }

        var labelEN = {
            Value: $labelen.text().trim().replace(":", ""),
            Style: $labelen.attr("style"),
            IsShow: ($labelen.length > 0 && !$labelen.hasClass("display-none")),
        }


        var twoDot = {
            Value: ":",
            Style: "",
            IsShow: $element.find(".two-dot").length > 0 && !$element.find(".two-dot").hasClass("display-none"),
        }

        if (twoDot.IsShow) {
            twoDot.Style = $element.find(".two-dot").attr("style");
        }

        if (dataField == "SubTitleInvoice") {
            var textValue = $value.html();
            textValue = textValue ? textValue.replaceAll("&nbsp;", "&#160;").replaceAll("<br>", "<br/>") : textValue;
            value.Value = textValue;
        }

        if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
            if (dataField == "QRCodeFieldTop") {
                label.Style = $element.find(".qrcode-parent").attr("style");
                labelEN.Style = $element.find(".qrcode").attr("style");
            }
        }

        var item = {
            DataField: dataField,
            IsShow: !$element.hasClass("display-none"),
            Style: $element.attr("style"),
            Value: value,
            Label: label,
            LabelEN: labelEN,
            TypeShow: typeShow,
            DivEmpty: $element.find(".empty-div").length > 0 ? $element.find(".empty-div")[0].outerHTML : "",
            MergeField: mergeField,
            BorderTaxCode: $element.hasClass("border-active"),
            TwoDot: twoDot,
            CannotShow: $element.hasClass("display-cannot-show"),
            ConfigBLP: $element.hasClass("fee-parent") ? 1 : 0,
        }

        if (configCallback.isBLPHaveFee() && groupField == "buyer-infor") {
            if (dataField == "FeeName") {
                item.CustomConfigValue = $(".FeeCode").text().trim();
                item.ConfigBLP = 2;
            } else if (dataField == "TotalAmountWithVAT") {
                item.CustomConfigValue = $(".TotalAmountTemplate").text().trim();
                item.ConfigBLP = 2;
                item.MergeStyle = $element.parents(".merge-row-infor").attr("style");
            } else if (dataField == "UnitCurrencyName") {
                item.CustomConfigValue = item.Value.Value;
                item.ConfigBLP = 2;
            }
            else if (dataField == "UnitName") {
                item.CustomConfigValue = $(".unit-name-ticket").first().text();
                item.ConfigBLP = 2;
            }
            else if (dataField == "TotalAmountWithVATInWords") {
                item.CustomConfigValue = item.Value.Value;
                item.ConfigBLP = 2;
            }
        }

        if (configCallback.isTVT()) {
            checkForTVT(item, dataField);
        }

        listData.push(item);
    }

    var $parentGroupField = $("[group-field=" + groupField + "]");
    var lstMege = $parentGroupField.find(".merge-row-infor");

    if (lstMege.length > 0) {
        removeResizeDragable();
        if (!(configCallback.isTVT() && groupField == "search-block")) {
            for (var i = 0; i < lstMege.length; i++) {
                resizeElementInfor($(lstMege[i]).attr("merge-field"));
            }
        }
    }

    return listData;
}

/**
 * Get data config cho ticket price
 * */
function getDataConfigTicketPrice() {
    var listData = [];
    var listDataField = $("[group-field=ticket-price] [data-field]");
    var haveFee = configCallback.isTVTHaveFee();
    for (var i = 0; i < listDataField.length; i++) {
        var $element = $(listDataField[i]);
        var $value = $element.find(".edit-value");
        var $label = $element.find(".edit-label");
        var $labelen = $element.find(".edit-label-en");
        var dataField = $element.attr("data-field");

        var value = {
            Value: $value.text(),
            Style: $value.attr("style"),
            IsShow: !$value.hasClass("display-none"),
        }

        var label = {
            Value: $label.text().trim().replace(":", ""),
            Style: $label.attr("style"),
            IsShow: !$label.hasClass("display-none"),
        }

        var labelEN = {
            Value: $labelen.text().trim().replace(":", ""),
            Style: $labelen.attr("style"),
            IsShow: ($labelen.length > 0 && !$labelen.hasClass("display-none")),
        }


        var twoDot = {
            Value: ":",
            Style: "",
            IsShow: $element.find(".two-dot").length > 0 && !$element.find(".two-dot").hasClass("display-none"),
        }

        if (twoDot.IsShow) {
            twoDot.Style = $element.find(".two-dot").attr("style");
        }

        var item = {
            DataField: dataField,
            IsShow: !$(".ticket-price-container").hasClass("display-none") && !$element.hasClass("display-none"),
            Style: $element.attr("style"),
            Value: value,
            Label: label,
            LabelEN: labelEN,
            TypeShow: 1,
            DivEmpty: "",
            MergeField: "",
            BorderTaxCode: false,
            TwoDot: twoDot,
            CannotShow: $element.hasClass("display-cannot-show"),
            ConfigBLP: haveFee ? 1 : 0,
        }

        if (configCallback.isTVT()) {
            item = checkForTVT(item, dataField);
        }

        if (configCallback.isTVT()) {
            if (dataField == "UnitName") {
                item.CustomConfigValue = $(".unit-name-ticket").first().text();
            }
        }

        listData.push(item);
    }

    return listData;
}

/**
 * bổ sung 1 số trường cho các giá trị đặc biệt
 * @param {any} item
 * @param {any} dataField
 */
function checkForTVT(item, dataField) {
    if (dataField == "TicketPrice") {
        item.CustomConfigValue = $(".TicketPrice").text().trim();
        item.ConfigBLP = $("[data-field=TicketPriceOther]").hasClass("display-none") ? 1 : 2;
    } else if (dataField == "VatRate") {
        item.CustomConfigValue = $(".VatRateTicket").text().trim();
        item.ConfigBLP = $("[data-field=VatRateOther]").hasClass("display-none") ? 1 : 2;
    } else if (dataField == "VatPrice") {
        item.CustomConfigValue = $(".VatPrice").text().trim();
        item.ConfigBLP = $("[data-field=VatPriceOther]").hasClass("display-none") ? 1 : 2;
    } else if (dataField == "TicketPriceBeforeVAT") {
        item.CustomConfigValue = $(".TicketPriceBeforeVAT").text().trim();
        item.ConfigBLP = $("[data-field=TicketPriceBeforeVATOther]").hasClass("display-none") ? 1 : 2;
    } else if (dataField == "Route" || dataField == "From" || dataField == "Destination") {
        if (configCallback.routeID) {
            item.CustomConfigValue = $("[data-field=" + dataField + "] .edit-value").text().trim();
        }
    } else if (dataField == "DepatureDateTime") {
        var type = $("[data-field=DepatureDateTime] .edit-value").attr("type-display");
        item.ConfigBLP = type ? type : 1;
    } else if ($(".IsTaxReduction43").text() == "true") {
        item.CustomConfigValue = $(".{0}".format(dataField)).text().trim();
    }
    else if (dataField == "InvoiceSeries") {
        if ($("[data-field=InvoiceSeries]").parent().attr("typeShow") == "column") {
            item.ConfigBLP = 1;
        }
        else {
            item.ConfigBLP = 0;
        }
    }

    return item;
}

/**
 * Get data config phần chữ ký
 * ndanh1 13/7/2020
 * */
function getDataConfigSignXml() {
    var listData = [];
    var listDataField = $("[group-field=sign-xml] [data-field]");
    for (var i = 0; i < listDataField.length; i++) {
        var $element = $(listDataField[i]).parents("[sign-region]");
        var dataField = $(listDataField[i]).attr("data-field");
        var $elementDataField = $("[group-field=sign-xml] [data-field=" + dataField + "]");
        var $value = $elementDataField.find(".edit-value");
        var $label = $elementDataField.find(".edit-label");
        var $labelen = $elementDataField.find(".edit-label-en");
        var mergeField = $element.attr("sign-region");
        var styleDataField = "";
        var styleCustom = "";

        var checkDuplicate = listData.filter(function (e, i) {
            return e.DataField == dataField;
        });
        if ((checkDuplicate.length > 0 || $(listDataField[i]).hasClass("not-show-preview")) && dataField != "SellerSignContent" && dataField != "TaxCodeSignContent" && dataField != "SellerSignDiv") {
            continue
        }

        var label = {
            Value: $label.text().replace(":", ""),
            Style: $label.attr("style"),
            IsShow: !$label.hasClass("display-none"),
        }

        var value = {
            Value: $value.text().replace(":", ""),
            Style: $value.attr("style"),
            IsShow: !$value.hasClass("display-none"),
        }

        var labelEN = {
            Value: $labelen.text().replace(":", ""),
            Style: $labelen.attr("style"),
            IsShow: !$labelen.hasClass("display-none"),
        }

        var twoDot = {
            Value: ":",
            Style: "",
            IsShow: $elementDataField.find(".two-dot").length > 0 && !$elementDataField.find(".two-dot").hasClass("display-none"),
        }

        if (twoDot.IsShow) {
            twoDot.Style = $elementDataField.find(".two-dot").attr("style");
        }

        if (configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent")) {
            styleDataField = $elementDataField.find(".content-sign").attr("style");
            styleCustom = $elementDataField.attr("style");
        }
        else {
            if ($elementDataField.attr("style")) {
                styleDataField = $elementDataField.attr("style");
            } else {
                styleDataField = $elementDataField.find(".content-sign").attr("style");
            }
        }
        

        if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
            if (dataField == "QRCodeField") {
                label.Style = $elementDataField.find(".qrcode-parent").attr("style");
                labelEN.Style = $elementDataField.find(".qrcode").attr("style");
            }
        }

        if (dataField == "SellerSignDiv") {
            labelEN = null;
            label = null;
            twoDot = null;
            value.IsShow = false;
            value.Value = null;
            value.Style = "";
            styleDataField = styleDataField ? styleDataField : "float: initial;";
        }

        var item = {
            DataField: dataField,
            IsShow: !$(listDataField[i]).hasClass("display-none"),
            Style: styleDataField,
            Value: value,
            LabelEN: labelEN,
            Label: label,
            MergeField: mergeField,
            TwoDot: twoDot,
            DivEmpty: styleCustom,
        }

        listData.push(item);
    }

    return listData;
}
/**
 * Get style của các thẻ td trong table
 * @param {any} $element
 * ndanh1 10/7/2020
 */
function getDataStyleTable($element) {
    var listStyle = [];
    var id = $element.parents("table").first().attr("id");
    var $tds = $element.parents("tr").first().find("td");

    for (var i = 0; i < $tds.length; i++) {
        if ($($tds[i]).parents("table").first().attr("id") == id) {
            var isShow = !$($tds[i]).hasClass("display-none") || !$($tds[i]).css("display") == "none";
            if ($($tds[i]).attr("sign-region") == "ConverterSignRegion") {
                isShow = true;
            }
            listStyle.push({
                Style: $($tds[i]).attr("style"),
                IsShow: isShow
            });
        }
    }
    return {
        TableID: id,
        ListStyle: listStyle,
    };
}

//Bỏ highlight trên mẫu và không cho người dùng chỉnh sửa trên mẫu
//vmthanh - 15/07/2020
function removeHighlightInvoice() {
    resizeTable("#tbDetail", true);
    resizeTable("#signXml", true);
    resizeTable("#firstInvoice", true);
    resizeTable("#invoiceInfor", true);
    $('.container .bg-picked-by-user').removeClass('bg-picked-by-user');
    $('.container .element-active').removeClass('element-active');
}

/*
* xử lý tăng giảm size chữ
* vmthanh(15/07/2020)
*/
function ChangeFontSize(isSizeUp) {
    //quất nốt mấy thằng méo giống ai - do cất là thay đổi font thì style inline 
    var lstEditLabel = $(".edit-label"),
        lstEditLabelEN = $(".edit-label-en"),
        lstEditValue = $(".edit-value"),
        lstTwoDot = $(".two-dot"),
        lstUnitName = $("[mark=UnitName]"),
        lstEsignTitle = $(".esign-title");
    lstEditLabel.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");

        if (fontsize <= 10) {
            $(item).closest("[data-field]").css("padding", "1px 0");
        }
        else {
            $(item).closest("[data-field]").css("padding", "4px 0");
        }

        $(item).closest("[data-field]").css("font-size", fontsize + "px");
    });
    lstEditValue.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");

        if ($(item).parent().find(".edit-label").css("display") != "none") {
            if (Number.parseInt($(item).parent().find(".edit-label").css("font-size")) <= 10) {
                $(item).closest("[data-field]").css("padding", "1px 0");
            }
            else {
                if (fontsize <= 10) {
                    $(item).closest("[data-field]").css("padding", "1px 0");
                }
                else {
                    $(item).closest("[data-field]").css("padding", "4px 0");
                }
            }
        }
        else {
            if (fontsize <= 10) {
                $(item).closest("[data-field]").css("padding", "1px 0");
            }
            else {
                $(item).closest("[data-field]").css("padding", "4px 0");
            }
        }

    });
    lstEditLabelEN.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");
    });
    lstTwoDot.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");
    });
    lstUnitName.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");
    });

    lstEsignTitle.each(function (index, item) {
        var fontsize = parseInt($(item).css("font-size").replace("px", ""));
        if (isSizeUp) {
            fontsize++;
        } else {
            fontsize--;
        }
        if (fontsize < 8) {
            fontsize = 8;
        }
        else if (fontsize > 30) {
            fontsize = 30;
        }
        item.style.fontSize = fontsize;
        $(item).css("font-size", fontsize + "px");

        if (fontsize <= 10) {
            $(item).css("padding", "1px 0");
        }
        else {
            $(item).css("padding", "4px 0");
        }

        var dataField = $(item).closest("[data-field]");
        if (fontsize <= 10) {
            dataField.find(".background-sign").css("background-size", "contain");
        }
        else {
            dataField.find(".background-sign").css("background-size", "");
        }
    });

    if (configCallback.isTVT() && !configCallback.isVN) {
        var sellerSize = Number.parseInt($("[data-field=SellerSignByClient] .edit-label").css("font-size").replace("px", ""));
        if (sellerSize > 20) {
            handleChangeStyleSignBlock(20, "SellerSignContent", 20);
        }

        var taxSize = Number.parseInt($("[data-field=TaxCodeSignDate] .edit-label").css("font-size").replace("px", ""));
        if (taxSize > 20) {
            handleChangeStyleSignBlock(20, "TaxCodeSignContent", 20);
        }
    }

    if (configCallback.isTVT()) {
        if ($("#en-amount-inword").length > 0) {
            let fontSize = Number.parseInt($("[data-field=TotalAmountInWords] .edit-value").css("font-size"));
            fontSize = fontSize ? fontSize - 1 : 12;
            $("#en-amount-inword").css("font-size", fontSize + "px");
        }
    }  
}

/*
* Update lại màu chữ của mẫu
* vmthanh(15/07/2020)
*/
function UpdateFontStyle(fonts) {
    while (fonts.indexOf("_") != -1) {
        fonts = fonts.replace("_", " ");
    }
    if (fonts == 'Roboto') {
        fonts = "'Roboto', sans-serif";
    }
    $("table").css("font-family", fonts);
    $("body").css("font-family", fonts);
    $(".container").css("font-family", fonts);
}

/*
* event khi sử dụng QR code
*/
function ChangeQRCode(isUseQRCode) {
    var qrCode = $(".qrcode-parent"),
        elementPrev = qrCode.prev();
    //nếu không sử dụng QR
    if (!isUseQRCode) {
        qrCode.addClass("display-none");
        elementPrev.css("width", "100%");
    }
    else {
        qrCode.removeClass("display-none");
        elementPrev.css("width", "83%");
    }
}

function ChangeQRCodeTicket(isUseQRCode) {
    if (!isUseQRCode) {
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").addClass("display-none");
        var $logo = $(".logo-template-content");
        var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
        var $invSeri = $($("[data-field=InvoiceSeries]")[0]);
        if (!hasLogo && configCallback.isVN) {
            $invSeri.parent().css("flex-direction", "row");
            $invSeri.parent().attr("typeShow", "row");
        }
    }
    else {
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").removeClass("display-none");
        $("[data-field=QRCodeField] .qrcode-parent").removeClass("display-none");

    }
}

/**
 * Ẩn hiện qr code với biên lai K80 
 * tqha (25/8/2022)
 * @param {any} isUseQRCode
 */
function ChangeQRCodeReceipt(isUseQRCode) {
    if (!isUseQRCode) {
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").addClass("display-none");
        var $logo = $(".logo-template-content");
        var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
        if (!hasLogo) {
            $("[group-field=other-invoice]").css("justify-content", "flex-end");
        }
    }
    else {
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").removeClass("display-none");
        $("[data-field=QRCodeField] .qrcode-parent").removeClass("display-none");

    }
}

//Update lại mẫu só và ký hiệu từ control vào mẫu
//created by vmthanh 11/08/2019
function updateInvSeriesInvNoToTemplate(invSeries) {
    if (configCallback.isBLP()) {
        $('[data-field="TemplateCode"] .edit-value').html(invSeries.toUpperCase());
    }
    else {
        $('[data-field="InvoiceSeries"] .edit-value').html(invSeries.toUpperCase());
    }

}

function updateInvTemplateCode(templateCode) {
    if (configCallback.isBLP()) {
        $('[data-field="InvoiceSeries"] .edit-value').html(templateCode.toUpperCase());
    }
    else {
        $('[data-field="TemplateCode"] .edit-value').html(templateCode.toUpperCase());
    }

}

/**
 * Set lại số dòng detail cho mẫu
 * @param {any} maxrow
 * ndanh1 16/7/2020
 */
function setMaxRowTable(maxrow) {
    var $element = $(".tr-symbol");
    if ($element.length == 0) {
        $element = $(".tr-header").last();
    }

    var cloneElement = $(".tr-data-detail").first();
    var htmlClone = cloneElement[0].outerHTML;
    $(".tr-data-detail").remove();
    for (var i = 0; i < maxrow; i++) {
        $element.after(htmlClone);
    }
}

/*
* lấy thông tin cấu hình của hình nền - dài rộng trên dưới trái phải
* ndanh1 17/7/2020
*/
function getInfoBackground() {
    var myBg = $(".bg-template-parent"),
        result = {};
    result.width = myBg.css("width");
    result.height = myBg.css("height");
    result.top = myBg.css("top");
    result.left = myBg.css("left");
    //result.transform = GetAngle(myBg.css("transform"));
    result.transform = "";
    result.opacity = myBg.children().eq(0).css("opacity");
    return result;
}

/**
 * Lấy thông tin cấu hình của hình nền mặc định
 * ndanh1 24/7/2020
 * */
function getInfoBgDefault() {
    var myBg = $(".bg-template-default"),
        result = {};
    result.width = myBg.css("width");
    result.height = myBg.css("height");
    result.top = myBg.css("top");
    result.left = myBg.css("left");
    result.opacity = myBg.children().eq(0).css("opacity");
    return result;
}

/*
* lấy thông tin cấu hình của logo - dài rộng trên dưới trái phải
* ndanh1 17/7/2020
*/
function getInfoLogo(positionLogo) {
    var mylogo = $(".logo-left .logo-template-content"),
        result = {};
    if (!getPoistionLogo()) {
        mylogo = $(".logo-right .logo-template-content");
    }
    result.width = mylogo.css("width");
    result.height = mylogo.css("height");
    result.top = mylogo.css("top");
    result.left = mylogo.css("left");
    return result;
}

/**
 * Lấy về vị trí logo: true là bên trái; false là bên phải
 * ndanh1 17/7/2020
 * */
function getPoistionLogo() {
    return $(".logo-right").parent().hasClass("display-none");
}

/**
 * Get base64 của frame, khung
 * ndanh1 17/7/2020
 * */
function getBase64FrameTemplate() {
    var element = document.querySelector('svg');
    if (element) {
        var svg = new XMLSerializer().serializeToString(element)
        var base64 = 'data:image/svg+xml;base64,' + btoa(svg);
        return base64;
    }
    return "";
}

/**
 * Get color frame
 * ndanh1 17/7/2020
 * */
function getColorFrameTemplate() {
    return $(".frame-template svg").css("fill");
}

/**
 * Get color bacground default
 * ndanh1 17/7/2020
 * */
function getColorFromBackground() {
    return $(".bg-template-default .bg-default svg").css("fill");
}

/**
 * Người sử dụng tích chọn mẫu song ngữ
 * ndanh1 27/7/2020
 * */
function useTemplateEnglish(use) {
    if (!use) {
        $('.edit-label-en').addClass('display-none');
        $('.inv-series-no-en').removeClass('inv-series-no-en');
        $('.inv-series-no').css('width', '145px');
        $('.discount-text-en').remove();
        $('.sign-full-en').addClass('display-none');
    } else {
        $('.edit-label-en').removeClass('display-none');
        $('.inv-series-no').addClass('inv-series-no-en');
        $('.inv-series-no').css('width', '190px');
        $('[data-field=TotalAmountWithoutVAT] .edit-label')
            .parent()
            .css('width', '260px');
        var textDiscountFakeEN =
            '<div class="padding-left-4 discount-text-en display-table-cell white-space-nowrap">(Discounted)</div>';
        if (configCallback.discount) {
            $("[data-field='TotalAmountWithoutVAT'] .edit-label-en").after(
                textDiscountFakeEN
            );
        }
        $('.sign-full-en').removeClass('display-none');

        if (configCallback.useTemplateMR) {
            if (!$("[data-field=TotalAmountKKKNTFooter] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountKKKNTFooter] td:first-child .edit-label-en").html("(Not required to declare, pay VAT)");
            }
            if (!$("[data-field=TotalAmountVatKCT] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountVatKCT] td:first-child .edit-label-en").html("(VAT exemption)");
            }
            if (!$("[data-field=TotalAmountWithVAT0] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountWithVAT0] td:first-child .edit-label-en").html("(VAT rate 0%)");
            }
            if (!$("[data-field=TotalAmountWithVAT5] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountWithVAT5] td:first-child .edit-label-en").html("(VAT rate 5%)");
            }
            if (!$("[data-field=TotalAmountWithVAT8] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountWithVAT8] td:first-child .edit-label-en").html("(VAT rate 8%)");
            }
            if (!$("[data-field=TotalAmountWithVAT10] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountWithVAT10] td:first-child .edit-label-en").html("(VAT rate 10%)");
            }
            if (!$("[data-field=TotalAmountVATKHACFooter] td:first-child .edit-label-en").html()) {
                $("[data-field=TotalAmountVATKHACFooter] td:first-child .edit-label-en").html("(Other VAT rates)");
            }
        }

        if (configCallback.isTVT()) {
            var sellerSize = Number.parseInt($("[data-field=SellerSignByClient] .edit-label").css("font-size").replace("px", ""));
            if (sellerSize > 20) {
                handleChangeStyleSignBlock(20, "SellerSignContent", 20);
            }

            var taxSize = Number.parseInt($("[data-field=TaxCodeSignDate] .edit-label").css("font-size").replace("px", ""));
            if (taxSize > 20) {
                handleChangeStyleSignBlock(20, "TaxCodeSignContent", 20);
            }
        }
    }

    resetAlignmentBuyerInfor($('[group-field=buyer-infor] [data-field]'), 1, true);
    resetAlignmentSellerInfor($('[group-field=seller-infor] [data-field]'), 1, true);
    resizeTable("#tbFooter", true);
    reloadNewWidthTable($("#tbFooter tr td"));
    reloadAlignSeller();
    reloadAlignBuyer();
}

/**
 * Thêm chữ ký mới
 * ndanh1 23/7/2020
 * */
function addNewSign() {
    var selectorSign = $("[sign-region=SellerSignRegion]");
    if ($("[sign-region=TaxCodeSignRegion]").length > 0) {
        selectorSign = $("[sign-region=TaxCodeSignRegion]");
    }
    if (selectorSign.length > 0) {
        resizeTable("#signXml", true);
        var widthTableNow = $("#signXml").width();
        var newFieldSign = generateDataField(".custom-sign", CUSTOM_SIGN, "sign-region");
        var elemenNewSign = '<td class="resize-width vertical-align-top custom-sign" sign-region="{0}" style="width: 120px">'
            + '<div class="edit-item {5} font-bold" data-field="{1}">'
            + '<span class="edit-label">Người ký mới</span>'
            + '<span class="edit-label-en padding-left-4 {3}">(New Sign)</span></div>'
            + '<div class="edit-item font-italic" data-field="{2}">'
            + '<div class="edit-label">(Ký, ghi rõ họ, tên)</div>'
            + '<div class="edit-label-en sign-full-en {3}">(Signature, full name)</div></div>'
            + '<div class="text-left content-sign font-bold sign-content display-none not-show-preview" data-field="{4}">'
            + '</div></td>';

        var isShow = configCallback.isVN ? "display-none" : "";
        var isEdit = (configCallback.typeOfTemplate == 10 || configCallback.typeOfTemplate == 11) ? "" : "disable-hiden";
        elemenNewSign = elemenNewSign.format(newFieldSign, newFieldSign + "Name", newFieldSign + "Full", isShow, newFieldSign + "Content", isEdit);
        selectorSign.before(elemenNewSign);
        if (configCallback.isBLP()) {
            $("[sign-region=" + newFieldSign + "]").addClass("padding-top-16");
        }
        var widthTableAfter = widthTableNow - $("[sign-region=" + newFieldSign + "]").width();
        var rate = widthTableAfter / widthTableNow;
        var signs = $("#signXml tr:first-child td:visible");
        for (var i = 0; i < signs.length; i++) {
            if ($(signs[i]).attr("sign-region") != newFieldSign) {
                $(signs[i]).css("width", ($(signs[i]).width() * rate) + "px");
            }
        }

        reloadNewWidthTable($("#signXml tr:first-child td:visible"));
        resizeTable("#signXml", false, true);

        if (configCallback.isDeliveryND123()) {
            changeStyleForSellerSignOfDelivery123();
        }
    }
}

/**
 * Xóa chữ ký
 * ndanh1 31/7/2020
 * */
function deleteSign(dataField) {
    resizeTable("#signXml", true);
    $("[data-field=" + dataField + "]").parent("[sign-region]").remove();
    reloadNewWidthTable($("#signXml tr:first-child td:visible"));
    resizeTable("#signXml", false, true);

    if (configCallback.isDeliveryND123()) {
        changeStyleForSellerSignOfDelivery123();
    }
}

/**
 * trả lại số dòng detail
 * ndanh1 16/7/2020
 * */
function returnNumberRowTable() {
    return $(".tr-data-detail").length;
}

/**
 * trả lại số lượng chữ ký
 * ndanh1 (23/7/2020)
 * */
function returnNumberSign() {
    return $("#signXml tr:first-child td:visible").length;
}

function setTypeCurrency(templateCurrency, type) {
    $(".content-currency").addClass("display-none");
    $(".exchange-rate-table").addClass("display-none");
    if (templateCurrency) {
        if (type == 1) {
            $(".content-currency").removeClass("display-none");
        } else if (type == 2) {
            $(".exchange-rate-table").removeClass("display-none");
        }
    } else {

    }
}

/**
 * set lại template theo các mode: mẫu, chuyển đổi, chiết khấu, ngoại tệ
 * ndanh1 29/7/2020
 * */
function setTypeTemplateByMode(modeTemplate, templateDiscount) {
    if (modeTemplate == 1) {
        //$('[data-field=DescriptionInvoiceClient]').removeClass('display-none');
        $('[data-field=DescriptionInvoicePaperClient]').addClass(
            'display-none'
        );
        $('[sign-region=ConverterSignRegion]').addClass('display-none');
        $('.cls-convert').addClass('display-none');
        $('.converterRegion').addClass('display-none');
    } else {
        $('[data-field=DescriptionInvoiceClient]').addClass('display-none');
        $('[data-field=DescriptionInvoicePaperClient]').removeClass(
            'display-none'
        );
        $('[sign-region=ConverterSignRegion]').removeClass('display-none');
        //$('[sign-region=ConverterSignRegion] [data-field]').removeClass(
        //    'display-none'
        //);
        $('.cls-convert').removeClass('display-none');
        $('.converterRegion').removeClass('display-none');

        var tdSign = $(".sign-region-content td");
        var width = (100 / tdSign.length) + "%";
        tdSign.each(function () {
            $(this).css("width", width);
        })

    }

    $('.discount-text').remove();
    $('.discount-text-en').remove();

    if (templateDiscount) {
        $('.discountAmount').removeClass('display-none');
        var textDiscountFake =
            '<div class="padding-left-4 discount-text display-table-cell white-space-nowrap">(Đã trừ CK)</div>';
        var textDiscountFakeEN =
            '<div class="padding-left-4 discount-text-en display-table-cell white-space-nowrap font-italic">(Discounted)</div>';
        $("[data-field='TotalAmountWithoutVAT'] .edit-label").after(
            textDiscountFake
        );
        if (!configCallback.isVN) {
            $("[data-field='TotalAmountWithoutVAT'] .edit-label-en").after(
                textDiscountFakeEN
            );
        }
    } else {
        $('.discount-text').remove();
        $('.discount-text-en').remove();
        $('.discountAmount').addClass('display-none');
    }
}

/**
 * check mẫu đã ký
 * ndanh1 31/7/2020
 * */
function checkSignalValid() {
    var value = $(".esign-date").text();
    if (value) {
        return true;
    } else {
        return false;
    }
}

/**
 * load lại Width cho table
 * @param {any} $element
 */
function reloadNewWidthTable($element) {
    var widths = [];
    for (var i = 0; i < $element.length; i++) {
        widths.push($($element[i]).width());
    }

    for (var i = 0; i < $element.length; i++) {
        $($element[i]).css("width", widths[i] + "px");
    }
}

/**
 * Lọc data trước khi lấy về
 * @param {any} str
 * danh1 29/6/2020
 */
function filterString(str) {
    return str.trim();
}

/**
 * replace all giống mấy hàm bt thôi
 * @param {any} str
 * @param {any} from
 * @param {any} to
 * danh1 29/6/2020
 */
String.prototype.replaceAll = function (from, to) {
    return this.split(from).join(to);
}

/**
 * Viết đè lại hàm format
 * danh1 29/6/2020
 * */
String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
}

/**
 * Viết đè lại trim
 * ndanh1 29/6/2020
 * */
String.prototype.trim = function () {
    return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' ');
}


//Sự kiện bỏ resize ảnh nền
//created by vmthanh 24/07/2020
function removeResizeBackground(kind) {
    if ($('.bg-template-default .ui-icon-gripsmall-diagonal-se').length > 0) {
        switch (kind) {
            case 'parent':
                $(".bg-template-parent .ui-icon-gripsmall-diagonal-se").removeClass("resizable");
                $(".bg-template-parent").removeClass('highlight-logo');
                $(".bg-template-parent").resizable('disable');
                $(".bg-template-parent").draggable({ disabled: true });
                $(".bg-template-parent").css("z-index", "-1");
                break;
            case 'default':
                $(".bg-template-default .ui-icon-gripsmall-diagonal-se").removeClass("resizable");
                $(".bg-template-default").resizable('disable');
                $(".bg-template-default").draggable({ disabled: true });
                $(".bg-template-default").removeClass('highlight-logo');
                $(".bg-template-default").css("z-index", "-1");
                break;
            case 'all':
                //if ($('.bg-template-default svg').html() && !$('.bg-template-parent .bg-template').css('background-image').includes('mau-hoa-don')) {
                $(".bg-template-parent .ui-icon-gripsmall-diagonal-se").removeClass("resizable");
                $(".bg-template-parent").removeClass('highlight-logo');
                $(".bg-template-parent").resizable('disable');
                $(".bg-template-parent").draggable({ disabled: true });
                $(".bg-template-parent").css("z-index", "-1");

                $(".bg-template-default .ui-icon-gripsmall-diagonal-se").removeClass("resizable");
                $(".bg-template-default").resizable('disable');
                $(".bg-template-default").draggable({ disabled: true });
                $(".bg-template-default").removeClass('highlight-logo');
                $(".bg-template-default").css("z-index", "-1");
                //}
                break;
        }
    }
}

//Sự kiện bỏ resize ảnh logo
//created by vmthanh 24/07/2020
function initEventResizeDraggableBackground(kind) {
    if ($('.bg-template-default .ui-icon-gripsmall-diagonal-se').length == 0) {
        resizeableBackground();
    }
    else {
        switch (kind) {
            case 'parent':
                $(".bg-template-parent").resizable('enable');
                $(".bg-template-parent").draggable({ disabled: false });
                $(".bg-template-parent").addClass('highlight-logo');
                $(".bg-template-parent .ui-icon-gripsmall-diagonal-se").addClass("resizable");
                $(".bg-template-parent").css("z-index", "99");
                break;
            case 'default':
                $(".bg-template-default").resizable('enable');
                $(".bg-template-default").draggable({ disabled: false });
                $(".bg-template-default").addClass('highlight-logo');
                $(".bg-template-default .ui-icon-gripsmall-diagonal-se").addClass("resizable");
                $(".bg-template-default").css("z-index", "99");
                break;
            case 'all':
                //if ($('.bg-template-default svg').html() && !$('.bg-template-parent .bg-template').css('background-image').includes('mau-hoa-don')) {
                $(".bg-template-default").resizable('enable');
                $(".bg-template-default").draggable({ disabled: false });
                $(".bg-template-default").addClass('highlight-logo');
                $(".bg-template-default .ui-icon-gripsmall-diagonal-se").addClass("resizable");
                $(".bg-template-default").css("z-index", "99");

                $(".bg-template-parent").resizable('enable');
                $(".bg-template-parent").draggable({ disabled: false });
                $(".bg-template-parent").addClass('highlight-logo');
                $(".bg-template-parent .ui-icon-gripsmall-diagonal-se").addClass("resizable");
                $(".bg-template-parent").css("z-index", "99");
                //}
                break;
        }
    }
}


//Sự kiện bỏ resize ảnh logo
//created by vmthanh 24/07/2020
function removeResizeLogo() {
    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    resizeTable("#firstInvoice", true);
    var itemResize = $(".logo-template-content .ui-icon-gripsmall-diagonal-se");
    if (!$('.logo-template-content').css('background-image').includes('mau-hoa-don')) {
        if ($($('.logo-template-content')[0]).children().length > 0) {
            $(".logo-template-content").resizable('disable');
            //$(".logo-template-content").css("z-index", "1");
            $(".logo-template-content").draggable({ disabled: true });
            $('.container').parent().parent().focusout();
            $("#rotate").hide();
        }
    }
    itemResize.removeClass("resizable");
    itemResize.parent().removeClass('highlight-logo');
    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    resizeTableFirstInvoice();
}


//Sự kiện bỏ resize ảnh logo
//created by vmthanh 24/07/2020
function initEventResizeDraggableLogo() {
    resizeTable("#firstInvoice", true);
    if (!$('.logo-template-content').css('background-image').includes('mau-hoa-don')) {
        if ($($('.logo-template-content')[0]).children().length == 0) {
            resizeableLogo();
        }
        else {
            $(".logo-template-content").resizable('enable');
            $(".logo-template-content").draggable({ disabled: false });
            $(".logo-template-content").addClass('highlight-logo');
            $(".logo-template .ui-icon-gripsmall-diagonal-se").addClass("resizable");
            $(".logo-template-content").css("z-index", "99");
        }
    }

    reloadNewWidthTable($("#firstInvoice tr .table-header-item:visible"));
    resizeTableFirstInvoice();
    $("#firstInvoice").addClass("bg-picked-by-user");
}

//Sự kiện check xem đang sử dụng những loại bg nào để còn bỏ sự kiện resize và dragg
//created by vmthanh 24/07/2020
function getKindForRemoveEventBackground() {
    var kind = '';

    if (!configCallback.isTVT()) {
        if (($('.bg-template-default svg').html() || !$('.bg-template-default .bg-default').css('background-image').includes('mau-hoa-don')) && $('.bg-template-parent .bg-template').css('background-image') != 'none') {
            kind = 'all';
        }
        else if ($('.bg-template-parent .bg-template').css('background-image') != 'none') {
            kind = 'parent';
        }
        else if ($('.bg-template-default svg').html() || $('.bg-template-default .bg-default').css('background-image') != 'none') {
            kind = 'default';
        }
        else {
            kind = 'nothing';
        }
    } else {
        kind = 'nothing';
    }
    return kind;
}

//Sự kiện check xem đang có thằng logo k
//created by vmthanh 24/07/2020
function isHasLogo() {
    var result = false;
    if ($('.logo-template-content').css('background-image').includes('base64')) {
        result = true
    }
    return result
}

//Sự kiện lấy thằng logo content trên mẫu
// vmthanh - 27/07/2020
function getContentLogoOnTemplate() {
    var logoContent = $('.logo-template-content').css('background-image');
    return logoContent;
}

//Sự kiện lấy thằng logo content trên mẫu
// vmthanh - 27/07/2020
function getContentBackgroundOnTemplate() {
    var backgroundContent = $('.bg-template').css('background-image');
    return backgroundContent;
}

//Set style cho thằng bg upload có sẵn ở mode sửa
// vmthanh - 28/07/2020
function setStyleBackground() {
    var $background = $('.bg-template-parent');
    $background.css('height', configCallback.dataAdvanced.BackgroundHeight);
    $background.css('width', configCallback.dataAdvanced.BackgroundWidth);
    $background.css('left', configCallback.dataAdvanced.BackgroundLeft);
    $background.css('top', configCallback.dataAdvanced.BackgroundTop);
}

//Set style cho thằng bg có sẵn ở mode sửa
// vmthanh - 28/07/2020
function setStyleBackgroundDefault() {
    var $backgroundDefault = $('.bg-template-default');
    $backgroundDefault.removeAttr('style');

    $backgroundDefault.css('height', configCallback.dataAdvanced.BackgroundDefaultHeight);
    $backgroundDefault.css('width', configCallback.dataAdvanced.BackgroundDefaultWidth);
    $backgroundDefault.css('left', configCallback.dataAdvanced.BackgroundDefaultLeft);
    $backgroundDefault.css('top', configCallback.dataAdvanced.BackgroundDefaultTop);
}

/**
 * reset style element về ban đầu
 * ndanh1 31/7/2020
 * */
function resetStyle(dataField, typeData) {
    if (configCallback.isTVT() && (dataField == "SellerSignContent" || dataField == "TaxCodeSignContent")) {
        resetStyleSignBlock(dataField);
        var item = getStyleForBlockSignTicket("", dataField);
        configCallback.updateStyleByItem(item, dataField);
    }
    else {
        var type = getTypeData(typeData);
        var $element = $("[data-field=" + dataField + "] ." + type);
        $element.removeAttr("style")
        $element.find("div").removeAttr("style");
        $element.find("span").removeAttr("style");
    }
}

/**
 * highlight element theo bước
 * vmthanh 31/7/2020
 * */
function highlightElementByStep(currentStep) {
    switch (parseInt(currentStep)) {
        case 1:
            initEventResizeDraggableLogo();
            break;
        case 2:
            initEventResizeDraggableBackground(getKindForRemoveEventBackground());
            break;
    }
}

/**
 * add border cho table footer
 * @param {any} addBorder
 */
function addBorderForTable(typeBorder, dataField, typeData) {
    resizeTable("#tbFooter", true);

    var type = getTypeData(typeData);
    var $element = $("[data-field=" + dataField + "] ." + type);
    var $td = $element.parents("td").first();

    var style = "1px solid #776b6b";
    switch (typeBorder) {
        case "full":
            $td.css("border", style);
            break;
        case "clear":
            $td.css("border", "none");
            break;
        case "left":
        case "bottom":
        case "right":
            $td.css("border-" + typeBorder, style);
            break;
    }
    $td.css("border-top", "none");

    reloadNewWidthTable($("#tbFooter td:visible"));
    resizeTableFooter();
}

/**
 * Nếu là mẫu nhiều thuế suất đặc biệt
 * ndanh1 8/5/2020
 * */
function isTableFooterMultiRates(dataField) {
    return $("[data-field=" + dataField + "]").parent().hasClass("tr-footer-header");
}

function isMultiRatesSpecial() {
    return $(".tr-multi-tax").length > 0;
}

function isMultiRatesSpecialByDataField(dataField) {
    return $("[data-field=" + dataField + "]").hasClass("tr-multi-tax");
}

/**
 * Nếu là mẫu nhiều thuế suất đặc biệt
 * ndanh1 8/5/2020
 * */
function getPositionSeller() {
    if (configCallback.isTVT() || configCallback.isBLPHasFeeK80()) {
        return 1;
    }

    if ($(".header-content-invoice .seller-infor").length > 0) {
        if ($("#invoiceInfor .seller-infor").length > 0) {
            return 3;
        }
        return 1;
    } else {
        return 2;
    }
}

function deleteCustomFieldOther(dataField) {
    $("[data-field=" + dataField + "]").remove();

    checkStepToExecute();
}

/**
 * init resize và drag cho phần chữ ký
 * ndanh1 28/8/2020
 * */
function initResizeDragForESign() {
    $("[data-field='SellerSignContent'] .content-sign").resizable({
        //maxWidth: $('[data-field="SellerSign"]').width(),
        minHeight: 65,
        minWidth: 150,
        maxHeight: 168,
        start: function (event, ui) {
        },
        stop: function (event, ui) {
        },
        resize: function (event, ui) {
            //Chặn không cho nó bị tràn ra ngoài khi resize
            var $target = $(event.target);
            var maxWidth = $target.parent().width() - 30;
            if (configCallback.isDeliveryND123()) {
                maxWidth = $("[sign-region=SellerSignRegion]").width() - 30;
            }
            $("[data-field='SellerSignContent'] .content-sign").resizable("option", "maxWidth", maxWidth);
            var offsetMinus = -(maxWidth - $target.width()) / 2;

            var container = $("[data-field='SellerSignContent'] .content-sign");
            var contentHeight = container.find("[data-field=SellerSignByClient]").height()
                + container.find("[data-field=SellerSignDateClient]").height()
                + container.find("[data-field=SellerSignByClient]").prev().height();
            var minHeight = contentHeight;
            $("[data-field='SellerSignContent'] .content-sign").resizable("option", "minHeight", minHeight);

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }
        },
        disabled: false
    });

    $("[data-field='SellerSignContent'] .content-sign").draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target);
            var offsetTop = ui.offset.top + getNumberOfPixel($target.css("margin-top"));
            var offsetLeft = ui.offset.left + getNumberOfPixel($target.css("margin-left"));

            var tdParent = $target.parents("[sign-region]");

            // Chặn trái
            var offsetLeftParent = tdParent.offset().left;
            var offsetTopParent = $('[data-field="SellerSignFull"]').offset().top + $('[data-field="SellerSignFull"]').height();
            if (offsetLeft < offsetLeftParent) {
                ui.position.left = ui.position.left + offsetLeftParent - offsetLeft;
            }

            // Chặn trên
            if (offsetTop < offsetTopParent) {
                ui.position.top = ui.position.top + offsetTopParent - offsetTop;
            }

            // Chặn dưới
            var maxTop = tdParent.offset().top + tdParent.height();
            if (configCallback.isDeliveryND51() && !$("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSign]").hasClass("display-none")) {
                maxTop = maxTop - $("[sign-region=TaxCodeSignRegion]").height();
            }
            var currentTop = offsetTop + $target.height() + 2 * getNumberOfPixel($target.css("padding-top")) + 2;
            if (currentTop > maxTop) {
                ui.position.top = ui.position.top + maxTop - currentTop;
            }

            // Chặn phải
            var maxLeft = tdParent.offset().left + tdParent.width();
            var currentLeft = offsetLeft + $target.width() + 2 * getNumberOfPixel($target.css("padding-right")) + 2;
            if (currentLeft > maxLeft) {
                ui.position.left = ui.position.left + maxLeft - currentLeft;
            }
        },
        disabled: false
    });
}

/**
 * init resize và drag cho phần chữ ký của CQT
 * tqha(13/09/2022)
 * */
function initResizeDragForTaxESign() {
    $("[data-field='TaxCodeSignContent'] .content-sign").resizable({
        //maxWidth: $('[data-field="SellerSign"]').width(),
        minHeight: 65,
        minWidth: 150,
        maxHeight: 168,
        start: function (event, ui) {
        },
        stop: function (event, ui) {
        },
        resize: function (event, ui) {
            //Chặn không cho nó bị tràn ra ngoài khi resize
            var $target = $(event.target);
            var maxWidth = $target.parent(["sign-region"]).width() - 30;
            $("[data-field='TaxCodeSignContent'] .content-sign").resizable("option", "maxWidth", maxWidth);

            var container = $("[data-field='TaxCodeSignContent'] .content-sign");
            var contentHeight = container.find("[data-field=TaxCodeSignByClient]").height()
                + container.find("[data-field=TaxCodeSignDateClient]").height()
                + container.find("[data-field=TaxCodeSignByClient]").prev().height();
            var minHeight = contentHeight;
            $("[data-field='TaxCodeSignContent'] .content-sign").resizable("option", "minHeight", minHeight);

            var offsetMinus = -(maxWidth - $target.width()) / 2;

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }
        },
        disabled: false
    });

    $("[data-field='TaxCodeSignContent'] .content-sign").draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target);
            var offsetTop = ui.offset.top + getNumberOfPixel($target.css("margin-top"));
            var offsetLeft = ui.offset.left + getNumberOfPixel($target.css("margin-left"));

            var tdParent = $target.parents("[sign-region]");

            // Chặn trái
            var offsetLeftParent = tdParent.offset().left;
            var offsetTopParent = $('[data-field="TaxCodeSignFull"]').offset().top + $('[data-field="TaxCodeSignFull"]').height();
            if (offsetLeft < offsetLeftParent) {
                ui.position.left = ui.position.left + offsetLeftParent - offsetLeft;
            }

            // Chặn trên
            if (offsetTop < offsetTopParent) {
                ui.position.top = ui.position.top + offsetTopParent - offsetTop;
            }

            // Chặn dưới
            var maxTop = tdParent.offset().top + tdParent.height();
            var currentTop = offsetTop + $target.height() + 2 * getNumberOfPixel($target.css("padding-top")) + 2;
            if (currentTop > maxTop) {
                ui.position.top = ui.position.top + maxTop - currentTop;
            }

            // Chặn phải
            var maxLeft = tdParent.offset().left + tdParent.width();
            var currentLeft = offsetLeft + $target.width() + 2 * getNumberOfPixel($target.css("padding-right")) + 2;
            if (currentLeft > maxLeft) {
                ui.position.left = ui.position.left + maxLeft - currentLeft;
            }
        },
        disabled: false
    });
}

/**
 * Chuyển từ px sang dạng số 30px => 30
 * @param {any} pixel
 */
function getNumberOfPixel(pixel) {
    if (pixel) {
        pixel = Number(pixel.replace("px", ""));
        return pixel;
    } else {
        return 0;
    }
}

/**
 * set dạng hiển thị của tên loại phí, lệ phí
 * @param {any} type
 * ndanh1 4/9/2020
 */
function changeTypeOfFeeParent(type) {
    if (type == 1) {
        $('[data-field="FeeName"]').addClass("fee-parent");
    } else {
        $('[data-field="FeeName"]').removeClass("fee-parent");
    }
}

/**
 * Trả lại dạng hiển thị của tên loại phí lệ phí
 * ndanh1 4/9/2020
 * */
function getTypeFeeParent() {
    return $('[data-field="FeeName"]').hasClass("fee-parent");
}

/**
 * return loại hình hiển thị giá trị của data-field
 * @param {any} dataField
 * ndanh1 22/2/2021
 */
function getTypeDisplayField(dataField) {
    var type = $('[data-field=' + dataField + '] .edit-value').attr("type-display");
    type = type ? type : 1;
    return type;
}

/**
 * set loại hình hiển thị lên data-field
 * @param {any} dataField
 */
function updateTypeDisplayField(dataField, type) {
    $('[data-field=' + dataField + '] .edit-value').attr("type-display", type);
}

/**
 * append thêm vào nếu chưa có nội dung thu, áp dụng cho các mẫu đã tạo trước đó
 * ndanh1 9/4/2020
 * */
function appendFeeContent() {
    if (configCallback.isBLP1LP() && $("[data-field=ContentReceipt]").length == 0) {
        var contentReceipt = '<div class="edit-item width-full float-left display-none" data-field="ContentReceipt">'
            + '<div class="edit-label white-space-nowrap display-table-cell">Nội dung thu:</div>'
            + '<div class="edit-value display-table-cell none-edit">'
            + '</div></div>';

        $("[data-field=TotalAmountWithVAT]").before(contentReceipt);
    }
}

/**
 * Thay đổi vị trí QR Code
 * @param {any} position
 */
function changePositionQRCode(position) {
    if (checkQRIsShow()) {
        var $groupField = $("[group-field=" + position + "]");
        $(".seller-clear").remove();
        $("[group-field=seller-infor]").removeClass("float-left");

        var $oldGroup = $("[group-field=seller-infor]");
        if (position == "seller-infor") {
            $oldGroup = $("[group-field=buyer-infor]");
        }

        var $qrCode = $(".qrcode-parent");

        $groupField.after($qrCode);
        $oldGroup.css("width", "100%");
        $groupField.css("width", "86%");

        if (position == "seller-infor") {
            $groupField.addClass("float-left");
            var divClear = '<div class="seller-clear clear"></div>';
            $qrCode.after(divClear);
        }
    }
}

/**
 * Get vị trí QR Code
 * ndanh1 7/9/2020
 * */
function getPositionQRCode() {
    var $qrCode = $(".qrcode-parent");
    var $prev = $qrCode.prev();

    return $prev.attr("group-field");
}

/**
 * set fee code
 * @param {any} feeCode
 */
function setFeeCode(feeCode) {
    $(".FeeCode").text(feeCode);
}

/**
 * set số tiền
 * @param {any} amount
 */
function setTotalAmountTemplate(amount) {
    $(".TotalAmountTemplate").text(amount);
}

/**
 * set custom fee
 * @param {any} dataField
 * @param {any} value
 */
function setCustomFee(dataField, value) {
    if (dataField == "TotalAmountInWords") {
        $("[data-field=TotalAmountWithVATInWords] .edit-value").text(value);
    }
    else if (dataField == "UnitCurrencyName") {
        if (configCallback.isBLPHasFeeK80()) {
            $("[mark=UnitName]").text(value);
        }
        else {
            $("[data-field=" + dataField + "] .edit-value").text(value);
        }
    }
    else {
        $("[data-field=" + dataField + "] .edit-value").text(value);
    }
}

/**
 * lấy thông tin fee
 * */
function getValueFeeInfor() {
    var item = {
        FeeCode: $(".FeeCode").text().trim(),
        FeeName: $("[data-field=FeeName] .edit-value").text().trim(),
        TotalAmount: $(".TotalAmountTemplate").text().trim(),
        InWord: $("[data-field=TotalAmountWithVATInWords] .edit-value").text().trim(),
        UnitName: $("[data-field=UnitCurrencyName] .edit-value").text().trim(),
    }

    if (configCallback.isBLPHasFeeK80()) {
        item.UnitName = $("[data-field=TotalAmountWithVAT] span[mark=UnitName]").text().trim();
    }
    return item;
}

/**
 * set text field align
 * @param {any} dataField
 * @param {any} align
 */
function setTextAlignBuyerInfor(dataField, align) {
    if ($(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").length > 0) {
        $(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").css("text-align", align);
        $(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").css("justify-content", align);
        if (align == "right") {
            $(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").find("[data-field]").removeClass("flex-1");
            $(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").find("[data-field]").first().addClass("flex-1");
        } else {
            $(".buyer-infor [data-field=" + dataField + "]").parents(".merge-row-infor").find("[data-field]").removeClass("flex-1");
        }
    } else {
        $(".buyer-infor [data-field=" + dataField + "]").css("text-align", align);
    }
}

/**
 * trigger click cho grField
 * @param {any} grField
 */
function groupFieldTriggerClick(grField) {
    $('[group-field=' + grField + ']').trigger("click");
}

/**
 * Thay đổi giá trị theo custom field
 * @param {any} dataFieldCustom
 * @param {any} displayText
 * @param {any} displayTextEN
 */
function changeValueCustomField(dataFieldCustom, displayText, displayTextEN) {
    var $element = $('[data-field=' + dataFieldCustom + ']');
    if ($element.length > 0) {
        $element.find(".edit-label").text(displayText);
        $element.find(".edit-label-en").text("(" + displayTextEN + ")");
    }
}

/**
 * Thay đổi giá vé
 * @param {any} money
 */
function changeValueTicket(money, field, moneyFormat) {
    var unitName = $(".unit-name-ticket").first().text();
    if (field == "TicketPrice") {
        $(".TicketPrice").text(money);
        $("[data-field=TicketPrice] .edit-value").text(moneyFormat);
    } else if (field == "VatRateTicket") {
        $(".VatRateTicket").text(money);
        $("[data-field=VatRateTicket] .edit-value").text(money);
        var valueRate = "";
        if (money === "") {
            valueRate = "";
        }
        else if (money >= 0) {
            valueRate = money + "%";
        } else if (money == -1 || money == -3) {
            valueRate = "X";
        }

        $("[data-field=VatRate] .edit-value").text(valueRate);
        $("[data-field=VatRateOther] .edit-value").text(valueRate);
    }
    if ($(".TicketPrice").text()) {
        var vatRate = Number($(".VatRateTicket").text());
        var totalMoney = Number($(".TicketPrice").text());
        if (vatRate < 0) {
            $("[data-field=TicketPriceBeforeVATOther] .edit-value").text(configCallback.formatNumberByCurrency($(".TicketPrice").text()));
            $("[data-field=VatPriceOther] .edit-value").text("X");
            $("[data-field=VatPriceOther] [mark=UnitName]").text("");

            $("[data-field=TicketPriceBeforeVAT] .edit-value").text(configCallback.formatNumberByCurrency($(".TicketPrice").text()));
            $("[data-field=VatPrice] .edit-value").text("X");
            $("[data-field=VatPrice] [mark=UnitName]").text("");

            $(".TicketPriceBeforeVAT").text($(".TicketPrice").text());
            $(".VatPrice").text(0);
        } else {
            var amountBeforeTax = totalMoney / (1 + (vatRate / 100));
            $("[data-field=TicketPriceBeforeVATOther] .edit-value").text(configCallback.formatNumberByCurrency(amountBeforeTax));
            $("[data-field=VatPriceOther] .edit-value").text(configCallback.formatNumberByCurrency(totalMoney - amountBeforeTax));
            $("[data-field=VatPriceOther] [mark=UnitName]").text(unitName);

            $("[data-field=TicketPriceBeforeVAT] .edit-value").text(configCallback.formatNumberByCurrency(amountBeforeTax));
            $("[data-field=VatPrice] .edit-value").text(configCallback.formatNumberByCurrency(totalMoney - amountBeforeTax));
            $("[data-field=VatPrice] [mark=UnitName]").text(unitName);

            $(".VatPrice").text(totalMoney - amountBeforeTax);
            $(".TicketPriceBeforeVAT").text(amountBeforeTax);
        }
        $("[data-field=TicketPriceOther] .edit-value").text(moneyFormat);
    }
}

function changeReduceTax43Ticket(IsTaxReduction43, AmountWithoutReduceVAT, RateTax, TaxReduction43Amount, valueRate) {
    if (IsTaxReduction43) {
        $(".IsTaxReduction43").text(IsTaxReduction43);
        (AmountWithoutReduceVAT || AmountWithoutReduceVAT == 0) ? $(".AmountWithoutReduceVAT").text(AmountWithoutReduceVAT) : "";
        if (valueRate != null && valueRate != "") {
            $(".TaxRate").text(RateTax);
        }
        else {
            $(".TaxRate").text(null);
        }
        (TaxReduction43Amount || TaxReduction43Amount == 0) ? $(".TaxReduction43Amount").text(TaxReduction43Amount) : "";
        addDescriptionForTicket43(TaxReduction43Amount);
    } else {
        $(".IsTaxReduction43").text("");
        $(".AmountWithoutReduceVAT").text("");
        $(".TaxRate").text("");
        $(".TaxReduction43Amount").text("");
        $("[data-field=DescriptionTicket43]").remove();
    }
}

function addDescriptionForTicket43(TaxReduction43Amount) {
    var formatNumber = configCallback.formatNumberByCurrency
    var description = 'Đã giảm {0} đồng, tương ứng 20% mức  tỷ lệ % để tính thuế giá trị gia tăng theo Nghị quyết số 43/2022/QH15.';
    description = description.format(formatNumber(TaxReduction43Amount))
    if ($("[data-field=DescriptionTicket43]").length > 0) {
        $("[data-field=DescriptionTicket43] .edit-value").text(description);
    } else {
        var elementStr = `<div class="disable-hiden" data-field="DescriptionTicket43">
            <span class="edit-value none-edit padding-none font-italic">{0}</span>
        </div>`;
        if (getPositionTVT() == 1) {
            $("[data-field=TicketPrice]").after(elementStr.format(description));
        } else {
            $("[data-field=TicketPriceOther]").after(elementStr.format(description));
        }
    }
}


/**
 * set border cho tem vé thẻ
 * @param {any} typeBorder
 */
function setBorderForTVT(typeBorder, dataField) {
    //var $groupField = $("[group-field=" + configCallback.customField + "]");
    var $groupField = $("[data-field=" + dataField + "]");
    switch (typeBorder) {
        case "full":
            $groupField.css("border", "2px solid #00000075");
            break;
        case "clear":
            $groupField.css("border", "none");
            break;
        case "top":
        case "left":
        case "bottom":
        case "right":
            $groupField.css("border-" + typeBorder, "2px solid #00000075");
            break;
    }
}

/**
 * show hide item by data-field
 * @param {any} dataField
 * @param {any} groupField
 * @param {any} isShow
 */
function showHideItemByDataField(dataField, groupField, isShow) {
    if (isShow) {
        $("[group-field=" + groupField + "] [data-field=" + dataField + "]").removeClass("display-none")
    } else {
        $("[group-field=" + groupField + "] [data-field=" + dataField + "]").addClass("display-none")
    }

    checkStepToExecute();
}

/**
 * show hide item by data-field
 * @param {any} dataField
 * @param {any} groupField
 * @param {any} isShow
 */
function changeValueItemByDataField(groupField, dataField, value) {
    $("[group-field=" + groupField + "] [data-field=" + dataField + "] .edit-value").text(value)
}

/**
 * ẩn, hiện đơn vị tính
 * */
function showHideUnitName(isShow) {
    if (isShow) {
        $(".unit-name-ticket").removeClass("display-none");
    } else {
        $(".unit-name-ticket").addClass("display-none");
    }
}

/**
 * set lại vị trí
 * */
function setPositionForTicketPrice(position) {
    if (position == 1) {
        if ($("[data-field=TicketPriceBeforeVATOther]").hasClass("display-none")) {
            $("[data-field=TicketPriceBeforeVAT]").addClass("display-none")
        } else {
            $("[data-field=TicketPriceBeforeVAT]").removeClass("display-none")
        }

        if ($("[data-field=VatPriceOther]").hasClass("display-none")) {
            $("[data-field=VatPrice]").addClass("display-none")
        } else {
            $("[data-field=VatPrice]").removeClass("display-none")
        }

        if ($("[data-field=IncludeTaxOther]").hasClass("display-none")) {
            $("[data-field=IncludeTax]").addClass("display-none")
        } else {
            $("[data-field=IncludeTax]").removeClass("display-none")
        }

        if ($("[data-field=VatRateOther]").hasClass("display-none")) {
            $("[data-field=VatRate]").addClass("display-none")
        } else {
            $("[data-field=VatRate]").removeClass("display-none")
        }

        $(".item-ticket-other").addClass("display-none");
        $(".ticket-price-container").removeClass("display-none");
        $("[data-field=TicketPrice]").removeClass("display-none");

        $("[data-field=TicketPrice]").after($('[data-field="DescriptionTicket43"]'));
    } else {
        if ($("[data-field=TicketPriceBeforeVAT]").hasClass("display-none")) {
            $("[data-field=TicketPriceBeforeVATOther]").addClass("display-none")
        } else {
            $("[data-field=TicketPriceBeforeVATOther]").removeClass("display-none")
        }

        if ($("[data-field=IncludeTax]").hasClass("display-none")) {
            $("[data-field=IncludeTaxOther]").addClass("display-none")
        } else {
            $("[data-field=IncludeTaxOther]").removeClass("display-none")
        }

        if ($("[data-field=VatPrice]").hasClass("display-none")) {
            $("[data-field=VatPriceOther]").addClass("display-none")
        } else {
            $("[data-field=VatPriceOther]").removeClass("display-none")
        }

        if ($("[data-field=VatRate]").hasClass("display-none")) {
            $("[data-field=VatRateOther]").addClass("display-none")
        } else {
            $("[data-field=VatRateOther]").removeClass("display-none")
        }

        $(".ticket-price-container").addClass("display-none");
        $("[data-field=TicketPriceOther]").removeClass("display-none");

        $("[data-field=TicketPriceOther]").after($('[data-field="DescriptionTicket43"]'));
    }
}

/**
 * Xét hình thức hiển thị tổng tiền cho bl k80
 * tqha(9/8/2022)
 * @param {any} position
 */
function setPositionForReceiptPrice(position) {
    if (position == 1) {
        $("[data-field=TotalAmountWithVAT]").css("display", "");
        $("[data-field=TotalAmountWithVAT]").css("width", "");
        $("[data-field=TotalAmountWithVAT] .edit-value").css("flex", "");
        $("[data-field=TotalAmountWithVAT] .edit-value").css("text-align", "center");
    } else {
        $("[data-field=TotalAmountWithVAT]").css("display", "flex");
        $("[data-field=TotalAmountWithVAT]").css("width", "100%");
        $("[data-field=TotalAmountWithVAT] .edit-value").css("flex", "1");
        $("[data-field=TotalAmountWithVAT] .edit-value").css("text-align", "right");
    }
}

/**
 * Lấy thiết lập vị trí tổng tiền của
 * */
function getPositionTotalAmountK80() {
    var position = 1;
    var style = $("[data-field=TotalAmountWithVAT] .edit-value").css("flex");
    if (style != "" && style[0] == "1") {
        position = 2;
    }
    return position;
}

/**
 * Thay đổi đơn vị
 * @param {any} unit
 */
function changeUnitNameTicket(unit) {
    $(".unit-name-ticket").text(unit);
    $("[data-field=UnitName] .edit-value").text(unit);
}

/**
 * Get vị trí hiện tại của tem vé
 * */
function getPositionTVT() {
    if ($(".ticket-price-container").hasClass("display-none")) {
        return 2;
    }

    return 1;
}

/**
 * get config value theo datafield
 * @param {any} dataField
 */
function getItemValueConfig(dataField) {
    var $dataField = $("[data-field=" + dataField + "]");
    var $value = $dataField.find(".edit-value");
    var $label = $dataField.find(".edit-label");
    var $labelEN = $dataField.find(".edit-label-en");

    var label = {
        value: $label.text() ? $label.text().trim().replace(":", "") : "",
        isShow: $label.length > 0 && !$label.hasClass("display-none"),
        canEdit: !$label.hasClass("none-edit"),
    }

    var labelEN = {
        value: $labelEN.text() ? $labelEN.text().trim().replace(":", "") : "",
        isShow: $labelEN.length > 0 && !$labelEN.hasClass("display-none"),
        canEdit: !$labelEN.hasClass("none-edit"),
    }

    var value = {
        value: $value.text() ? $value.text().trim().replace(":", "") : "",
        isShow: ($value.length > 0 && !$value.hasClass("display-none")),
        canEdit: !$value.hasClass("none-edit"),
        placeholder: "",
        hasValue: $value.length > 0,
    }

    var item = {
        value: value,
        label: label,
        labelEN: labelEN,
    };

    return item;
}

/**
 * set vị trí align của text
 * @param {any} dataField
 * @param {any} align
 */
function setTextAlignTicket(dataField, align) {
    $("[data-field=" + dataField + "]").css("text-align", align);
}

/**
 * set vị trí align của text vùng thông tin người bán
 * @param {any} dataField
 * @param {any} align
 */
function setTextAlignSeller(dataField, align) {
    $("[data-field=" + dataField + "]").css("display", 'flex');
    $("[data-field=" + dataField + "]").css("align-items", 'baseline');
    $("[data-field=" + dataField + "]").css("text-align", align);
    let minLength = 0;
    if ($("[data-field=" + dataField + "] .edit-label").css("min-width") != "0px"
        && $("[data-field=" + dataField + "] .edit-label").css("min-width") != "unset") {
        minLength += getWidth($("[data-field=" + dataField + "] .edit-label"));
    }
    else minLength += $("[data-field=" + dataField + "] .edit-label").text().length * 8;
    minLength += $("[data-field=" + dataField + "] .two-dot").text().length * 8;
    if ($("[data-field=" + dataField + "] .edit-label-en").is(':visible')) {
        minLength += $("[data-field=" + dataField + "] .edit-label-en").text().length * 8;
    }
    let maxLengthValue = $("[data-field=" + dataField + "]").width() - minLength;
    if (align == 'center') {
        $("[data-field=" + dataField + "]").css("justify-content", 'center');
        $("[data-field=" + dataField + "] > .edit-value").css("max-width", maxLengthValue + "px");
    }
    else if (align == 'right') {
        $("[data-field=" + dataField + "]").css("justify-content", 'flex-end');
        $("[data-field=" + dataField + "] > .edit-value").css("max-width", maxLengthValue + "px");
    }
    else {
        $("[data-field=" + dataField + "]").css("display", '');
        $("[data-field=" + dataField + "]").css("align-items", '');
        $("[data-field=" + dataField + "]").css("justify-content", '');
        $("[data-field=" + dataField + "] > .edit-value").css("max-width", "");
    }

}

/**
 * Check mẫu a4 ngang
 * */
function isA4N() {
    return $("[PAPER='A4-HORIZONTAL']");
}

function checkStepToExecute() {
    if (configCallback.getCurrentStep() == "4") {
        if ($("[group-field=buyer-infor] [data-field]:visible").length <= 0) {
            if ($(".div-fake-infor").length == 0) {
                var divFake = "<div class='div-fake-infor center' style='padding: 10px 0;cursor: pointer; font-style: italic; background: aliceblue' > Nhấn vào đây để bổ sung thêm thông tin </div>";
                $("[group-field=buyer-infor]").find(".clear").remove();
                $("[group-field=buyer-infor]").append(divFake);
                $("[group-field=buyer-infor]").append("<div class='clear'></div>");
            }
        } else {
            $(".div-fake-infor").remove();
        }

        if (configCallback.isCashRegister()) {
            if ($("[group-field=sign-xml] [data-field]:visible").length <= 0) {
                if ($(".div-fake-sign").length == 0) {
                    var divFake = "<div class='div-fake-sign center' style='padding: 10px 0;cursor: pointer; font-style: italic; background: aliceblue; text-align: left; width: {0}px;' > Nhấn vào đây để bổ sung thêm thông tin </div>";
                    if (configCallback.isA5()) {
                        divFake = divFake.format("790");
                    }
                    else {
                        divFake = divFake.format("775");
                    }
                    
                    $("[group-field=sign-xml]").append(divFake);
                }
            }
            else {
                $(".div-fake-sign").remove();
            }
        }
    } else {
        $(".div-fake-infor").remove();
        $(".div-fake-sign").remove();
    }
}

/**
 * update các thông tin: tiền vé trước thuế, tiền vé
 * */
function updateForTicketPrice(dataField, number, numberValue) {
    var money = "";
    var formatNumber = configCallback.formatNumberByCurrency;
    if (number) {
        money = Number(number);
    }

    if (dataField == "VatPrice" || dataField == "VatPriceOther") {
        $(".VatPrice").text(money);
        $("[data-field=VatPrice] .edit-value").text(numberValue);
        $("[data-field=VatPriceOther] .edit-value").text(numberValue);
    } else if (dataField == "TicketPriceBeforeVAT" || dataField == "TicketPriceBeforeVATOther") {
        $(".TicketPriceBeforeVAT").text(money);
        $("[data-field=TicketPriceBeforeVAT] .edit-value").text(numberValue);
        $("[data-field=TicketPriceBeforeVATOther] .edit-value").text(numberValue);

        var ticketPrice = $(".TicketPrice").text();
        if (!money) {
            money = 0;
        } else {
            money = Number(money);
        }

        if (!ticketPrice) {
            ticketPrice = 0
        } else {
            ticketPrice = Number(ticketPrice);
        }

        var vatPrice = ticketPrice - money;
        $(".VatPrice").text(vatPrice);
        $("[data-field=VatPrice] .edit-value").text(formatNumber(vatPrice));
        $("[data-field=VatPriceOther] .edit-value").text(formatNumber(vatPrice));

        var listDataField = ["VatPrice"];
        if (dataField == "TicketPriceBeforeVATOther") {
            listDataField = ["VatPriceOther"];
        }
        var listConfig = getConfigData("", listDataField);
        configCallback.updateItemBlockByDataFieldConfig(listConfig[0]);
    }
}

/**
 * check lại tổng tiền
 * ndanh1 18/1/2021
 * */
function checkTotalAmountTicket() {
    var formatNumber = configCallback.formatNumberByCurrency;

    var priceBeforeVat = Number($(".TicketPriceBeforeVAT").text());
    var vatPrice = Number($(".VatPrice").text());
    var vatRate = $(".VatRateTicket").text();
    var vatRateText = "";

    if (vatRate == "") {
        vatRate = "";
        vatRateText = ""
    }
    else if (vatRate == "-1" || vatRate == "-3") {
        vatRate = 0;
        vatRateText = 0;
    } else {
        vatRate = Number($(".VatRateTicket").text());
        vatRateText = vatRate + "%";
    }

    var ticketPrice = Number($(".TicketPrice").text());
    var msg = "";

    if (vatRate != "" && formatNumber(priceBeforeVat * (vatRate / 100)) != formatNumber(vatPrice)) {
        msg = "Tiền thuế GTGT <{0}> khác với Số tiền chưa thuế <{1}> * Thuế suất GTGT <{2}> </br>";
        msg = msg.format(formatNumber(vatPrice), formatNumber(priceBeforeVat), vatRateText);
    }

    if (formatNumber(priceBeforeVat + vatPrice) != formatNumber(ticketPrice)) {
        msg += "Tổng tiền <{0}> khác với Số tiền chưa thuế <{1}> + Tiền thuế GTGT <{2}> </br>";
        msg = msg.format(formatNumber(ticketPrice), formatNumber(priceBeforeVat), formatNumber(vatPrice));
    }

    if (priceBeforeVat > ticketPrice) {
        msg = "Số tiền chưa thuế <{0}> không được lớn hơn Tổng tiền <{1}>.NotGreaterTotalPrice";
        msg = msg.format(formatNumber(priceBeforeVat), formatNumber(ticketPrice));
    }

    return msg;
}

/**
 * Get vị trí tổng tiền
 * ndanh1 22/1/2021
 * */
function checkPositionTicketPrice() {
    return $(".ticket-price-container").hasClass("display-none") ? 2 : 1;
}

/**
 * get style unitName
 * ndanh1 27/1/2021
 * */
function getStyleUnitName() {
    var arrStyle = [];
    var lstUnitName = $(".unit-name-ticket");
    for (var i = 0; i < lstUnitName.length; i++) {
        var style = $(lstUnitName[i]).attr("style");
        arrStyle.push(style);
    }
    return arrStyle;
}

/**
 * get url image
 * ndanh1 17/2/2021
 * */
function getUrlBackgroundImage(element) {
    var bg = $(element).css("background-image");
    bg = bg.replace('url(', '').replace(')', '').replace(/\"/gi, "");
    return bg;
}

/**
 * Update thông tin tuyến đường
 * @param {any} routeDetail
 */
function updateRouteDetail(routeDetail) {
    if (routeDetail) {
        $("[data-field=Route] .edit-value").text(routeDetail.RouteName);
        $("[data-field=From] .edit-value").text(routeDetail.From);
        $("[data-field=Destination] .edit-value").text(routeDetail.Destination);
    } else {
        $("[data-field=Route] .edit-value").text("");
        $("[data-field=From] .edit-value").text("");
        $("[data-field=Destination] .edit-value").text("");
    }

}

/**
 * Kiểm tra 3 thông tin tuyến đường, bến đến, bến đi xem có đồng thời bị ẩn không ?
 * ndanh1 23/2/2021
 * */
function checkRouteFromDestination() {
    if ($("[data-field=Route]").hasClass("display-none") && $("[data-field=From]").hasClass("display-none") && $("[data-field=Destination]").hasClass("display-none")) {
        return false;
    }

    return true;
}

/**
 * 
 * */
function checkIsDeleteBackground() {
    if ($(".bg-template").css("background-image") === 'none') {
        return true;
    }

    return false;
}

function checkIsDeleteLogo() {
    if ($(".logo-template-content").css("background-image") === 'none') {
        return true;
    }

    return false;
}

function changeTitleInvoice(show) {
    var div = $("[data-field=NonTariffArea]");
    if (show) {
        div.removeClass("display-none");

    }
    else {
        div.addClass("display-none");
    }
}

function changeWrapText(status, dataField, typeChange) {
    var field = $(`[data-field=${dataField}] .${typeChange}`)
    if (status) {
        field.css("white-space", "unset");
        //field.removeClass("display-table-cell");
        if (configCallback.isTVT() && dataField == "TotalAmountInWords") {
            $(`[data-field=${dataField}] .#en-amount-inword`).css("white-space", "unset");
        }
    }
    else {
        field.css("white-space", "nowrap");
        //field.addClass("display-table-cell");
        if (configCallback.isTVT() && dataField == "TotalAmountInWords") {
            $(`[data-field=${dataField}] .#en-amount-inword`).css("white-space", "nowrap");
        }
    }
}

function changeWordBreak(status, dataField, typeChange) {
    var field = $(`[data-field=${dataField}] .${typeChange}`)
    if (status) {
        field.css("word-break", "break-all");
    }
    else {
        field.css("word-break", "unset");
    }
}


function addNewRedutionTax43() {
    var tax10 = $('[data-field=TotalAmountWithVAT10]');
    var tax8 = $('[data-field=TotalAmountWithVAT8]');
    if (tax10.length > 0 && tax8.length < 1) {
        var newTax = `<tr class="tr-multi-tax tr-footer-data display-none" data-field="TotalAmountWithVAT8">
            <td class="text-left" style="width: 231.188px;">
                <div class="edit-label none-edit display-table-cell white-space-nowrap">Tổng tiền chịu thuế suất 8%</div>
                <div class="two-dot display-table-cell white-space-nowrap">:</div>
            </td>
            <td class="number" style="width: 192.5px;">
                <div class="padding-right-2">
                </div>
            </td>
            <td class="number" style="width: 192.5px;">
                <div class="padding-right-2">
                </div>
            </td>
            <td class="number" style="width: 153.812px;">
                <div class="padding-right-2">
                </div>
            </td>
        </tr>`;
        $('[data-field=TotalAmountWithVAT10]').before(newTax);
    }
}

/**
 * Thay đổi tên dịch vụ 
 * @param {string} serviceName: tên dịch vụ
 * tqha
 */
function changeServiceName(serviceName) {
    var service = $("[data-field=ServiceName]");
    service.find(".edit-label").html(serviceName);
}

/**
 * Ẩn phần chữ ký người bán
 * tqha
 * */
function disableHiddenSellerSign(disabled) {
    if (disabled) {
        $("[data-field=SellerSignContent]").addClass("disable-hiden");
    }
    else {
        $("[data-field=SellerSignContent]").removeClass("disable-hiden");
    }
}

/**
 * Thay đổi cách hiển thị số vé
 * @param {boolean} show: hiển thị đầy đủ (false), rút gọn(true)
 * tqha
 */
function changeMethodShowInvoiceNumber(show) {
    if (show) {
        $("[data-field=InvoiceNumber]").addClass("show-only-number");
        $("[data-field=InvoiceNumber] .edit-value").html("0");
    }
    else {
        $("[data-field=InvoiceNumber]").removeClass("show-only-number");
        $("[data-field=InvoiceNumber] .edit-value").html("00000000");
    }
}

/**
 * Kiểm tra loại hiển thị số vé
 * tqha
 * */
function checkMethodShowNumber() {
    return $("[data-field=InvoiceNumber]").hasClass("show-only-number");
}

/**
 * Kiểm tra đang hiển thị điểm đến của mẫu vận tải
 * tqha
 * */
function checkDisplayRoute() {
    if ($("[data-field=Route]").css("display") == "none") {
        if ($("[data-field=From]").css("display") == "none" || $("[data-field=Destination]").css("display") == "none") {
            return false;
        }
    }
    return true;
}
/**
 * Kiểm tra có đang hiển thị dòng tồng tiền chịu thuế suất 8%
 * tqha
 * */
function checkDisplayVAT8() {
    return $("[data-field=TotalAmountWithVAT8]").css("display") != "none";
}

/**
 * Thêm nội dung tiếng anh cho dòng tổng tiền chịu ...
 * tqha
 */
function addBonusLabelENMR() {
    var enDiv = "";
    if (configCallback.isVN) {
        enDiv = '<div class="edit-label-en display-none display-table-cell white-space-nowrap"></div>';
    }
    else {
        enDiv = '<div class="edit-label-en display-table-cell white-space-nowrap"></div>';
    }
    $(enDiv).insertAfter($("[data-field=TotalAmountKKKNTFooter] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountVatKCT] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountWithVAT0] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountWithVAT5] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountWithVAT8] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountWithVAT10] td:first-child .edit-label"));
    $(enDiv).insertAfter($("[data-field=TotalAmountVATKHACFooter] td:first-child .edit-label"));

    //Bỏ none-edit
    $("[data-field=TotalAmountKKKNTFooter] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountVatKCT] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountWithVAT0] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountWithVAT5] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountWithVAT8] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountWithVAT10] td:first-child .edit-label").removeClass("none-edit");
    $("[data-field=TotalAmountVATKHACFooter] td:first-child .edit-label").removeClass("none-edit");
}

/**
 * Chỉnh lại kích thước ô table footer (trường hợp khách hàng chỉnh độ rộng mỗi 1 chiều)
 * tqha (1/4/2022)
 * */
function updateTableFooter() {
    //Bỏ những mẫu nhiều thuế suất: không care
    if (!configCallback.useTemplateMR) {
        var tableF = $("#tbFooter");
        var tdObject = [];
        if (tableF.length == 1) {
            var tr = tableF.find("tr");
            if (tr.length > 2) {
                if (tr.eq(0).css("display") != "none") {
                    var td = tr.eq(0).find("td");
                    for (var i = 0; i < td.length; i++) {
                        tdObject.push(td.eq(i).css("width"));
                    }
                }
                else {
                    var td = tr.eq(1).find("td");
                    for (var i = 0; i < td.length; i++) {
                        tdObject.push(td.eq(i).css("width"));
                    }
                }

                for (var k = 0; k < tr.length; k++) {
                    var tD = tr.eq(k).find("td");
                    if (tD.length == 4) {
                        for (var l = 0; l < tD.length; l++) {
                            tD.eq(l).css("width", tdObject[l]);
                        }
                    }
                }
            }
        }
    }

}

function changePreviewWhenChooseInvoiceType(isInvoiceWithCode, invSeries) {
    $("[data-field=InvoiceSeries] .edit-value").text(invSeries);
    var codeCQTHML = "";
    if (configCallback.isTVT()) {
        codeCQTHML = `<div class="disable-hiden text-left" data-field="InvoiceCode" style="">
                        <div class="edit-label white-space-nowrap display-table-cell">Mã CQT cấp</div>
                        <div class="edit-label-en white-space-nowrap display-table-cell padding-left-4 display-none">(Code)</div>
                        <div class="two-dot white-space-nowrap display-table-cell">:</div>
                        <div class="edit-value display-table-cell font-bold style-title padding-none none-edit">
                     </div>`;
        if (!configCallback.isVN) {
            codeCQTHML = `<div class="disable-hiden text-left" data-field="InvoiceCode" style="">
                        <div class="edit-label white-space-nowrap display-table-cell">Mã CQT cấp</div>
                        <div class="edit-label-en white-space-nowrap display-table-cell padding-left-4">(Code)</div>
                        <div class="two-dot white-space-nowrap display-table-cell">:</div>
                        <div class="edit-value display-table-cell font-bold style-title padding-none none-edit">
                     </div>`;
        }
    }
    else {
        codeCQTHML = `<div class="center disable-hiden" data-field="InvoiceCode" style="">
                        <span class="edit-label white-space-nowrap font-italic" style="white-space:nowrap">Mã CQT</span>
                        <span class="edit-label-en white-space-nowrap padding-left-4 display-none" style="">(Code)</span>
                        <span class="two-dot white-space-nowrap font-italic" style="">:</span><span class="edit-value font-italic padding-left-4 break-all none-edit" style=""></span>
                     </div>`;
        if (!configCallback.isVN) {
            codeCQTHML = `<div class="center disable-hiden" data-field="InvoiceCode" style="">
                        <span class="edit-label white-space-nowrap font-italic" style="white-space:nowrap">Mã CQT</span>
                        <span class="edit-label-en white-space-nowrap padding-left-4" style="">(Code)</span>
                        <span class="two-dot white-space-nowrap font-italic" style="">:</span><span class="edit-value font-italic padding-left-4 break-all none-edit" style=""></span>
                     </div>`;
        }
    }
    if (isInvoiceWithCode) {
        $("[data-field=InvoiceCode]").remove();
        if (configCallback.isTVT()) {
            $(codeCQTHML).insertBefore($("[data-field=SellerName]"))
            $('[data-field="SellerSignContent"]').removeClass('display-none');
            $('[data-field="TaxCodeSignContent"]').removeClass('not-show-preview');
            $('[data-field="TaxCodeSignContent"]').removeClass('display-none');
            if (configCallback.isToStep5) {
                resizeDragForTicketSign();
            }
        }
        else {
            $("[group-field=invoice-infor]").append(codeCQTHML);
        }
    } else {
        $("[data-field=InvoiceCode]").remove();
        if (configCallback.isTVT()) {
            $('[data-field="TaxCodeSignContent"]').addClass('not-show-preview');
            $('[data-field="TaxCodeSignContent"]').addClass('display-none');
        }
    }
}

/**
 * Mở thiết lập ẩn hiên thông tin Số xe, Số ghế, Giờ khởi hành với vé vận tải
 * tqha (8/6/2022)
 * */
function enableHiddenSomeField() {
    $("[data-field=Seat]").removeClass("disable-hiden");
    $("[data-field=VehicleNo]").removeClass("disable-hiden");
    $("[data-field=DepatureDateTime]").removeClass("disable-hiden");
}

function changeTotalAmountInWordEN(sayMoney) {
    $("#en-amount-inword").html("(" + sayMoney + ")");

}

/**
 * Add thêm trường tiếng anh cho các datafield chưa có
 * tqha(21/6/2022)
 * */
function addLabelENForTicket() {
    if ($("#en-amount-inword").length > 0) {
        $("#en-amount-inword").remove();
    }
    var divTotalAmountInWordEN = '<br style="display: contents"/><div id="en-amount-inword" class="white-space-nowrap edit-lable-en font-bold font-italic font-size-en totalAmountEN display-none" style="white-space: normal;font-size:12px"></div >';
    $(divTotalAmountInWordEN).insertAfter($("[data-field=TotalAmountInWords]").find(".edit-value"));
    //Cập nhật styel cho dòng tổng tiền viết bằng song ngữ dựa theo style của dòng tổng tiền
    let value = $("[data-field=TotalAmountInWords] .edit-value");
    let fontWeight = value.css("font-weight"),
        fontStyle = value.css("font-style"),
        fontSize = value.css("font-size"),
        lineHeight = value.css("line-height"),
        whiteSpace = value.css("white-space");

    fontSize = fontSize ? Number.parseInt(fontSize) - 1 + "px" : "12px";
    lineHeight = lineHeight ? Number.parseInt(lineHeight) - 1 + "px" : "12px";

    $("#en-amount-inword").css("font-weight", fontWeight);
    $("#en-amount-inword").css("font-style", fontStyle);
    $("#en-amount-inword").css("font-size", fontSize);
    $("#en-amount-inword").css("line-height", lineHeight);
    $("#en-amount-inword").css("white-space", whiteSpace);
    if (configCallback.editMode != 1) {
        var data = JSON.parse(configCallback.data.OtherInfo);
        var ticketPrice = data.filter(e => e.FieldName == "TicketPrice");
        if (ticketPrice.length > 0) {
            var money = ticketPrice[0].CustomConfigValue;
            if (money) {
                if (configCallback.totalAmountInWordEN) {
                    configCallback.sayMoney(money);
                    if ($("[data-field=TotalAmountInWords]").css("display") != "none") {
                        $("#en-amount-inword").css("display", "contents");
                        $("[data-field=TotalAmountInWords] .edit-value").css("display", "contents");
                        $("#en-amount-inword").prev().css("display", "");
                    }

                }

            }
        }
    }

    if ($("[data-field=ServiceName]").find(".edit-label-en").length == 0) {
        $('<span class="edit-label-en display-none"></span>').insertAfter($("[data-field=ServiceName]").find(".edit-label"))
    }

    $("[data-field=InforWebsiteSearch]").find(".edit-label-en").removeClass("none-edit");

    var $ticketTransaction = $("[data-field=TransactionID]").find(".edit-label-en");
    if ($ticketTransaction.length > 0) {
        var divLabelEN = '<span class="edit-label-en display-none">{0}</span>';
        $(divLabelEN.format("")).insertAfter($("[data-field=TimeIssue]").find(".edit-label"))

        $("[data-field=TransactionFake]").html("");
        var divTransaction = '<span class="edit-label">Mã tra cứu</span ><span class="edit-label-en display-none">(Invoice code)</span><span class="two-dot display-none">:</span><span class="edit-value"></span>';
        $("[data-field=TransactionFake]").html(divTransaction);
        $("[data-field=TransactionID]").find(".edit-label-en").remove();

        $(divLabelEN.format("(Total amount)")).insertAfter($("[data-field=TicketPrice]").find(".edit-label"))

        $("[data-field=IncludeTax]").css("display", "flex");
        $("[data-field=IncludeTax]").css("flex-direction", "column");
        if (configCallback.getBusinessArea() == "23") {
            $("[data-field=TypeInvoice]").find(".edit-label-en").html("(ENTRANCE TICKET)")
            $(divLabelEN.format("(VAT included)")).insertAfter($("[data-field=IncludeTax]").find(".edit-label"))
        }
        else if (configCallback.getBusinessArea() == "24") {
            $("[data-field=TypeInvoice]").find(".edit-label-en").html("(CASH TICKET)")
            $(divLabelEN.format("(VAT included)")).insertAfter($("[data-field=IncludeTax]").find(".edit-label"))
        }
        else {
            $("[data-field=TypeInvoice]").find(".edit-label-en").html("(PASSENGER CAR TICKET)")
            $(divLabelEN.format("(VAT and insurance included)")).insertAfter($("[data-field=IncludeTax]").find(".edit-label"))
            $("[data-field=DepatureDateTime]").find(".edit-label-en").html("(Departure time)");
        }

        var contentETicket = $(".description-invoice").find(".edit-label-en").html().replaceAll("Invoice", "Ticket");
        contentETicket = contentETicket.replaceAll("invoice", "ticket");
        $(".description-invoice").find(".edit-label-en").html(contentETicket)

        var contentConvert = $(".description-invoice-client").find(".edit-label-en").html().replaceAll("Invoice", "Ticket");
        contentConvert = contentConvert.replaceAll("invoice", "ticket");
        $(".description-invoice-client").find(".edit-label-en").html(contentConvert)

        $("[data-field=Route]").find(".edit-label-en").html("(Route)");
        $("[data-field=From]").find(".edit-label-en").html("(From)");
        $("[data-field=Destination]").find(".edit-label-en").html("(To)");
        $("[data-field=VehicleNo]").find(".edit-label-en").html("(License plates)");

        $("[data-field=TotalAmountInWords]").find(".edit-label-en").html("(In words)");
        $("[data-field=TicketPriceBeforeVAT]").find(".edit-label-en").html("(Amount)");
        $("[data-field=VatPrice]").find(".edit-label-en").html("(VAT amount)");
        $("[data-field=ConverterDateSign]").find(".edit-label-en").html("(Converted date)");
        $("[data-field=TaxCodeSignDate]").find(".edit-label-en").html("(Signing date)");
        $("[data-field=SellerSignDateClient]").find(".edit-label-en").html("(Signing date)");
        $("[data-field=SellerSignDate]").find(".edit-label-en").html("(Signing date)");
        $("[data-field=SellerSignBy]").find(".edit-label-en").html("(Signed by)");
        $("[data-field=SellerSignByClient]").find(".edit-label-en").html("(Signed by)");
        $("[data-field=TaxCodeSign]").find(".edit-label-en").html("(Signed by)");
        $("[data-field=SellerAddress]").find(".edit-label-en").html("(Add)");
        $("[data-field=InforWebsiteSearch]").find(".edit-label-en").html("");

        $("body .edit-label-en").css("font-style", "italic");
        $("body .edit-label-en").css("font-size", "13px");
        $("body .edit-label-en").css("padding-left", "4px");



        // Loop through the stylesheets...
        $.each(document.styleSheets, function (_, sheet) {
            // Loop through the rules...
            var keepGoing = true;
            $.each(sheet.cssRules || sheet.rules, function (index, rule) {
                // Is this the rule we want to delete?
                if (rule.selectorText === ".edit-label-en") {
                    // Yes, do it and stop looping
                    sheet.deleteRule(index);
                    return keepGoing = false;
                }
            });
            return keepGoing;
        });
    }
}

/**
 * Thiết lập vị trí Ký hiệu, số vé khi chọn song ngữ
 * tqha(21/6/2022)
 * @param {any} type
 */
function changePositionTicketNumber(type) {
    var $invSeri = $($("[data-field=InvoiceSeries]")[0]);
    var $inword = $("[data-field=TotalAmountInWords]");
    var $logo = $(".logo-template-content");
    var $dateInvoice = $("[data-field=DateInvoice]");

    var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
    if (type == 0) {
        if (!hasLogo) {
            $invSeri.parent().css("flex-direction", "row");
            $invSeri.parent().attr("typeShow", "row");
        }
        $inword.find(".edit-value").css("float", "");
        $("#en-amount-inword").css("display", "none");

    }
    else {
        if (!hasLogo) {
            $invSeri.parent().css("flex-direction", "column");
            $invSeri.parent().attr("typeShow", "column");
        }
        $inword.find(".edit-value").css("float", "left");

        if ($("[data-field=TotalAmountInWords]").css("display") != "none") {
            if (configCallback.totalAmountInWordEN) {
                $("#en-amount-inword").css("display", "contents");
                $("[data-field=TotalAmountInWords] .edit-value").css("display", "contents");
                $("#en-amount-inword").prev().css("display", "");
            }
            else {
                $("#en-amount-inword").css("display", "none");
            }
        }

        $dateInvoice.find(".edit-label-en").css("padding-left", "2px");
    }

}

/**
 * Ẩn hiện thông tin số tiền viết bằng chữ song ngữ
 * tqha(22/6/2022)
 * @param {any} checked
 */
function showTotalAmountInWordENTicket(checked) {
    if (checked) {
        $("#en-amount-inword").css("display", "contents");
        $("[data-field=TotalAmountInWords] .edit-value").css("display", "contents");
        $("#en-amount-inword").prev().css("display", "");
        if (configCallback.editMode != 1) {
            var dMoney = $(".TicketPrice").text();
            //Nếu rỗng thì gán lại = 0 không thì để nguyên
            dMoney = !dMoney ? 0 : dMoney;
            configCallback.sayMoneyByEditMode(Number.parseInt(dMoney));
        }
    }
    else {
        $("#en-amount-inword").css("display", "none");
        $("#en-amount-inword").prev().css("display", "none");
        $("[data-field=TotalAmountInWords] .edit-value").css("display", "");
    }
}

/**
 * Kiểm tra vị trí QR trên mẫu vé
 * tqha(22/6/2022)
 * */
function checkPositionQRTicket() {
    if ($("[data-field=QRCodeFieldTop]").length > 0 && $("[data-field=QRCodeFieldTop]").css("display") != "none") {
        return 0;
    }
    else if ($("[data-field=QRCodeField]").length > 0 && $("[data-field=QRCodeField]").css("display") != "none") {
        return 1;
    }
    else {
        return 2;
    }
}


/**
 * Thay đổi vị trí hiển thị QR code với mẫu vé
 * tqha(22/6/2022)
 * @param {any} position
 */
function changePositionQRCodeTicket(position) {
    var $invSeri = $($("[data-field=InvoiceSeries]")[0]);
    var $logo = $(".logo-template-content");
    var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
    if (position == "transaction") {
        if (!hasLogo && configCallback.isVN) {
            $invSeri.parent().css("flex-direction", "row");
            $invSeri.parent().attr("typeShow", "row");
        }
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").removeClass("display-none");
        //$("[data-field=QRCodeField] .qrcode-parent").removeClass("display-none");
        $("[data-field=QRCodeField] .qrcode").removeClass("display-none");

        resizableQRCodeDown();
    }
    else {
        if (!hasLogo) {
            $invSeri.parent().css("flex-direction", "column");
            $invSeri.parent().attr("typeShow", "column");
        }

        $("[data-field=QRCodeFieldTop]").removeClass("display-none");
        $("[data-field=QRCodeFieldTop] .qrcode-parent").removeClass("display-none");
        $("[data-field=QRCodeField]").addClass("display-none");

        resizableQRCodeTop();
    }
}

/**
 * Thay đổi vị trí qr code cho biên lai k80
 * tqha(9/8/2022)
 * @param {any} position
 */
function changePositionQRCodeReceipt(position) {
    var $invSeri = $($("[group-field=other-invoice]")[0]);
    var $logo = $(".logo-template-content");
    var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
    var positionLogo = Number.parseInt($logo.eq(1).css("width")) > 0 ? 1 : 0;
    if (position == "transaction") {
        if (!hasLogo) {
            $invSeri.css("justify-content", "flex-end");
        }
        else {
            if (positionLogo == 1) {
                $invSeri.css("justify-content", "flex-start");
            }
        }
        $("[data-field=QRCodeFieldTop]").addClass("display-none");
        $("[data-field=QRCodeField]").removeClass("display-none");
        //$("[data-field=QRCodeField] .qrcode-parent").removeClass("display-none");
        $("[data-field=QRCodeField] .qrcode").removeClass("display-none");

        resizableQRCodeDown();
    }
    else {
        if (!hasLogo) {
            $invSeri.css("justify-content", "flex-start");
        }
        else {
            if (positionLogo == 1) {
                $invSeri.css("justify-content", "center");
            }
        }

        $("[data-field=QRCodeFieldTop]").removeClass("display-none");
        $("[data-field=QRCodeFieldTop] .qrcode-parent").removeClass("display-none");
        $("[data-field=QRCodeField]").addClass("display-none");

        resizableQRCodeTop();
    }
}

/**
 * init resize cho qr top của vé
 * tqha(23/6/2022)
 * */
function resizableQRCodeTop() {
    initDraggableAndResizableQRCodeTop();
    var itemResize = $("[data-field=QRCodeFieldTop] .qrcode"),
        itemResizeParent = $("[data-field=QRCodeFieldTop] .qrcode-parent ");
    itemResizeParent.addClass('highlight-logo');

    itemResizeParent.resizable('enable');
    itemResizeParent.draggable({ disabled: false });

}

/**
 * init resize cho qr down của vé
 * tqha(23/6/2022)
 * */
function resizableQRCodeDown() {

    var curLeft = $("[data-field=QRCodeField] .qrcode").position().left;
    var curWidth = Number.parseInt($("[data-field=QRCodeField] .qrcode").css("width"));
    var curHeight = Number.parseInt($("[data-field=QRCodeField] .qrcode").css("height"));

    initDraggableAndResizableQRCodeDown();
    var itemResize = $("[data-field=QRCodeField] .qrcode"),
        itemResizeParent = $("[data-field=QRCodeField] .qrcode-parent ");
    itemResize.addClass('highlight-logo');

    itemResize.resizable('enable');
    itemResize.draggable({ disabled: false });

    itemResizeParent.find(".ui-wrapper").css("overflow", "unset");
    itemResizeParent.find(".ui-wrapper").css("left", "0px");

    itemResize.css("z-index", "1");
    itemResize.css({ "left": curLeft });
    var icon = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-se");
    icon.css({
        "left": curLeft + curWidth - 15,
        "top": curHeight - 15
    });
    var row = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-s");
    row.css({
        "left": curLeft,
        "top": curHeight
    });

    var col = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-e");
    col.css({
        "left": curLeft + curWidth,
        "top": 0
    });
}


/**
 * Thiết lập resize và dragg cho QR top
 * tqha(23/6/2022)
 * */
function initDraggableAndResizableQRCodeTop() {
    var itemQR = $("[data-field=QRCodeFieldTop] .qrcode-parent");
    itemQR.resizable({
        maxWidth: 100,
        maxHeight: 100,
        minWidth: 60,
        minHeight: 60,
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
            setTimeout(function () {
                $(event.target).data("dragging", false);
            }, 1);
        },
        resize: function (e, ui) {
            var wr = $(this).css("width");
            var hr = $(this).css("height");

            $(this).find(".qrcode").each(function (i, elm) {
                var w = Number.parseInt(wr);
                var h = Number.parseInt(hr);

                if (w < 0) {
                    w = 0
                }
                if (h < 0) {
                    h = 0
                }
                $(elm).css({
                    "width": w,
                    "height": h,
                });

            });
        },
    });
    itemQR.draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target);
            widthTD = itemQR.width();
            if ($target.width() + ui.position.left >= widthTD) {
                ui.position.left = widthTD - $target.width();
            }
            if (ui.position.left < -30) {
                ui.position.left = -30;
            }
            if ((Math.abs(ui.position.top) + ($target.height() / 2)) > 80) {
                if (ui.position.top < 0) {
                    ui.position.top = ($target.height() / 2) - 80;
                } else {
                    ui.position.top = 80 - ($target.height() / 2);
                }
            }

        },
    });
}

/**
 * Thiết lập resize và dragg cho QR down
 * tqha(23/6/2022)
 * */
function initDraggableAndResizableQRCodeDown() {
    var itemQR = $("[data-field=QRCodeField] .qrcode");
    itemQR.resizable({
        maxWidth: 400,
        minWidth: 60,
        minHeight: 60,
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
            setTimeout(function () {
                $(event.target).data("dragging", false);
            }, 1);
        },
        resize: function (e, ui) {
            var wr = itemQR.css("width");
            var hr = itemQR.css("height");
            var left = itemQR.css("left")

            var w = Number.parseInt(wr);
            var h = Number.parseInt(hr);
            var lf = Number.parseInt(left);

            var icon = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-se");
            icon.css({
                "left": lf + w - 15,
                "top": h - 15
            });
            var row = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-s");
            row.css({
                "left": lf,
                "top": h
            });

            var col = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-e");
            col.css({
                "left": lf + w,
                "top": 0
            });
        },
    });
    itemQR.draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
            setTimeout(function () {
                $(event.target).data("dragging", false);
            }, 1);

            var wr = itemQR.css("width");
            var hr = itemQR.css("height");
            var left = itemQR.css("left")

            var w = Number.parseInt(wr);
            var h = Number.parseInt(hr);
            var lf = Number.parseInt(left);

            var icon = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-se");
            icon.css({
                "left": lf + w - 15,
                "top": h - 15
            });

            var row = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-s");
            row.css({
                "left": lf,
                "top": h
            });

            var col = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-e");
            col.css({
                "left": lf + w,
                "top": 0
            });
        },
        drag: function (event, ui) {
            var $target = $(event.target), // cái ảnh nền
                $ifr = $('.container').parent().parent(),
                bodyIfr = $("body"),
                matrix = /matrix\((-?\d*\.?\d+),\s*0,\s*0,\s*(-?\d*\.?\d+),\s*0,\s*0\)/,
                maxWidth = 0, maxHeight = 0;

            lstScale = bodyIfr.css("transform").match(matrix);
            if (lstScale.length > 1) {
                curScale = parseFloat(lstScale[1])//0.87
                maxWidth = $ifr.width() / curScale;
                maxHeight = $ifr.height() / curScale;
            }

            if (maxWidth > 0 && maxHeight > 0) {
                //kéo ngang
                if (ui.position.left < 0) {
                    ui.position.left = 0;
                }
                if ($target.width() + ui.position.left >= maxWidth) {
                    ui.position.left = maxWidth - $target.width();
                }
                if (ui.position.left > 219) {
                    ui.position.left = 219;
                }
                //kéo dọc
                ui.position.top = 0;

                //if (ui.position.top < 0) {
                //    ui.position.top = 0;
                //}
                //if ($target.height() + ui.position.top >= maxHeight) {
                //    ui.position.top = 0;
                //}
                var wr = $(this).css("width");
                var hr = $(this).css("height");
                var left = $(this).css("left")

                var w = Number.parseInt(wr);
                var h = Number.parseInt(hr);
                var lf = Number.parseInt(left);

                var icon = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-se");
                icon.css({
                    "left": lf + w - 15,
                    "top": h - 15
                });

                var row = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-s");
                row.css({
                    "left": lf,
                    "top": h
                });

                var col = $("[data-field=QRCodeField] .qrcode-parent").find(".ui-resizable-e");
                col.css({
                    "left": lf + w,
                    "top": 0
                });
            }

        },
    });
}


/**
 * Bỏ resize cho qr
 * tqha(23/6/2022)
 * */
function removeResizeQR() {
    var postion = checkPositionQRTicket();
    if (postion == 0) {
        $("[data-field=QRCodeFieldTop] .qrcode-parent ").removeClass('highlight-logo');
        if ($("[data-field=QRCodeFieldTop] .qrcode-parent ").is('.ui-resizable')) {
            $("[data-field=QRCodeFieldTop] .qrcode-parent ").resizable('disable');
            $("[data-field=QRCodeFieldTop] .qrcode-parent ").draggable({ disabled: true });
        }

    }
    else if (postion == 1) {
        $("[data-field=QRCodeField] .qrcode ").removeClass('highlight-logo');
        if ($("[data-field=QRCodeField] .qrcode ").is('.ui-resizable')) {
            $("[data-field=QRCodeField] .qrcode ").resizable('disable');
            $("[data-field=QRCodeField] .qrcode ").draggable({ disabled: true });
        }

    }

}

/**
 * setup resize cho phần ký của vé
 * tqha (19/9/2022)
 * */
function resizeDragForTicketSign() {
    initResizeDragForSellerSignTicket();
    var taxSign = $("[data-field=TaxCodeSignContent]");
    if (!taxSign.hasClass("display-none")) {
        initResizeDragForTaxCodeSignTicket();
    }

    $("[data-field=SellerSignContent]").css("cursor", "all-scroll");
    $("[data-field=TaxCodeSignContent]").css("cursor", "all-scroll");
}

/**
 * init resize và drag cho phần chữ ký của người bán (vé)
 * tqha (14/9/2022)
 * */
function initResizeDragForSellerSignTicket() {
    var $contentSign = $("[data-field='SellerSignContent'] .content-sign");
    $("[data-field='SellerSignContent'] .content-sign").resizable({
        minHeight: 51,
        maxHeight: 168,
        minWidth: 130,
        maxWidth: 270,
        start: function (event, ui) {
        },
        stop: function (event, ui) {
        },
        resize: function (event, ui) {
            var hr = $contentSign.css("height");
            var h = Number.parseInt(hr);
            var divHeight = Math.ceil(h / 3) - 8;
            var fontSize = 10;
            if (divHeight >= 9 && divHeight <= 14) {
                fontSize = divHeight - 1;
            }
            else if (divHeight <= 19) {
                fontSize = divHeight - 2;
            }
            else {
                fontSize = divHeight - 3;
            }
            
            if (fontSize < 10) {
                fontSize = 10;
            }
            else if (fontSize > 14) {
                fontSize = 14;
            }

            var minWidth = configCallback.isVN ? Math.ceil(fontSize * 6.7) + 54 : Math.ceil(fontSize * 6.7) + 80;
            if (minWidth > 270) {
                minWidth = 270;
            }

            var lineHeight = fontSize;
            var $signBY = $contentSign.find("[data-field=SellerSignByClient] .esign-label");
            var currentFontSize = Number.parseInt($signBY.css("font-size"));
            if ((fontSize < currentFontSize) || (fontSize > currentFontSize && fontSize <= 30)) {
                handleChangeStyleSignBlock(fontSize, "SellerSignContent", lineHeight);
                var item = getStyleForBlockSignTicket("", "SellerSignContent");
                configCallback.updateStyleByItem(item, "SellerSignContent");
            }
            $("[data-field='SellerSignContent'] .content-sign").resizable("option", "minWidth", minWidth);

            //Chặn không cho nó bị tràn ra ngoài khi resize
            var $target = $(event.target);
            var maxWidth = 270;
            var offsetMinus = -(maxWidth - $target.width()) / 2;

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }

        },
        disabled: false
    });

    $("[data-field='SellerSignContent'] .content-sign").draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target);
            var maxWidth = 270;
            var offsetMinus = -(maxWidth - $target.width()) / 2;

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }
            //kéo dọc
            ui.position.top = 0;


        },
        disabled: false
    });
}

/**
 * init resize và drag cho phần chữ ký của cqt (vé)
 * tqha (15/9/2022)
 * */
function initResizeDragForTaxCodeSignTicket() {
    var $contentSign = $("[data-field='TaxCodeSignContent'] .content-sign");
    $("[data-field='TaxCodeSignContent'] .content-sign").resizable({
        minHeight: 51,
        maxHeight: 168,
        minWidth: 130,
        maxWidth: 270,
        start: function (event, ui) {
        },
        stop: function (event, ui) {
        },
        resize: function (event, ui) {
            var hr = $contentSign.css("height");
            var h = Number.parseInt(hr);
            var divHeight = Math.ceil(h / 3) - 8;
            var fontSize = 10;
            if (divHeight >= 9 && divHeight <= 14) {
                fontSize = divHeight - 1;
            }
            else if (divHeight <= 19) {
                fontSize = divHeight - 2;
            }
            else {
                fontSize = divHeight - 3;
            }

            if (fontSize < 10) {
                fontSize = 10;
            }
            else if (fontSize > 14) {
                fontSize = 14;
            }

            var minWidth = configCallback.isVN ? Math.ceil(fontSize * 6.7) + 54 : Math.ceil(fontSize * 6.7) + 80;
            if (minWidth > 270) {
                minWidth = 270;
            }

            var lineHeight = fontSize;
            var $signBY = $contentSign.find("[data-field=TaxCodeSign] .esign-label");
            var currentFontSize = Number.parseInt($signBY.css("font-size"));
            if ((fontSize < currentFontSize) || (fontSize > currentFontSize && fontSize <= 30)) {
                handleChangeStyleSignBlock(fontSize, "TaxCodeSignContent", lineHeight);
                var item = getStyleForBlockSignTicket("", "TaxCodeSignContent");
                configCallback.updateStyleByItem(item, "TaxCodeSignContent");
            }
            $("[data-field='TaxCodeSignContent'] .content-sign").resizable("option", "minWidth", minWidth);

            //Chặn không cho nó bị tràn ra ngoài khi resize
            var $target = $(event.target);
            var maxWidth = 270;
            var offsetMinus = -(maxWidth - $target.width()) / 2;

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }

        },
        disabled: false
    });

    $("[data-field='TaxCodeSignContent'] .content-sign").draggable({
        start: function (event, ui) {
            $(this).data("dragging", true);
        },
        stop: function (event, ui) {
        },
        drag: function (event, ui) {
            var $target = $(event.target);
            var maxWidth = 270;
            var offsetMinus = -(maxWidth - $target.width()) / 2;

            if (ui.position.left < offsetMinus) {
                ui.position.left = offsetMinus;
            }

            if (ui.position.left + (- offsetMinus) + $target.width() >= maxWidth) {
                ui.position.left = maxWidth + offsetMinus - $target.width();
            }
            //kéo dọc
            ui.position.top = 0;


        },
        disabled: false
    });
}


/**
 * Thiết lập lại độ rộng ô ký hiệu khi có logo và qr top
 * tqha(23/6/2022)
 * */
function resetWithInvoiceSeries() {
    var $logo = $(".logo-template-content");
    var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
    var postion = checkPositionQRTicket();
    if (hasLogo && postion == 0) {
        if (configCallback.isVN) {
            $("#firstInvoice").find("tr:first-child .item-invoice-infor").css("width", "143px")
            $("#firstInvoice").find("tr:first-child .qrcodetop").css("width", "60px")
        }
        else {
            $("#firstInvoice").find("tr:first-child .item-logo").css("width", "75px");
            $("#firstInvoice").find("tr:first-child .item-invoice-infor").css("width", "160px");
            $("#firstInvoice").find("tr:first-child .qrcodetop").css("width", "60px");
        }

    }
}

function changeStyleForInvoiceSeries(position) {
    if (position == POSITION_LOGO.Right) {
        $("[group-field=other-invoice]").css("justify-content", "flex-start");
    }
    else {
        $("[group-field=other-invoice]").css("justify-content", "flex-end");
    }
}

/**
 * Thêm mới div qr top vào mẫu vé chưa có
 * tqha(23/6/2022)
 * */
function addQRCodeForTicket() {
    var qrtop = $("[data-field=QRCodeFieldTop]")
    if (qrtop.length == 0) {
        var divQR = '<td class="qrcodetop">'
            + '<div class="qrcode-container disable-hiden display-none" data-field="QRCodeFieldTop">'
            + '<div class="qrcode-parent offset-resize-width" style="width: 100%;position: relative;height:auto;">'
            + '<img class="qrcode" style="width:70px; height:70px;left:0px;" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAALkAAAC5CAYAAAB0rZ5cAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAu6SURBVHhe7dJBrgM3EAPR3P/SSQ7wFgRISMZ8FVA7Nrtlzz//Ph4f533kj8/zPvLH53kf+ePzvI/88XneR/74PO8jf3ye95E/Ps/7yB+f533kj8/zPvLH53kf+ePzvI/88XneR/74PO8jf3ye95E/Ps/7yB+f533kj8/zPvLH53kf+ePzzD/yf/7552dM0awUyjU2qE8K5W65Zt6oo2+ZolkplGtsUJ8Uyt1yzbxRR98yRbNSKNfYoD4plLvlmnmjjr5limalUK6xQX1SKHfLNfNGHX3LFM1KoVxjg/qkUO6Wa+aNJ44WJ/Zqxy/ZsO5LObF33njiaHFir3b8kg3rvpQTe+eNJ44WJ/Zqxy/ZsO5LObF33njiaHFir3b8kg3rvpQTe+eNJ44WJ/Zqxy/ZsO5LObF33pgerVyqWOdEOrvOiXT2RC5VpLmGeWN6tHKpYp0T6ew6J9LZE7lUkeYa5o3p0cqlinVOpLPrnEhnT+RSRZprmDemRyuXKtY5kc6ucyKdPZFLFWmuYd6YHq1cqljnRDq7zol09kQuVaS5hnljerRyqaLJpYomJ0WaE+lsk0sVaa5h3pgerVyqaHKposlJkeZEOtvkUkWaa5g3pkcrlyqaXKpoclKkOZHONrlUkeYa5o3p0cqliiaXKpqcFGlOpLNNLlWkuYZ5Y3q0cqmiyaWKJidFmhPpbJNLFWmuYd6YHq1cqmhyUqS5FPVJoVyqaHKpIs01zBvTo5VLFU1OijSXoj4plEsVTS5VpLmGeWN6tHKposlJkeZS1CeFcqmiyaWKNNcwb0yPVi5VNDkp0lyK+qRQLlU0uVSR5hrmjenRyqWKJidFmktRnxTKpYomlyrSXMO88cTRIt2rnExpZhvSvevcmhN7540njhbpXuVkSjPbkO5d59ac2DtvPHG0SPcqJ1Oa2YZ07zq35sTeeeOJo0W6VzmZ0sw2pHvXuTUn9s4bTxwt0r3KyZRmtiHdu86tObF33qijbyleLs/dcs28UUffUrxcnrvlmnmjjr6leLk8d8s180YdfUvxcnnulmvmjTr6luLl8twt1+wbf5z0R01zKWlfmhOalX+NP/fi9E9PcylpX5oTmpV/jT/34vRPT3MpaV+aE5qVf40/9+L0T09zKWlfmhOalX+NP/fi9E9PcylpX5oTmpV/jfmL0x9VubUp6Wyaa9COxhTNrr3FfHP6OOXWpqSzaa5BOxpTNLv2FvPN6eOUW5uSzqa5Bu1oTNHs2lvMN6ePU25tSjqb5hq0ozFFs2tvMd+cPk65tSnpbJpr0I7GFM2uvcV8sx4nhXIyRbNSKHdCoVxqimZTG9Z9Yt6oo6VQTqZoVgrlTiiUS03RbGrDuk/MG3W0FMrJFM1KodwJhXKpKZpNbVj3iXmjjpZCOZmiWSmUO6FQLjVFs6kN6z4xb9TRUignUzQrhXInFMqlpmg2tWHdJ/aNIH2IcidMSWeVa3x0vI/8f1PSWeUaHx3vI//flHRWucZHx/vI/zclnVWu8dHxPvL/TUlnlWt8dMx/Qf1JjaLJyZR0dp1LUd9asc6tmW/RQxpFk5Mp6ew6l6K+tWKdWzPfooc0iiYnU9LZdS5FfWvFOrdmvkUPaRRNTqaks+tcivrWinVuzXyLHtIompxMSWfXuRT1rRXr3Jr5Fj1ENqhPNqR9TU6KdW6N9spbzDfrcbJBfbIh7WtyUqxza7RX3mK+WY+TDeqTDWlfk5NinVujvfIW8816nGxQn2xI+5qcFOvcGu2Vt5hv1uNkg/pkQ9rX5KRY59Zor7zFfHP6OOUahXIyRbMyRbNSKHfLhnWfmDemRyvXKJSTKZqVKZqVQrlbNqz7xLwxPVq5RqGcTNGsTNGsFMrdsmHdJ+aN6dHKNQrlZIpmZYpmpVDulg3rPjFvTI9WrlEoJ1M0K1M0K4Vyt2xY94l5o46WIs2J9ay8hW6RDepbe4v5Zj1OijQn1rPyFrpFNqhv7S3mm/U4KdKcWM/KW+gW2aC+tbeYb9bjpEhzYj0rb6FbZIP61t5ivlmPkyLNifWsvIVukQ3qW3uL+eb0cco1ijTXkO5Y54RmpUhzDSd2iPmW9CHKNYo015DuWOeEZqVIcw0ndoj5lvQhyjWKNNeQ7ljnhGalSHMNJ3aI+Zb0Ico1ijTXkO5Y54RmpUhzDSd2iPmW9CHKNYo015DuWOeEZqVIcw0ndogzW4AevDYlnVVONqhPpjSzKdohb3Fts36EtSnprHKyQX0ypZlN0Q55i2ub9SOsTUlnlZMN6pMpzWyKdshbXNusH2FtSjqrnGxQn0xpZlO0Q97i2mb9CGtT0lnlZIP6ZEozm6Id8hbzzenjlJO/hO5b26C+E4p1rmHemB6tnPwldN/aBvWdUKxzDfPG9Gjl5C+h+9Y2qO+EYp1rmDemRysnfwndt7ZBfScU61zDvDE9Wjn5S+i+tQ3qO6FY5xrmjenRykmh3Jddox2NIs2dYL45fZxyUij3ZddoR6NIcyeYb04fp5wUyn3ZNdrRKNLcCeab08cpJ4VyX3aNdjSKNHeC+eb0ccpJodyXXaMdjSLNnWC+ef24pk+zqQ1pX5pLUZ9M0axMaWYb5lvWD2n6NJvakPaluRT1yRTNypRmtmG+Zf2Qpk+zqQ1pX5pLUZ9M0axMaWYb5lvWD2n6NJvakPaluRT1yRTNypRmtmG+Zf2Qpk+zqQ1pX5pLUZ9M0axMaWYb5lvShygnU359VjkpmtwJhXKpa+aN6dHKyZRfn1VOiiZ3QqFc6pp5Y3q0cjLl12eVk6LJnVAol7pm3pgerZxM+fVZ5aRocicUyqWumTemRysnU359VjkpmtwJhXKpa/aNP076oyonG9QnG070rV2zb/xx0h9VOdmgPtlwom/tmn3jj5P+qMrJBvXJhhN9a9fsG3+c9EdVTjaoTzac6Fu7Zt/446Q/qnKyQX2y4UTf2jXzRh19yxPc2it0ixRNLvUW88163C1PcGuv0C1SNLnUW8w363G3PMGtvUK3SNHkUm8x36zH3fIEt/YK3SJFk0u9xXyzHnfLE9zaK3SLFE0u9Rbzzbcel+5VLrUh7UtzKb/U18w2zLdce0i4V7nUhrQvzaX8Ul8z2zDfcu0h4V7lUhvSvjSX8kt9zWzDfMu1h4R7lUttSPvSXMov9TWzDfMt1x4S7lUutSHtS3Mpv9TXzDbMt6QPUS5VrHNCs7Ih7Wtya8U61zBvTI9WLlWsc0KzsiHta3JrxTrXMG9Mj1YuVaxzQrOyIe1rcmvFOtcwb0yPVi5VrHNCs7Ih7Wtya8U61zBvTI9WLlWsc0KzsiHta3JrxTrXMG9Mj1YuVdzKCc2mCuVST6C98hbzzenjlEsVt3JCs6lCudQTaK+8xXxz+jjlUsWtnNBsqlAu9QTaK28x35w+TrlUcSsnNJsqlEs9gfbKW8w3p49TLlXcygnNpgrlUk+gvfIW883p45RLFU1OCuVkimZTRZO75QnmW9KHKJcqmpwUyskUzaaKJnfLE8y3pA9RLlU0OSmUkymaTRVN7pYnmG9JH6JcqmhyUignUzSbKprcLU8w35I+RLlU0eSkUE6maDZVNLlbnmC+5dpDir2aTU3RrBTKSaGcFMqlijS3Zr7l2kOKvZpNTdGsFMpJoZwUyqWKNLdmvuXaQ4q9mk1N0awUykmhnBTKpYo0t2a+5dpDir2aTU3RrBTKSaGcFMqlijS3Zr7l2kOKvZpNTdGsFMpJoZwUyqWKNLdmvkUPuaVQTjaoL7Wh6dOsTGlm18w363G3FMrJBvWlNjR9mpUpzeya+WY97pZCOdmgvtSGpk+zMqWZXTPfrMfdUignG9SX2tD0aVamNLNr5pv1uFsK5WSD+lIbmj7NypRmds29zY/HId5H/vg87yN/fJ73kT8+z/vIH5/nfeSPz/M+8sfneR/54/O8j/zxed5H/vg87yN/fJ73kT8+z/vIH5/nfeSPz/M+8sfneR/54/O8j/zxed5H/vg87yN/fJ73kT8+zr///gfJTRMcGc0NLAAAAABJRU5ErkJggg==" />'
            + '</div>'
            + '</div>'
            + '</td>';
        $(".invoice-first tr.tr-invoice-first").append($(divQR));
    }

    var qrbottom = $("[data-field=QRCodeField]")
    if (qrbottom.length == 0) {
        var divQR = '<div class="qrcode-container disable-hiden display-none" data-field="QRCodeField">'
            + '<div class="qrcode-parent offset-resize-width" style="width: 100%;position: relative;height:auto;">'
            + '<img class="qrcode" style="width: 160px;height: 160px;" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAALkAAAC5CAYAAAB0rZ5cAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAu6SURBVHhe7dJBrgM3EAPR3P/SSQ7wFgRISMZ8FVA7Nrtlzz//Ph4f533kj8/zPvLH53kf+ePzvI/88XneR/74PO8jf3ye95E/Ps/7yB+f533kj8/zPvLH53kf+ePzvI/88XneR/74PO8jf3ye95E/Ps/7yB+f533kj8/zPvLH53kf+ePzzD/yf/7552dM0awUyjU2qE8K5W65Zt6oo2+ZolkplGtsUJ8Uyt1yzbxRR98yRbNSKNfYoD4plLvlmnmjjr5limalUK6xQX1SKHfLNfNGHX3LFM1KoVxjg/qkUO6Wa+aNJ44WJ/Zqxy/ZsO5LObF33njiaHFir3b8kg3rvpQTe+eNJ44WJ/Zqxy/ZsO5LObF33njiaHFir3b8kg3rvpQTe+eNJ44WJ/Zqxy/ZsO5LObF33pgerVyqWOdEOrvOiXT2RC5VpLmGeWN6tHKpYp0T6ew6J9LZE7lUkeYa5o3p0cqlinVOpLPrnEhnT+RSRZprmDemRyuXKtY5kc6ucyKdPZFLFWmuYd6YHq1cqljnRDq7zol09kQuVaS5hnljerRyqaLJpYomJ0WaE+lsk0sVaa5h3pgerVyqaHKposlJkeZEOtvkUkWaa5g3pkcrlyqaXKpoclKkOZHONrlUkeYa5o3p0cqliiaXKpqcFGlOpLNNLlWkuYZ5Y3q0cqmiyaWKJidFmhPpbJNLFWmuYd6YHq1cqmhyUqS5FPVJoVyqaHKpIs01zBvTo5VLFU1OijSXoj4plEsVTS5VpLmGeWN6tHKposlJkeZS1CeFcqmiyaWKNNcwb0yPVi5VNDkp0lyK+qRQLlU0uVSR5hrmjenRyqWKJidFmktRnxTKpYomlyrSXMO88cTRIt2rnExpZhvSvevcmhN7540njhbpXuVkSjPbkO5d59ac2DtvPHG0SPcqJ1Oa2YZ07zq35sTeeeOJo0W6VzmZ0sw2pHvXuTUn9s4bTxwt0r3KyZRmtiHdu86tObF33qijbyleLs/dcs28UUffUrxcnrvlmnmjjr6leLk8d8s180YdfUvxcnnulmvmjTr6luLl8twt1+wbf5z0R01zKWlfmhOalX+NP/fi9E9PcylpX5oTmpV/jT/34vRPT3MpaV+aE5qVf40/9+L0T09zKWlfmhOalX+NP/fi9E9PcylpX5oTmpV/jfmL0x9VubUp6Wyaa9COxhTNrr3FfHP6OOXWpqSzaa5BOxpTNLv2FvPN6eOUW5uSzqa5Bu1oTNHs2lvMN6ePU25tSjqb5hq0ozFFs2tvMd+cPk65tSnpbJpr0I7GFM2uvcV8sx4nhXIyRbNSKHdCoVxqimZTG9Z9Yt6oo6VQTqZoVgrlTiiUS03RbGrDuk/MG3W0FMrJFM1KodwJhXKpKZpNbVj3iXmjjpZCOZmiWSmUO6FQLjVFs6kN6z4xb9TRUignUzQrhXInFMqlpmg2tWHdJ/aNIH2IcidMSWeVa3x0vI/8f1PSWeUaHx3vI//flHRWucZHx/vI/zclnVWu8dHxPvL/TUlnlWt8dMx/Qf1JjaLJyZR0dp1LUd9asc6tmW/RQxpFk5Mp6ew6l6K+tWKdWzPfooc0iiYnU9LZdS5FfWvFOrdmvkUPaRRNTqaks+tcivrWinVuzXyLHtIompxMSWfXuRT1rRXr3Jr5Fj1ENqhPNqR9TU6KdW6N9spbzDfrcbJBfbIh7WtyUqxza7RX3mK+WY+TDeqTDWlfk5NinVujvfIW8816nGxQn2xI+5qcFOvcGu2Vt5hv1uNkg/pkQ9rX5KRY59Zor7zFfHP6OOUahXIyRbMyRbNSKHfLhnWfmDemRyvXKJSTKZqVKZqVQrlbNqz7xLwxPVq5RqGcTNGsTNGsFMrdsmHdJ+aN6dHKNQrlZIpmZYpmpVDulg3rPjFvTI9WrlEoJ1M0K1M0K4Vyt2xY94l5o46WIs2J9ay8hW6RDepbe4v5Zj1OijQn1rPyFrpFNqhv7S3mm/U4KdKcWM/KW+gW2aC+tbeYb9bjpEhzYj0rb6FbZIP61t5ivlmPkyLNifWsvIVukQ3qW3uL+eb0cco1ijTXkO5Y54RmpUhzDSd2iPmW9CHKNYo015DuWOeEZqVIcw0ndoj5lvQhyjWKNNeQ7ljnhGalSHMNJ3aI+Zb0Ico1ijTXkO5Y54RmpUhzDSd2iPmW9CHKNYo015DuWOeEZqVIcw0ndogzW4AevDYlnVVONqhPpjSzKdohb3Fts36EtSnprHKyQX0ypZlN0Q55i2ub9SOsTUlnlZMN6pMpzWyKdshbXNusH2FtSjqrnGxQn0xpZlO0Q97i2mb9CGtT0lnlZIP6ZEozm6Id8hbzzenjlJO/hO5b26C+E4p1rmHemB6tnPwldN/aBvWdUKxzDfPG9Gjl5C+h+9Y2qO+EYp1rmDemRysnfwndt7ZBfScU61zDvDE9Wjn5S+i+tQ3qO6FY5xrmjenRykmh3Jddox2NIs2dYL45fZxyUij3ZddoR6NIcyeYb04fp5wUyn3ZNdrRKNLcCeab08cpJ4VyX3aNdjSKNHeC+eb0ccpJodyXXaMdjSLNnWC+ef24pk+zqQ1pX5pLUZ9M0axMaWYb5lvWD2n6NJvakPaluRT1yRTNypRmtmG+Zf2Qpk+zqQ1pX5pLUZ9M0axMaWYb5lvWD2n6NJvakPaluRT1yRTNypRmtmG+Zf2Qpk+zqQ1pX5pLUZ9M0axMaWYb5lvShygnU359VjkpmtwJhXKpa+aN6dHKyZRfn1VOiiZ3QqFc6pp5Y3q0cjLl12eVk6LJnVAol7pm3pgerZxM+fVZ5aRocicUyqWumTemRysnU359VjkpmtwJhXKpa/aNP076oyonG9QnG070rV2zb/xx0h9VOdmgPtlwom/tmn3jj5P+qMrJBvXJhhN9a9fsG3+c9EdVTjaoTzac6Fu7Zt/446Q/qnKyQX2y4UTf2jXzRh19yxPc2it0ixRNLvUW88163C1PcGuv0C1SNLnUW8w363G3PMGtvUK3SNHkUm8x36zH3fIEt/YK3SJFk0u9xXyzHnfLE9zaK3SLFE0u9Rbzzbcel+5VLrUh7UtzKb/U18w2zLdce0i4V7nUhrQvzaX8Ul8z2zDfcu0h4V7lUhvSvjSX8kt9zWzDfMu1h4R7lUttSPvSXMov9TWzDfMt1x4S7lUutSHtS3Mpv9TXzDbMt6QPUS5VrHNCs7Ih7Wtya8U61zBvTI9WLlWsc0KzsiHta3JrxTrXMG9Mj1YuVaxzQrOyIe1rcmvFOtcwb0yPVi5VrHNCs7Ih7Wtya8U61zBvTI9WLlWsc0KzsiHta3JrxTrXMG9Mj1YuVdzKCc2mCuVST6C98hbzzenjlEsVt3JCs6lCudQTaK+8xXxz+jjlUsWtnNBsqlAu9QTaK28x35w+TrlUcSsnNJsqlEs9gfbKW8w3p49TLlXcygnNpgrlUk+gvfIW883p45RLFU1OCuVkimZTRZO75QnmW9KHKJcqmpwUyskUzaaKJnfLE8y3pA9RLlU0OSmUkymaTRVN7pYnmG9JH6JcqmhyUignUzSbKprcLU8w35I+RLlU0eSkUE6maDZVNLlbnmC+5dpDir2aTU3RrBTKSaGcFMqlijS3Zr7l2kOKvZpNTdGsFMpJoZwUyqWKNLdmvuXaQ4q9mk1N0awUykmhnBTKpYo0t2a+5dpDir2aTU3RrBTKSaGcFMqlijS3Zr7l2kOKvZpNTdGsFMpJoZwUyqWKNLdmvkUPuaVQTjaoL7Wh6dOsTGlm18w363G3FMrJBvWlNjR9mpUpzeya+WY97pZCOdmgvtSGpk+zMqWZXTPfrMfdUignG9SX2tD0aVamNLNr5pv1uFsK5WSD+lIbmj7NypRmds29zY/HId5H/vg87yN/fJ73kT8+z/vIH5/nfeSPz/M+8sfneR/54/O8j/zxed5H/vg87yN/fJ73kT8+z/vIH5/nfeSPz/M+8sfneR/54/O8j/zxed5H/vg87yN/fJ73kT8+zr///gfJTRMcGc0NLAAAAABJRU5ErkJggg==" />'
            + '</div>'
            + '</div>'

        $(divQR).insertBefore($("[data-field=TransactionFake]"));
    }

    $("[data-field=InvoiceNumber] .edit-value").css("white-space", "nowrap");

}

/**
 * Đối với mẫu cũ thì thêm phần ký của CQT
 * tqha (21/9/2022)
 * */
function appendBlockTaxSignForInvoice() {
    var taxSign = $("[sign-region=TaxCodeSignRegion]");
    if (taxSign.length == 0) {
        if (!configCallback.isCashRegister()) {
            var isShow = configCallback.isVN ? "display-none" : "";
            if (configCallback.isDeliveryND51()) {
                var blockTaxSign = '<div class="tax-sign-region display-none" sign-region="TaxCodeSignRegion" style="margin-top: 20px;">'
                    + '	<div class="edit-item font-bold display-none" data-field="TaxCodeSign">'
                    + '		<div class="edit-label">Tổng cục thuế</div>'
                    + '		<div class="edit-label-en padding-left-4 {0}">(General Dept. of TX)</div>'
                    + '	</div>'
                    + '	<div class="edit-item font-italic display-none" data-field="TaxCodeSignFull">'
                    + '		<div class="edit-label">(Chữ ký số)</div>'
                    + '		<div class="edit-label-en sign-full-en {0}">(Digital signature)</div>'
                    + '	</div>'
                    + '	<div class="text-left font-bold sign-content display-none not-show-preview" data-field="TaxCodeSignContent">'
                    + '		<div class="content-sign">'
                    + '			<div>'
                    + '				<div>Signature Valid</div>'
                    + '				<div class="seller-sign-content disable-hiden" data-field="TaxCodeSignByClient">'
                    + '					<div class="esign-label edit-label display-table-cell white-space-nowrap font-italic">Ký bởi</div>'
                    + '					<div class="esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-4 {0}">(Signed By)</div>'
                    + '					<div class="two-dot display-table-cell white-space-nowrap">:</div>'
                    + '					<div class="esign-value display-table-cell padding-left-4"></div>'
                    + '				</div>'
                    + '				<div class="seller-sign-content disable-hiden" data-field="TaxCodeSignDateClient"><span class="esign-label edit-label font-italic">Ký ngày</span><span class="esign-label-en edit-label-en padding-left-4 {0}">(Signing Date)</span><span class="two-dot">:</span><span class="esign-date esign-value padding-left-4"></span></div>'
                    + '			</div>'
                    + '		</div>'
                    + '	</div>'
                    + '</div>';

                blockTaxSign = blockTaxSign.format(isShow);
                $(blockTaxSign).insertAfter($("[sign-region=SellerSignRegion] [data-field=SellerSignContent]"));
            }
            else {
                var blockTaxSign = '<td class="resize-width vertical-align-top tax-sign-region display-none" sign-region="TaxCodeSignRegion" style="width: 263px;">'
                    + '	<div class="edit-item font-bold display-none" data-field="TaxCodeSign"><span class="edit-label">Tổng cục thuế</span><span class="edit-label-en padding-left-4 {0}">(General Dept. of TX)</span></div>'
                    + '	<div class="edit-item font-italic display-none" data-field="TaxCodeSignFull">'
                    + '		<div class="edit-label">(Chữ ký số)</div>'
                    + '		<div class="edit-label-en sign-full-en {0}">(Digital signature)</div>'
                    + '	</div>'
                    + '	<div class="text-left font-bold sign-content display-none not-show-preview" data-field="TaxCodeSignContent">'
                    + '		<div class="content-sign">'
                    + '			<div>'
                    + '				<div>Signature Valid</div>'
                    + '				<div class="seller-sign-content disable-hiden" data-field="TaxCodeSignByClient">'
                    + '					<div class="esign-label edit-label display-table-cell white-space-nowrap font-italic">Ký bởi</div>'
                    + '					<div class="esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-4 {0}">(Signed By)</div>'
                    + '					<div class="two-dot display-table-cell white-space-nowrap">:</div>'
                    + '					<div class="esign-value display-table-cell padding-left-4"></div>'
                    + '				</div>'
                    + '				<div class="seller-sign-content disable-hiden" data-field="TaxCodeSignDateClient"><span class="esign-label edit-label font-italic">Ký ngày</span><span class="esign-label-en edit-label-en padding-left-4 {0}">(Signing Date)</span><span class="two-dot">:</span><span class="esign-date esign-value padding-left-4"></span></div>'
                    + '			</div>'
                    + '		</div>'
                    + '	</div>'
                    + '</td>';

                blockTaxSign = blockTaxSign.format(isShow);
                $(blockTaxSign).insertBefore($("[sign-region=SellerSignRegion]"));
            }
        }
    }
    else {
        //Tránh trường hợp lúc đầu append vào quên không bỏ class display-none đi
        if (!configCallback.isVN) {
            taxSign.find(".edit-label-en").removeClass("display-none");
        }
    }
}

/**
 * Hàm xử lý insert các thông tin còn thiếu vào mẫu pxk NĐ51
 * createdBy nmquang2 27/09/2022
 * */
function handleAppendHtmlOutward() {
    //Đơn vị nhận hàng
    let consigneeUnitName = $("[data-field=ConsigneeUnitName]");
    //Mã số thuế
    let consigneeUnitTaxcode = $("[data-field=ConsigneeUnitTaxcode]");
    //Tên người xuất hàng
    let stockOutFullName = $("[data-field=StockOutFullName]");
    //Tên người nhập hàng
    let stockInFullName = $("[data-field=StockInFullName]");
    if (consigneeUnitName.length == 0) {
        let htmlUnitName = `<div class="edit-item display-none width-full float-left" data-field="ConsigneeUnitName">`
            + `  <div class="edit-label white-space-nowrap display-table-cell" style="min-width: 0px;">Đơn vị nhận hàng</div>`
            + `  <div class="edit-label-en display-table-cell white-space-nowrap display-none" style="min-width: 0px;">(Consignee unit)</div>`
            + `  <div class="two-dot display-table-cell white-space-nowrap" style="min-width: unset;">:</div>`
            + `  <div class="edit-value display-table-cell none-edit">`
            + `  </div>`
            + `</div>`
        $(htmlUnitName).insertAfter($("[data-field=TransportationMethod]"))
    }
    if (consigneeUnitTaxcode.length == 0) {
        let htmlUnitTaxcode = `<div class="edit-item display-none width-full float-left" data-field="ConsigneeUnitTaxcode">`
            + `  <div class="edit-label white-space-nowrap display-table-cell" style="min-width: 0px;">Mã số thuế</div>`
            + `  <div class="edit-label-en display-table-cell white-space-nowrap display-none" style="min-width: 0px;">(Consignee unit taxcode)</div>`
            + `  <div class="two-dot display-table-cell white-space-nowrap" style="min-width: unset;">:</div>`
            + `  <div class="edit-value display-table-cell none-edit">`
            + `  </div>`
            + `</div>`
        $(htmlUnitTaxcode).insertAfter($("[data-field=ConsigneeUnitName]"))
    }
    if (stockOutFullName.length == 0) {
        let htmlOutFullName = `<div class="edit-item display-none width-fourFive float-left flex-1" data-field="StockOutFullName">`
            + `	<div class="edit-label white-space-nowrap display-table-cell" style="min-width: 0px;">Tên người xuất hàng</div>`
            + `	<div class="edit-label-en display-table-cell white-space-nowrap display-none" style="min-width: 0px;">(Exporter's name)</div>`
            + `	<div class="two-dot display-table-cell white-space-nowrap" style="min-width: unset;">:</div>`
            + `	<div class="edit-value display-table-cell none-edit">`
            + `	</div>`
            + `</div>`
        $(htmlOutFullName).insertAfter($("[data-field=FromWarehouseName]"))
    }
    if (stockInFullName.length == 0) {
        let htmlInFullName = `<div class="edit-item display-none width-fourFive float-left flex-1 element-active" data-field="StockInFullName">`
            + `	<div class="edit-label white-space-nowrap display-table-cell" style="min-width: 0px;">Tên người nhận hàng</div>`
            + `	<div class="edit-label-en display-table-cell white-space-nowrap display-none" style="min-width: 0px;">(Stock In LegalName)</div>`
            + `	<div class="two-dot display-table-cell white-space-nowrap" style="min-width: unset;">:</div>`
            + `	<div class="edit-value display-table-cell none-edit">`
            + `	</div>`
            + `</div>`
        $(htmlInFullName).insertAfter($("[data-field=ToWarehouseName]"))
    }
}


/**
 * Kiểm tra xem nếu chưa có phần chữ ký của cqt thì thêm vào
 * tqha (14/9/2022)
 * */
function appendBlockTaxSign() {
    var taxSign = $("[data-field=TaxCodeSignContent]");
    if (taxSign.length == 0) {
        var blockTaxSign = '<div class="width-full vertical-align-top SellerSignRegion not-show-preview" data-field="TaxCodeSignContent">' +
            '  <div class="text-left font-bold">' +
            '	<div class="edit-value padding-none">' +
            '	  <div class="content-sign">' +
            '		<div class="background-sign display-table-cell"></div>' +
            '		<div class="display-table-cell">' +
            '		  <div class="p-4-0 esign-title">Signature Valid</div>' +
            '		  <div class="seller-sign-content disable-hiden not-show-preview" data-field="TaxCodeSign">' +
            '			<div class="esign-label edit-label display-table-cell white-space-nowrap">Ký bởi</div>' +
            '			<div class="esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-3 display-none">(Signed by)</div>' +
            '			<div class="two-dot display-table-cell white-space-nowrap">:</div>' +
            '			<div class="esign-value display-table-cell padding-left-3"></div>' +
            '		  </div>' +
            '		  <div class="seller-sign-content disable-hiden not-show-preview" data-field="TaxCodeSignDate"><span class="esign-label edit-label display-table-cell white-space-nowrap">Ký ngày</span><span class="esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-3 display-none">(Signing date)</span><span class="two-dot display-table-cell white-space-nowrap">:</span><span class="esign-date esign-value padding-left-3 display-table-cell"></span></div>' +
            '		</div>' +
            '	  </div>' +
            '	</div>' +
            '  </div>' +
            '</div>';

        $(blockTaxSign).insertAfter($("[data-field=SellerSignContent]"));
        taxSign = $("[data-field=TaxCodeSignContent]");

    }
    else {
        //Đoạn này để đổi lại class cho phần ký bởi ký ngày
        //Do trước bị sai class nên style bị lỗi khi thu nhỏ, chỉnh lại cho những mẫu đã tạo trước
        var taxSignDateLabel = taxSign.find("[data-field=TaxCodeSignDate] .esign-label"),
            taxSignDateLabelEN = taxSign.find("[data-field=TaxCodeSignDate] .esign-label-en"),
            taxSignDateTwoDot = taxSign.find("[data-field=TaxCodeSignDate] .two-dot"),
            taxSignDateValue = taxSign.find("[data-field=TaxCodeSignDate] .esign-value");

        taxSignDateLabel.attr('class', '');
        taxSignDateLabelEN.attr('class', '');
        taxSignDateTwoDot.attr('class', '');
        taxSignDateValue.attr('class', '');

        taxSignDateLabel.addClass("esign-label edit-label display-table-cell white-space-nowrap");
        taxSignDateLabelEN.addClass("esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-3 display-none");
        taxSignDateTwoDot.addClass("two-dot display-table-cell white-space-nowrap");
        taxSignDateValue.addClass("esign-date esign-value padding-left-3 display-table-cell");

        $("[data-field=TaxCodeSign]").prev().addClass("esign-title");
    }

    //Cập nhật lại style cho phần chữ ký của người bán
    var sellerSign = $("[data-field=SellerSignContent]");
    var sellerSignDateLabel = sellerSign.find("[data-field=SellerSignDateClient] .esign-label"),
        sellerSignDateLabelEN = sellerSign.find("[data-field=SellerSignDateClient] .esign-label-en"),
        sellerSignDateTwoDot = sellerSign.find("[data-field=SellerSignDateClient] .two-dot"),
        sellerSignDateValue = sellerSign.find("[data-field=SellerSignDateClient] .esign-value");

    sellerSignDateLabel.attr('class', '');
    sellerSignDateLabelEN.attr('class', '');
    sellerSignDateTwoDot.attr('class', '');
    sellerSignDateValue.attr('class', '');

    sellerSignDateLabel.addClass("esign-label edit-label display-table-cell white-space-nowrap");
    sellerSignDateLabelEN.addClass("esign-label-en edit-label-en display-table-cell white-space-nowrap padding-left-3 display-none");
    sellerSignDateTwoDot.addClass("two-dot display-table-cell white-space-nowrap");
    sellerSignDateValue.addClass("esign-date esign-value padding-left-3 display-table-cell");

    $("[data-field=SellerSignByClient]").prev().addClass("esign-title");

    if (!configCallback.isTemplateWithCode) {
        taxSign.addClass("not-show-preview display-none");
    }
    else {
        taxSign.removeClass("not-show-preview");
    }
}

/**
 * Xử lý ẩn hiện phần mã của cqt theo thiết lập
 * tqha(14/9/2022)
 * */
function showHideTaxSignForTVT() {
    var taxSign = $("[data-field=TaxCodeSignContent]");
    if (configCallback.isTemplateWithCode) {

    }
    else {
        taxSign.addClass("not-show-preview display-none");
    }
}

function addStyleForConverter() {
    var converterSign = $("[data-field=ConverterSign]").find(".edit-value");
    converterSign.css("text-align", "left");
}

/**
 * Thay đổi style transform text cho thông tin LPLP
 * tqha (15/8/2022)
 * @param {any} style
 */
function setStyleTransformForFeeName(style) {
    var feeName = $("[data-field=FeeName] .edit-value");
    typeTextTransform = feeName.css("text-transform", style);
}

/**
 * Kiểm tra xem có logo không
 * tqha (5/9/2022)
 * */
function checkHasLogo() {
    var $logo = $(".logo-template-content");
    var hasLogo = (Number.parseInt($logo.eq(1).css("width")) > 0 || Number.parseInt($logo.eq(0).css("width")) > 0) ? true : false;
    return hasLogo;
}

/**
 * Thay đổi vị trí qr code nếu như ẩn hết thông tin người mua vỡi mẫu máy tính tiền
 * tqha (5/9/2022)
 * */
function changePositionQRCodeCashRegister() {
    if (configCallback.isCashRegister()) {
        var numberDataFieldShow = $("[group-field=buyer-infor] div[data-field]:not(.display-none)").length;
        if (getPositionSeller() == 1) {
            if (numberDataFieldShow == 0) {
                changePositionQRCode("seller-infor");
                if (checkHasLogo()) {
                    $("[group-field=seller-infor]").css("width", "83%");
                }
                else {
                    $("[group-field=seller-infor]").css("width", "86%");
                }
            }
            else {
                changePositionQRCode("buyer-infor");
            }
        }
        else if (getPositionSeller() == 2) {
            if (numberDataFieldShow == 0) {
                configCallback.chooseQRPositionSeller();
            }
        }
    }
}

/**
 * Check xem nếu là mẫu vé đã tạo trước R52 thì sửa lại nội dung phần thông tin Misa
 * tqha (19/9/2022)
 * */
function changeLabelMisaInfo() {
    var misaInfo = $("[group-field=search-block]");
    misaInfo.css("border-top", "2px solid rgba(0, 0, 0, 0.46)");
    var info = misaInfo.find("[data-field=MISAINFO] .edit-label");
    var company = misaInfo.find("[data-field=MISACOMPANY] .edit-label");

    var infoLabel = info.html().replace("phần mềm", "");
    info.html(infoLabel);
    var companyLabel = company.html().replace("cổ phần", "CP");
    company.html(companyLabel);

    info.parent().css("border-top", "");

    //Áp style hiện tại vào để lưu không bị ảnh hưởng
    var companyTwoDot = misaInfo.find("[data-field=MISACOMPANY] .two-dot");
    var companyValue = misaInfo.find("[data-field=MISACOMPANY] .edit-value");

    info.css("font-size", info.css("font-size"));
    info.css("line-height", info.css("line-height"));
    info.css("font-weight", info.css("font-weight"));
    info.css("font-style", info.css("font-style"));

    company.css("font-size", company.css("font-size"));
    company.css("line-height", company.css("line-height"));
    company.css("font-weight", company.css("font-weight"));
    company.css("font-style", company.css("font-style"));

    companyTwoDot.css("font-size", companyTwoDot.css("font-size"));
    companyTwoDot.css("line-height", companyTwoDot.css("line-height"));
    companyTwoDot.css("font-weight", companyTwoDot.css("font-weight"));
    companyTwoDot.css("font-style", companyTwoDot.css("font-style"));

    companyValue.css("font-size", companyValue.css("font-size"));
    companyValue.css("line-height", companyValue.css("line-height"));
    companyValue.css("font-weight", companyValue.css("font-weight"));
    companyValue.css("font-style", companyValue.css("font-style"));
}

/*
* xử lý tăng giảm line height
* tqha(19/09/2022)
*/
function ChangeLineHeight(isSizeUp) {
    var lstEditLabel = $(".edit-label"),
        lstEditLabelEN = $(".edit-label-en"),
        lstEditValue = $(".edit-value"),
        lstTwoDot = $(".two-dot"),
        lstUnitName = $("[mark=UnitName]"),
        lstTable = $("table"),
        lstEsignTitle = $(".esign-title");

    var lstDataField = $("[data-field]");
    
    lstEditLabel.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");

        if (!configCallback.isTVT()) {
            if ($(item).is("span")) {
                if ($(item).parent().attr("data-field") == "SearchInfo") {
                    $(item).parent().parent().css("line-height", lineHeight + "px");
                }

                if ($(item).parent().attr("data-field") == "MeinvoiceNote") {
                    $(item).parent().css("line-height", lineHeight + "px");
                }
            }
            
        }
    });
    lstEditValue.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });
    lstEditLabelEN.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });
    lstTwoDot.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });
    lstUnitName.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });

    lstTable.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });

    lstEsignTitle.each(function (index, item) {
        var lineHeight = parseInt($(item).css("line-height").replace("px", ""));
        var fontSize = parseInt($(item).css("font-size").replace("px", ""));
        if (!lineHeight) {
            lineHeight = 16;
        }
        if (isSizeUp) {
            lineHeight++;
        } else {
            lineHeight--;
        }
        if (lineHeight < 8) {
            lineHeight = 8;
        }
        else if (lineHeight > 30) {
            lineHeight = 30;
        }
        if (lineHeight < fontSize) {
            lineHeight = fontSize;
        }
        item.style.lineHeight = lineHeight;
        $(item).css("line-height", lineHeight + "px");
    });

    if (configCallback.isTVT()) {
        $(".content-sign").css("height", "");

        if ($("#en-amount-inword").length > 0) {
            let lineHeight = Number.parseInt($("[data-field=TotalAmountInWords] .edit-value").css("line-height"));
            lineHeight = lineHeight ? lineHeight - 1 : 15;
            $("#en-amount-inword").css("line-height", lineHeight + "px");
        }
    }
}

/**
 * Kiểm tra có đang hiển thị phần ký của CQT không
 * tqha (12/9/2022)
 * */
function checkShowDisplayTaxCodeSign() {
    var taxCodeSign = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSign]");
    return !taxCodeSign.hasClass("display-none");
}

/**
 * Xử lý ẩn hiện phần nội dung ký của cqt
 * tqha (13/9/2022)
 * @param {any} isShow
 */
function handleShowHideTaxCodeSign(isShow) {
    resizeTable("#signXml", true);
    var taxCodeSignDive = $(".tax-sign-region");
    var taxCodeSignContent = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSignContent]");
    var taxCodeFullName = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSignFull]");
    if (isShow) {
        taxCodeSignContent.removeClass("display-none");
        taxCodeFullName.removeClass("display-none");
        taxCodeSignDive.css("display", "");
        taxCodeSignDive.removeClass("display-none");
        initResizeDragForTaxESign();
    }
    else {
        taxCodeSignContent.addClass("display-none");
        taxCodeFullName.addClass("display-none");
        taxCodeSignDive.css("display", "none");
        taxCodeSignDive.addClass("display-none");
    }

    if (configCallback.isDeliveryND123()) {
        changeStyleForSellerSignOfDelivery123();
        reloadNewWidthTable($("#signXml tr:first-child td:visible"));
    }
    if (!configCallback.isDeliveryND51()) {
        handleRewidthAfterShowHideTaxSign(isShow);
    }
    resizeTable("#signXml", false, true);
}

/**
 * Xử lý ẩn hiện chữ ký cqt khi thay đổi hình thức hóa đơn
 * tqha (13/9/2022)
 * @param {any} isShow
 */
function handleChangeTypeInvoiceForTaxSign(isShow) {
    var taxCodeSignDiv = $(".tax-sign-region");
    if (!isShow) {
        var taxCodeSign = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSign]");
        var taxCodeSignContent = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSignContent]");
        var taxCodeFullName = $("[sign-region=TaxCodeSignRegion] [data-field=TaxCodeSignFull]");
        taxCodeSign.addClass("display-none");
        taxCodeSignContent.addClass("display-none");
        taxCodeFullName.addClass("display-none");
        
    }

    taxCodeSignDiv.css("display", "none");
    taxCodeSignDiv.addClass("display-none");

    if (configCallback.isDeliveryND123()) {
        changeStyleForSellerSignOfDelivery123();
        reloadNewWidthTable($("#signXml tr:first-child td:visible"));
    }
}

/**
 * Bo style float right neu co tu 2 chu ky tro len voi pxk 123
 * tqha (18/9/2022)
 * */
function changeStyleForSellerSignOfDelivery123() {
    var $divSeller = $("[sign-region=SellerSignRegion] > div");
    if ($divSeller.hasClass("not-show-preview")) {
        $divSeller.addClass("not-show-preview");
        $divSeller.attr("data-field", "SellerSignDiv");
    }

    var numberTdSign = $(".sign-region-content td:not(.display-none)").length;
    if (numberTdSign > 1) {
        $divSeller.css("float", "");
    }
    else {
        $divSeller.css("float", "right");
    }

}

/**
 * Set lai do rong cho td
 * tqha (21/9/2022)
 * @param {any} isShow
 */
function handleRewidthAfterShowHideTaxSign(isShow) {
    if (isShow) {
        var signs = $("#signXml tr:first-child td:visible");
        for (var i = 0; i < signs.length; i++) {
            if ($(signs[i]).attr("sign-region") == "TaxCodeSignRegion" || $(signs[i]).attr("sign-region") == "SellerSignRegion") {
                if ($(signs[i]).width() > 263) {
                    $(signs[i]).css("width", "263px");
                }
                else {
                    $(signs[i]).css("width", $(signs[i]).width() + "px");
                }
            }
            else {
                $(signs[i]).css("width", $(signs[i]).width() + "px");
            }
        }
    }
}

/**
 * Set giá trị style hiện tại của phần giá vé trước khi xóa bỏ rule
 * tqha (28/9/2022)
 * */
function setCurrentStyleOfTicketPrice() {
    var element = $("[data-field=TicketPrice]");
    var label = element.find(".edit-label");
    var labelEN = element.find(".edit-label-en");
    var twoDot = element.find(".two-dot");
    var value = element.find(".edit-value");
    var mark = element.find(".unit-name-ticket");

    //Chỉ update font-size với font-weight tại mẫu cũ bị ảnh hưởng mới rule ticket-price
    //Trong rule này chỉ set font-size và font-weight
    var lstElement = [label, labelEN, , twoDot, value, mark];
    lstElement.forEach(function (e) {
        e.css("font-size", e.css("font-size"));
        e.css("font-weight", e.css("font-weight"));
    })
}

/**
 * Xóa bỏ rule trong internal style
 * tqha (28/9/2022)
 * */
function removeInteralStyleTicket() {
    var rules = [".ticket-price"];

    for (var i = 0; i < rules.length; i++) {
        $.each(document.styleSheets, function (_, sheet) {
            // Loop through the rules...
            var keepGoing = true;
            $.each(sheet.cssRules || sheet.rules, function (index, rule) {
                // Is this the rule we want to delete?
                if (rule.selectorText === rules[i]) {
                    // Yes, do it and stop looping
                    sheet.deleteRule(index);
                    return keepGoing = false;
                }
            });
            keepGoing = false;
            return keepGoing;
        });
    }
}
