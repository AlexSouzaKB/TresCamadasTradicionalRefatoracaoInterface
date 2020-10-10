using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security;

namespace DataBase
{
    public abstract class ADb
    {
        public virtual void Salvar()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                ValidationAttribute columAttribute = p.GetCustomAttribute<ValidationAttribute>();
                if (columAttribute != null && columAttribute.Presence && p.GetValue(this) == null)
                {
                    throw new Exception("Nome é obrigatório");
                }
            }

            new Db().Salvar(this);
        }

        public virtual List<ADb> Todos()
        {
            return new Db().Todos(this);
        }
    }
}
