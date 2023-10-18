// See https://aka.ms/new-console-template for more information


using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
/**
static async void Main(string[] args) {
    // read files
    String fileName1 = args[0], fileName2 = args[1], fileName3 = args[2];
    Task<List<String>> readFile1 = AsyncReadFileWords(fileName1);
    Task<List<String>> readFile2 = AsyncReadFileWords(fileName2);
    Task<List<String>> readFile3 = AsyncReadFileWords(fileName3);

    await Task.WhenAll(readFile1, readFile2, readFile3);

    List<String> words1 = await readFile1;
    List<String> words2 = await readFile2;
    List<String> words3 = await readFile3;

    SortWords(words1 , words2, words3);
}

static List<String> SortWords(List<String> words1, List<String> words2, List<String> words3)
{
    List<String> newList = new List<String>();
    List<String> _tempList = new List<String>();

    for (int i  = 0; i < words1.Count && i < words2.Count && i < words3.Count; ++i)
    {
        // create local variable for all words in INDEX 0
        // each iteration we compare the 3 local variables and then
        // delete them from each list.
        _tempList.Add(words1[0]);
        _tempList.Add(words2[0]);
        _tempList.Add(words3[0]);

        // TODO: Check if this is actually ASC
        _tempList.Sort();

        // because we iterate only through the length of the shortest,
        // we want to add all three words each iteration

        foreach (String word in _tempList) {
            if (newList.Count > 0 &&
                !newList[newList.Count-1].Equals(word))
                newList.Add(word);
        }

        //Now delete each of the words from the total lists
        words1.RemoveAt(0);
        words2.RemoveAt(0);
        words3.RemoveAt(0);

        _tempList = new List<string>();
    }

    // Now we need to worry about which lists still contain content.
    // there's def a better way than hard coding this but that's for later


    return null;
}
*/




class ReadingFiles
{
    static void Main(String[] args)
    {
        Console.WriteLine(args[0]);

        Char delim = ' ';

        ConcurrentDictionary<String, int> words = new ConcurrentDictionary<string, int>();
        String fileName1 = args[0], fileName2 = args[1], fileName3 = args[2];
        Task readFile1 = Task.Run(() => AsyncReadFileWords(fileName1, words, delim));
        Task readFile2 = Task.Run(() => AsyncReadFileWords(fileName2, words, delim));
        Task readFile3 = Task.Run(() => AsyncReadFileWords(fileName3, words, delim));

        Task.WaitAll(readFile1, readFile2, readFile3);

        Dictionary<String, int> wordsAlpha;
        wordsAlpha = words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        
        foreach (KeyValuePair<String, int> pair in wordsAlpha)
        {
            Console.Write(pair.Key + "-> " + pair.Value + '\n');
        }
        wordsAlpha = words.OrderBy(x => x.Key.ToLower()).ToDictionary(x => x.Key, x => x.Value);

        WriteWordsToFile("file4.txt", wordsAlpha.Keys.ToList());
        Console.WriteLine("The file F4 has been created.");

        Console.WriteLine("-------");
        Console.ReadLine();
    }

    static void WriteWordsToFile(String filename, List<String> wordList)
    {
        using (StreamWriter sw = new StreamWriter(filename)) {
            foreach (String word in wordList)
            {
                sw.WriteLine(word);
            }
        }
    }

    static void AsyncReadFileWords(String filename, ConcurrentDictionary<String, int> words, Char delim)
    {
        if (!File.Exists(filename))
            return;

        using (StreamReader sr = new StreamReader(filename))
        {
            // want to read not line by line but word by word
            // sep by "space" for now
            while (!sr.EndOfStream)
            {
                String word = GetNextWord(sr, delim);

                if (word != String.Empty)
                {
                    words.AddOrUpdate(word, 1, (s, i) => i + 1);
                }

            }
        }
    }

    static String GetNextWord(StreamReader sr, Char delim)
    {
        String word = "";

        char c;
        do
        {
            c = Convert.ToChar(sr.Read());

            // is the character an alpha character
            if ((c >= 65 && c <= 90) ||
                (c >= 97 && c <= 122))
                word += c;
        } while (!sr.EndOfStream && c != delim);
        return word;

    }
}



