using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;
using System.Collections;
using Core.Common.Extensions;
using System.Linq.Expressions;
using Core.Common.Utils;
using System.ComponentModel;
using Core.Common.Contracts;
using System.ComponentModel.Composition.Hosting;

namespace Core.Common.Core
{
	// this is taken from RentalCar -> Core.Common.Core.ObjectBase
	// this is used as the base object for Model / entity/ object model classes
	public abstract class ClientEntityBase : ObservableObject , IExtensibleDataObject, IDataErrorInfo,IDirtyCapable,IIdentifiableEntity
	{
		public ClientEntityBase()
		{
			_Validator = GetValidator();
			Validate();
		}

		protected bool _IsDirty = false;
		protected IValidator _Validator = null;

		protected IEnumerable<ValidationFailure> _ValidationErrors = null;
		
		public static CompositionContainer Container { get; set; }

		#region IExtensibleDataObject Members

		public ExtensionDataObject ExtensionData { get; set; }

		#endregion

		#region IDirtyCapable members

		[NotNavigable]
		public virtual bool IsDirty
		{
			get { return _IsDirty; }
			protected set
			{
				_IsDirty = value;
				RaisePropertyChanged("IsDirty");//, false
			}
		}

		public virtual bool IsAnythingDirty()
		{
			bool isDirty = false;

			WalkObjectGraph(
			o =>
			{
				if (o.IsDirty)
				{
					isDirty = true;
					return true; // short circuit
				}
				else
					return false;
			}, coll => { });

			return isDirty;
		}

		public List<IDirtyCapable> GetDirtyObjects()
		{
			List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

			WalkObjectGraph(
			o =>
			{
				if (o.IsDirty)
					dirtyObjects.Add(o);

				return false;
			}, coll => { });

			return dirtyObjects;
		}

		public void CleanAll()
		{
			WalkObjectGraph(
			o =>
			{
				if (o.IsDirty)
					o.IsDirty = false;
				return false;
			}, coll => { });
		}

		#endregion

		#region Protected methods

		protected void WalkObjectGraph(Func<ClientEntityBase, bool> snippetForObject,
									   Action<IList> snippetForCollection,
									   params string[] exemptProperties)
		{
			List<ClientEntityBase> visited = new List<ClientEntityBase>();
			Action<ClientEntityBase> walk = null;

			List<string> exemptions = new List<string>();
			if (exemptProperties != null)
				exemptions = exemptProperties.ToList();

			walk = (o) =>
			{
				if (o != null && !visited.Contains(o))
				{
					visited.Add(o);

					bool exitWalk = snippetForObject.Invoke(o);

					if (!exitWalk)
					{
						PropertyInfo[] properties = o.GetBrowsableProperties();
						foreach (PropertyInfo property in properties)
						{
							if (!exemptions.Contains(property.Name))
							{
								if (property.PropertyType.IsSubclassOf(typeof(ClientEntityBase)))
								{
									ClientEntityBase obj = (ClientEntityBase)(property.GetValue(o, null));
									walk(obj);
								}
								else
								{
									IList coll = property.GetValue(o, null) as IList;
									if (coll != null)
									{
										snippetForCollection.Invoke(coll);

										foreach (object item in coll)
										{
											if (item is ClientEntityBase)
												walk((ClientEntityBase)item);
										}
									}
								}
							}
						}
					}
				}
			};

			walk(this);
		}

		#endregion

		#region Property change notification

		//protected override void OnPropertyChanged(string propertyName)
		//{
		//	OnPropertyChanged(propertyName, true);
		//}

		//protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
		//{
		//	string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
		//	OnPropertyChanged(propertyName, makeDirty);
		//}

		//protected void OnPropertyChanged(string propertyName, bool makeDirty)
		//{
		//	base.OnPropertyChanged(propertyName);

		//	if (makeDirty)
		//		IsDirty = true;

		//	Validate();
		//}

		#endregion

		#region Validation

		protected virtual IValidator GetValidator()
		{
			return null;
		}

		[NotNavigable]
		public IEnumerable<ValidationFailure> ValidationErrors
		{
			get { return _ValidationErrors; }
			set { }
		}

		public void Validate()
		{
			if (_Validator != null)
			{
				ValidationResult results = _Validator.Validate(this);
				_ValidationErrors = results.Errors;
			}
		}

		[NotNavigable]
		public virtual bool IsValid
		{
			get
			{
				if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
					return false;
				else
					return true;
			}
		}

		#endregion

		#region IDataErrorInfo members

		string IDataErrorInfo.Error
		{
			get { return string.Empty; }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				StringBuilder errors = new StringBuilder();

				if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
				{
					foreach (ValidationFailure validationError in _ValidationErrors)
					{
						if (validationError.PropertyName == columnName)
							errors.AppendLine(validationError.ErrorMessage);
					}
				}

				return errors.ToString();
			}
		}

		#endregion



		public int EntityId
		{
			get { return -1; }
			set{int i = value;}
		}

		public string PathName => String.Empty;
	}
}
