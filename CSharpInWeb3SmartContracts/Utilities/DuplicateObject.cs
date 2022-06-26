namespace CSharpInWeb3SmartContracts.Utilities
{
    public static class DuplicateObject
    {
        public static object? CreateDuplicateObject(object originalObject)
        {
            try
            {
                if (originalObject is null)
                {
                    return null;
                }

                if (originalObject.GetType() is null)
                {
                    return null;
                }

                if (originalObject.GetType() == typeof(string))
                {
                    return originalObject.ToString();
                }

                if (originalObject.GetType() == typeof(Int32))
                {
                    return Convert.ToInt32(originalObject);
                }

                //create new instance of the object
                object newObject = Activator.CreateInstance(originalObject.GetType());

                //get list of all properties
                //loop through each property
                foreach (var property in originalObject.GetType().GetProperties())
                {
                    //set the value for property
                    property.SetValue(newObject, property.GetValue(originalObject, null), null);
                }

                //get list of all fields  
                //loop through each field
                foreach (var field in originalObject.GetType().GetFields())
                {
                    //set the value for field
                    field.SetValue(newObject, field.GetValue(originalObject));
                }

                // return the newly created object with all the properties and fields values copied from original object
                return newObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
