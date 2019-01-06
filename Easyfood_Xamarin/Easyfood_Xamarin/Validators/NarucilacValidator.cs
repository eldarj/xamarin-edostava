using Easyfood_Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Easyfood_Xamarin.Validators
{
    public class NarucilacValidator
    {
        private List<ValidatorField> _fields = new List<ValidatorField>(); // onemogući setovanje izvan ove klase..
        public List<ValidatorField> ValidatedFields { get { return _fields; } }

        private bool _validityStatus { get; set; } // onemogući setovanje izvan ove klase..
        public bool IsValid { get { return _validityStatus; } }

        public NarucilacValidator(string ime = ValidatorRules.SKIPEVAL, 
            string prezime = ValidatorRules.SKIPEVAL,
            string adresa = ValidatorRules.SKIPEVAL,
            string username = ValidatorRules.SKIPEVAL,
            string email = ValidatorRules.SKIPEVAL,
            string telefon = ValidatorRules.SKIPEVAL,
            string password = ValidatorRules.SKIPEVAL,
            string passwordTrenutni = ValidatorRules.SKIPEVAL,
            string passwordNovi = ValidatorRules.SKIPEVAL,
            string passwordNoviPonovo = ValidatorRules.SKIPEVAL)
        {

            // Za svaku od proslijeđenih vrijednosti, želimo je provjerit protiv ispod navedenog regex-a,
            // osim ako određeni parametar nije proslijeđen (tj. ako je default vrijednost = SKIPEVAL)
            //      ::: PropName -> naziv errora/labele koji koristimo za prikaz na frontendu
            //      ::: ErrorMsg -> error poruka za popuniti labelu na frontendu
            //      ::: IsValid -> govori nam da li je vrijednost tačna (ako jeste sakrivaćemo već prikazane error-e)
            //          --- ime != null --- je tu da bismo provjerili taj parametar, a on je null jer je npr. polje forme zapravo prazno
            //          --- SKIPEVAL --- (Default-na vr.) je tu u slučaju da odr. parametar ne želimo provjeravat (param koji nije proslijeđen u ovaj konstruktor)

            _fields.Add(new ValidatorField {
                PropName = "ImeError", 
                ErrorMsg = String.Format(ValidatorRules.MSG_ALPHA_REGULAR, "ime"),
                IsValid = (ime != null && Regex.IsMatch(ime, ValidatorRules.RULE_ALPHA_REGULAR)) || ime == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PrezimeError",
                ErrorMsg = String.Format(ValidatorRules.MSG_ALPHA_REGULAR, "prezime"),
                IsValid = (prezime != null && Regex.IsMatch(prezime, ValidatorRules.RULE_ALPHA_REGULAR)) || prezime == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "AdresaError",
                ErrorMsg = String.Format(ValidatorRules.MSG_ADRESA, "adresa korisnika"),
                IsValid = (adresa != null && Regex.IsMatch(adresa, ValidatorRules.RULE_ADRESA)) || adresa == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "TelefonError",
                ErrorMsg = String.Format(ValidatorRules.MSG_TELEFON),
                IsValid = (telefon != null && Regex.IsMatch(telefon, ValidatorRules.RULE_TELEFON)) || telefon == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "EmailError",
                ErrorMsg = String.Format(ValidatorRules.MSG_EMAIL),
                IsValid = (email != null && Regex.IsMatch(email, ValidatorRules.RULE_EMAIL)) || email == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "UsernameError",
                ErrorMsg = String.Format(ValidatorRules.MSG_ALPHANUM_REGULAR, "username"),
                IsValid = (username != null && Regex.IsMatch(username, ValidatorRules.RULE_ALPHANUM_REGULAR)) || username == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PasswordError",
                ErrorMsg = ValidatorRules.MSG_PASSWORD,
                IsValid = (password != null && Regex.IsMatch(password, ValidatorRules.RULE_PASSWORD)) || password == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PasswordTrenutniError",
                ErrorMsg = ValidatorRules.MSG_PASSWORD,
                IsValid = (passwordTrenutni != null && Regex.IsMatch(passwordTrenutni, ValidatorRules.RULE_PASSWORD)) || passwordTrenutni == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PasswordNoviError",
                ErrorMsg = ValidatorRules.MSG_PASSWORD,
                IsValid = (passwordNovi != null && Regex.IsMatch(passwordNovi, ValidatorRules.RULE_PASSWORD)) || passwordNovi == ValidatorRules.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PasswordNoviPonovoError",
                ErrorMsg = ValidatorRules.MSG_PASSWORD,
                IsValid = (passwordNoviPonovo != null && Regex.IsMatch(passwordNoviPonovo, ValidatorRules.RULE_PASSWORD)) || passwordNoviPonovo == ValidatorRules.SKIPEVAL
            });

            // na kraju postavi status cijele validacije na false samo ako imamo barem jedno polje false..
            _validityStatus = _fields.Where(f => !f.IsValid).ToList().Count > 0 ? false : true;
        }
    }
}
