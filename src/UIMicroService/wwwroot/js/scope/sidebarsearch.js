//function SearchSideBarItems() {
//    // Declare variables
//    var input, filter, ul, li, ul2, li2, a,b, i,span, txtValue;
//    input = document.getElementById('sidebar-SearchBox');

//    filter = input.value.toUpperCase();
//    ul = document.getElementById("LayoutUl");
//    li = ul.getElementsByTagName('li');

//    // Loop through all list items, and hide those who don't match the search query
//    for (i = 0; i < li.length; i++) {
//        a = li[i].getElementsByTagName("li")[0]; //a
//        if (a != undefined) {
//            txtValue = a.textContent || a.innerText;
//            if (txtValue.toUpperCase().indexOf(filter) > -1) {
//                li[i].style.display = "";
//            } else {
//                li[i].style.display = "none";
//            }

//        }
//    }
//}

//on sidebar input box search functionality
//$('#sidebar-SearchBox').on('keyup', function() {
//    FilterSidebarElements($(this));
//});
////Searchable sidebar function
//function FilterSidebarElements(element) {
//    var value = $(element).val();
//    $('#LayoutUl li').each(function () {
//        var temp = $(this).text().search(new RegExp(value, 'i'));
//        if ($(this).text().search(new RegExp(value, 'i')) > -1) {
//            $(this).show();
//        } else {
//            $(this).hide();
//        }
//    });
//}
$('#sidebar-SearchBox').keyup(function () {
    var searchTerms = $(this).val();
   
    if (searchTerms === "") {
        console.log(searchTerms);
        searchTerms = ' ';
        resetMenu(searchTerms);
        if ($("#LayoutUl li ul").css('display') === 'block') {
            $('#LayoutUl li ul').slideToggle();
            return;
        } else {
            $('#LayoutUl li ul').hide();
            return;
        }
    }
    resetMenu(searchTerms);
});
function resetMenu(searchTerms) {
    $('#LayoutUl li').each(function () {
        var $li = $(this);
        var hasMatch = searchTerms.length === 0 || $li.text().toLowerCase().indexOf(searchTerms.toLowerCase()) > 0;
        $li.toggle(hasMatch);
        if ($li.is(':hidden')) {
            $li.closest("ul").show('slow');
        }
    });
};

//For searchbar shortcut key setup
$(window).keydown(async function (event) {
    let sKey = 70; //F
    let Skey = 102; //f
    if (event.ctrlKey && (event.which === sKey || event.which === Skey)) {
        
        $('#mainHead').trigger('click');
        $('#sidebar-SearchBox').focus();
        event.preventDefault();
    }
});
