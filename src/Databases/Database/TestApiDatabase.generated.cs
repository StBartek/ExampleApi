﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace Database
{
	/// <summary>
	/// Database       : TestApiDb
	/// Data Source    : localhost
	/// Server Version : 14.00.3370
	/// </summary>
	public partial class TestApiDb : LinqToDB.Data.DataConnection
	{
		public ITable<Addresses>       Addresses       { get { return this.GetTable<Addresses>(); } }
		public ITable<Cities>          Cities          { get { return this.GetTable<Cities>(); } }
		public ITable<Contacts>        Contacts        { get { return this.GetTable<Contacts>(); } }
		public ITable<DicContactType>  DicContactType  { get { return this.GetTable<DicContactType>(); } }
		public ITable<Streets>         Streets         { get { return this.GetTable<Streets>(); } }
		public ITable<Users>           Users           { get { return this.GetTable<Users>(); } }
		public ITable<UsersLAddresses> UsersLAddresses { get { return this.GetTable<UsersLAddresses>(); } }

		public TestApiDb()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public TestApiDb(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="Addresses")]
	public partial class Addresses
	{
		[Column(DataType=DataType.Guid),                PrimaryKey,  NotNull] public Guid   AddressId { get; set; } // uniqueidentifier
		[Column(DataType=DataType.Int32),                            NotNull] public int    CityId    { get; set; } // int
		[Column(DataType=DataType.Int32),                  Nullable         ] public int?   StreetId  { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=50),    Nullable         ] public string HouseNo   { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=50),    Nullable         ] public string FlatNo    { get; set; } // nvarchar(50)

		#region Associations

		/// <summary>
		/// FK_UsersLAddresses_Addresses_BackReference
		/// </summary>
		[Association(ThisKey="AddressId", OtherKey="AddressId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UsersLAddresses> UsersLAddresses { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Cities")]
	public partial class Cities
	{
		[Column(DataType=DataType.Int32),                PrimaryKey,  Identity] public int    CityId        { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=100), NotNull              ] public string Name          { get; set; } // nvarchar(100)
		[Column(DataType=DataType.NVarChar, Length=6),      Nullable          ] public string PostCode      { get; set; } // nvarchar(6)
		[Column(DataType=DataType.Int32),                   Nullable          ] public int?   ResidentCount { get; set; } // int

		#region Associations

		/// <summary>
		/// FK_Streets_Cities_BackReference
		/// </summary>
		[Association(ThisKey="CityId", OtherKey="CityId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Streets> Streets { get; set; }

		/// <summary>
		/// FK_Users_Cities_BackReference
		/// </summary>
		[Association(ThisKey="CityId", OtherKey="CityId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Users> Users { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Contacts")]
	public partial class Contacts
	{
		[Column(DataType=DataType.Int32),                PrimaryKey,  Identity] public int    ContactId { get; set; } // int
		[Column(DataType=DataType.Int32),                NotNull              ] public int    TypeId    { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=250), NotNull              ] public string Value     { get; set; } // nvarchar(250)
		[Column(DataType=DataType.Int32),                   Nullable          ] public int?   UserId    { get; set; } // int

		#region Associations

		/// <summary>
		/// FK_Contacts_DicContactType
		/// </summary>
		[Association(ThisKey="TypeId", OtherKey="DicContactTypeId", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_Contacts_DicContactType", BackReferenceName="Contacts")]
		public DicContactType Type { get; set; }

		/// <summary>
		/// FK_Contacts_Users
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="FK_Contacts_Users", BackReferenceName="Contacts")]
		public Users User { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="DicContactType")]
	public partial class DicContactType
	{
		[Column(DataType=DataType.Int32),               PrimaryKey, Identity] public int    DicContactTypeId { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=50), NotNull             ] public string Name             { get; set; } // nvarchar(50)

		#region Associations

		/// <summary>
		/// FK_Contacts_DicContactType_BackReference
		/// </summary>
		[Association(ThisKey="DicContactTypeId", OtherKey="TypeId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Contacts> Contacts { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Streets")]
	public partial class Streets
	{
		[Column(DataType=DataType.Int32),                PrimaryKey,  Identity] public int    StreetId { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=250), NotNull              ] public string Name     { get; set; } // nvarchar(250)
		[Column(DataType=DataType.Int32),                NotNull              ] public int    CityId   { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=6),      Nullable          ] public string PostCode { get; set; } // nvarchar(6)

		#region Associations

		/// <summary>
		/// FK_Streets_Cities
		/// </summary>
		[Association(ThisKey="CityId", OtherKey="CityId", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_Streets_Cities", BackReferenceName="Streets")]
		public Cities City { get; set; }

		/// <summary>
		/// FK_Users_Streets_BackReference
		/// </summary>
		[Association(ThisKey="StreetId", OtherKey="StreetId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Users> Users { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Users")]
	public partial class Users
	{
		[Column(DataType=DataType.Int32),                 PrimaryKey,  Identity] public int    UserId     { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=50),   NotNull              ] public string FirstName  { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=50),   NotNull              ] public string Surname    { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=250),  NotNull              ] public string Email      { get; set; } // nvarchar(250)
		[Column(DataType=DataType.Int32),                    Nullable          ] public int?   Age        { get; set; } // int
		[Column(DataType=DataType.Int32),                 NotNull              ] public int    CityId     { get; set; } // int
		[Column(DataType=DataType.Int32),                    Nullable          ] public int?   StreetId   { get; set; } // int
		[Column(DataType=DataType.NVarChar, Length=50),      Nullable          ] public string HouseNo    { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=50),      Nullable          ] public string FlatNo     { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=50),   NotNull              ] public string Password   { get; set; } // nvarchar(50)
		[Column(DataType=DataType.NVarChar, Length=2000),    Nullable          ] public string SearchData { get; set; } // nvarchar(2000)
		[Column(DataType=DataType.NVarChar, Length=12),      Nullable          ] public string Phone      { get; set; } // nvarchar(12)

		#region Associations

		/// <summary>
		/// FK_Users_Cities
		/// </summary>
		[Association(ThisKey="CityId", OtherKey="CityId", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_Users_Cities", BackReferenceName="Users")]
		public Cities City { get; set; }

		/// <summary>
		/// FK_Contacts_Users_BackReference
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Contacts> Contacts { get; set; }

		/// <summary>
		/// FK_Users_Streets
		/// </summary>
		[Association(ThisKey="StreetId", OtherKey="StreetId", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="FK_Users_Streets", BackReferenceName="Users")]
		public Streets Street { get; set; }

		/// <summary>
		/// FK_UsersLAddresses_Users_BackReference
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UsersLAddresses> UsersLAddresses { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="UsersLAddresses")]
	public partial class UsersLAddresses
	{
		[Column(DataType=DataType.Int32), NotNull] public int  UserId    { get; set; } // int
		[Column(DataType=DataType.Guid),  NotNull] public Guid AddressId { get; set; } // uniqueidentifier

		#region Associations

		/// <summary>
		/// FK_UsersLAddresses_Addresses
		/// </summary>
		[Association(ThisKey="AddressId", OtherKey="AddressId", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_UsersLAddresses_Addresses", BackReferenceName="UsersLAddresses")]
		public Addresses Address { get; set; }

		/// <summary>
		/// FK_UsersLAddresses_Users
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="UserId", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_UsersLAddresses_Users", BackReferenceName="UsersLAddresses")]
		public Users User { get; set; }

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Addresses Find(this ITable<Addresses> table, Guid AddressId)
		{
			return table.FirstOrDefault(t =>
				t.AddressId == AddressId);
		}

		public static Cities Find(this ITable<Cities> table, int CityId)
		{
			return table.FirstOrDefault(t =>
				t.CityId == CityId);
		}

		public static Contacts Find(this ITable<Contacts> table, int ContactId)
		{
			return table.FirstOrDefault(t =>
				t.ContactId == ContactId);
		}

		public static DicContactType Find(this ITable<DicContactType> table, int DicContactTypeId)
		{
			return table.FirstOrDefault(t =>
				t.DicContactTypeId == DicContactTypeId);
		}

		public static Streets Find(this ITable<Streets> table, int StreetId)
		{
			return table.FirstOrDefault(t =>
				t.StreetId == StreetId);
		}

		public static Users Find(this ITable<Users> table, int UserId)
		{
			return table.FirstOrDefault(t =>
				t.UserId == UserId);
		}
	}
}

#pragma warning restore 1591
