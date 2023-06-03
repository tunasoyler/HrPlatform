using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.Entities.Entities
{
    public class CompanyManager:BaseEntity
    {
        public byte[]? ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string TCNo { get; set; }
        public DateTime DateOfRecruitment { get; set; }
        public string Profession { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Company Company { get; set; }
    }
}
//1 site yöneticisi şirket eklicek
//2şirket bilgileri
//ad, ünvan, mersis no, vergi no, vergi dairesi, logosu, tel, adres, email, çalışan sayısı, kuruluş yılı, sözleşme başlangıç tarihi, söz bit tarihi, aktiflik durumu 
//şirket listeleme
//Şirketle ilgili tüm bilgiler güncellenebilir ve detaylar da eklenebilir 
//şirket yöneticisi ekleyeceğiz
//foto, ad,2.ad, soyad,2.soyad, DT, DY, TC, işe giriş tarihi, işten çıkış tarihi, meslek, departman, email, adres, telefon ve şirket özellikleri()
//birden fazla şirket yöneticisi olabilir bu platformda 
//Şirket yöneticisi emaili=ad.soyad.bilgeadamboost

//22'sinde bitecek  ==96 saat task
