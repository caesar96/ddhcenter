using DDHCenter.Core.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DDHCenter.Core.Models
{
    public class Cliente
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("referencePhone")]
        public string ReferencePhone { get; set; }

        [JsonProperty("age")]
        public double Age { get; set; }

        [JsonProperty("gender")]
        public GenderType Gender { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("curp")]
        public string Curp { get; set; }

        [JsonProperty("referenceAddress")]
        public string ReferenceAddress { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("registeredDate")]
        public DateTime RegisteredDate { get; set; }

        [JsonProperty("birthDay")]
        public DateTime BirthDay { get; set; }

        [JsonProperty("nextProgrammedDates")]
        public List<Cita> NextProgrammedDates { get; set; }

        [JsonProperty("recommendedMeds")]
        public List<Medicamento> RecommendedMeds { get; set; }

        [JsonProperty("socios")]
        public List<Socio> Socios { get; set; }

        [JsonProperty("boughtMeds")]
        public List<Venta> BoughtMeds { get; set; }
    }

    public class Venta
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("soldMeds")]
        public List<Medicamento> SoldMeds { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("isSold")]
        public bool IsSold { get; set; }

        [JsonProperty("deliveredDate")]
        public DateTime DeliveredDate { get; set; }

        [JsonProperty("sellDate")]
        public DateTime SellDate { get; set; }
    }

    public class Cita
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("citaDate")]
        public DateTime CitaDate { get; set; }

        [JsonProperty("isPending")]
        public bool IsPending { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }
    }

    public class Medicamento
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("expireDate")]
        public DateTime ExpireDate { get; set; }

        [JsonProperty("clientes")]
        public List<Cliente> Clientes { get; set; }

        [JsonProperty("solds")]
        public List<Venta> Solds { get; set; }

        [JsonProperty("categorias")]
        public List<Categoria> Categorias { get; set; }
    }

    public class Categoria
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("medicamentos")]
        public List<Medicamento> Medicamentos { get; set; }
    }

    public class Socio
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("dim")]
        public string Dim { get; set; }

        [JsonProperty("curp")]
        public string Curp { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("gender")]
        public GenderType Gender { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("registeredDate")]
        public DateTime RegisteredDate { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("clientes")]
        public List<Cliente> Clientes { get; set; }

        [JsonProperty("promotores")]
        public List<Promotor> Promotores { get; set; }
    }

    public class Promotor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("fistName")]
        public string FistName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("registeredDate")]
        public DateTimeOffset RegisteredDate { get; set; }

        [JsonProperty("curp")]
        public string Curp { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("gender")]
        public GenderType Gender { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("birthDate")]
        public DateTimeOffset BirthDate { get; set; }

        [JsonProperty("socio")]
        public Socio Socio { get; set; }
    }

    public class Lugar
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("agency")]
        public string Agency { get; set; }

        [JsonProperty("colony")]
        public string Colony { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("president")]
        public string President { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("secretary")]
        public string Secretary { get; set; }

        [JsonProperty("isProfitable")]
        public bool IsProfitable { get; set; }

        [JsonProperty("isContactable")]
        public bool IsContactable { get; set; }

        [JsonProperty("dealDate")]
        public DateTimeOffset DealDate { get; set; }
    }
}
