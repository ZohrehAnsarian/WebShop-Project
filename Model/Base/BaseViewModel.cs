using Model.ApplicationDomainModels;
using Model.ViewModels.Product;

using System;
using System.Collections.Generic;

namespace Model.Base
{
    public class BaseViewModel
    {
        public int DefaultCountryId { get; set; }
        public string GoogleRecaptchaResponse { get; set; }

        private MessageHandler _MessageHandler;
        public MessageHandler ActionMessageHandler
        {
            get
            {
                if (_MessageHandler == null)
                {
                    _MessageHandler = new MessageHandler();
                }

                return _MessageHandler;
            }
        }
        public string FormTitle { get; set; }
        public string CssClass { get; set; }

        public int CurrentLanguageId { get; set; }
        public LayoutViewModel Layout { get; set; }
        public Dictionary<string, string> LanguageDictionary { get; set; }
        public string JsonLanguageDictionary { get; set; }
        public string CurrentCultureName { get; set; }
        public string CurrentUserId { get; set; }
        public bool AllowAcceptReject { get; set; }
        public IEnumerable<string> CurrentUserRoles { get; set; }

        public string activeLanguageCommaSepatated;
        public bool MostSetWelcomeMessage = false;
        public string WelcomeMessage = "خوش آمدید ";
        public string[] WelcomeMessageList =
            {
                "Hey ",
                "Good to see you here ",
                "It’s nice to have you here ",
                "How do you do ",
                "You make us SPECIAL just by being her ",
                "Smile, it's a wonderful start ",
                "It is a beautiful day, enjoy it ",
                "Let's just enjoy the moment ",
                "We hope you enjoy it here ",
                "We are happy to serve you "
            };
        public bool ReadOnly { get; set; }
        public bool HasError { get; set; }
        public string CurrentUserName { get; set; }
        public string ClientMethod { get; set; }

        public BaseViewModel()
        {
        }

        public string this[string refrenceWord]
        {
            get
            {
                try
                {
                    if (LanguageDictionary == null)
                    {

                        return refrenceWord;
                    }

                    if (LanguageDictionary.ContainsKey(refrenceWord) == false)
                    {

                         

                    }
                    return LanguageDictionary[refrenceWord];
                }
                catch (Exception ex)
                {
                    return refrenceWord;
                }
            }
        }
    }
}