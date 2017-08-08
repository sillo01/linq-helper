﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="sampleDB")]
	public partial class SampleEntitiesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    partial void InsertCountryCode(CountryCode instance);
    partial void UpdateCountryCode(CountryCode instance);
    partial void DeleteCountryCode(CountryCode instance);
    #endregion
		
		public SampleEntitiesDataContext() : 
				base(global::DataLayer.Properties.Settings.Default.sampleDBConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SampleEntitiesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SampleEntitiesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SampleEntitiesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SampleEntitiesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<CountryCode> CountryCode
		{
			get
			{
				return this.GetTable<CountryCode>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CountryCode")]
	public partial class CountryCode : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdCountryCode;
		
		private string _OfficialShort;
		
		private string _OfficialLong;
		
		private int _ISOcode;
		
		private string _ISOshort;
		
		private string _ISOlong;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdCountryCodeChanging(int value);
    partial void OnIdCountryCodeChanged();
    partial void OnOfficialShortChanging(string value);
    partial void OnOfficialShortChanged();
    partial void OnOfficialLongChanging(string value);
    partial void OnOfficialLongChanged();
    partial void OnISOcodeChanging(int value);
    partial void OnISOcodeChanged();
    partial void OnISOshortChanging(string value);
    partial void OnISOshortChanged();
    partial void OnISOlongChanging(string value);
    partial void OnISOlongChanged();
    #endregion
		
		public CountryCode()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdCountryCode", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int IdCountryCode
		{
			get
			{
				return this._IdCountryCode;
			}
			set
			{
				if ((this._IdCountryCode != value))
				{
					this.OnIdCountryCodeChanging(value);
					this.SendPropertyChanging();
					this._IdCountryCode = value;
					this.SendPropertyChanged("IdCountryCode");
					this.OnIdCountryCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OfficialShort", DbType="NVarChar(25) NOT NULL", CanBeNull=false)]
		public string OfficialShort
		{
			get
			{
				return this._OfficialShort;
			}
			set
			{
				if ((this._OfficialShort != value))
				{
					this.OnOfficialShortChanging(value);
					this.SendPropertyChanging();
					this._OfficialShort = value;
					this.SendPropertyChanged("OfficialShort");
					this.OnOfficialShortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OfficialLong", DbType="NVarChar(250) NOT NULL", CanBeNull=false)]
		public string OfficialLong
		{
			get
			{
				return this._OfficialLong;
			}
			set
			{
				if ((this._OfficialLong != value))
				{
					this.OnOfficialLongChanging(value);
					this.SendPropertyChanging();
					this._OfficialLong = value;
					this.SendPropertyChanged("OfficialLong");
					this.OnOfficialLongChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ISOcode", DbType="Int NOT NULL")]
		public int ISOcode
		{
			get
			{
				return this._ISOcode;
			}
			set
			{
				if ((this._ISOcode != value))
				{
					this.OnISOcodeChanging(value);
					this.SendPropertyChanging();
					this._ISOcode = value;
					this.SendPropertyChanged("ISOcode");
					this.OnISOcodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ISOshort", DbType="NChar(2) NOT NULL", CanBeNull=false)]
		public string ISOshort
		{
			get
			{
				return this._ISOshort;
			}
			set
			{
				if ((this._ISOshort != value))
				{
					this.OnISOshortChanging(value);
					this.SendPropertyChanging();
					this._ISOshort = value;
					this.SendPropertyChanged("ISOshort");
					this.OnISOshortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ISOlong", DbType="NChar(3) NOT NULL", CanBeNull=false)]
		public string ISOlong
		{
			get
			{
				return this._ISOlong;
			}
			set
			{
				if ((this._ISOlong != value))
				{
					this.OnISOlongChanging(value);
					this.SendPropertyChanging();
					this._ISOlong = value;
					this.SendPropertyChanged("ISOlong");
					this.OnISOlongChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591