var myApp = angular.module('myApp', [
    'datatables',
    'ui.bootstrap',
    'ngSanitize',
    'toaster',
    'cp.ngConfirm',
    'bootstrapLightbox',
    'angularTreeview'
]);

var root = $('#rootPath').attr('href');
