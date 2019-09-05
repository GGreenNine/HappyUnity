using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;


namespace HappyUnity.Serialization
{
    public static class MethodJsonSerialization 
{
    public static MethodCallExpression GetMethodInfo<T>(Expression<Action<T>> expression)
    {
        if (expression.Body is MethodCallExpression member)
            return member;

        throw new ArgumentException("Expression is not a method", "expression");
    }

    public static string GenerateMethodInfoPackage<T>(Expression<Action<T>> expression)
    {
        var methodCallExpression = GetMethodInfo(expression);

        var test = ResolveArgs(expression);
        
        var query = methodCallExpression.Arguments.AsEnumerable() // anything past this is done outside of sql server
            .Select(item => new KeyValuePair<Type, object>(item.Type, (item as ConstantExpression)?.Value)).ToList();
        
        var methodInfo = methodCallExpression.Method;

        var s = new MethodConstructor(methodInfo.Name, methodInfo.GetParameters().Select(p => p.ParameterType).ToArray(), methodInfo.ReturnType, test.ToArray() );
        
         return JsonConvert.SerializeObject(s);
    }
    
    private static KeyValuePair<Type, object>[] ResolveArgs<T>(Expression<Action<T>> expression)
    {
        var body = (System.Linq.Expressions.MethodCallExpression)expression.Body;
        var values = new List<KeyValuePair<Type, object>>();

        foreach (var argument in body.Arguments)
        {
            var exp = ResolveMemberExpression(argument);
            var type = argument.Type;

            var value = GetValue(exp);

            values.Add(new KeyValuePair<Type, object>(type, value));
        }

        return values.ToArray();
    }

    public static MemberExpression ResolveMemberExpression(Expression expression)
    {

        if (expression is MemberExpression)
        {
            return (MemberExpression)expression;
        }
        else if (expression is UnaryExpression)
        {
            // if casting is involved, Expression is not x => x.FieldName but x => Convert(x.Fieldname)
            return (MemberExpression)((UnaryExpression)expression).Operand;
        }
        else
        {
            throw new NotSupportedException(expression.ToString());
        }
    }

    private static object GetValue(MemberExpression exp)
    {
        // expression is ConstantExpression or FieldExpression
        if (exp.Expression is ConstantExpression)
        {
            return (((ConstantExpression)exp.Expression).Value)
                .GetType()
                .GetField(exp.Member.Name)
                .GetValue(((ConstantExpression)exp.Expression).Value);    
        }
        else if (exp.Expression is MemberExpression)
        {
            return GetValue((MemberExpression)exp.Expression);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    
    
}
public class MethodConstructor
{
    public string methodName;
    public Type[] parameters;
    public Type returnType;
    public KeyValuePair<Type, object>[] ParamatersExpression;
     
    public MethodConstructor(string methodName, Type[] parameters, Type returnType, KeyValuePair<Type, object>[] ParamatersExpression)
    {
        this.methodName = methodName;
        this.parameters = parameters;
        this.returnType = returnType;
        this.ParamatersExpression = ParamatersExpression;
    }
}


}
