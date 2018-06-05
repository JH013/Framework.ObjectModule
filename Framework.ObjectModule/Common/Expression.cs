using System.Collections.Generic;

namespace Framework.ObjectModule
{
    public class Expression
    {
        private List<object> _components;
        public Expression Add(string column, object value)
        {
            if (_components == null)
            {
                _components = new List<object>();
            }
            _components.Add(new Component { Column = column, Value = value, Operator = Operator.Equal });
            return this;
        }

        public Expression Add(string column, object value, Operator opera)
        {
            if (_components == null)
            {
                _components = new List<object>();
            }
            _components.Add(new Component { Column = column, Value = value, Operator = opera });
            return this;
        }

        public string GenerateSqlText()
        {
            string temp = " WHERE";
            foreach (var component in _components)
            {
                if (component is Symbol)
                {
                    switch (component)
                    {
                        case Symbol.And:
                            temp += " AND";
                            break;
                        case Symbol.Or:
                            temp += " OR";
                            break;
                        case Symbol.LeftBracket:
                            temp += " (";
                            break;
                        case Symbol.RightBracket:
                            temp += " )";
                            break;
                    }
                }
                else if (component is Component)
                {
                    Component _component = component as Component;
                    if (_component.Value == null)
                    {
                        if (_component.Operator == Operator.Equal)
                        {
                            temp += string.Format(" {0} IS NULL", _component.Column);
                        }
                        else if (_component.Operator == Operator.NotEqual)
                        {
                            temp += string.Format(" {0} IS NOT NULL", _component.Column);
                        }
                    }
                    else
                    {
                        string value = _component.Value is string ? string.Format("'{0}'", _component.Value.ToString()) : _component.Value.ToString();
                        string operaStr = string.Empty;
                        switch (_component.Operator)
                        {
                            case Operator.Equal:
                                operaStr = "=";
                                break;
                            case Operator.NotEqual:
                                operaStr = "<>";
                                break;
                            case Operator.GreaterThan:
                                operaStr = ">";
                                break;
                            case Operator.GreaterThanOrEqual:
                                operaStr = ">=";
                                break;
                            case Operator.LessThan:
                                operaStr = "<";
                                break;
                            case Operator.LessThanOrEqual:
                                operaStr = "<=";
                                break;
                        }
                        temp += string.Format(" {0} {1} {2}", _component.Column, operaStr, value);
                    }
                }
            }
            return temp;
        }

    }

    public class Component
    {
        public string Column { get; set; }
        public object Value { get; set; }
        public Operator Operator { get; set; }
    }

    public enum Operator
    {
        Equal = 0,
        NotEqual = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThan = 4,
        LessThanOrEqual = 5
    }

    public enum Symbol
    {
        And = 0,
        Or = 1,
        LeftBracket = 2,
        RightBracket = 3
    }
}
