/*
	@license Angular Treeview version 0.1.6
	ⓒ 2013 AHN JAE-HA http://github.com/eu81273/angular.treeview
	License: MIT


	[TREE attribute]
	angular-treeview: the treeview directive
	tree-id : each tree's unique id.
	tree-model : the tree model on $scope.
	node-id : each node's id
	node-label : each node's label
	node-children: each node's children

	<div
		data-angular-treeview="true"
		data-tree-id="tree"
		data-tree-model="roleList"
		data-node-id="roleId"
		data-node-label="roleName"
		data-node-children="children" >
	</div>
*/

(function (angular) {
    'use strict';

    angular.module('angularTreeview', []).directive('treeModel', ['$compile', function ($compile) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                //tree id
                var treeId = attrs.treeId;

                //tree model
                var treeModel = attrs.treeModel;

                //node id
                var nodeId = attrs.nodeId || 'id';

                //node label
                var nodeLabel = attrs.nodeLabel || 'label';

                //children
                var nodeChildren = attrs.nodeChildren || 'children';

                var nodeTemplate = attrs.nodeTemplate || 'template';

                //tree template
                var template =
                    '<ul>' +
                    '<li data-ng-repeat="node in ' + treeModel + '">' +
                    '<i class="collapsed" data-ng-show="node.' + nodeChildren + '.length && node.collapsed" data-ng-click="' + treeId + '.selectNodeHead(node)"></i>' +
                    '<i class="expanded" data-ng-show="node.' + nodeChildren + '.length && !node.collapsed" data-ng-click="' + treeId + '.selectNodeHead(node)"></i>' +
                    '<i class="normal" data-ng-hide="node.' + nodeChildren + '.length"></i> ' +
                    '<span data-ng-class="node.selected" data-ng-click="' + treeId + '.selectNodeLabel(node)">{{node.' + nodeLabel + '}}</span>' +
                    '<a title="Create order" href="' + root + 'DrawingOrders/New?car={{node.Id}}" data-ng-show="node.car && !node.IsDrawingAvailable" style="padding-left:5px;" target="_blank"><i class="fa fa-external-link-square"></i></a>' +
                    '<a title="Create order" href="' + root + 'DrawingOrders/New?car={{node.CarId}}&part={{node.Id}}" data-ng-show="node.carPart && !node.IsDrawingAvailable" style="padding-left:5px;" target="_blank"><i class="fa fa-external-link-square"></i></a>' +
                    '<a title="Create order" href="' + root + 'DrawingOrders/New?car={{node.CarId}}&part={{node.CarPartId}}&comp={{node.Id}}" data-ng-show="node.carPartComp && !node.IsDrawingAvailable" style="padding-left:5px;" target="_blank"><i class="fa fa-external-link-square"></i></a>' +
                    '<a title="View Drawing" href="javascript:void(0)" ng-click="openDrawingsList(node)" data-ng-show="node.IsDrawingAvailable" style="padding-left:5px;"><i class="fa fa-eye"></i></a>' +
                    '<div data-ng-hide="node.collapsed" data-tree-id="' + treeId + '" data-tree-model="node.' + nodeChildren + '" data-node-id=' + nodeId + ' data-node-label=' + nodeLabel + ' data-node-children=' + nodeChildren + '></div>' +
                    '</li>' +
                    '</ul>';

                var template_book =
                    '<ul>' +
                    '<li data-ng-repeat="node in ' + treeModel + '">' +
                    '<i class="collapsed" data-ng-show="node.' + nodeChildren + '.length && node.collapsed" data-ng-click="' + treeId + '.selectNodeHead(node)"></i>' +
                    '<i class="expanded" data-ng-show="node.' + nodeChildren + '.length && !node.collapsed" data-ng-click="' + treeId + '.selectNodeHead(node)"></i>' +
                    '<i class="normal" data-ng-hide="node.' + nodeChildren + '.length"></i> ' +
                    '<span data-ng-class="node.selected" data-ng-click="' + treeId + '.selectNodeLabel(node)">{{node.' + nodeLabel + '}}</span>' +

                    '<a title="Borrow book" href="' + root + 'BookBorrow?bookId={{node.bookId}}" data-ng-show="(node.car || node.carPart || node.carPartComp ||node.carPartCompDesc) && node.bookAvailable" style="padding-left:5px;" target="_blank"><i class="fa fa-external-link-square"></i></a>' +
                    //'<a title="Borrow is not available right now" href="#" data-ng-show="(node.car || node.carPart || node.carPartComp ||node.carPartCompDesc) && !node.bookAvailable" style="padding-left:5px;" class="disabled"><i class="fa fa-external-link-square"></i></a>' +
                    '<a title="Download book" href="javascript:void(0)" ng-click="openBooksList(node.softBookId)" data-ng-show="(node.car || node.carPart || node.carPartComp ||node.carPartCompDesc) && node.softCopy && node.softCopyAvailable" style="padding-left:5px;"><i class="fa fa-download"></i></a>' +
                    '<a title="Part code paper" href="' + root + 'HttpHandlers/FileRequestHandler.ashx?Type=GetBookPartCode&&BookId={{node.bookId}}&&SoftBookId={{node.softBookId}}" data-ng-show="(node.car || node.carPart || node.carPartComp ||node.carPartCompDesc) && (node.softCopy || node.bookAvailable) && node.partCodeAvailable" style="padding-left:5px;"><i class="fa fa-file-text-o"></i></a>' +
                    '<a title="Maintenance plan" href="' + root + 'HttpHandlers/FileRequestHandler.ashx?Type=GetMediaFile&&FileId={{node.maintainancePlanId}}" data-ng-show="node.car && node.maintainancePlanAvailable" style="padding-left:5px;" target="_blank"><i class="fa fa-car"></i></a>' +

                    '<div data-ng-hide="node.collapsed" data-node-template="' + nodeTemplate + '" data-tree-id="' + treeId + '" data-tree-model="node.' + nodeChildren + '" data-node-id=' + nodeId + ' data-node-label=' + nodeLabel + ' data-node-children=' + nodeChildren + '></div>' +
                    '</li>' +
                    '</ul>';


                //check tree id, tree model
                if (treeId && treeModel) {

                    //root node
                    if (attrs.angularTreeview) {

                        //create tree object if not exists
                        scope[treeId] = scope[treeId] || {};

                        //if node head clicks,
                        scope[treeId].selectNodeHead = scope[treeId].selectNodeHead || function (selectedNode) {

                            //Collapse or Expand
                            selectedNode.collapsed = !selectedNode.collapsed;
                        };

                        //if node label clicks,
                        scope[treeId].selectNodeLabel = scope[treeId].selectNodeLabel || function (selectedNode) {

                            //remove highlight from previous node
                            if (scope[treeId].currentNode && scope[treeId].currentNode.selected) {
                                scope[treeId].currentNode.selected = undefined;
                            }

                            //set highlight to selected node
                            selectedNode.selected = 'selected';

                            //set currentNode
                            scope[treeId].currentNode = selectedNode;
                        };
                    }
                    //alert(attrs.nodeTemplate);
                    //Rendering template.
                    element.html('').append($compile(attrs.nodeTemplate == 'template_book' ? template_book : template)(scope));
                }
            }
        };
    }]);
})(angular);
