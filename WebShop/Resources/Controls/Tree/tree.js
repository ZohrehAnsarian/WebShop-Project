
var selectedOperationButton = null;
var selectedNode = null;

var readOnly = false;

var addButtonShow = "";
var editButtonShow = "";
var deleteButtonShow = "";

function loadTree(parameterObject) {

    if (parameterObject.addButtonShow === false) {
        addButtonShow = "hidden";
    }

    if (parameterObject.editButtonShow === false) {
        editButtonShow = "hidden";
    }

    if (parameterObject.deleteButtonShow === false) {
        deleteButtonShow = "hidden";
    }

    readOnly = parameterObject.readOnly;

    var treeRequest = $.ajax(
        {
            type: 'post',
            dataType: 'html',
            url: parameterObject.routeAddress,
            data: {

                parentId: parameterObject.parentId,
                expandedLevel: parameterObject.expandedLevel,
                containerId: parameterObject.containerId,

                AddCallback: parameterObject.addCallback,
                EditCallback: parameterObject.editCallback,
                DeleteCallback: parameterObject.deleteCallback,
                SelectNodeCallback: parameterObject.selectNodeCallback,

                ReadOnly: parameterObject.readOnly,
                ShowAdditionalDataInLeaf: parameterObject.showAdditionalDataInLeaf,
                AdditionalDataRenderCallback: parameterObject.additionalDataRenderCallback,
            },

        });

    treeRequest.done(function (data) {

        $('#' + parameterObject.containerId).html(data);
        parameterObject.loadedCallback();
    });

    treeRequest.fail(function (jqXHR, textStatus) {

        alert("Request failed: " + textStatus);
    });

    treeRequest.always(function () {

    });

}

function treeNodeToggle(self, submenu) {

    if ($(self).hasClass("fa-plus-circle")) {
        $(self).removeClass("fa-plus-circle");
        $(self).addClass("fa-minus-circle");
        $(submenu).show();
    }
    else {
        $(self).removeClass("fa-minus-circle");
        $(self).addClass("fa-plus-circle");
        $(submenu).hide();
    }
}

function removeTreeNodeOperation(self) {

    var operationDiv = $("#treeNodeOperationDiv" + $(self).data('id'));
    operationDiv.remove();
    $(self).remove($("#treeNodeOperationDiv" + $(self).data('id')));
    operationDiv = null;

}

function generateTreeNodeOperation(node) {

    if (readOnly === false) {

        event.stopImmediatePropagation();

        var nodeId = $(node).data('id');

        if ($('#treeNodeOperationDiv' + $(node).data('id')).length === 0) {

            var width = '120px';

            var left = $(node).width() + 25;

            if ($(node).parent().children().first().hasClass('fa-minus-circle')) {

                left += 20;
            }

            left += 'px';

            var operationDiv =

                "<div id='treeNodeOperationDiv" + nodeId + "' class='tree-node-operation-div' "
                + "style='--width:" + width + ";--left:" + left + "'>"


                + "<i id='deleteTreeNodeOperationButton' class='fa fa-minus-circle tree-node-operation-div-button-delete " + deleteButtonShow + "'"
                + "onclick = 'setModalDeleteAction(" + nodeId + "); '/>"

                + "<i id='editTreeNodeOperationButton' class='fa fa-pencil-square tree-node-operation-div-button-edit " + editButtonShow + "'"
                + "onclick='setModalEditAction($(this).parent().parent());' />"

                + "<i id='addTreeNodeOperationButton' class='fa fa-plus-square tree-node-operation-div-button-add " + addButtonShow + "' "
                + "onclick='setModalAddAction($(this).parent().parent());' />"

                + "</div>";

            $(node).append($(operationDiv));
        }
    }
}

function addClientTreeNode(newNodeId, nodeName, isDefault, isDefalutCaption) {
    
    var level = $(selectedNode).data('level') + 1;
    var parentNodeId = "treeParentNode_" + $(selectedNode).data('id');
    var nodeId = "treeChildNode_" + newNodeId;
    var newNodeHtml;
    var parentElement = $(selectedNode).parent();
    var defaultCaption = isDefault ? isDefalutCaption : "";
    var captionColor = isDefault ? "default-node" : "";
    debugger
    if (parentElement.children().first().hasClass('fa-plus-circle') ||
        parentElement.children().first().hasClass('fa-minus-circle')) {
        newNodeHtml = "<li id='" + nodeId + "' class='tree-li-border'>" +
            "<a>" +
            "<span class='tree-node-item " + captionColor + "' title='" + defaultCaption + "' data-level='" + level + "' data-id='" + newNodeId +
            "' data-is_leaf='" + false +
            "' data-is_Default='" + isDefault +
            "' style='background-color: yellow' onmouseover='generateTreeNodeOperation(this);' onmouseleave='removeTreeNodeOperation(this);' onclick='selectCurrentNode(this);'>" +
            nodeName +
            "</span>" +
            "</a>" +
            "</li>";

       
        $("#" + parentNodeId).append($(newNodeHtml));
    }
    else {
        parentElement.prepend("<span class='fa tree-parent-node-sign fa-plus-circle' onclick='treeNodeToggle(this, " + parentNodeId + ")'></span>")
        newNodeHtml = "<ul id='" + parentNodeId + "' class='hidden tree-ul-border' >" +
            "<li id='" + nodeId + "' class='tree-li-border'>" +
            "<a>" +
            "<span class='tree-node-item " + captionColor + "' title='" + defaultCaption + "' data-level='" + level + "' data-id='" + newNodeId +
            "' data-is_leaf='" + true +
            "' data-is_Default='" + isDefault +
            "' style='background-color: yellow' onmouseover='generateTreeNodeOperation(this);' onmouseleave='removeTreeNodeOperation(this);' onclick='selectCurrentNode(this);'>" +
            nodeName +
            "</span>" +
            "</a>" +
            "</li></ul>";

        $("#treeChildNode_" + $(selectedNode).data('id')).append($(newNodeHtml));
    }
    if (parentElement.children().first().hasClass('fa-plus-circle'))
        $(parentElement.children().first()).click();
}

function editClientTreeNode(nodeId, nodeName, isDefault, isDefalutCaption) {

    $('*[data-Id="' + nodeId + '"]').text(nodeName);
    if (isDefault === true) {
        $('*[data-Id="' + nodeId + '"]').addClass('default-node');
        $('*[data-Id="' + nodeId + '"]').attr('title', isDefalutCaption);
    }
    else {
        $('*[data-Id="' + nodeId + '"]').removeClass('default-node');
        $('*[data-Id="' + nodeId + '"]').attr('title','');
    }

    $('*[data-Id="' + nodeId + '"]').data('is_default', isDefault);

}

function refreshTree(nodeId, nodeName, isDefault, isDefalutCaption) {
    debugger
    if (selectedOperationButton === 'add') {
        addClientTreeNode(nodeId, nodeName, isDefault, isDefalutCaption);
    }
    else if (selectedOperationButton === 'delete') {

        $("#treeChildNode_" + nodeId).remove();
    }
    else if (selectedOperationButton === 'edit') {
        editClientTreeNode(nodeId, nodeName, isDefault, isDefalutCaption);
    }
}

