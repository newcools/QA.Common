$(
	function()
	{
		$(".my-collapse")
			.click(
				function(event)
				{
					var me = $(this);
					var divId = me.data("target-id");
					var div = $("#" + divId);
					var display = div.css("display");
					if (display != "none")
						div.css("display", "none");
					else
					    div.css("display", "block");
						
					event.preventDefault();
				}
			);
	}
);