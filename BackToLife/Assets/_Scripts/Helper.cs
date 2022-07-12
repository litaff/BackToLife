namespace BackToLife
{
    public static class Helper
    {
        public static string ReverseString(string str) {  
            var chars = str.ToCharArray();  
            var result = new char[chars.Length];  
            for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--) {  
                result[i] = chars[j];  
            }  
            return new string(result);  
        }
    }
}