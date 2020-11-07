var myApp = angular.module('myApp', [
    'datatables',
    'datatables.buttons',
    'ui.bootstrap',
    'ngSanitize',
    'toaster',
    'cp.ngConfirm',
    'bootstrapLightbox',
    'angularTreeview'
]);

var root = $('#rootPath').attr('href');
