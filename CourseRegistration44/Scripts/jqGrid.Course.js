jQuery(document).ready(function () {
	$('#listTableNonRegistered').jqGrid({
		url: '/User/CoursesDataNonEnrolled/',  
		datatype: 'json',
		mtype: 'GET',
		colNames: ['Course Id', 'Course Name', 'Prof First Name', 'Prof Last Name', 'Prof Email'],
		colModel: [
			{ name: 'CourseId', index: 'CourseId', width: 80, align: 'center' },
			{ name: 'CourseName', index: 'CourseName', width: 120, align: 'center' },
			{ name: 'ProfessorFirstName', index: 'ProfessorFirstName', width: 200, align: 'center' },
			{ name: 'ProfessorLastName', index: 'ProfessorLastName', width: 200, align: 'center' },
			{ name: 'ProfessorEmail', index: 'ProfessorEmail', width: 200, align: 'center' }],
		pager: jQuery('#pager'),
		rowNum: 10,
		rowList: [5, 10, 20, 50],
		sortname: 'Id',
		sortorder: "desc",
		viewrecords: true,
        multiselect: true,
		imgpath: '',
		caption: 'My first grid'
	});
});
jQuery(document).ready(function () {
    $('#listTableRegistered').jqGrid({
        url: '/User/CoursesDataEnrolled/',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Course Id', 'Course Name', 'Prof First Name', 'Prof Last Name', 'Prof Email'],
        colModel: [
			{ name: 'CourseId', index: 'CourseId', width: 80, align: 'center' },
			{ name: 'CourseName', index: 'CourseName', width: 120, align: 'center' },
			{ name: 'ProfessorFirstName', index: 'ProfessorFirstName', width: 200, align: 'center' },
			{ name: 'ProfessorLastName', index: 'ProfessorLastName', width: 200, align: 'center' },
			{ name: 'ProfessorEmail', index: 'ProfessorEmail', width: 200, align: 'center' }],
        pager: jQuery('#pagerRegistered'),
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: "desc",
        viewrecords: true,
        multiselect: true,
        imgpath: '',
        caption: 'My first grid'
    });
});

jQuery("#refreshButton").click(function () {
    $("#listTableNonRegistered").trigger('reloadGrid');
    $("#listTableRegistered").trigger('reloadGrid');
});

jQuery("#regButton").click(function regShow() {
    $("#popupRegister").show();
});

$("#popupSub").click(function () {
    s = parseInt(jQuery("#listTableNonRegistered").jqGrid('getGridParam', 'selarrrow'));
    var s2 = $("#notes").val();
    var s3 = $("#resume").val();
    alert(s);
    alert(s2);

    $.post("/User/Register2", { courseId: s, notes: s2, resume: s3 },
        function (data) {
            if (data) {
                //alert("Registration successful");
            }
            else {
                //alert("Registration unsuccessful. Try again");
            }
            $("#listTableNonRegistered").trigger('reloadGrid');
        });
});