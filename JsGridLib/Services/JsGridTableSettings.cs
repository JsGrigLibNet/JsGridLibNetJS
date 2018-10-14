namespace SampleHttpsServer
{
    public class JsGridTableSettings
    {
        public JsGridTableSettings()
        {
            this.insertRowLocation = "bottom";
            this.width = "100%";//"auto";
            this.height =  "500px";//"auto";
            this.autoload = true;
            this.heading = true;
            this.filtering = true;
            this.inserting = true;
            this.editing = true;
            this.selecting = true;
            this.sorting = true;
            this.paging = true;
            this.pageLoading = true;
            this.noDataContent = "Not found";
            this.confirmDeleting = true;
            this.deleteConfirm = "Are you sure?";
            this.pagerContainer = null;
            this.pageIndex = 1;
            this.pageSize = 20;
            this.pageButtonCount = 5;
            this.pagerFormat = "Pages : {first} {prev} {pages} {next} {last} &nbsp;&nbsp; {pageIndex} of {pageCount}";
            this.pagePrevText = "Prev";
            this.pageNextText = "Next";
            this.pageFirstText = "First";
            this.pageLastText = "Last";
            this.pageNavigatorNextText = "...";
            this.pageNavigatorPrevText = "...";
            this.invalidMessage = "Invalid data entered!";
            this.loadIndication = true;
            this.loadIndicationDelay = 500;
            this.loadMessage = "Please; wait...";
            this.loadShading = true;
            this.updateOnResize = true;
            this.rowRenderer = null;
            this.headerRowRenderer = null;
            this.filterRowRenderer = null;
            this.insertRowRenderer = null;
            this.editRowRenderer = null;
            this.pagerRenderer = null;
        }
        public string insertRowLocation { set; get; }// "bottom";
        public string width { set; get; }// "100%";//"auto";
        public string height { set; get; }// "500px";//"auto";
        public bool autoload { set; get; }// true;
        public bool heading { set; get; }// true;
        public bool filtering { set; get; }// true;
        public bool inserting { set; get; }// true;
        public bool editing { set; get; }// true;
        public bool selecting { set; get; }// true;
        public bool sorting { set; get; }// true;
        public bool paging { set; get; }// true;
        public bool pageLoading { set; get; }// true;
        public string noDataContent { set; get; }// "Not found";
        public bool confirmDeleting { set; get; }// true;
        public string deleteConfirm { set; get; }// "Are you sure?";
        public string pagerContainer { set; get; }// 5;
        public int pageIndex { set; get; }// 1;
        public int pageSize { set; get; }// 20;
        public int pageButtonCount { set; get; }// 15;
        public string pagerFormat { set; get; }// "Pages{set;get;}// {first} {prev} {pages} {next} {last} &nbsp;&nbsp; {pageIndex} of {pageCount}";
        public string pagePrevText { set; get; }// "Prev";
        public string pageNextText { set; get; }// "Next";
        public string pageFirstText { set; get; }// "First";
        public string pageLastText { set; get; }// "Last";
        public string pageNavigatorNextText { set; get; }// "...";
        public string pageNavigatorPrevText { set; get; }// "...";
        public string invalidMessage { set; get; }// "Invalid data entered!";    
        public bool loadIndication { set; get; }// true;
        public int loadIndicationDelay { set; get; }// 500;
        public string loadMessage { set; get; }// "Please; wait...";
        public bool loadShading { set; get; }// true;
        public bool updateOnResize { set; get; }// true;
        public string rowRenderer { set; get; }// null;
        public string headerRowRenderer { set; get; }// null;
        public string filterRowRenderer { set; get; }// null;
        public string insertRowRenderer { set; get; }// null;
        public string editRowRenderer { set; get; }// null;
        public PagerRenderer pagerRenderer { set; get; }// null
    }
}