using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
using GuruComponents.EditorDemo.Commands;
using System.IO;
using GuruComponents.Netrix;
using NetrixHtmlEditorDemo.Windows;
using System.Web.UI.WebControls;
using System.Drawing;
using RibbonLib;
using RibbonLib.Interop;
using GuruComponents.EditorDemo.Dialogs;
using GuruComponents.Netrix.WebEditing.Elements;
using WeifenLuo.WinFormsUI.Docking;
using GuruComponents.Netrix.WebEditing.HighLighting;
using System.Windows.Forms;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using GuruComponents.Netrix.Networking;

namespace NetrixHtmlEditorDemo {
  public partial class MainForm {
    #region Ribbon Elements
    private RibbonApplicationMenu _applicationMenu;
    private RibbonHelpButton _helpButton;
    private RibbonRecentItems _recentItems;

    private RibbonTab _TabEdit;
    private RibbonButton _buttonHelpEdit;
    private RibbonButton _buttonHelpElement;
    private RibbonButton _buttonHelpDisplay;
    private RibbonButton _buttonHelpControl;
    private RibbonButton _buttonHelpText;
    private RibbonButton _buttonHelpStyles;
    private RibbonButton _buttonHelpHelpline;
    private RibbonButton _buttonHelpSpeller;
    private RibbonButton _buttonHelpTableEditor;
    private RibbonButton _buttonHelpTableOptions;

    private RibbonButton _buttonNew;
    private RibbonButton _buttonOpen;
    private RibbonButton _buttonSave;
    private RibbonButton _buttonPrint;
    private RibbonButton _buttonProperties;

    private RibbonButton _buttonExit;
    private RibbonButton _buttonUndo;
    private RibbonComboBox _comboboxUndoStack;
    private RibbonButton _buttonUndoStep;
    private RibbonButton _buttonRedoStep;
    private RibbonButton _buttonUndoAddPackedStack;
    private RibbonButton _buttonRedo;
    private RibbonFontControl _fontControl;
    private RibbonButton _buttonCopy;
    private RibbonButton _buttonPaste;
    private RibbonButton _buttonPasteImage;
    private RibbonButton _buttonCut;
    private RibbonButton _buttonInsertUL;
    private RibbonButton _buttonInsertOL;
    private RibbonButton _buttonOutdent;
    private RibbonButton _buttonIndent;
    private RibbonButton _buttonAlignRight;
    private RibbonButton _buttonAlignLeft;
    private RibbonButton _buttonAlignCenter;
    private RibbonButton _buttonAlignFull;
    private RibbonToggleButton _buttonViewSource;
    private RibbonButton _buttonSearch;
    private RibbonButton _buttonReplace;
    private RibbonSplitButtonGallery _splGlyphsVariant;
    private RibbonButton _buttonGlyphVariantNone;
    private RibbonButton _buttonGlyphVariantStandard;
    private RibbonButton _buttonGlyphVariantColored;
    private RibbonComboBox _comboboxGlyphKind;
    private RibbonToggleButton _togglebuttonDesignMode;

    // INSERT
    private RibbonTab _TabInsert;
    private RibbonDropDownGallery _galleryParagraphElementsHeader;
    private RibbonButton _buttonRemoveFormat;
    private RibbonButton _buttonRemoveInlineFormat;
    private RibbonButton _buttonParagraphElementsPre;
    private RibbonButton _buttonParagraphElementsCode;

    // GRID
    private RibbonTab _TabDisplay;
    private RibbonToggleButton _togglebuttonShowGrid;
    private RibbonSpinner _spinnerGridSize;
    private RibbonDropDownColorPicker _dropdownGridColor;
    private RibbonComboBox _comboboxGridStyle;
    private RibbonToggleButton _togglebuttonPositionMultipleSelection;
    // SCROLLBAR
    private RibbonToggleButton _toggleButtonScrollbarsEnabled;
    private RibbonDropDownColorPicker _colorpickerScrollbarColor;
    private RibbonButton _buttonScrollbarLeft;
    private RibbonButton _buttonScrollbarUp;
    private RibbonButton _buttonScrollbarDown;
    private RibbonButton _buttonScrollbarRight;
    private RibbonSpinner _spinnerScrollbarX;
    private RibbonSpinner _spinnerScrollbarY;
    // HELPLINE        
    private RibbonTabGroup _tabgroupHelpLine;
    private RibbonToggleButton _togglebuttonHelpLine_A;
    private RibbonDropDownColorPicker _dropdownHelpLineColor_A;
    private RibbonComboBox _comboboxHelpLineLineDash_A;
    private RibbonSpinner _spinnerHelpLineLineSize_A;
    private RibbonCheckBox _checkboxHelpLineX_A;
    private RibbonSpinner _spinnerHelpLinePositionX_A;
    private RibbonCheckBox _checkboxHelpLineY_A;
    private RibbonSpinner _spinnerHelpLinePositionY_A;
    private RibbonCheckBox _checkboxHelpLineSnap_A;
    private RibbonCheckBox _checkboxHelpLineSnapToGrid_A;
    private RibbonSpinner _spinnerHelpLineSnapGridSize_A;
    private RibbonToggleButton _togglebuttonHelpLine_B;
    private RibbonToggleButton _toggleRuler;
    // POSITION
    private RibbonToggleButton _toggleButtonPositionAbsolute;
    private RibbonButton _buttonPositionToFront;
    private RibbonButton _buttonPositionToBack;
    //TABLE
    private RibbonTab _TabTableEdit;
    private RibbonTab _TabTableOptions;
    private RibbonTabGroup _tabgroupTableTools;
    private RibbonButton _buttonInsertTable;
    private RibbonButton _buttonDeleteRow;
    private RibbonButton _buttonDeleteColumn;
    private RibbonButton _buttonInsertRow;
    private RibbonButton _buttonInsertColumn;
    private RibbonButton _buttonMergeCells;
    private RibbonButton _buttonMergeDown;
    private RibbonButton _buttonMergeUp;
    private RibbonButton _buttonMergeLeft;
    private RibbonButton _buttonMergeRight;
    private RibbonButton _buttonSplitCellHorizontal;
    private RibbonButton _buttonSplitCellVertical;
    private RibbonCheckBox _checkboxTableInsertAfter;
    private RibbonCheckBox _checkboxTableSliderActive;
    private RibbonCheckBox _checkboxTableSliderAddMode;
    private RibbonCheckBox _checkboxTableWithCellSelection;
    private RibbonCheckBox _checkboxTableProcessTabKey;
    private RibbonCheckBox _checkboxTableStaticBehavior;
    private RibbonCheckBox _checkboxTableAdvancedParameter;
    private RibbonDropDownColorPicker _colorpickerTableSliderColor;
    private RibbonDropDownColorPicker _colorpickerTableBorderColor;
    private RibbonDropDownColorPicker _colorpickerTableCellColor;
    // SPELLER
    private RibbonTab _TabSpeller;
    private RibbonTabGroup _spellerTabGroup;
    private RibbonDropDownGallery _dropdownSpellerDictionary;
    private RibbonCheckBox _checkboxSpellerIgnoreHtml;
    private RibbonCheckBox _checkboxSpellerIgnoreWordsWithDigits;
    private RibbonSpinner _spinnerSpellermaxSuggestionsCount;
    private RibbonCheckBox _checkboxSpellerignoreUpperCaseWords;
    private RibbonComboBox _comboboxSpellerSuggestionEnum;
    private RibbonDropDownColorPicker _colorpickerSpellerHighlightColor;
    private RibbonComboBox _comboboxSpellerHighlightUnderlineStyle;
    private RibbonToggleButton _toggleSpellerCommandBackground;
    private RibbonButton _buttonSpellerCommandCheckWordStart;
    private RibbonButton _buttonSpellerCommandCheckWordStop;
    private RibbonButton _buttonSpellerCommandRemoveHighlight;
    private RibbonButton _buttonSpellerCommandClearBuffer;
    // TEXT HIGHLIGHT
    private RibbonTab _TabText;
    private RibbonButton _buttonTextHighLightInvoke;
    private RibbonButton _buttonTextHighLightRemove;
    private RibbonDropDownColorPicker _colorpickerTextHighLightColor;
    private RibbonDropDownColorPicker _colorpickerTextHighLightTextColor;
    private RibbonDropDownColorPicker _colorpickerTextHighLightBackColor;
    private RibbonComboBox _comboboxTextHighLightText;
    private RibbonCheckBox _checkboxTextHighLightPatternLineThroughSingle;
    private RibbonCheckBox _checkboxTextHighLightPatternLineThroughDouble;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineDash;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineDotDash;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineDotDotDash;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineDotted;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineSingle;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineThick;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineThickDash;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineWave;
    private RibbonCheckBox _checkboxTextHighLightPatternUnderLineWords;
    private RibbonButton _buttonTextManipulationAllLowercase;
    private RibbonButton _buttonTextManipulationAllUppercase;
    private RibbonButton _buttonTextManipulationUppercase;
    private RibbonButton _buttonGroupTextShowSelection;
    private RibbonButton _buttonGroupTextSelectWord;
    private RibbonButton _buttonGroupTextSelectSentence;
    private RibbonButton _buttonGroupTextSelectParagraph;
    private RibbonButton _buttonGroupTextSelectAll;

    // CONTROL EVENTS
    private RibbonTab _TabControl;
    private RibbonToggleButton _togglebuttonControlAttachEvent;
    private RibbonToggleButton _togglebuttonControlShowEvent;
    private RibbonCheckBox _checkboxControlElementNameA;
    private RibbonCheckBox _checkboxControlElementNameIMG;
    private RibbonCheckBox _checkboxControlElementNameDIV;
    private RibbonCheckBox _checkboxControlElementNameBUTTON;
    // CONTROL EVENTS --> Single Event controls
    private RibbonCheckBox _checkboxControlEventNameClick;
    private RibbonCheckBox _checkboxControlEventNameDblClick;
    private RibbonCheckBox _checkboxControlEventNameMouseDown;
    private RibbonCheckBox _checkboxControlEventNameMouseEnter;
    private RibbonCheckBox _checkboxControlEventNameMouseLeave;
    private RibbonCheckBox _checkboxControlEventNameMouseOut;
    private RibbonCheckBox _checkboxControlEventNameMouseUp;
    private RibbonCheckBox _checkboxControlEventNameMouseWheel;
    private RibbonCheckBox _checkboxControlEventNameBeforeCut;
    private RibbonCheckBox _checkboxControlEventNameCut;
    private RibbonCheckBox _checkboxControlEventNameBeforeCopy;
    private RibbonCheckBox _checkboxControlEventNameCopy;
    private RibbonCheckBox _checkboxControlEventNameBeforePaste;
    private RibbonCheckBox _checkboxControlEventNamePaste;
    private RibbonCheckBox _checkboxControlEventNameResize;
    private RibbonCheckBox _checkboxControlEventNameResizeEnd;
    private RibbonCheckBox _checkboxControlEventNameResizeStart;
    private RibbonCheckBox _checkboxControlEventNameMove;
    private RibbonCheckBox _checkboxControlEventNameMoveEnd;
    private RibbonCheckBox _checkboxControlEventNameMoveStart;
    private RibbonCheckBox _checkboxControlEventNameDrag;
    private RibbonCheckBox _checkboxControlEventNameDragEnd;
    private RibbonCheckBox _checkboxControlEventNameDragEnter;
    private RibbonCheckBox _checkboxControlEventNameDragLeave;
    private RibbonCheckBox _checkboxControlEventNameDragStart;
    private RibbonCheckBox _checkboxControlEventNameDragOver;
    private RibbonCheckBox _checkboxControlEventNameDrop;
    private RibbonCheckBox _checkboxControlEventNameControlSelect;
    private RibbonCheckBox _checkboxControlEventNameSelect;
    private RibbonCheckBox _checkboxControlEventNameSelectStart;
    private RibbonCheckBox _checkboxControlEventNameChange;
    private RibbonCheckBox _checkboxControlEventNameSelectionChange;
    private RibbonCheckBox _checkboxControlEventNameFocus;
    private RibbonCheckBox _checkboxControlEventNameFocusin;
    private RibbonCheckBox _checkboxControlEventNameFocusout;
    private RibbonCheckBox _checkboxControlEventNameBlur;
    // STYLES
    private RibbonToggleButton _toggleButtonStylesLinkedStylesEnabled;
    private RibbonButton _toggleStylesEditBodyStyle;


    private RibbonToggleButton _toggleControlToggleOutline;

    #endregion

    #region Ribbon Actions

