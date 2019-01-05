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
        private List<ValidatorField> _fields = new List<ValidatorField>();
        public List<ValidatorField> ValidatedFields { get { return _fields; } }

        private bool _validityStatus { get; set; }
        public bool IsValid { get { return _validityStatus; } }

        public NarucilacValidator(string ime = ValidationHelper.SKIPEVAL, 
            string prezime = ValidationHelper.SKIPEVAL,
            string adresa = ValidationHelper.SKIPEVAL,
            string username = ValidationHelper.SKIPEVAL,
            string password = ValidationHelper.SKIPEVAL)
        {

            // Za svaku od proslijeđenih vrijednosti, želimo je provjerit protiv ispod navedenog regex-a,
            // osim ako određeni parametar nije proslijeđen (tj. ako je default vrijednost = SKIPEVAL)
            //      ::: PropName -> naziv label-a/error-a koji ćemo koristiti za prikaz na frontendu
            //      ::: ErrorMsg -> error poruka za popuniti label na frontendu
            //      ::: IsValid -> govori nam da li je vrijednost tačna (ako jeste tačna sakrivaćemo već prikazane error-e)
            //          --- ime != null --- je tu da bismo provjerili taj parametar, a on je null jer je npr. polje forme zapravo prazno
            //          --- SKIPEVAL --- (Default-na vr.) je tu u slučaju da odr. parametar ne želimo provjeravat (nije proslijeđen u ovaj konstruktor)

            _fields.Add(new ValidatorField {
                PropName = "ImeError", 
                ErrorMsg = String.Format(ValidationHelper.MSG_ALPHA_REGULAR, "ime"),
                IsValid = (ime != null && Regex.IsMatch(ime, ValidationHelper.RULE_ALPHA_REGULAR)) || ime == ValidationHelper.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PrezimeError",
                ErrorMsg = String.Format(ValidationHelper.MSG_ALPHA_REGULAR, "prezime"),
                IsValid = (prezime != null && Regex.IsMatch(prezime, ValidationHelper.RULE_ALPHA_REGULAR)) || prezime == ValidationHelper.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "AdresaError",
                ErrorMsg = String.Format(ValidationHelper.MSG_ADRESA, "adresa korisnika"),
                IsValid = (adresa != null && Regex.IsMatch(adresa, ValidationHelper.RULE_ADRESA)) || adresa == ValidationHelper.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "UsernameError",
                ErrorMsg = String.Format(ValidationHelper.MSG_ALPHANUM_REGULAR, "username"),
                IsValid = (username != null && Regex.IsMatch(username, ValidationHelper.RULE_ALPHANUM_REGULAR)) || username == ValidationHelper.SKIPEVAL
            });

            _fields.Add(new ValidatorField
            {
                PropName = "PasswordError",
                ErrorMsg = ValidationHelper.MSG_PASSWORD,
                IsValid = (password != null && Regex.IsMatch(password, ValidationHelper.RULE_PASSWORD)) || password == ValidationHelper.SKIPEVAL
            });

            // na kraju postavi status cijele validacije na false samo ako imamo barem jedno polje false..
            _validityStatus = _fields.Where(f => !f.IsValid).ToList().Count > 0 ? false : true;
        }
    }
}
