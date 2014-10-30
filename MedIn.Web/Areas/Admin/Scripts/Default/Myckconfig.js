
//CKEDITOR.config.contentsCss = "/styles/style.css"; // тут омжно подключить стилевой файл для контента
CKEDITOR.config.language = 'ru';
CKEDITOR.config.htmlEncodeOutput = false;
CKEDITOR.config.toolbar_Full =
[
    ['Source','-',/*'Save','NewPage',*/'Preview'/*,'-','Templates'*/],
    ['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print'/*, 'SpellChecker', 'Scayt'*/,
    'Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
    /*
	['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],['TextColor','BGColor','Maximize', 'ShowBlocks'],
    */
	'/',
    ['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
    ['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
    ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
    '/',
    ['Link','Unlink','Anchor','Image','Flash','Table',/*'HorizontalRule',*/'Smiley','SpecialChar'/*,'PageBreak'*/],
	'/',
	['Styles','Format','Font','FontSize','-','About']
];

CKEDITOR.on('instanceReady', function (ev) {
    var tags = ['p', 'ol', 'ul', 'li', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'div']; // etc.

    for (var key in tags) 
    {
        ev.editor.dataProcessor.writer.setRules(tags[key],
            {
                indent: false,
                breakBeforeOpen: true,
                breakAfterOpen: false,
                breakBeforeClose: false,
                breakAfterClose: true
            });
    }
});

CKEDITOR.config.contentsCss = '/Styles/default.css';