jQuery(document).ready(function () {
    $('#listTableRequests').jqGrid({
        url: '/User/PendingRequests/',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['First Name', 'Last Name'],
        colModel: [
			{ name: 'FirstName', index: 'FirstName', width: 200, align: 'center' },
			{ name: 'LastName', index: 'LastName', width: 200, align: 'center' }],
        pager: jQuery('#pagerRequests'),
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: "desc",
        viewrecords: true,
        multiselect: true,
        onSelectRow: function (id) {
            if (id && id !== lastSel) {
                $("#enrollId").data(id);
            }
        }
    });
});

jQuery("#profileLink").click(function () {
    s = parseInt(jQuery("#listTableRequests").jqGrid('getGridParam', 'selarrrow'));
    var loc = "PendingRequestProfile/" + s

    $("#profileInfo").load(loc);
    alert("done");
});

jQuery("#profileDetails").click(function () {
    s = parseInt(jQuery("#listTableRequests").jqGrid('getGridParam', 'selarrrow'));
    var loc = "PendingRequestInfo/" + s

    $("#profileInfo").load(loc);
});

jQuery("#acceptRequest").click(function () {
    s = parseInt(jQuery("#listTableRequests").jqGrid('getGridParam', 'selarrrow'));

    $.post("/User/RequestStatus", { Id: s, Status: true });
    $("#listTableRequests").trigger('reloadGrid');
});

jQuery("#rejectRequest").click(function () {
    s = parseInt(jQuery("#listTableRequests").jqGrid('getGridParam', 'selarrrow'));

    $.post("/User/RequestStatus", { Id: s, Status: false });
    $("#listTableRequests").trigger('reloadGrid');

});

$("#").click(function () {
    s = parseInt(jQuery("#listTableRequests").jqGrid('getGridParam', 'selarrrow'));
    var s2 = $("#notes").val();
    var s3 = $("#resume").val();
    alert(s);
    alert(s2);

    $.post("/User/Register2", { courseId: s, notes: s2, resume: s3 },
        function (data) {
        });
});