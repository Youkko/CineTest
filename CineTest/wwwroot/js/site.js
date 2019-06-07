// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
	var showCard = function (movieInfo) {
		var hasPoster = movieInfo.poster.substr(movieInfo.poster.length - 1) !== "/",
			hasBackdrop = movieInfo.backdrop.substr(movieInfo.backdrop.length - 1) !== "/",
			html = "<div class='movieInfo'>";
		html += "<div class='movieInfo-title'>" + (movieInfo.title || "") + "</div>";
		html += "<div class='movieInfo-poster'><img src=\"" + (hasPoster ? movieInfo.poster + "\" title=\"" + (movieInfo.title || "") + "\"" : "/img/noposter.png\" title=\"no poster found!\"") + " border =\"0\" /></div>";
		html += "<div class='movieInfo-overview'>" + (movieInfo.overview || "<center><i>- No overview found -</i></center>") + "</div>";
		html += "<div class='movieInfo-genres'><span class='genres-title'>Genre(s):</span> " + (movieInfo.genres || "<i>Unknown</i>") + "<br /><br /></div>";
		html += "<div class='movieInfo-releaseDate'><span class='releaseDate-title'>Release date:</span> " + (movieInfo.releaseDate || "<i>Unknown</i>") + "</div>";
		html += "</div>";
		$("#movieInfo").html(html);
		$("#movieInfo").css("background-image", hasBackdrop ? "url('" + movieInfo.backdrop + "')" : "");
	},
		setupButtons = function () {
			$("#movieList a").on("click", function () {
				$("#movieList a.active").removeClass("active");
				$(this).addClass("active");
				showCard({
					movieId: $(this).data("movieid"),
					poster: $(this).data("poster"),
					genres: $(this).data("genres"),
					backdrop: $(this).data("backdrop"),
					releaseDate: $(this).data("releasedate"),
					overview: $(this).data("overview"),
					title: $(this).text()
				});
			});

			if ($("#Search").val() !== "") {
				$("#searchField").val($("#Search").val());
			}

			$("#searchField").keyup(function (event) {
				if (event.key === "Enter") {
					$("#Search").val($(this).val());
					$("#ResetPagination").val(true);
					$("form").submit();
				}
			});
			$("#clearSearch").on("click", function () {
				$("#Search").val("");
				$("form").submit();
			});
		},
		createPageLink = function (currentPage, counter) {
			return "<span>" + (currentPage === counter ? "[&nbsp;" + counter + "&nbsp;]" : "<a href=\"#\" data-targetPage=\"" + counter + "\">" + counter + "</a>") + "</span>";
		},
		setupPagination = function () {

			var currentPageControl = $("input[name=CurrentPage]"),
				totalPagesControl  = $("input[name=TotalPages]"),
				currentPage		   = parseInt(currentPageControl.val()),
				totalPages		   = parseInt(totalPagesControl.val()),
				paginationTags	   = "",
				i				   = 1,
				printNumber		   = true,
				canPrintDots	   = false;

			paginationTags += "<span>" + (currentPage <= 1 ? "<< First" : "<a href=\"#\" data-targetPage=\"1\"><< First</a>") + "</span>";
			paginationTags += "<span>" + (currentPage <= 1 ? "< Previous" : "<a href=\"#\" data-targetPage=\"" + (currentPage > 1 ? currentPage - 1 : currentPage) + "\">< Previous</a>") + "</span>";
			for (i = 1; i <= totalPages; i++) {
				printNumber = i < 3 || i > totalPages - 2 || i >= 3 && i <= totalPages - 2 && i >= currentPage - 1 && i <= currentPage + 1;

				if (printNumber) {
					paginationTags += createPageLink(currentPage, i);
					canPrintDots = true;
				}
				else {
					if (canPrintDots) {
						paginationTags += "<span>...</span>";
						canPrintDots = false;
					}
				}
			}
			paginationTags += "<span>" + (totalPages < 2 ? "Next >" : "<a href=\"#\" data-targetPage=\"" + (currentPage + 1) + "\">Next ></a>") + "</span>";
			paginationTags += "<span>" + (totalPages < 2 ? "Last >>" : "<a href=\"#\" data-targetPage=\"" + totalPages + "\">Last >></a>") + "</span>";
			$("#Pagination").html(paginationTags);

			$("#Pagination span a").on("click", function () {
				$("input[name=CurrentPage]").val($(this).data("targetpage"));
				$("form").submit();
			});
		},
		init = function()
		{
			setupButtons();
			setupPagination();
			$("#movieList a:first-child").click();
		};


	$(document).ready(init);
})();