    void AttachRibbonEvents() {
      _applicationMenu = new RibbonApplicationMenu(ribbon1, (uint)RibbonCommands.cmdApplicationMenu);
      _helpButton = new RibbonHelpButton(ribbon1, (uint)RibbonCommands.cmdHelp);
      _recentItems = new RibbonRecentItems(ribbon1, (uint)RibbonCommands.cmdRecentFiles);

      _TabEdit = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabEdit);
      _TabInsert = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabInsert);
      _TabControl = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabControl);
      _TabDisplay = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabDisplay);
      _TabText = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabText);
      _TabTableEdit = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabTableTool);
      _TabTableOptions = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabTableOptions);
      _TabSpeller = new RibbonTab(ribbon1, (uint)RibbonCommands.cmdTabSpeller);

      _buttonHelpEdit = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpEdit);
      _buttonHelpElement = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpElement);
      _buttonHelpDisplay = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpDisplay);
      _buttonHelpControl = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpControl);
      _buttonHelpText = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpText);
      _buttonHelpStyles = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpStyles);
      _buttonHelpHelpline = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpHelpline);
      _buttonHelpSpeller = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpSpeller);
      _buttonHelpTableEditor = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpTableEditor);
      _buttonHelpTableOptions = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdHelpTableOptions);

      _buttonNew = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonNew);
      _buttonOpen = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonOpen);
      _buttonSave = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonSave);
      _buttonPrint = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonPrint);
      _buttonProperties = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonProperties);
      _buttonExit = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonExit);
      _buttonUndo = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonUndo);
      _comboboxUndoStack = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdUndoStack);
      _buttonRedo = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonRedo);
      _buttonUndoStep = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdUndoStep);
      _buttonRedoStep = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdRedoStep);
      _buttonUndoAddPackedStack = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdUndoAddPackedStack);

      _fontControl = new RibbonFontControl(ribbon1, (uint)RibbonCommands.cmdFont);

      _buttonPaste = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonPaste);
      _buttonPasteImage = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonPasteImage);
      _buttonCut = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonCut);
      _buttonCopy = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonCopy);

      _buttonInsertUL = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonInsertUL);
      _buttonInsertOL = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonInsertOL);
      _buttonOutdent = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonInsertOutdent);
      _buttonIndent = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonInsertIndent);
      _buttonAlignLeft = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonAlignLeft);
      _buttonAlignRight = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonAlignRight);
      _buttonAlignCenter = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonAlignCenter);
      _buttonAlignFull = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonAlignFull);
      _splGlyphsVariant = new RibbonSplitButtonGallery(ribbon1, (uint)RibbonCommands.cmdGlyphsVariant);
      _buttonGlyphVariantNone = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGlyphVariantNone);
      _buttonGlyphVariantStandard = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGlyphVariantStandard);
      _buttonGlyphVariantColored = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGlyphVariantColored);
      _comboboxGlyphKind = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdGlyphKind);
      _comboboxGlyphKind.RepresentativeString = "XXXXXXXXX";

      _togglebuttonDesignMode = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdDesignMode);

      _buttonViewSource = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdButtonViewSource);
      _buttonSearch = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonSearch);
      _buttonReplace = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdButtonReplace);
      // INSERT ELEMENT
      _galleryParagraphElementsHeader = new RibbonDropDownGallery(ribbon1, (uint)RibbonCommands.cmdGroupParagraphElementsHeader);
      _buttonRemoveFormat = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdRemoveFormat);
      _buttonRemoveInlineFormat = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdRemoveInlineFormat);
      _buttonParagraphElementsPre = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupParagraphElementsPre);
      _buttonParagraphElementsCode = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupParagraphElementsCode);
      // GRID
      _togglebuttonShowGrid = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdGridShow);
      _spinnerGridSize = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdGridSize);
      _dropdownGridColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdGridColor);
      _comboboxGridStyle = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdGridStyle);
      _comboboxGridStyle.RepresentativeString = "XXXXXXXXXXXX";
      _togglebuttonPositionMultipleSelection = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdPositionMultipleSelection);
      // SCROLLBAR
      _toggleButtonScrollbarsEnabled = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdScrollbarsEnabled);
      _colorpickerScrollbarColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdScrollbarColor);
      _buttonScrollbarLeft = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdScrollbarLeft);
      _buttonScrollbarUp = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdScrollbarUp);
      _buttonScrollbarDown = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdScrollbarDown);
      _buttonScrollbarRight = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdScrollbarRight);
      _spinnerScrollbarX = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdScrollbarX);
      _spinnerScrollbarX.DecimalPlaces = 0;
      _spinnerScrollbarY = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdScrollbarY);
      _spinnerScrollbarY.DecimalPlaces = 0;
      // HELPLINE
      _tabgroupHelpLine = new RibbonTabGroup(ribbon1, (uint)RibbonCommands.cmdHelplineTools);
      _tabgroupHelpLine.ContextAvailable = ContextAvailability.Available;
      _togglebuttonHelpLine_A = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdHelpLine_A);
      _dropdownHelpLineColor_A = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdHelpLineColor_A);
      _comboboxHelpLineLineDash_A = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdHelpLineLineDash_A);
      _comboboxHelpLineLineDash_A.RepresentativeString = "Line Dash Pattern";
      _spinnerHelpLineLineSize_A = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdHelpLineLineSize_A);
      _checkboxHelpLineX_A = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdHelpLineX_A);
      _spinnerHelpLinePositionX_A = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdHelpLinePositionX_A);
      _checkboxHelpLineY_A = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdHelpLineY_A);
      _spinnerHelpLinePositionY_A = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdHelpLinePositionY_A);
      _checkboxHelpLineSnap_A = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdHelpLineSnap_A);
      _checkboxHelpLineSnapToGrid_A = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdHelpLineSnapToGrid_A);
      _spinnerHelpLineSnapGridSize_A = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdHelpLineSnapGridSize_A);
      _togglebuttonHelpLine_B = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdHelpLine_B);
      _toggleRuler = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdRuler);

      _toggleButtonPositionAbsolute = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdPositionAbsolute);
      _buttonPositionToFront = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdPositionToFront);
      _buttonPositionToBack = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdPositionToBack);
      // TABLE
      _tabgroupTableTools = new RibbonTabGroup(ribbon1, (uint)RibbonCommands.cmdTabGroupTableTools);
      _buttonInsertTable = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdInsertTABLE);
      _buttonDeleteRow = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdDeleteRow);
      _buttonDeleteColumn = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdDeleteColumn);
      _buttonInsertRow = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdInsertRow);
      _buttonInsertColumn = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdInsertColumn);
      _buttonMergeCells = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdMergeCells);
      _buttonMergeDown = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdMergeDown);
      _buttonMergeUp = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdMergeUp);
      _buttonMergeLeft = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdMergeLeft);
      _buttonMergeRight = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdMergeRight);
      _buttonSplitCellHorizontal = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSplitCellHorizontal);
      _buttonSplitCellVertical = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSplitCellVertical);
      _checkboxTableInsertAfter = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableInsertAfter);
      _checkboxTableSliderActive = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableSliderActive);
      _checkboxTableSliderAddMode = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableSliderAddMode);
      _checkboxTableWithCellSelection = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableWithCellSelection);
      _checkboxTableProcessTabKey = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableProcessTabKey);
      _checkboxTableStaticBehavior = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableStaticBehavior);
      _checkboxTableAdvancedParameter = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTableAdvancedParameter);
      _colorpickerTableSliderColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTableSliderColor);
      _colorpickerTableBorderColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTableBorderColor);
      _colorpickerTableCellColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTableCellColor);
      // SPELLER
      _spellerTabGroup = new RibbonTabGroup(ribbon1, (uint)RibbonCommands.cmdSpellerTools);
      _spellerTabGroup.ContextAvailable = ContextAvailability.Available;
      _dropdownSpellerDictionary = new RibbonDropDownGallery(ribbon1, (uint)RibbonCommands.cmdSpellerDictionary);
      _checkboxSpellerIgnoreHtml = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdSpellerIgnoreHtml);
      _checkboxSpellerIgnoreWordsWithDigits = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdSpellerIgnoreWordsWithDigits);
      _spinnerSpellermaxSuggestionsCount = new RibbonSpinner(ribbon1, (uint)RibbonCommands.cmdSpellermaxSuggestionsCount);
      _checkboxSpellerignoreUpperCaseWords = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdSpellerignoreUpperCaseWords);
      _comboboxSpellerSuggestionEnum = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdSpellerSuggestionEnum);
      _colorpickerSpellerHighlightColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdSpellerHighlightColor);
      _comboboxSpellerHighlightUnderlineStyle = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdSpellerHighlightUnderlineStyle);
      _toggleSpellerCommandBackground = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdSpellerCommandBackground);
      _buttonSpellerCommandCheckWordStart = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSpellerCommandCheckWordStart);
      _buttonSpellerCommandCheckWordStop = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSpellerCommandCheckWordStop);
      _buttonSpellerCommandRemoveHighlight = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSpellerCommandRemoveHighlight);
      _buttonSpellerCommandClearBuffer = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdSpellerCommandClearBuffer);
      // TEXT HIGHLIGHT
      _buttonTextHighLightInvoke = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdTextHighLightInvoke);
      _buttonTextHighLightRemove = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdTextHighLightRemove);
      _colorpickerTextHighLightColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTextHighLightColor);
      _colorpickerTextHighLightTextColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTextHighLightTextColor);
      _colorpickerTextHighLightBackColor = new RibbonDropDownColorPicker(ribbon1, (uint)RibbonCommands.cmdTextHighLightBackColor);
      _comboboxTextHighLightText = new RibbonComboBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightText);
      _checkboxTextHighLightPatternLineThroughSingle = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternLineThroughSingle);
      _checkboxTextHighLightPatternLineThroughDouble = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternLineThroughDouble);
      _checkboxTextHighLightPatternUnderLineDash = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineDash);
      _checkboxTextHighLightPatternUnderLineDotDash = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineDotDash);
      _checkboxTextHighLightPatternUnderLineDotDotDash = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineDotDotDash);
      _checkboxTextHighLightPatternUnderLineDotted = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineDotted);
      _checkboxTextHighLightPatternUnderLineSingle = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineSingle);
      _checkboxTextHighLightPatternUnderLineThick = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineThick);
      _checkboxTextHighLightPatternUnderLineThickDash = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineThickDash);
      _checkboxTextHighLightPatternUnderLineWave = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineWave);
      _checkboxTextHighLightPatternUnderLineWords = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdTextHighLightPatternUnderLineWords);
      _buttonTextManipulationAllLowercase = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdTextManipulationAllLowercase);
      _buttonTextManipulationAllUppercase = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdTextManipulationAllUppercase);
      _buttonTextManipulationUppercase = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdTextManipulationUppercase);
      _buttonGroupTextShowSelection = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupTextShowSelection);
      _buttonGroupTextSelectWord = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupTextSelectWord);
      _buttonGroupTextSelectSentence = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupTextSelectSentence);
      _buttonGroupTextSelectParagraph = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupTextSelectParagraph);
      _buttonGroupTextSelectAll = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdGroupTextSelectAll);

      // CONTROL EVENTS
      _togglebuttonControlAttachEvent = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdControlAttachEvent);
      _togglebuttonControlShowEvent = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdControlShowEvent);
      _checkboxControlElementNameA = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlElementNameA);
      _checkboxControlElementNameIMG = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlElementNameIMG);
      _checkboxControlElementNameDIV = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlElementNameDIV);
      _checkboxControlElementNameBUTTON = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlElementNameBUTTON);

      _checkboxControlEventNameClick = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameClick);
      _checkboxControlEventNameDblClick = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDblClick);
      _checkboxControlEventNameMouseDown = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseDown);
      _checkboxControlEventNameMouseEnter = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseEnter);
      _checkboxControlEventNameMouseLeave = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseLeave);
      _checkboxControlEventNameMouseOut = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseOut);
      _checkboxControlEventNameMouseUp = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseUp);
      _checkboxControlEventNameMouseWheel = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMouseWheel);
      _checkboxControlEventNameBeforeCut = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameBeforeCut);
      _checkboxControlEventNameCut = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameCut);
      _checkboxControlEventNameBeforeCopy = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameBeforeCopy);
      _checkboxControlEventNameCopy = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameCopy);
      _checkboxControlEventNameBeforePaste = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameBeforePaste);
      _checkboxControlEventNamePaste = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNamePaste);
      _checkboxControlEventNameResize = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameResize);
      _checkboxControlEventNameResizeEnd = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameResizeEnd);
      _checkboxControlEventNameResizeStart = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameResizeStart);
      _checkboxControlEventNameMove = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMove);
      _checkboxControlEventNameMoveEnd = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMoveEnd);
      _checkboxControlEventNameMoveStart = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameMoveStart);
      _checkboxControlEventNameDrag = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDrag);
      _checkboxControlEventNameDragEnd = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDragEnd);
      _checkboxControlEventNameDragEnter = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDragEnter);
      _checkboxControlEventNameDragLeave = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDragLeave);
      _checkboxControlEventNameDragStart = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDragStart);
      _checkboxControlEventNameDragOver = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDragOver);
      _checkboxControlEventNameDrop = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameDrop);
      _checkboxControlEventNameControlSelect = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameControlSelect);
      _checkboxControlEventNameSelect = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameSelect);
      _checkboxControlEventNameSelectStart = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameSelectStart);
      _checkboxControlEventNameChange = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameChange);
      _checkboxControlEventNameSelectionChange = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameSelectionChange);
      _checkboxControlEventNameFocus = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameFocus);
      _checkboxControlEventNameFocusin = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameFocusin);
      _checkboxControlEventNameFocusout = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameFocusout);
      _checkboxControlEventNameBlur = new RibbonCheckBox(ribbon1, (uint)RibbonCommands.cmdControlEventNameBlur);
      _toggleControlToggleOutline = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdControlToggleOutline);

      // STYLES
      _toggleButtonStylesLinkedStylesEnabled = new RibbonToggleButton(ribbon1, (uint)RibbonCommands.cmdStylesLinkedStylesEnabled);
      _toggleStylesEditBodyStyle = new RibbonButton(ribbon1, (uint)RibbonCommands.cmdStylesEditBodyStyle);

      // EVENTS

      _buttonHelpEdit.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpEdit_ExecuteEvent);
      _buttonHelpElement.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpElement_ExecuteEvent);
      _buttonHelpDisplay.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpDisplay_ExecuteEvent);
      _buttonHelpControl.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpControl_ExecuteEvent);
      _buttonHelpText.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpText_ExecuteEvent);
      _buttonHelpStyles.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpStyles_ExecuteEvent);
      _buttonHelpHelpline.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpHelpline_ExecuteEvent);
      _buttonHelpSpeller.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpSpeller_ExecuteEvent);
      _buttonHelpTableEditor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpTableEditor_ExecuteEvent);
      _buttonHelpTableOptions.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonHelpTableOptions_ExecuteEvent);

      _helpButton.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_helpButton_ExecuteEvent);
      _buttonNew.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonNew_ExecuteEvent);
      _buttonOpen.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonOpen_ExecuteEvent);
      _buttonSave.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSave_ExecuteEvent);
      _buttonPrint.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonPrint_ExecuteEvent);
      _buttonProperties.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonProperties_ExecuteEvent);
      _buttonExit.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonExit_ExecuteEvent);
      _buttonUndo.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonUndo_ExecuteEvent);
      _comboboxUndoStack.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxUndoStack_ItemsSourceReady);
      _comboboxUndoStack.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxUndoStack_ExecuteEvent);
      _comboboxUndoStack.RepresentativeString = "XXXXXXXXXXXX";
      _buttonRedo.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonRedo_ExecuteEvent);
      _buttonUndoStep.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonUndoStep_ExecuteEvent);
      _buttonRedoStep.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonRedoStep_ExecuteEvent);
      _buttonUndoAddPackedStack.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonUndoAddPackedStack_ExecuteEvent);

      _recentItems.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_recentItems_ExecuteEvent);

      _fontControl.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_fontControl_ExecuteEvent);

      _buttonPaste.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonPaste_ExecuteEvent);
      _buttonPasteImage.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonPasteImage_ExecuteEvent);
      _buttonCut.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonCut_ExecuteEvent);
      _buttonCopy.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonCopy_ExecuteEvent);

      _buttonInsertUL.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonInsertUL_ExecuteEvent);
      _buttonInsertOL.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonInsertOL_ExecuteEvent);
      _buttonIndent.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonIndent_ExecuteEvent);
      _buttonOutdent.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonOutdent_ExecuteEvent);
      _buttonAlignLeft.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonAlignLeft_ExecuteEvent);
      _buttonAlignRight.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonAlignRight_ExecuteEvent);
      _buttonAlignCenter.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonAlignCenter_ExecuteEvent);
      _buttonAlignFull.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonAlignFull_ExecuteEvent);

      _splGlyphsVariant.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGlyphsVariant_ExecuteEvent);
      _buttonGlyphVariantNone.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGlyphVariantNone_ExecuteEvent);
      _buttonGlyphVariantStandard.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGlyphVariantStandard_ExecuteEvent);
      _buttonGlyphVariantColored.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGlyphVariantColored_ExecuteEvent);
      _comboboxGlyphKind.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxGlyphKind_ItemsSourceReady);
      _comboboxGlyphKind.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxGlyphKind_ExecuteEvent);
      _buttonViewSource.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonViewSource_ExecuteEvent);

      _togglebuttonDesignMode.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleButtonDesignMode_ExecuteEvent);

      _buttonSearch.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSearch_ExecuteEvent);
      _buttonReplace.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonReplace_ExecuteEvent);
      // INSERT ELEMENT
      _galleryParagraphElementsHeader.ItemsSourceReady += new EventHandler<EventArgs>(_galleryParagraphElementsHeader_ItemsSourceReady);
      _buttonRemoveFormat.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonRemoveFormat_ExecuteEvent);
      _buttonRemoveInlineFormat.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonRemoveInlineFormat_ExecuteEvent);
      _galleryParagraphElementsHeader.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_galleryParagraphElementsHeader_ExecuteEvent);
      _buttonParagraphElementsPre.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonParagraphElementsPre_ExecuteEvent);
      _buttonParagraphElementsCode.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonParagraphElementsCode_ExecuteEvent);

      // GRID
      _togglebuttonShowGrid.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonShowGrid_ExecuteEvent);
      _spinnerGridSize.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerGridSize_ExecuteEvent);
      _dropdownGridColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_dropdownGridColor_ExecuteEvent);
      _comboboxGridStyle.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxGridStyle_ExecuteEvent);
      _togglebuttonPositionMultipleSelection.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_togglebuttonPositionMultipleSelection_ExecuteEvent);
      // SCROLLBAR
      _toggleButtonScrollbarsEnabled.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleButtonScrollbarsEnabled_ExecuteEvent);
      _colorpickerScrollbarColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_colorpickerScrollbarColor_ExecuteEvent);
      _buttonScrollbarLeft.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonScrollbarLeft_ExecuteEvent);
      _buttonScrollbarUp.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonScrollbarUp_ExecuteEvent);
      _buttonScrollbarDown.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonScrollbarDown_ExecuteEvent);
      _buttonScrollbarRight.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonScrollbarRight_ExecuteEvent);
      // --> note: spinner used to display current scrollbar position only
      _spinnerScrollbarX.DecimalValue = 0;
      _spinnerScrollbarX.Enabled = false;
      _spinnerScrollbarY.DecimalValue = 0;
      _spinnerScrollbarY.Enabled = false;

      // HELPLINE
      _togglebuttonHelpLine_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleHelpLine_A_ExecuteEvent);
      _dropdownHelpLineColor_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_dropdownHelpLineColor_A_ExecuteEvent);
      _comboboxHelpLineLineDash_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxHelpLineLineDash_A_ExecuteEvent);
      _spinnerHelpLineLineSize_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerHelpLineLineSize_A_ExecuteEvent);
      _checkboxHelpLineX_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxHelpLineX_A_ExecuteEvent);
      _spinnerHelpLinePositionX_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerHelpLinePositionX_A_ExecuteEvent);
      _checkboxHelpLineY_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxHelpLineY_A_ExecuteEvent);
      _spinnerHelpLinePositionY_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerHelpLinePositionY_A_ExecuteEvent);
      _checkboxHelpLineSnap_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxHelpLineSnap_A_ExecuteEvent);
      _checkboxHelpLineSnapToGrid_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxHelpLineSnapToGrid_A_ExecuteEvent);
      _spinnerHelpLineSnapGridSize_A.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerHelpLineSnapGridSize_A_ExecuteEvent);
      _togglebuttonHelpLine_B.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_togglebuttonHelpLine_B_ExecuteEvent);
      _toggleRuler.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleRuler_ExecuteEvent);

      // POSITION
      _toggleButtonPositionAbsolute.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleButtonPositionAbsolute_ExecuteEvent);
      _buttonPositionToFront.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonPositionToFront_ExecuteEvent);
      _buttonPositionToBack.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonPositionToBack_ExecuteEvent);

      // GRID
      _comboboxGridStyle.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxGridStyle_ItemsSourceReady);
      // HELPLINE
      _comboboxHelpLineLineDash_A.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxHelpLineLineDash_A_ItemsSourceReady);


      // TABLE
      _buttonInsertTable.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonInsertTable_ExecuteEvent);
      _buttonDeleteRow.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonDeleteRow_ExecuteEvent);
      _buttonDeleteColumn.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonDeleteColumn_ExecuteEvent);
      _buttonInsertRow.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonInsertRow_ExecuteEvent);
      _buttonInsertColumn.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonInsertColumn_ExecuteEvent);
      _buttonMergeCells.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonMergeCells_ExecuteEvent);
      _buttonMergeDown.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonMergeDown_ExecuteEvent);
      _buttonMergeUp.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonMergeUp_ExecuteEvent);
      _buttonMergeLeft.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonMergeLeft_ExecuteEvent);
      _buttonMergeRight.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonMergeRight_ExecuteEvent);
      _buttonSplitCellHorizontal.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSplitCellHorizontal_ExecuteEvent);
      _buttonSplitCellVertical.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSplitCellVertical_ExecuteEvent);
      _checkboxTableSliderActive.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableSliderActive_ExecuteEvent);
      _checkboxTableSliderAddMode.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableSliderAddMode_ExecuteEvent);
      _checkboxTableWithCellSelection.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTableWithCellSelection_ExecuteEvent);
      _checkboxTableProcessTabKey.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableProcessTabKey_ExecuteEvent);
      _checkboxTableStaticBehavior.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableStaticBehavior_ExecuteEvent);
      _checkboxTableAdvancedParameter.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableAdvancedParameter_ExecuteEvent);
      _colorpickerTableSliderColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableSliderColor_ExecuteEvent);
      _colorpickerTableBorderColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableBorderColor_ExecuteEvent);
      _colorpickerTableCellColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTableCellColor_ExecuteEvent);

      // SPELLER
      _dropdownSpellerDictionary.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_dropdownSpellerDictionary_ExecuteEvent);
      _dropdownSpellerDictionary.ItemsSourceReady += new EventHandler<EventArgs>(_dropdownSpellerDictionary_ItemsSourceReady);
      _checkboxSpellerIgnoreHtml.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxSpellerIgnoreHtml_ExecuteEvent);
      _checkboxSpellerIgnoreWordsWithDigits.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxSpellerIgnoreWordsWithDigits_ExecuteEvent);
      _spinnerSpellermaxSuggestionsCount.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_spinnerSpellermaxSuggestionsCount_ExecuteEvent);
      _checkboxSpellerignoreUpperCaseWords.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxSpellerignoreUpperCaseWords_ExecuteEvent);
      _comboboxSpellerSuggestionEnum.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxSpellerSuggestionEnum_ExecuteEvent);
      _comboboxSpellerSuggestionEnum.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxSpellerSuggestionEnum_ItemsSourceReady);

      _colorpickerSpellerHighlightColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_colorpickerSpellerHighlightColor_ExecuteEvent);
      _comboboxSpellerHighlightUnderlineStyle.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_comboboxSpellerHighlightUnderlineStyle_ExecuteEvent);
      _comboboxSpellerHighlightUnderlineStyle.ItemsSourceReady += new EventHandler<EventArgs>(_comboboxSpellerHighlightUnderlineStyle_ItemsSourceReady);
      _toggleSpellerCommandBackground.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleSpellerCommandBackground_ExecuteEvent);
      _buttonSpellerCommandCheckWordStart.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSpellerCommandCheckWordStart_ExecuteEvent);
      _buttonSpellerCommandCheckWordStop.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSpellerCommandCheckWordStop_ExecuteEvent);
      _buttonSpellerCommandRemoveHighlight.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSpellerCommandRemoveHighlight_ExecuteEvent);
      _buttonSpellerCommandClearBuffer.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonSpellerCommandClearBuffer_ExecuteEvent);

      // TEXT HIGHLIGHT
      _buttonTextHighLightInvoke.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTextHighLightInvoke_ExecuteEvent);
      _buttonTextHighLightRemove.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTextHighLightRemove_ExecuteEvent);
      _colorpickerTextHighLightColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_colorpickerTextHighLightColor_ExecuteEvent);
      _colorpickerTextHighLightTextColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_colorpickerTextHighLightTextColor_ExecuteEvent);
      _colorpickerTextHighLightBackColor.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_colorpickerTextHighLightBackColor_ExecuteEvent);
      // the invoke action collects the settings from other tools, other tools have no immediate action 
      _checkboxTextHighLightPatternLineThroughSingle.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternLineThroughDouble.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineDash.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineDotDash.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineDotDotDash.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineDotted.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineSingle.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineThick.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineThickDash.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineWave.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _checkboxTextHighLightPatternUnderLineWords.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_checkboxTextHighLightPattern_ExecuteEvent);
      _buttonTextManipulationAllLowercase.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTextManipulationAllLowercase_ExecuteEvent);
      _buttonTextManipulationAllUppercase.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTextManipulationAllUppercase_ExecuteEvent);
      _buttonTextManipulationUppercase.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonTextManipulationUppercase_ExecuteEvent);
      _buttonGroupTextShowSelection.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGroupTextShowSelection_ExecuteEvent);
      _buttonGroupTextSelectWord.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGroupTextSelectWord_ExecuteEvent);
      _buttonGroupTextSelectSentence.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGroupTextSelectSentence_ExecuteEvent);
      _buttonGroupTextSelectParagraph.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGroupTextSelectParagraph_ExecuteEvent);
      _buttonGroupTextSelectAll.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_buttonGroupTextSelectAll_ExecuteEvent);

      // CONTROL EVENTS
      _togglebuttonControlAttachEvent.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_togglebuttonControlAttachEvent_ExecuteEvent);
      _togglebuttonControlShowEvent.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_togglebuttonControlShowEvent_ExecuteEvent);
      _toggleControlToggleOutline.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleControlToggleOutline_ExecuteEvent);

      // STYLE EVENTS
      _toggleButtonStylesLinkedStylesEnabled.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleButtonStylesLinkedStylesEnabled_ExecuteEvent);
      _toggleStylesEditBodyStyle.ExecuteEvent += new EventHandler<ExecuteEventArgs>(_toggleStylesEditBodyStyle_ExecuteEvent);

    }


    #region STYLES

    private EditStyles editStylesDialog = null;

    void _toggleStylesEditBodyStyle_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (editStylesDialog == null) {
        editStylesDialog = new EditStyles();
      }
      IElement currentElement = EditorDocument.HtmlEditor.GetCurrentElement();
      string cssText = currentElement.GetStyle();
      editStylesDialog.Element = currentElement;
      editStylesDialog.CssText = cssText;
      if (editStylesDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
        currentElement.SetStyle(editStylesDialog.CssText);
      }
    }

    void _toggleButtonStylesLinkedStylesEnabled_ExecuteEvent(object sender, ExecuteEventArgs e) {
      bool enable = _toggleButtonStylesLinkedStylesEnabled.BooleanValue;
      // assume the file "LinkedStylesheet.css" is in the output directory
      // for the first time, we add it to the current document
      bool alreadyLinked = false;
      if (EditorDocument.HtmlEditor.DocumentStructure.LinkedStylesheets.Count > 0) {
        IElement linkedCss = EditorDocument.HtmlEditor.GetElementById("LinkElement2134");
        if (linkedCss != null && linkedCss is LinkElement) {
          alreadyLinked = true;
          linkedCss.ElementDom.RemoveElement(false);
        }
      }
      string css = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\Css\\LinkedStylesheet.css");
      if (File.Exists(css) && !alreadyLinked && enable) {
        LinkElement link = EditorDocument.HtmlEditor.CreateElement("LINK") as LinkElement;
        link.rel = "Stylesheet";
        link.media = LinkElementMedia.All;
        link.type = "text/css";
        link.href = css;
        link.ID = "LinkElement2134";
        EditorDocument.HtmlEditor.DocumentStructure.LinkedStylesheets.Add(link);
      }
      if (enable) {
        MessageBox.Show(String.Format("{0} linked styles found", EditorDocument.HtmlEditor.DocumentStructure.LinkedStylesheets.Count), "Linked Stylesheet", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    #endregion

    #region INSERT ELEMENTS

    void _buttonRemoveFormat_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.RemoveParagraphFormat();
    }

    void _buttonRemoveInlineFormat_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.RemoveInlineFormat();
    }

    void _galleryParagraphElementsHeader_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection headerItems = _galleryParagraphElementsHeader.ItemsSource;
      headerItems.Clear();
      // here we create the Header images on the fly 
      int[] fontmap = new int[] { 16, 14, 12, 10, 9, 8, 6 };
      Brush b = new SolidBrush(Color.Black);
      for (int i = 1; i < 7; i++) {
        Bitmap image = new Bitmap(32, 32);
        using (Graphics g = Graphics.FromImage(image)) {
          System.Drawing.Font f = new Font(FontFamily.GenericSansSerif, fontmap[i]);
          string t = String.Format("H{0}", i);
          SizeF sf = g.MeasureString(t, f);
          g.DrawString(t, f, b, 16 - sf.Width / 2, 16 - sf.Height / 2);
          headerItems.Add(new GalleryItemPropertySet() {
            ItemImage = ribbon1.ConvertToUIImage(image),
            Label = String.Format("Heading{0}", i),
            CategoryID = 1
          });
        }
      }
    }

    void _galleryParagraphElementsHeader_ExecuteEvent(object sender, ExecuteEventArgs e) {
      uint id = _galleryParagraphElementsHeader.SelectedItem;
      object item;
      // use the label to transport the enum's value we need to format a paragraph
      _galleryParagraphElementsHeader.ItemsSource.GetItem(id, out item);
      if (item != null) {
        HtmlFormat format = (HtmlFormat)Enum.Parse(typeof(HtmlFormat), ((GalleryItemPropertySet)item).Label);
        EditorDocument.HtmlEditor.TextFormatting.SetHtmlFormat(format);
      }
    }

    void _buttonParagraphElementsCode_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Document.InsertCode();
    }

    void _buttonParagraphElementsPre_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Document.InsertPreformatted();
    }

    #endregion INSERT ELEMENTS

    #region UNDO

    void _buttonUndoStep_ExecuteEvent(object sender, EventArgs e) {
      EditorDocument.HtmlEditor.Undo();
    }

    void _buttonRedoStep_ExecuteEvent(object sender, EventArgs e) {
      EditorDocument.HtmlEditor.Redo();
    }

    void _buttonUndoAddPackedStack_ExecuteEvent(object sender, EventArgs e) {
      // The purpose of the undo packer is to pack several steps into one undo step. If the user "undoes" this step, all packed steps are undone
      IUndoStack stack = EditorDocument.HtmlEditor.GetUndoManager("Private Step");
      // all steps within this section are packed into one step. It appears with the given name "Private Step"

      // Close to finish the stack
      stack.Close();
    }

    IUICollection undoTools = null;

    void _comboboxUndoStack_ItemsSourceReady(object sender, EventArgs e) {
      undoTools = _comboboxUndoStack.ItemsSource;
    }

    void _comboboxUndoStack_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // Undo Until Step
      int item = (int)_comboboxUndoStack.SelectedItem;
      // Get list of steps and Undo
      IUndoStack stack = EditorDocument.HtmlEditor.GetUndoManager("");
      List<IUndoObject> undos = ((BatchedUndoUnit)stack).GetUndoHistory();
      if (undos != null) {
        undos[item].Do();
      }
    }

    void MainForm_NextOperationAdded(object sender, UndoEventArgs e) {
      // Refresh Undo display
      IUndoStack stack = EditorDocument.HtmlEditor.GetUndoManager("");
      List<IUndoObject> undos = ((BatchedUndoUnit)stack).GetUndoHistory();
      if (undoTools != null) {
        undoTools.Clear();
        foreach (UndoObject undo in undos) {
          undoTools.Add(new GalleryItemPropertySet() { Label = undo.Description });
        }
      }
    }

    #endregion

    #region HELPER WINDOWS

    void _toggleControlToggleOutline_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (_toggleControlToggleOutline.BooleanValue) {
        m_outlineWindow = new OutlineWindow();
        m_outlineWindow.FormClosed += new FormClosedEventHandler((o, ea) => {
          this.m_outlineWindow = null;
          _toggleControlToggleOutline.BooleanValue = false;
        });
        m_outlineWindow.SetEditor(EditorDocument.HtmlEditor);
        m_outlineWindow.Show(dockPanel);
        m_outlineWindow.Refresh();
      } else {
        m_outlineWindow.Close();
        m_outlineWindow = null;
      }
    }

    #endregion HELPER WINDOWS

    #region HELP

    void _buttonHelpTableOptions_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.TableOptions.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpTableEditor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.TableEditor.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpSpeller_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Speller.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpHelpline_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Helpline.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpStyles_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Styles.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpText_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Text.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpControl_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Control.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpDisplay_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Display.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpElement_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Elements.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
    }

    void _buttonHelpEdit_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = true;
      WaitForReady(true);
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Edit.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
    }


    void _helpButton_ExecuteEvent(object sender, ExecuteEventArgs e) {
      ShowCommonHelp();
    }

    private void ShowCommonHelp() {
      using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.Common.htm")) {
        StreamReader sr = new StreamReader(s);
        EditorDocument.HtmlEditor.LoadHtml(sr.ReadToEnd());
        sr.Dispose();
      }
    }

    private void WaitForReady(bool wait) {
      if (wait) {
        while (!EditorDocument.HtmlEditor.IsReady) {
          Application.DoEvents();
        }
      }
    }

    #endregion HELP

    #region CONTROL EVENTS

    private EventWindow m_eventWindow = null;

    void _togglebuttonControlShowEvent_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (m_eventWindow == null) {
        m_eventWindow = new EventWindow();
        m_docWindow.RegisterEventWindow(m_eventWindow);
        m_eventWindow.FormClosed += (o, ea) => {
          m_eventWindow = null;
          _togglebuttonControlShowEvent.BooleanValue = false;
        };
        m_eventWindow.TopMost = true;
      }
      if (_togglebuttonControlShowEvent.BooleanValue) {
        m_eventWindow.Show(this);
      } else {
        m_eventWindow.Close();
      }
    }

    void elementEventDialog_FormClosed(object sender, FormClosedEventArgs e) {
      _togglebuttonControlShowEvent.BooleanValue = false;
      elementEventDialog = null;
    }

    ElementEventWindow elementEventDialog = null;

    void _togglebuttonControlAttachEvent_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (elementEventDialog == null) {
        elementEventDialog = new ElementEventWindow();
        elementEventDialog.FormClosed += new FormClosedEventHandler(elementEventDialog_FormClosed);
        elementEventDialog.TopMost = true;
      }
      if (_togglebuttonControlAttachEvent.BooleanValue) {
        // Attach Events
        elementEventDialog.Show();
        if (_checkboxControlElementNameA.BooleanValue) {
          foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("A")) {
            AttachDetachEvent(el);
          }
        }
        if (_checkboxControlElementNameIMG.BooleanValue) {
          foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("IMG")) {
            AttachDetachEvent(el);
          }
        }
        if (_checkboxControlElementNameDIV.BooleanValue) {
          foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("DIV")) {
            AttachDetachEvent(el);
          }
        }
        if (_checkboxControlElementNameBUTTON.BooleanValue) {
          foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("BUTTON")) {
            AttachDetachEvent(el);
          }
        }

      } else {
        // Detach Events
        if (elementEventDialog != null) {
          elementEventDialog.Close();
        }
        foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("A")) {
          AttachDetachEvent(el, true);
        }
        foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("IMG")) {
          AttachDetachEvent(el, true);
        }
        foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("DIV")) {
          AttachDetachEvent(el, true);
        }
        foreach (IElement el in EditorDocument.HtmlEditor.GetElementsByTagName("BUTTON")) {
          AttachDetachEvent(el, true);
        }
      }
    }

    private void AttachDetachEvent(IElement el) {
      AttachDetachEvent(el, false);
    }

    private void AttachDetachEvent(IElement el, bool detach) {

      if (_checkboxControlEventNameClick.BooleanValue && !detach)
        el.Click += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Control, "Click", o, e));
      else
        el.Click -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Control, "Click", o, e));

      if (_checkboxControlEventNameDblClick.BooleanValue && !detach)
        el.DblClick += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Control, "Double Click", o, e));
      else
        el.DblClick -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Control, "Double Click", o, e));

      if (_checkboxControlEventNameMouseDown.BooleanValue && !detach)
        el.MouseDown += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Down", o, e));
      else
        el.MouseDown -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Down", o, e));

      if (_checkboxControlEventNameMouseEnter.BooleanValue && !detach)
        el.MouseEnter += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Enter", o, e));
      else
        el.MouseEnter -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Enter", o, e));

      if (_checkboxControlEventNameMouseLeave.BooleanValue && !detach)
        el.MouseLeave += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Leave", o, e));
      else
        el.MouseLeave -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Leave", o, e));

      if (_checkboxControlEventNameMouseOut.BooleanValue && !detach)
        el.MouseOut += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Out", o, e));
      else
        el.MouseOut -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Out", o, e));

      if (_checkboxControlEventNameMouseUp.BooleanValue && !detach)
        el.MouseUp += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Up", o, e));
      else
        el.MouseUp -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Up", o, e));

      if (_checkboxControlEventNameMouseWheel.BooleanValue && !detach)
        el.MouseWheel += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Wheel", o, e));
      else
        el.MouseWheel -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Mouse, "Mouse Wheel", o, e));

      if (_checkboxControlEventNameBeforeCut.BooleanValue && !detach)
        el.BeforeCut += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Cut", o, e));
      else
        el.BeforeCut -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Cut", o, e));

      if (_checkboxControlEventNameCut.BooleanValue && !detach)
        el.Cut += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Cut", o, e));
      else
        el.Cut -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Cut", o, e));

      if (_checkboxControlEventNameBeforeCopy.BooleanValue && !detach)
        el.BeforeCopy += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Copy", o, e));
      else
        el.BeforeCopy -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Copy", o, e));

      if (_checkboxControlEventNameCopy.BooleanValue && !detach)
        el.Copy += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Copy", o, e));
      else
        el.Copy -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Copy", o, e));

      if (_checkboxControlEventNameBeforePaste.BooleanValue && !detach)
        el.BeforePaste += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Paste", o, e));
      else
        el.BeforePaste -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Before Paste", o, e));

      if (_checkboxControlEventNamePaste.BooleanValue && !detach)
        el.Paste += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Paste", o, e));
      else
        el.Paste -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Edit, "Paste", o, e));

      if (_checkboxControlEventNameResize.BooleanValue && !detach)
        el.Resize += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize", o, e));
      else
        el.Resize -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize", o, e));

      if (_checkboxControlEventNameResizeEnd.BooleanValue && !detach)
        el.ResizeEnd += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize End", o, e));
      else
        el.ResizeEnd -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize End", o, e));

      if (_checkboxControlEventNameResizeStart.BooleanValue && !detach)
        el.ResizeStart += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize Start", o, e));
      else
        el.ResizeStart -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Resize, "Resize Start", o, e));

      if (_checkboxControlEventNameMove.BooleanValue && !detach)
        el.Move += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move", o, e));
      else
        el.Move -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move", o, e));

      if (_checkboxControlEventNameMoveEnd.BooleanValue && !detach)
        el.MoveEnd += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move End", o, e));
      else
        el.MoveEnd -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move End", o, e));

      if (_checkboxControlEventNameMoveStart.BooleanValue && !detach)
        el.MoveStart += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move Start", o, e));
      else
        el.MoveStart -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Move, "Move Start", o, e));

      if (_checkboxControlEventNameDrag.BooleanValue && !detach)
        el.Drag += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag", o, e));
      else
        el.Drag -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag", o, e));

      if (_checkboxControlEventNameDragEnd.BooleanValue && !detach)
        el.DragEnd += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag End", o, e));
      else
        el.DragEnd -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag End", o, e));

      if (_checkboxControlEventNameDragEnter.BooleanValue && !detach)
        el.DragEnter += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Enter", o, e));
      else
        el.DragEnter -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Enter", o, e));

      if (_checkboxControlEventNameDragLeave.BooleanValue && !detach)
        el.DragLeave += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Leave", o, e));
      else
        el.DragLeave -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Leave", o, e));

      if (_checkboxControlEventNameDragStart.BooleanValue && !detach)
        el.DragStart += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Start", o, e));
      else
        el.DragStart -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Start", o, e));

      if (_checkboxControlEventNameDragOver.BooleanValue && !detach)
        el.DragOver += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Over", o, e));
      else
        el.DragOver -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drag Over", o, e));

      if (_checkboxControlEventNameDrop.BooleanValue && !detach)
        el.Drop += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drop", o, e));
      else
        el.Drop -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Drag, "Drop", o, e));

      if (_checkboxControlEventNameControlSelect.BooleanValue && !detach)
        el.ControlSelect += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Control Select", o, e));
      else
        el.ControlSelect -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Control Select", o, e));

      if (_checkboxControlEventNameSelect.BooleanValue && !detach)
        el.Select += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Select", o, e));
      else
        el.Select -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Select", o, e));

      if (_checkboxControlEventNameSelectStart.BooleanValue && !detach)
        el.SelectStart += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Select Start", o, e));
      else
        el.SelectStart -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Element, "Select Start", o, e));

      if (_checkboxControlEventNameChange.BooleanValue && !detach)
        el.Change += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Change, "Change", o, e));
      else
        el.Change -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Change, "Change", o, e));

      if (_checkboxControlEventNameSelectionChange.BooleanValue && !detach)
        el.SelectionChange += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Change, "Selection Change", o, e));
      else
        el.SelectionChange -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Change, "Selection Change", o, e));

      if (_checkboxControlEventNameFocus.BooleanValue && !detach)
        el.Focus += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus", o, e));
      else
        el.Focus -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus", o, e));

      if (_checkboxControlEventNameFocusin.BooleanValue && !detach)
        el.FocusIn += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus In", o, e));
      else
        el.FocusIn -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus In", o, e));

      if (_checkboxControlEventNameFocusout.BooleanValue && !detach)
        el.FocusOut += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus Out", o, e));
      else
        el.FocusOut -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Focus Out", o, e));

      if (_checkboxControlEventNameBlur.BooleanValue && !detach)
        el.Blur += new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Blur", o, e));
      else
        el.Blur -= new DocumentEventHandler((o, e) => elementEventDialog.RegisterEvent(EventGroup.Focus, "Blur", o, e));
    }

    #endregion

    #region TEXT HIGHLIGHT

    void _buttonGroupTextSelectAll_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SelectAll();
    }

    void _buttonGroupTextSelectParagraph_ExecuteEvent(object sender, ExecuteEventArgs e) {
      ITextSelector ts = EditorDocument.HtmlEditor.TextSelector;
      ts.ResetRangePointers();
      ts.MovePointersToCaret();
      ts.MoveFirstPointer(MoveTextPointer.PrevBlock);
      ts.MoveSecondPointer(MoveTextPointer.NextBlock);
      ts.SelectPointerRange(true);
    }

    void _buttonGroupTextSelectSentence_ExecuteEvent(object sender, ExecuteEventArgs e) {
      ITextSelector ts = EditorDocument.HtmlEditor.TextSelector;
      ts.ResetRangePointers();
      ts.MovePointersToCaret();
      ts.MoveFirstPointer(MoveTextPointer.PrevSentence);
      ts.MoveSecondPointer(MoveTextPointer.NextSentence);
      ts.MoveSecondPointer(MoveTextPointer.PrevClusterEnd);
      ts.SelectPointerRange(true);
    }

    void _buttonGroupTextSelectWord_ExecuteEvent(object sender, ExecuteEventArgs e) {
      ITextSelector ts = EditorDocument.HtmlEditor.TextSelector;
      ts.ResetRangePointers();
      ts.MovePointersToCaret();
      ts.MoveFirstPointer(MoveTextPointer.PrevWordBegin);
      ts.MoveSecondPointer(MoveTextPointer.PrevWordEnd);
      ts.MoveSecondPointer(MoveTextPointer.NextWordEnd);
      ts.SelectPointerRange(true);
    }

    void _buttonGroupTextShowSelection_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (EditorDocument.HtmlEditor.Selection.HasTextSelection) {
        MessageBox.Show(this,
            String.Format("Current selection in the document is:\n\n{0}",
            EditorDocument.HtmlEditor.Selection.Text),
            "Selection",
            MessageBoxButtons.OK);
      }
    }


    void _colorpickerTextHighLightTextColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      currentStyle.TextColor = new HighlightColor(_colorpickerTextHighLightTextColor.Color, ColorType.Color);
    }

    void _colorpickerTextHighLightBackColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      currentStyle.TextBackgroundColor = new HighlightColor(_colorpickerTextHighLightBackColor.Color, ColorType.Color);
    }

    void _colorpickerTextHighLightColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      currentStyle.LineColor = new HighlightColor(_colorpickerTextHighLightColor.Color, ColorType.Color);
    }

    void _checkboxTextHighLightPattern_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // reset all checkbox elements
      _checkboxTextHighLightPatternLineThroughSingle.BooleanValue = false;
      _checkboxTextHighLightPatternLineThroughDouble.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineDash.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineDotDash.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineDotDotDash.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineDotted.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineSingle.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineThick.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineThickDash.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineWave.BooleanValue = false;
      _checkboxTextHighLightPatternUnderLineWords.BooleanValue = false;
      // retrieve current and set style
      RibbonCommands c = (RibbonCommands)((RibbonCheckBox)sender).CommandID;
      switch (c) {
        case RibbonCommands.cmdTextHighLightPatternUnderLineWords:
          _checkboxTextHighLightPatternUnderLineWords.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Words;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineWave:
          _checkboxTextHighLightPatternUnderLineWave.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Wave;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineThickDash:
          _checkboxTextHighLightPatternUnderLineThickDash.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.ThickDash;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineThick:
          _checkboxTextHighLightPatternUnderLineThick.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Thick;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineSingle:
          _checkboxTextHighLightPatternUnderLineSingle.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Single;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineDotted:
          _checkboxTextHighLightPatternUnderLineDotted.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Dotted;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineDotDotDash:
          _checkboxTextHighLightPatternUnderLineDotDotDash.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.DotDotDash;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineDotDash:
          _checkboxTextHighLightPatternUnderLineDotDash.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.DotDash;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternUnderLineDash:
          _checkboxTextHighLightPatternUnderLineDash.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Dash;
          currentStyle.LineThroughStyle = LineThroughStyle.Undefined;
          break;
        case RibbonCommands.cmdTextHighLightPatternLineThroughSingle:
          _checkboxTextHighLightPatternLineThroughSingle.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Undefined;
          currentStyle.LineThroughStyle = LineThroughStyle.Single;
          break;
        case RibbonCommands.cmdTextHighLightPatternLineThroughDouble:
          _checkboxTextHighLightPatternLineThroughDouble.BooleanValue = true;
          currentStyle.UnderlineStyle = UnderlineStyle.Undefined;
          currentStyle.LineThroughStyle = LineThroughStyle.Double;
          break;
      }
    }

    private List<IHighLightSegment> textHighlightSegments;
    private IHighLightStyle currentStyle = new HighLightStyle();

    void _buttonTextHighLightRemove_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (textHighlightSegments != null && textHighlightSegments.Count > 0) {
        foreach (HighLightSegment s in textHighlightSegments) {
          s.RemoveSegment();
        }
      }

    }

    void _buttonTextHighLightInvoke_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // remove first, we can store one collection of pattern only
      _buttonTextHighLightRemove_ExecuteEvent(sender, e);
      // reset 
      textHighlightSegments = new List<IHighLightSegment>();
      // set properties
      string searchFor = _comboboxTextHighLightText.StringValue.Trim();
      if (String.IsNullOrEmpty(searchFor)) {
        MessageBox.Show(this, "You cannot invoke this method because there is no text to look for.", "Error Using Function", MessageBoxButtons.OK, MessageBoxIcon.Error);
      } else {
        // search and highlight
        EditorDocument.HtmlEditor.TextSelector.SetHighLightStyle(currentStyle);
        while (EditorDocument.HtmlEditor.TextSelector.FindTextBetweenPointers(searchFor, false, false, true)) {
          EditorDocument.HtmlEditor.TextSelector.HighLightRange();
          textHighlightSegments.Add(EditorDocument.HtmlEditor.TextSelector.LastSegment);
        }
      }
    }

    // TEXT MANIPULATION

    void _buttonTextManipulationUppercase_ExecuteEvent(object sender, ExecuteEventArgs e) {
      SetTextCase("title");
    }

    void _buttonTextManipulationAllUppercase_ExecuteEvent(object sender, ExecuteEventArgs e) {
      SetTextCase("upper");
    }

    void _buttonTextManipulationAllLowercase_ExecuteEvent(object sender, ExecuteEventArgs e) {
      SetTextCase("lower");
    }

    private void SetTextCase(string tocase) {
      ITextSelector ts = EditorDocument.HtmlEditor.TextSelector;
      ts.MovePointersToCaret();
      ts.MoveFirstPointer(MoveTextPointer.PrevWordBegin);
      ts.MoveSecondPointer(MoveTextPointer.PrevWordEnd);
      ts.MoveSecondPointer(MoveTextPointer.NextWordEnd);
      string word = ts.GetTextBetweenPointers();
      switch (tocase) {
        case "upper":
          word = word.ToUpper();
          break;
        case "lower":
          word = word.ToLower();
          break;
        case "title":
          word = (word.Length > 0) ? word[0].ToString().ToUpper() + word.Substring(1) : word;
          break;
      }
      ts.SetTextBetweenPointers(word);
    }


    #endregion TEXT HIGHLIGHT

    #region SEARCH

    private SearchReplace SearchReplaceDialog;

    void _buttonReplace_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (SearchReplaceDialog == null) {
        SearchReplaceDialog = new SearchReplace();
      }
      SearchReplaceDialog.UseReplace = true;
      HandleSearchAndReplace(true);
    }

    void _buttonSearch_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (SearchReplaceDialog == null) {
        SearchReplaceDialog = new SearchReplace();
      }
      SearchReplaceDialog.UseReplace = false;
      HandleSearchAndReplace(false);
    }

    private void HandleSearchAndReplace(bool replace) {
      SearchReplaceDialog.NextWord += new EventHandler(SearchReplaceDialog_NextWord);
      SearchReplaceDialog.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SearchReplaceDialog_FormClosed);
      SearchReplaceDialog.Show(this);
    }

    void SearchReplaceDialog_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e) {
      if (SearchReplaceDialog.DialogResult == System.Windows.Forms.DialogResult.OK) {
        // last search before close
        SearchReplaceDialog_NextWord(sender, EventArgs.Empty);
      }
      SearchReplaceDialog = null;
    }

    void SearchReplaceDialog_NextWord(object sender, EventArgs e) {
      if (SearchReplaceDialog.UseReplace) {
        EditorDocument.HtmlEditor.ReplaceNext(
            SearchReplaceDialog.SearchFor,
            SearchReplaceDialog.ReplaceWith,
            SearchReplaceDialog.CaseSensitive,
            SearchReplaceDialog.WholeWord,
            SearchReplaceDialog.SearchUp);
      } else {
        EditorDocument.HtmlEditor.Find(
            SearchReplaceDialog.SearchFor,
            SearchReplaceDialog.CaseSensitive,
            SearchReplaceDialog.WholeWord,
            SearchReplaceDialog.SearchUp);
      }
    }

    #endregion SEARCH

    #region SCROLLBAR

    void _buttonScrollbarRight_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Window.ScrollBy(1, 0);
    }

    void _buttonScrollbarDown_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Window.ScrollBy(0, -1);
    }

    void _buttonScrollbarUp_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Window.ScrollBy(0, 1);
    }

    void _buttonScrollbarLeft_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Window.ScrollBy(-1, 0);
    }

    void _colorpickerScrollbarColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.GetBodyElement().RuntimeStyle.cssText = "scrollbar-face-color:#" + ColorTranslator.ToHtml(_colorpickerScrollbarColor.Color);
    }

    void _toggleButtonScrollbarsEnabled_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.ScrollBarsEnabled = _toggleButtonScrollbarsEnabled.BooleanValue;
    }

    #endregion SCROLLBAR

    #region SPELLER


    void _comboboxSpellerHighlightUnderlineStyle_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection styleItems = _comboboxSpellerHighlightUnderlineStyle.ItemsSource;
      styleItems.Clear();
      foreach (string val in Enum.GetNames(typeof(GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle))) {
        styleItems.Add(new GalleryItemPropertySet() { Label = val });
      }
    }


    void _comboboxSpellerSuggestionEnum_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection suggestionItems = _comboboxSpellerSuggestionEnum.ItemsSource;
      suggestionItems.Clear();
      foreach (string val in Enum.GetNames(typeof(GuruComponents.Netrix.SpellChecker.NetSpell.SuggestionEnum))) {
        suggestionItems.Add(new GalleryItemPropertySet() { Label = val });
      }
    }

    void _dropdownSpellerDictionary_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection dictionaryItems = _dropdownSpellerDictionary.ItemsSource;
      dictionaryItems.Clear();
      for (int i = 0; i < imageListFlags.Images.Count; i++) {
        System.Drawing.Image img = imageListFlags.Images[i];
        string name = imageListFlags.Images.Keys[i];
        dictionaryItems.Add(new GalleryItemPropertySet() {
          ItemImage = ribbon1.ConvertToUIImage((Bitmap)img),
          Label = name // Ref to country names
        });
      }
    }

    void _buttonSpellerCommandClearBuffer_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.ClearBuffer);
    }

    void _buttonSpellerCommandRemoveHighlight_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.RemoveHighLight);
    }

    void _buttonSpellerCommandCheckWordStop_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StopWordByWord);
    }

    void _buttonSpellerCommandCheckWordStart_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StopBackground);
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StartWordByWord);
    }

    void _toggleSpellerCommandBackground_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (_toggleSpellerCommandBackground.BooleanValue) {
        _buttonSpellerCommandCheckWordStart.Enabled = false;
        _buttonSpellerCommandCheckWordStop.Enabled = false;
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StopWordByWord);
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.RemoveHighLight);
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.ClearBuffer);
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StartBackground);
      } else {
        _buttonSpellerCommandCheckWordStart.Enabled = true;
        _buttonSpellerCommandCheckWordStop.Enabled = true;
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StopBackground);
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.RemoveHighLight);
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.ClearBuffer);
      }
    }

    void _comboboxSpellerHighlightUnderlineStyle_ExecuteEvent(object sender, ExecuteEventArgs e) {
      GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle style = (GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle)Enum.Parse(typeof(GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle), _comboboxSpellerHighlightUnderlineStyle.StringValue);
      EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).HighLightStyle.UnderlineStyle = style;
    }

    void _colorpickerSpellerHighlightColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).HighLightStyle.LineColor.ColorValue = _colorpickerSpellerHighlightColor.Color;
    }

    void _comboboxSpellerSuggestionEnum_ExecuteEvent(object sender, ExecuteEventArgs e) {
      GuruComponents.Netrix.SpellChecker.NetSpell.SuggestionEnum suggestionMode = (GuruComponents.Netrix.SpellChecker.NetSpell.SuggestionEnum)Enum.Parse(typeof(GuruComponents.Netrix.SpellChecker.NetSpell.SuggestionEnum), _comboboxSpellerSuggestionEnum.StringValue);
      EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).SuggestionMode = suggestionMode;
    }

    void _checkboxSpellerignoreUpperCaseWords_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreUpperCaseWords = _checkboxSpellerignoreUpperCaseWords.BooleanValue;
    }

    void _spinnerSpellermaxSuggestionsCount_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).MaxSuggestionsCount = (int)_spinnerSpellermaxSuggestionsCount.DecimalValue;
    }

    void _checkboxSpellerIgnoreWordsWithDigits_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreWordsWithDigits = _checkboxSpellerIgnoreWordsWithDigits.BooleanValue;
    }

    void _checkboxSpellerIgnoreHtml_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreHtml = _checkboxSpellerIgnoreHtml.BooleanValue;
    }

    void _dropdownSpellerDictionary_ExecuteEvent(object sender, ExecuteEventArgs e) {
      object item;
      _dropdownSpellerDictionary.ItemsSource.GetItem(_dropdownSpellerDictionary.SelectedItem, out item);
      // assume we keep the default path
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).DictionaryPath = "Dictionary";
      // and get the dictionary name from drop down
      EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).Dictionary = String.Format("{0}.dic", ((GalleryItemPropertySet)item).Label);
      // stop current checking, remove all segments and restart immediately
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StopBackground);
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.RemoveHighLight);
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.ClearBuffer);
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.SpellChecker.Commands.StartBackground);
    }

    #endregion SPELLER

    #region TABLE

    void _buttonTableCellColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).CellSelectionColor = _colorpickerTableCellColor.Color;
    }

    void _buttonTableBorderColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).CellSelectionBorderColor = _colorpickerTableBorderColor.Color;
    }

    void _buttonTableSliderColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderLine.SliderLineColor = _colorpickerTableSliderColor.Color;
    }

    void _buttonTableAdvancedParameter_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).AdvancedParameters = _checkboxTableAdvancedParameter.BooleanValue;
    }

    void _buttonTableStaticBehavior_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).StaticBehavior = _checkboxTableStaticBehavior.BooleanValue;
    }

    void _checkboxTableWithCellSelection_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).WithCellSelection = _checkboxTableWithCellSelection.BooleanValue;
    }

    void _buttonTableProcessTabKey_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).ProcessTABKey = _checkboxTableProcessTabKey.BooleanValue;
    }

    void _buttonTableSliderAddMode_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderAddMode = _checkboxTableSliderAddMode.BooleanValue;
    }

    void _buttonTableSliderActive_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderActivated = _checkboxTableSliderActive.BooleanValue;
    }

    void _buttonSplitCellHorizontal_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.SplitHorizontal);
    }

    void _buttonSplitCellVertical_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.SplitVertical);
    }

    void _buttonMergeRight_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.MergeRight);
    }

    void _buttonMergeLeft_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.MergeLeft);
    }

    void _buttonMergeUp_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.MergeUp);
    }

    void _buttonMergeDown_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.MergeDown);
    }

    void _buttonMergeCells_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.MergeCells);
    }

    void _buttonInsertColumn_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (_checkboxTableInsertAfter.BooleanValue)
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.InsertTableColumnAfter);
      else
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.InsertTableColumn);
    }

    void _buttonInsertRow_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (_checkboxTableInsertAfter.BooleanValue)
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.InsertTableRowAfter);
      else
        EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.InsertTableRow);
    }

    void _buttonDeleteColumn_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.DeleteTableColumn);
    }

    void _buttonDeleteRow_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.InvokeCommand(EditorDocument.TableDesigner.Commands.DeleteTableRow);
    }

    private InsertTable tableDialog;

    void _buttonInsertTable_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // CALL Insert Dialog
      if (tableDialog == null) {
        tableDialog = new InsertTable();
      }
      if (tableDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
        // insert
        IElement newTable = EditorDocument.HtmlEditor.Document.InsertTable(tableDialog.Rows, tableDialog.Columns, "", (int)tableDialog.BorderWidth.Value);
        // and set more properties
        if (newTable != null && newTable is TableElement) {
          ((TableElement)newTable).cellPadding = tableDialog.CellPadding;
          ((TableElement)newTable).cellSpacing = tableDialog.CellSpacing;
          ((TableElement)newTable).width = Unit.Percentage(100);
        }
      }
    }

    #endregion TABLE

    #region HELPLINE


    void _toggleRuler_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // Cast because all UI helper features are not exposed by the interface
      ((HtmlEditor)EditorDocument.HtmlEditor).ShowHorizontalRuler = _toggleRuler.BooleanValue;
      ((HtmlEditor)EditorDocument.HtmlEditor).ShowVerticalRuler = _toggleRuler.BooleanValue;
    }

    void _comboboxHelpLineLineDash_A_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection dashPattern = _comboboxHelpLineLineDash_A.ItemsSource;
      dashPattern.Clear();
      foreach (string val in Enum.GetNames(typeof(System.Drawing.Drawing2D.DashStyle))) {
        dashPattern.Add(new GalleryItemPropertySet() { Label = val });
      }
    }

    void _togglebuttonHelpLine_B_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineB.LineVisible = _togglebuttonHelpLine_B.BooleanValue;
    }

    void _comboboxHelpLineLineDash_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.LineDash = (System.Drawing.Drawing2D.DashStyle)Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), _comboboxHelpLineLineDash_A.StringValue);
    }

    void _spinnerHelpLineSnapGridSize_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.SnapGrid = Convert.ToInt32(_spinnerHelpLineSnapGridSize_A.DecimalValue);
    }

    void _checkboxHelpLineSnapToGrid_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.SnapToGrid = _checkboxHelpLineSnapToGrid_A.BooleanValue;
    }

    void _checkboxHelpLineSnap_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.SnapElements = _checkboxHelpLineSnap_A.BooleanValue;
    }

    void _spinnerHelpLinePositionY_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.Y = Convert.ToInt32(_spinnerHelpLinePositionY_A.DecimalValue);
    }

    void _checkboxHelpLineY_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.LineYEnabled = _checkboxHelpLineY_A.BooleanValue;
    }

    void _spinnerHelpLinePositionX_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.X = Convert.ToInt32(_spinnerHelpLinePositionX_A.DecimalValue);
    }

    void _checkboxHelpLineX_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.LineXEnabled = _checkboxHelpLineX_A.BooleanValue;
    }

    void _spinnerHelpLineLineSize_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.LineWidth = Convert.ToInt32(_spinnerHelpLineLineSize_A.DecimalValue);
    }

    void _dropdownHelpLineColor_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      _dropdownHelpLineColor_A.ColorType = SwatchColorType.RGB;
      EditorDocument.HelpLineA.LineColor = _dropdownHelpLineColor_A.Color;
    }

    void _toggleHelpLine_A_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HelpLineA.LineVisible = _togglebuttonHelpLine_A.BooleanValue;
      // disable other controls that we don't need if the helpline is invisible
      _spinnerHelpLinePositionX_A.Enabled = _togglebuttonHelpLine_A.BooleanValue;
      _spinnerHelpLinePositionY_A.Enabled = _togglebuttonHelpLine_A.BooleanValue;
      _checkboxHelpLineX_A.Enabled = _togglebuttonHelpLine_A.BooleanValue;
      _checkboxHelpLineY_A.Enabled = _togglebuttonHelpLine_A.BooleanValue;
      _dropdownHelpLineColor_A.Enabled = _togglebuttonHelpLine_A.BooleanValue;
    }


    void m_docWindow_HelpLineAMoving(object sender, GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs e) {
      _spinnerHelpLinePositionX_A.Enabled = false;
      _spinnerHelpLinePositionY_A.Enabled = false;
      _spinnerHelpLinePositionX_A.DecimalValue = e.X;
      _spinnerHelpLinePositionY_A.DecimalValue = e.Y;
    }

    void m_docWindow_HelpLineAMoved(object sender, GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs e) {
      _spinnerHelpLinePositionX_A.DecimalValue = e.X;
      _spinnerHelpLinePositionY_A.DecimalValue = e.Y;
      _spinnerHelpLinePositionX_A.Enabled = true;
      _spinnerHelpLinePositionY_A.Enabled = true;
    }


    #endregion HELPLINE

    #region GRID

    void _comboboxGridStyle_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection gridStylePattern = _comboboxGridStyle.ItemsSource;
      gridStylePattern.Clear();
      foreach (string val in Enum.GetNames(typeof(GuruComponents.Netrix.WebEditing.Behaviors.GridType))) {
        gridStylePattern.Add(new GalleryItemPropertySet() { Label = val });
      }
    }

    void _spinnerGridSize_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // X + Y not separate at the moment
      EditorDocument.HtmlEditor.Grid.GridSize = new Size((int)_spinnerGridSize.DecimalValue, (int)_spinnerGridSize.DecimalValue);
    }

    void _buttonShowGrid_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Grid.GridVisible = _togglebuttonShowGrid.BooleanValue;
      _spinnerGridSize.Enabled = _togglebuttonShowGrid.BooleanValue;
      _comboboxGridStyle.Enabled = _togglebuttonShowGrid.BooleanValue;
      _dropdownGridColor.Enabled = _togglebuttonShowGrid.BooleanValue;
    }

    void _comboboxGridStyle_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Grid.GridVisualisation = (GuruComponents.Netrix.WebEditing.Behaviors.GridType)Enum.Parse(typeof(GuruComponents.Netrix.WebEditing.Behaviors.GridType), _comboboxGridStyle.StringValue);
    }

    void _dropdownGridColor_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Grid.GridColor = _dropdownGridColor.Color;
    }

    void _togglebuttonPositionMultipleSelection_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.MultipleSelectionEnabled = _togglebuttonPositionMultipleSelection.BooleanValue;
    }

    #endregion

    #region POSITION

    void _buttonPositionToBack_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Selection.SendToBack();
    }

    void _buttonPositionToFront_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Selection.BringToFront();
    }

    void _toggleButtonPositionAbsolute_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.AbsolutePositioningEnabled = _toggleButtonPositionAbsolute.BooleanValue;
      _buttonPositionToBack.Enabled = _toggleButtonPositionAbsolute.BooleanValue;
      _buttonPositionToFront.Enabled = _toggleButtonPositionAbsolute.BooleanValue;
      _togglebuttonPositionMultipleSelection.Enabled = _toggleButtonPositionAbsolute.BooleanValue;
    }

    #endregion

    #region Paragraph and Formatting

    void _buttonViewSource_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.ShowPane(_buttonViewSource.BooleanValue ? 1 : 0);
    }

    void _comboboxGlyphKind_ItemsSourceReady(object sender, EventArgs e) {
      IUICollection glyphItems = _comboboxGlyphKind.ItemsSource;
      glyphItems.Clear();
      foreach (string s in Enum.GetNames(typeof(GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind))) {
        glyphItems.Add(new GalleryItemPropertySet() {
          Label = s
        });
      }
    }

    void _comboboxGlyphKind_ExecuteEvent(object sender, ExecuteEventArgs e) {
      uint id = _comboboxGlyphKind.SelectedItem;
      object item;
      _comboboxGlyphKind.ItemsSource.GetItem(id, out item);
      if (item is GalleryItemPropertySet) {
        string kindLabel = ((GalleryItemPropertySet)item).Label;
        GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind kind = (GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind)Enum.Parse(typeof(GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind), kindLabel);
        EditorDocument.HtmlEditor.Glyphs.GlyphKind = kind;
      }
    }

    void _buttonGlyphVariantColored_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Glyphs.GlyphsVisible = true;
      if (EditorDocument.HtmlEditor.Glyphs.GlyphKind == GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind.None) {
        EditorDocument.HtmlEditor.Glyphs.GlyphKind = GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind.AllTags;
        uint count = 0;
        _comboboxGlyphKind.ItemsSource.GetCount(out count);
        _comboboxGlyphKind.SelectedItem = count - 1;
      }
      EditorDocument.HtmlEditor.Glyphs.GlyphVariant = GuruComponents.Netrix.WebEditing.Glyphs.GlyphVariant.Colored;
    }

    void _buttonGlyphVariantStandard_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Glyphs.GlyphsVisible = true;
      if (EditorDocument.HtmlEditor.Glyphs.GlyphKind == GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind.None) {
        EditorDocument.HtmlEditor.Glyphs.GlyphKind = GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind.AllTags;
        uint count = 0;
        _comboboxGlyphKind.ItemsSource.GetCount(out count);
        _comboboxGlyphKind.SelectedItem = count - 1;
      }
      EditorDocument.HtmlEditor.Glyphs.GlyphVariant = GuruComponents.Netrix.WebEditing.Glyphs.GlyphVariant.Standard;
    }

    void _buttonGlyphsVariant_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // TODO: Toggle glyphs
      EditorDocument.HtmlEditor.Glyphs.GlyphsVisible = !EditorDocument.HtmlEditor.Glyphs.GlyphsVisible;
      if (EditorDocument.HtmlEditor.Glyphs.GlyphsVisible) {
        EditorDocument.HtmlEditor.Glyphs.GlyphVariant = GuruComponents.Netrix.WebEditing.Glyphs.GlyphVariant.Standard;
        EditorDocument.HtmlEditor.Glyphs.GlyphKind = GuruComponents.Netrix.WebEditing.Glyphs.HtmlGlyphsKind.AllTags;
      }
    }

    void _buttonGlyphVariantNone_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.Glyphs.GlyphsVisible = false;
    }

    void _buttonAlignFull_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetAlignment(Alignment.Full);
    }

    void _buttonAlignCenter_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetAlignment(Alignment.Center);
    }

    void _buttonAlignRight_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetAlignment(Alignment.Right);
    }

    void _buttonAlignLeft_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetAlignment(Alignment.Left);
    }

    void _buttonOutdent_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.UnIndent();
    }

    void _buttonIndent_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.Indent();
    }

    void _buttonInsertOL_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetHtmlFormat(HtmlFormat.OrderedList);
    }

    void _buttonInsertUL_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.TextFormatting.SetHtmlFormat(HtmlFormat.UnorderedList);
    }

    #endregion

    #region FONT

    void _fontControl_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // Toggle Buttons need to be wrapped
      if (CanToggle(_fontControl.Bold, HtmlCommand.Bold)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleBold();
        return;
      }
      if (CanToggle(_fontControl.Italic, HtmlCommand.Italic)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleItalics();
        return;
      }
      if (CanToggle(_fontControl.Underline, HtmlCommand.Underline)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleUnderline();
        return;
      }
      if (CanToggle(_fontControl.Strikethrough, HtmlCommand.StrikeThrough)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleStrikethrough();
        return;
      }
      if (CanToggleSubScript(_fontControl.VerticalPositioning, HtmlCommand.SubScript)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleSubscript();
        return;
      }
      if (CanToggleSuperScript(_fontControl.VerticalPositioning, HtmlCommand.SuperScript)) {
        EditorDocument.HtmlEditor.TextFormatting.ToggleSuperscript();
        return;
      }
      // need ARGB to know that "Black" equals "#000000" in HTML, all because ColorTranslator.ToHtml does not know a thing
      var fc = _fontControl.ForegroundColor;
      var fcc = Color.FromArgb(0, fc);
      var hfc = EditorDocument.HtmlEditor.CssTextFormatting.ForeColor;
      var hfcc = Color.FromArgb(0, hfc);
      if (hfcc.ToArgb() != fcc.ToArgb()) {
        EditorDocument.HtmlEditor.CssTextFormatting.ForeColor = fcc;
        return;
      }
      var bc = _fontControl.BackgroundColor;
      var bcc = Color.FromArgb(0, fc);
      var hbc = EditorDocument.HtmlEditor.CssTextFormatting.BackColor;
      var hbcc = Color.FromArgb(0, hfc);
      if (hbcc.ToArgb() != bcc.ToArgb()) {
        EditorDocument.HtmlEditor.CssTextFormatting.BackColor = bcc;
        return;
      }
      if (EditorDocument.HtmlEditor.CssTextFormatting.FontName != _fontControl.Family) {
        EditorDocument.HtmlEditor.CssTextFormatting.FontName = _fontControl.Family;
        return;
      }
      Unit size = Unit.Point(Convert.ToInt32(_fontControl.Size));
      if (EditorDocument.HtmlEditor.CssTextFormatting.CssFontSize != size) {
        EditorDocument.HtmlEditor.CssTextFormatting.CssFontSize = size;
        return;
      }
    }

    bool CanToggleSubScript(RibbonLib.Interop.FontVerticalPosition prop, HtmlCommand check) {
      return (prop == RibbonLib.Interop.FontVerticalPosition.SubScript
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Checked)
              ||
             (_fontControl.Bold == RibbonLib.Interop.FontProperties.NotSet
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Enabled);
    }

    bool CanToggleSuperScript(RibbonLib.Interop.FontVerticalPosition prop, HtmlCommand check) {
      return (prop == RibbonLib.Interop.FontVerticalPosition.SuperScript
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Checked)
              ||
             (_fontControl.Bold == RibbonLib.Interop.FontProperties.NotSet
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Enabled);
    }

    bool CanToggle(RibbonLib.Interop.FontProperties prop, HtmlCommand check) {
      return (prop == RibbonLib.Interop.FontProperties.Set
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Checked)
              ||
              (_fontControl.Bold == RibbonLib.Interop.FontProperties.NotSet
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Enabled);
    }

    bool CanToggle(RibbonLib.Interop.FontUnderline prop, HtmlCommand check) {
      return (prop == RibbonLib.Interop.FontUnderline.Set
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Checked)
              ||
             (_fontControl.Bold == RibbonLib.Interop.FontProperties.NotSet
              &&
              EditorDocument.HtmlEditor.CommandStatus(check) != HtmlCommandInfo.Enabled);
    }

    #endregion FONT

    #region FILE

    private Properties PropertiesDialog = null;

    void _buttonProperties_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (PropertiesDialog == null) {
        PropertiesDialog = new Properties();
        PropertiesDialog.SetEditor(EditorDocument.HtmlEditor);
      }
      PropertiesDialog.ShowDialog(this);
    }

    void _buttonPrint_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.PrintWithPreview();
    }

    void _recentItems_ExecuteEvent(object sender, ExecuteEventArgs e) {
      //
    }

    void _buttonExit_ExecuteEvent(object sender, ExecuteEventArgs e) {
      // handle Save
      Close();
    }

    void _buttonSave_ExecuteEvent(object sender, ExecuteEventArgs e) {
      string file = m_docWindow.FileName;
      bool hasName = false;
      if (!String.IsNullOrEmpty(file)) {
        if (File.Exists(file)) {
          hasName = true;
          EditorDocument.HtmlEditor.SaveFile();
          return;
        }
      }
      if (!hasName) {
        if (saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
          file = saveFileDialog1.FileName;
          EditorDocument.HtmlEditor.SaveFile(file);
          // force Reloading
          m_docWindow.Text = Path.GetFileName(file);
          EditorDocument.FileName = file;
        }
      }
    }

    void _buttonOpen_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
        string file = openFileDialog1.FileName;
        if (File.Exists(file)) {
          EditorDocument.FileName = file;
          if (_recentItems.RecentItems == null) {
            _recentItems.RecentItems = new List<RecentItemsPropertySet>();
          }
          IList<RecentItemsPropertySet> items = _recentItems.RecentItems;
          var result = (from i in items
                        where i.LabelDescription == file
                        select i).Count();
          if (result == 0) {

            _recentItems.RecentItems.Add(new RecentItemsPropertySet() {
              Label = Path.GetFileName(file),
              LabelDescription = file,
              Pinned = false
            });
          }
        }
      }
    }

    void _buttonNew_ExecuteEvent(object sender, ExecuteEventArgs e) {
      if (m_docWindow != null) {
        m_docWindow.Close();
        m_docWindow = null;
      }
      EditorDocument.HtmlEditor.NewDocument();
      EditorDocument.FileName = String.Empty;
    }

    #endregion FILE

    #region COMMON EDIT

    void _toggleButtonDesignMode_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.HtmlEditor.DesignModeEnabled = _togglebuttonDesignMode.BooleanValue;
      _togglebuttonDesignMode.Enabled = false; // enable after IsReady occurs
    }

    void _buttonPaste_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.Paste();
    }

    void _buttonPasteImage_ExecuteEvent(object sender, ExecuteEventArgs e) {
      bool error = true;
      while (true) {
        string path = "";
        if (Clipboard.ContainsData(DataFormats.Bitmap)) {
          System.Drawing.Image image = Clipboard.GetImage();
          if (!String.IsNullOrEmpty(EditorDocument.HtmlEditor.TempFile)) {
            string file = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".jpg";
            path = Path.GetDirectoryName(EditorDocument.HtmlEditor.TempFile) + Path.DirectorySeparatorChar + file;
          } else {
            path = Path.GetTempFileName().Replace(".tmp", ".jpg");
          }
          try {
            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
          } catch (Exception ex) {
            MessageBox.Show("Cannot save image to temporary location.\n\nMessage: " + ex.Message, "Exception");
            break;
          }
          error = false;
        }
        if (Clipboard.ContainsData(DataFormats.FileDrop)) {
          path = ((string[])Clipboard.GetData(DataFormats.FileDrop))[0];
          // we take the original path here and leave the file untouched
          error = false;
        }
        ImageElement img = EditorDocument.HtmlEditor.Document.InsertImage() as ImageElement;
        img.src = path;
        EditorDocument.HtmlEditor.Selection.SelectElement(img);
        break;
      }
      if (error) {
        MessageBox.Show("Clipboard does not contain an image. Copy an image file to clipboard and invoke this command again.");
      }
    }

    void _buttonCut_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.Cut();
    }

    void _buttonCopy_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.Copy();
    }


    void _buttonRedo_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.Redo();
    }

    void _buttonUndo_ExecuteEvent(object sender, ExecuteEventArgs e) {
      EditorDocument.Undo();
    }

    #endregion COMMON EDIT

    #endregion

    #region FirstTime Loader

    private HtmlDoc EditorDocument
    {
      get
      {
        if (m_docWindow == null) {

          m_docWindow = new HtmlDoc();

          m_docWindow.HtmlEditorPaneActive += new EventHandler(m_docWindow_HtmlEditorPaneActive);
          m_docWindow.CodeEditorPaneActive += new EventHandler(m_docWindow_CodeEditorPaneActive);

          m_docWindow.FileName = "";
          m_docWindow.Text = "New Document";

          m_docWindow.DocumentReady += new EventHandler(m_docWindow_DocumentReady);
          m_docWindow.UIUpdated += new EventHandler(m_docWindow_UIUpdated);

          m_docWindow.HelpLineAMoved += new EventHandler<GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs>(m_docWindow_HelpLineAMoved);
          m_docWindow.HelpLineAMoving += new EventHandler<GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs>(m_docWindow_HelpLineAMoving);

          m_docWindow.TableActive += new EventHandler<GuruComponents.Netrix.TableDesigner.TableEventArgs>(m_docWindow_TableActive);
          m_docWindow.TableInActive += new EventHandler<GuruComponents.Netrix.TableDesigner.TableEventArgs>(m_docWindow_TableInActive);

          m_docWindow.Show(dockPanel, DockState.Document);
          m_docWindow.FormClosed += new System.Windows.Forms.FormClosedEventHandler(m_docWindow_FormClosed);
          // Let the toolbox have access to the editor
          m_toolbox.SetEditorReference(m_docWindow.HtmlEditor);
          // Register custom moniker
          InternetSessionRegistry.Register("dotnet", new DotNetResourceHandlerFactory(m_docWindow.HtmlEditor));
        }

        return m_docWindow;
      }
    }

    void m_docWindow_CodeEditorPaneActive(object sender, EventArgs e) {
      m_propertyWindow.SetObject(sender);
    }

    void m_docWindow_HtmlEditorPaneActive(object sender, EventArgs e) {
      m_propertyWindow.SetObject(EditorDocument.HtmlEditor.GetBodyElement());
    }

    void m_docWindow_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e) {
      m_docWindow.Dispose();
      m_docWindow = null;
    }

    void Window_Scroll(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e) {
      _spinnerScrollbarX.DecimalValue = e.ScreenXY.X;
      _spinnerScrollbarY.DecimalValue = e.ScreenXY.Y;
    }

    void m_docWindow_TableInActive(object sender, GuruComponents.Netrix.TableDesigner.TableEventArgs e) {
      _tabgroupTableTools.ContextAvailable = ContextAvailability.NotAvailable;
    }

    void m_docWindow_TableActive(object sender, GuruComponents.Netrix.TableDesigner.TableEventArgs e) {
      _tabgroupTableTools.ContextAvailable = ContextAvailability.Available;
    }

    // Here we handle immediate updates after key or mouse operations by user 
    void m_docWindow_UIUpdated(object sender, EventArgs e) {
      #region FILE
      _buttonSave.Enabled = !String.IsNullOrEmpty(m_docWindow.FileName);
      #endregion
      #region PARA
      _buttonAlignCenter.Enabled = CheckState(HtmlCommand.JustifyCenter) != FontProperties.NotAvailable;
      _buttonAlignLeft.Enabled = CheckState(HtmlCommand.JustifyLeft) != FontProperties.NotAvailable;
      _buttonAlignRight.Enabled = CheckState(HtmlCommand.JustifyRight) != FontProperties.NotAvailable;
      _buttonAlignFull.Enabled = CheckState(HtmlCommand.JustifyFull) != FontProperties.NotAvailable;
      #endregion
      #region FONT
      _fontControl.Italic = CheckState(HtmlCommand.Italic);
      _fontControl.Strikethrough = CheckState(HtmlCommand.StrikeThrough);
      _fontControl.Underline = CheckStateUnderline(HtmlCommand.Underline);
      _fontControl.Bold = CheckState(HtmlCommand.Bold);
      _fontControl.Family = EditorDocument.HtmlEditor.TextFormatting.FontName;
      _fontControl.ForegroundColor = EditorDocument.HtmlEditor.TextFormatting.ForeColor;
      _fontControl.BackgroundColor = EditorDocument.HtmlEditor.TextFormatting.BackColor;
      #endregion
      #region EDIT
      _buttonPaste.Enabled = EditorDocument.HtmlEditor.CanPaste;
      _buttonCut.Enabled = EditorDocument.HtmlEditor.CanCut;
      _buttonCopy.Enabled = EditorDocument.HtmlEditor.CanCopy;
      #endregion
      #region POSITION
      _buttonPositionToBack.Enabled = EditorDocument.HtmlEditor.Selection.CanChangeZIndex;
      _buttonPositionToFront.Enabled = EditorDocument.HtmlEditor.Selection.CanChangeZIndex;
      #endregion POSITION
      #region TABLE
      // check only if visible
      if (_tabgroupTableTools.ContextAvailable == ContextAvailability.Active) {
        _buttonMergeCells.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanMergeCells;
        _buttonMergeUp.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanMergeUp;
        _buttonMergeDown.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanMergeDown;
        _buttonMergeRight.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanMergeRight;
        _buttonMergeLeft.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanMergeLeft;
        _buttonInsertColumn.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanInsertTableColumn;
        _buttonInsertRow.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanInsertTableRow;
        _buttonDeleteColumn.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanDeleteTableColumn;
        _buttonDeleteRow.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanDeleteTableRow;
        _buttonSplitCellHorizontal.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanSplitHorizontal;
        _buttonSplitCellVertical.Enabled = EditorDocument.TableDesigner.GetFormatter(EditorDocument.HtmlEditor).CanSplitVertical;
      }
      #endregion
    }

    RibbonLib.Interop.FontUnderline CheckStateUnderline(HtmlCommand cmd) {
      if (EditorDocument.HtmlEditor.CommandStatus(cmd) == HtmlCommandInfo.Checked
         ||
         EditorDocument.HtmlEditor.CommandStatus(cmd) == HtmlCommandInfo.Both) {
        return RibbonLib.Interop.FontUnderline.Set;
      } else {
        _fontControl.Bold = RibbonLib.Interop.FontProperties.NotSet;
      }
      if (EditorDocument.HtmlEditor.CommandStatus(cmd) != HtmlCommandInfo.Enabled
          &&
          EditorDocument.HtmlEditor.CommandStatus(cmd) != HtmlCommandInfo.Both) {
        return RibbonLib.Interop.FontUnderline.NotAvailable;
      }
      return RibbonLib.Interop.FontUnderline.NotSet;
    }

    RibbonLib.Interop.FontProperties CheckState(HtmlCommand cmd) {
      if (EditorDocument.HtmlEditor.CommandStatus(cmd) == HtmlCommandInfo.Checked
          ||
          EditorDocument.HtmlEditor.CommandStatus(cmd) == HtmlCommandInfo.Both) {
        return RibbonLib.Interop.FontProperties.Set;
      } else {
        _fontControl.Bold = RibbonLib.Interop.FontProperties.NotSet;
      }
      if (EditorDocument.HtmlEditor.CommandStatus(cmd) != HtmlCommandInfo.Enabled
          &&
          EditorDocument.HtmlEditor.CommandStatus(cmd) != HtmlCommandInfo.Both) {
        return RibbonLib.Interop.FontProperties.NotAvailable;
      }
      return RibbonLib.Interop.FontProperties.NotSet;
    }

    void m_docWindow_DocumentReady(object sender, EventArgs e) {
      m_propertyWindow.SetObject(EditorDocument.HtmlEditor.GetBodyElement());

      EditorDocument.HtmlEditor.HtmlElementChanged += new GuruComponents.Netrix.Events.HtmlElementChangedHandler(editorInstanceReference_HtmlElementChanged);
      EditorDocument.HtmlEditor.Selection.SelectionChanged += new GuruComponents.Netrix.Events.SelectionChangedEventHandler(Selection_SelectionChanged);
      EditorDocument.HtmlEditor.DocumentStructure.SelectionChange += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_SelectionChange);
      EditorDocument.HtmlEditor.Window.Scroll += new GuruComponents.Netrix.Events.DocumentEventHandler(Window_Scroll);

      EditorDocument.HtmlEditor.NextOperationAdded += new EventHandler<UndoEventArgs>(MainForm_NextOperationAdded);

      RefreshRibbonUI();
      // Refresh other windows
      if (_toggleControlToggleOutline.BooleanValue && m_outlineWindow != null) {
        m_outlineWindow.Refresh();
      }
      _togglebuttonDesignMode.Enabled = true;
    }

    // here we put the ribbon into default state after loading a document
    private void RefreshRibbonUI() {
      _togglebuttonDesignMode.BooleanValue = EditorDocument.HtmlEditor.DesignModeEnabled;

      #region GRID
      _togglebuttonShowGrid.BooleanValue = EditorDocument.HtmlEditor.Grid.GridVisible;
      _spinnerGridSize.Enabled = _togglebuttonShowGrid.BooleanValue;
      _spinnerGridSize.DecimalPlaces = 0;
      _spinnerGridSize.DecimalValue = EditorDocument.HtmlEditor.Grid.GridSize.Width;
      _dropdownGridColor.ColorType = SwatchColorType.RGB;
      _dropdownGridColor.Color = EditorDocument.HtmlEditor.Grid.GridColor;
      _dropdownGridColor.Enabled = _togglebuttonShowGrid.BooleanValue;
      _comboboxGridStyle.Enabled = _togglebuttonShowGrid.BooleanValue;
      #endregion GRID
      #region SCROLLBAR
      _toggleButtonScrollbarsEnabled.BooleanValue = EditorDocument.HtmlEditor.ScrollBarsEnabled;
      _spinnerScrollbarX.DecimalValue = EditorDocument.HtmlEditor.HorizontalScrollPosition;
      _spinnerScrollbarY.DecimalValue = EditorDocument.HtmlEditor.VerticalScrollPosition;
      #endregion
      #region HELPLINE
      _togglebuttonHelpLine_B.BooleanValue = EditorDocument.HelpLineB.LineVisible;
      _togglebuttonHelpLine_A.BooleanValue = EditorDocument.HelpLineA.LineVisible;
      _spinnerHelpLineSnapGridSize_A.DecimalValue = EditorDocument.HelpLineA.SnapGrid;
      _spinnerHelpLineSnapGridSize_A.DecimalPlaces = 0;
      _checkboxHelpLineSnapToGrid_A.BooleanValue = EditorDocument.HelpLineA.SnapToGrid;
      _checkboxHelpLineSnap_A.BooleanValue = EditorDocument.HelpLineA.SnapElements;
      _spinnerHelpLinePositionY_A.DecimalValue = EditorDocument.HelpLineA.Y;
      _spinnerHelpLinePositionY_A.DecimalPlaces = 0;
      _checkboxHelpLineY_A.BooleanValue = EditorDocument.HelpLineA.LineYEnabled;
      _spinnerHelpLinePositionX_A.DecimalValue = EditorDocument.HelpLineA.X;
      _spinnerHelpLinePositionX_A.DecimalPlaces = 0;
      _checkboxHelpLineX_A.BooleanValue = EditorDocument.HelpLineA.LineXEnabled;
      _spinnerHelpLineLineSize_A.DecimalValue = EditorDocument.HelpLineA.LineWidth;
      _spinnerHelpLineLineSize_A.DecimalPlaces = 0;
      _dropdownHelpLineColor_A.ColorType = SwatchColorType.RGB;
      _dropdownHelpLineColor_A.Color = EditorDocument.HelpLineA.LineColor;
      #endregion HELPLINE
      #region POSITION
      _toggleButtonPositionAbsolute.BooleanValue = EditorDocument.HtmlEditor.AbsolutePositioningEnabled;
      #endregion
      #region TABLE
      _colorpickerTableCellColor.ColorType = SwatchColorType.RGB;
      _colorpickerTableCellColor.Color = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).CellSelectionColor;
      _colorpickerTableBorderColor.ColorType = SwatchColorType.RGB;
      _colorpickerTableBorderColor.Color = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).CellSelectionBorderColor;
      _colorpickerTableSliderColor.ColorType = SwatchColorType.RGB;
      _colorpickerTableSliderColor.Color = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderLine.SliderLineColor;
      _checkboxTableAdvancedParameter.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).AdvancedParameters;
      _checkboxTableStaticBehavior.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).StaticBehavior;
      _checkboxTableWithCellSelection.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).WithCellSelection;
      _checkboxTableProcessTabKey.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).ProcessTABKey;
      _checkboxTableSliderAddMode.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderAddMode;
      _checkboxTableSliderActive.BooleanValue = EditorDocument.TableDesigner.GetTableDesigner(EditorDocument.HtmlEditor).SliderActivated;
      #endregion TABLE
      #region SPELLER
      _comboboxSpellerHighlightUnderlineStyle.SelectedItem = (uint)EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).HighLightStyle.UnderlineStyle;
      _comboboxSpellerHighlightUnderlineStyle.RepresentativeString = "XXXXXXXXXXXX";
      _colorpickerSpellerHighlightColor.ColorType = SwatchColorType.RGB;
      _colorpickerSpellerHighlightColor.Color = EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).HighLightStyle.LineColor.ColorValue;
      _comboboxSpellerSuggestionEnum.SelectedItem = (uint)EditorDocument.SpellChecker.GetSpellChecker(EditorDocument.HtmlEditor).SuggestionMode;
      _comboboxSpellerSuggestionEnum.RepresentativeString = "XXXXXXXXXXXXXXX";
      _checkboxSpellerignoreUpperCaseWords.BooleanValue = EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreUpperCaseWords;
      _spinnerSpellermaxSuggestionsCount.DecimalPlaces = 0;
      _spinnerSpellermaxSuggestionsCount.DecimalValue = EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).MaxSuggestionsCount;
      _checkboxSpellerIgnoreWordsWithDigits.BooleanValue = EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreWordsWithDigits;
      _checkboxSpellerIgnoreHtml.BooleanValue = EditorDocument.SpellChecker.GetSpeller(EditorDocument.HtmlEditor).IgnoreHtml;
      #endregion
      #region TEXT HIGHLIGHT
      _comboboxTextHighLightText.RepresentativeString = "AAAAAAAAAAAAAAAAAAAAAAAAA";
      _colorpickerTextHighLightColor.ColorType = SwatchColorType.RGB;
      #endregion
    }

    void DocumentStructure_SelectionChange(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e) {
      // this event might fire even if the document is not ready
      if (EditorDocument.HtmlEditor.IsReady) {
        m_docWindow_UIUpdated(sender, EventArgs.Empty);
      }
    }

    void Selection_SelectionChanged(object sender, GuruComponents.Netrix.Events.SelectionChangedEventArgs e) {
      m_propertyWindow.SetObject(e.SelectedElement);
    }

    void editorInstanceReference_HtmlElementChanged(object sender, GuruComponents.Netrix.Events.HtmlElementChangedEventArgs e) {
      m_propertyWindow.SetObject(e.CurrentElement);
    }

    #endregion
  }
}
