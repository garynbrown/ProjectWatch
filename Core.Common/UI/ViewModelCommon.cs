using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using FluentValidation;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace Core.Common.UI
{
	public class ViewModelCommon : ViewModelBase, IDataErrorInfo
	{
		bool _ErrorsVisible = false;
		protected IValidator _Validator = null;

		protected IEnumerable<ValidationFailure> _ValidationErrors = null;

		public object ViewLoaded
		{
			get
			{
				OnViewLoaded();
				return null;
			}
		}

		protected virtual void OnViewLoaded() { }

		protected void WithClient<T>(T proxy, Action<T> codeToExecute)
		{
			codeToExecute.Invoke(proxy);

			IDisposable disposableClient = proxy as IDisposable;
			if (disposableClient != null)
				disposableClient.Dispose();
		}

		public virtual string ViewTitle
		{
			get { return String.Empty; }
		}

		List<ClientEntityBase> _Models;

		protected virtual void AddModels(List<ClientEntityBase> models) { }

		protected void ValidateModel()
		{
			if (_Models == null)
			{
				_Models = new List<ClientEntityBase>();
				AddModels(_Models);
			}

			_ValidationErrors = new List<ValidationFailure>();

			if (_Models.Count > 0)
			{
				foreach (ClientEntityBase modelObject in _Models)
				{
					if (modelObject != null)
						modelObject.Validate();

					_ValidationErrors = _ValidationErrors.Union(modelObject.ValidationErrors).ToList();
				}

				RaisePropertyChanged(() => ValidationErrors);
				RaisePropertyChanged(() => ValidationHeaderText);
				RaisePropertyChanged(() => ValidationHeaderVisible);
			}
		}

		public DelegateCommand<object> ToggleErrorsCommand { get; protected set; }

		public virtual bool ValidationHeaderVisible
		{
			get { return ValidationErrors != null && ValidationErrors.Count() > 0; }
		}

		public virtual bool ErrorsVisible
		{
			get { return _ErrorsVisible; }
			set
			{
				if (_ErrorsVisible == value)
					return;

				_ErrorsVisible = value;
				RaisePropertyChanged(() => ErrorsVisible);
			}
		}

		public virtual string ValidationHeaderText
		{
			get
			{
				string ret = string.Empty;

				if (ValidationErrors != null)
				{
					string verb = (ValidationErrors.Count() == 1 ? "is" : "are");
					string suffix = (ValidationErrors.Count() == 1 ? "" : "s");

					if (!IsValid)
						ret = string.Format("There {0} {1} validation error{2}.", verb, ValidationErrors.Count(), suffix);
				}

				return ret;
			}
		}

		protected virtual void OnToggleErrorsCommandExecute(object arg)
		{
			ErrorsVisible = !ErrorsVisible;
		}

		protected virtual bool OnToggleErrorsCommandCanExecute(object arg)
		{
			return !IsValid;
		}
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
				FluentValidation.Results.ValidationResult results = _Validator.Validate(this);
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
	}
}
