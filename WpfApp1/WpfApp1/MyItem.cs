namespace WpfApp1
{
    public class MyItem
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Pesel}";
        }
    }
}
