using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using DbHidrator.Entities;

namespace DbHidrator.Generator
{
    internal class TypeGenerator
    {
        static AssemblyBuilder asmBuilder;
        static ModuleBuilder modBuilder;
        ILGenerator ilgen;
        public TypeGenerator()
        {
            if (asmBuilder == null)
            {
                asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DbHidratorRuntimeTypes"), AssemblyBuilderAccess.RunAndCollect);
            }
            if (modBuilder == null)
            {
                modBuilder = asmBuilder.DefineDynamicModule("RuntimeTypesLibrary");
            }
        }

        public Type CreateType(Table table)
        {
            TypeBuilder typeBuilder = modBuilder.DefineType("RuntimeTypesLibrary." + table.Name, TypeAttributes.Class | TypeAttributes.Public);
            ConstructorBuilder ctorBuilder = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            foreach (var column in table.Columns)
            {
                Type tipo = GetType(column);
                FieldBuilder campo = typeBuilder.DefineField("_" + column.Name, tipo, FieldAttributes.Private);
                PropertyBuilder propiedad = typeBuilder.DefineProperty(column.Name, PropertyAttributes.HasDefault, tipo, null);
                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;
                MethodBuilder getPropBuilder = typeBuilder.DefineMethod("get_" + column.Name, getSetAttr, tipo, Type.EmptyTypes);
                MethodBuilder setPropBuilder = typeBuilder.DefineMethod("set_" + column.Name, getSetAttr, null, new Type[] { tipo });

                ilgen = getPropBuilder.GetILGenerator();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, campo);
                ilgen.Emit(OpCodes.Ret);

                ilgen = setPropBuilder.GetILGenerator();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldarg_1);
                ilgen.Emit(OpCodes.Stfld, campo);
                ilgen.Emit(OpCodes.Ret);

                propiedad.SetGetMethod(getPropBuilder);
                propiedad.SetSetMethod(setPropBuilder);
            }

            return typeBuilder.CreateType();
        }

        private Type GetType(Column column)
        {
            Type tipo = typeof(string);
            switch (column.TypeName.ToLower())
            {
                case "image":
                    tipo= typeof(byte[]);
                    break;
                case "text":
                case "varchar":
                case "nvarchar":
                case "sysname":
                case "ntext":
                    tipo = typeof(string);
                    break;
                case "uniqueidentifier":
                    tipo = typeof(byte[]);
                    break;
                case "date":
                case "time":
                case "datetime2":
                case "datetimeoffset":
                case "smalldatetime":
                case "datetime":
                    tipo = typeof(DateTime);
                    break;
                case "tinyint":
                case "smallint":
                    tipo = typeof(Int16);
                    break;
                case "int":
                    tipo = typeof(int);
                    break;
                case "bigint":
                    tipo = typeof(Int64);
                    break;
                case "real":
                case "numeric":
                case "smallmoney":
                case "decimal":
                    tipo = typeof(decimal);
                    break;
                case "money":
                case "float":
                    tipo = typeof(double);
                    break;
                case "bit":
                    tipo = typeof(bool);
                    break;
                case "varbinary":
                case "binary":
                case "timestamp":
                    tipo = typeof(byte[]);
                    break;
                case "char":
                case "nchar":
                    tipo = typeof(char);
                    break;
            }
            return tipo;
        }
    }
}
