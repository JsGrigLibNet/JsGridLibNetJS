


ej.base.enableRipple(true);
/**
 * RichTextEditor sample
 */

var defaultRTE = new ej.richtexteditor.RichTextEditor({
    toolbarSettings: {
        items: ['Bold', 'Italic', 'Underline', 'StrikeThrough',
            'FontName', 'FontSize', 'FontColor', 'BackgroundColor',
            'LowerCase', 'UpperCase', '|',
            'Formats', 'Alignments', 'OrderedList', 'UnorderedList',
            'Outdent', 'Indent', '|',
            'CreateTable', 'CreateLink', 'Image', '|', 'ClearFormat', 'Print',
            'SourceCode', 'FullScreen', '|', 'Undo', 'Redo']
    },
    showCharCount: true,
    maxLength: 2000
});
defaultRTE.appendTo("#inplace_comment_editor");

document.getElementById("btn1").addEventListener("click", function () {
    var rteObj = document.getElementById("defaultRTE").ej2_instances[0];
    console.log(rteObj.getHtml());
});
document.getElementById("btn2").addEventListener("click", function () {
    var rteObj = document.getElementById("defaultRTE").ej2_instances[0];
    console.log(rteObj.getText());
});