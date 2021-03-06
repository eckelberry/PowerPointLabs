namespace PowerPointLabs.TextCollection
{
    internal static class ELearningLabText
    {
        public const string RibbonMenuId = "eLearningLabMenu";
        public const string RibbonMenuLabel = "e-Learning";
        public const string RibbonMenuSupertip =
            "Use eLearning Lab to create audio, callouts, captions and tooltips all in one go!";

        public const string ELearningTaskPaneTag = "E-Learning Workspace";
        public const string ELearningTaskPaneSuperTip =
            "This is the work space for creating e-learning slides.";
        public const string ELearningTaskPaneLabel = "e-Learning Lab";

        public const string ELearningLabSettingsTag = "ELearningLabSettings";

        public const string Identifier = "PPTL";
        public const string Underscore = "_";
        public const string CalloutIdentifier = "Callout";
        public const string CaptionIdentifier = "Caption";
        public const string AudioIdentifier = "Audio";
        public const string DefaultAudioIdentifier = "Default";
        public const string TextStorageIdentifier = "Storage";
        public const string SelfExplanationTextIdentifier = "SelfExplanationText";
        public const string CalloutTextIdentifier = "CalloutText"; 
        public const string CaptionTextIdentifier = "CaptionText";
        public const string TagNoIdentifier = "TagNo";
        public const string ClickNumIdentifier = "ClickNum";
        public const string VoiceLabel = "VoiceLabel";
        public const string TriggerOnClick = "TriggerOnClick";
        public const string SelfExplanationItemIdentifier = "Item";

        public const string NoSlideSelectedMessage = "No slide is selected";
        public const string OnLoadingMessage = "Now Loading...";

        public const string ExplanationItem_IsCallout = "IsCallout";
        public const string ExplanationItem_IsCaption = "IsCaption";
        public const string ExplanationItem_IsVoice = "IsVoice";
        public const string ExplanationItem_HasShortVersion = "HasShortVersion";
        public const string ExplanationItem_CalloutText = "CalloutText";
        public const string ExplanationItem_CaptionText = "CaptionText";
        public const string ExplanationItem_VoiceLabel = "VoiceLabel";
        public const string ExplanationItem_TriggerIndex = "TriggerIndex";
        public const string ExplanationItem_IsDummyItem = "IsDummyItem";
        public const string ExplanationItem_IsTriggerTypeComboBoxEnabled = "IsTriggerTypeComboBoxEnabled";
        public const string ExplanationItem_IsVoiceLabelInvalid = "IsVoiceLabelInvalid";

        public const string ELearningLabTextStorageShapeName = Identifier + Underscore + TextStorageIdentifier;

        public const string AudioDefaultLabelFormat = "{0}" + Underscore + DefaultAudioIdentifier;
        public const string AudioFileNameFormat = "Slide {0} ClickNo {1} Speech.wav";
        public const string AudioPreviewFileNameFormat = "PPTL_preview_{0}.wav";
        public const string WatsonAudioTestFileName = "PPTL_Test_Watson.wav";
        public const string SelfExplanationItemFormat = "Item" + Underscore + "{0}";
        public const string CaptionShapeNameFormat = Identifier + Underscore + "{0}" + Underscore + CaptionIdentifier;
        public const string CalloutShapeNameFormat = Identifier + Underscore + "{0}" + Underscore + CalloutIdentifier;
        public const string AudioCustomShapeNameFormat = Identifier + Underscore + "{0}" + Underscore + AudioIdentifier + Underscore + "{1}";
        public const string AudioDefaultShapeNameFormat = Identifier + Underscore + "{0}" + 
            Underscore + AudioIdentifier + Underscore + "{1}" + Underscore + DefaultAudioIdentifier;
        public const string ProgressStatusLabelFormat = "Progress: {0}%";
        public const string TempFolderNameFormat = @"\PowerPointLabs Temp\" + "{0}" + @"\";

        public const string ExtractTagNoRegex = Identifier + Underscore + @"([1-9][0-9]*)" + 
            Underscore + "(" + CalloutIdentifier + "|" + CaptionIdentifier + "|" + AudioIdentifier + @").*";
        public const string ExtractFunctionRegex = Identifier + Underscore + @"[1-9][0-9]*" + Underscore
               + "(" + CalloutIdentifier + "|" + CaptionIdentifier + "|" + AudioIdentifier + ")" + @".*";
        public const string ExtractVoiceNameRegex = Identifier + Underscore + @"[1-9][0-9]*" + Underscore
              + CalloutIdentifier + "|" + CaptionIdentifier + "|" + AudioIdentifier + Underscore + @"(.*)";
        public const string VoiceShapeNameRegex = Identifier + Underscore + @"[1-9][0-9]*" + Underscore
            + AudioIdentifier + Underscore + @".*";
        public const string CalloutShapeNameRegex = Identifier + Underscore + @"[1-9][0-9]*" + Underscore
            + CalloutIdentifier;
        public const string CaptionShapeNameRegex = Identifier + Underscore + @"[1-9][0-9]*" + Underscore 
            + CaptionIdentifier;
        public const string PPTLShapeNameRegex = Identifier + Underscore + @"[1-9][0-9]*" +
            Underscore + "(" + CalloutIdentifier + "|" + CaptionIdentifier + "|" + AudioIdentifier + @").*";
        public const string PromptToSyncMessage = "ELearningLab detected that you have unsynced items in your workspace.\n" +
                    "Do you want to sync them now?";
    }
}
