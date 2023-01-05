using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace KaizenCase
{
    public class ReceiptFunctions
    {
        int MaxSize = 100;
        public List<string> GetRecipt()
        {
            StreamReader r = new StreamReader("response.json");
            string json = r.ReadToEnd();
            return GetRecipt(json);
        }

        // Json string verisi ReceiptModel e göre deserialize edilir.
        // Daha sonra her bir objenin x ve y konum noktalarından ortalama bir merkez konumu hesaplanır.
        // Bu sayede elimizde her bir obje için tek bir x ve y değeri kalır.
        // Bu merkez değerlerine göre önce y değerine göre sıralanır. Bu sıralama ile satırlar hizalanır.
        // Daha sonra x e göre sıralanır buda satırlardaki stunlar sıralamnır.
        // Bu sayede elimizdeki dizinlerde ilk sırada fişteki ilk satırın ilk kelimesi bulunur.
        // Dizinin son sırasında ise son satırın son kelimesi bulunur.
        // Daha sonra dizi içerisinde objeler tek bir satır olarak düşünülür. 
        // Bu dizin içerisinde her bir obje bir sonraki ile aynı satırda olup olmadığına bakılır.
        // Eğer aynı satırda değilse receipt listesine eklenir. 
        public List<string> GetRecipt(string ReciptJson)
        {
            List<string> receipt = new List<string>();
            ReceiptModel[] receiptModelsTemp = JsonConvert.DeserializeObject<ReceiptModel[]>(ReciptJson);
            List<ReceiptModel> receiptModels = receiptModelsTemp
                .OrderBy(o => o.boundingPoly.center.y)
                .ThenBy(o => o.boundingPoly.center.x).ToList();
            string lineText = "";
            for (int i = 0; i < receiptModels.Count; i++)
            {
                if (receiptModels[i].description.Length < MaxSize)
                {
                    lineText = lineText + receiptModels[i].description + " ";
                    if (i + 1 < receiptModels.Count)
                    {
                        if (!SameLineControl(receiptModels[i].boundingPoly, receiptModels[i + 1].boundingPoly))
                        {
                            receipt.Add(lineText);
                            lineText = "";
                        }
                    }
                    else
                        receipt.Add(lineText);
                }
            }
            return receipt;
        }

        // Gelen 2 objenin konumlarına bakılarak aynı satırda olup olmadıkları belirlenir.
        // Gelen ilk objenin maxy ve miny değerleri belirlenir ve
        // ikinci objenin merkez y değeri bu ilk objenin maxy ve miny değerleri içerisinde ise bu iki obje aynı satırdadır.
        bool SameLineControl(BoundingPoly boundingPoly1, BoundingPoly boundingPoly2)
        {
            bool result = false;
            int miny1 = int.MaxValue;
            int maxy1 = int.MinValue;
            for (int i = 0; i < boundingPoly1.vertices.Count; i++)
            {
                if (miny1 > boundingPoly1.vertices[i].y)
                    miny1 = boundingPoly1.vertices[i].y;
                if (maxy1 < boundingPoly1.vertices[i].y)
                    maxy1 = boundingPoly1.vertices[i].y;
            }
            if (boundingPoly2.center.y > miny1 && boundingPoly2.center.y < maxy1)
                result = true;
            return result;
        }
    }
}
