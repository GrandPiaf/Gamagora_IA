using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;
using FANNCSharp.Float;

namespace NN_Languages
{
    public class Program
    {
        static void Main(string[] args) {

            //XorExample();
            LanguageNN();
        }

        private static void LanguageNN() {

            string trainingFilename = "languages.data";
            string resultFilename = "languages_float.net";

            // Connectivity & layers
            const float connectivity = 1f;
            const int num_layers = 3;
            const int num_input = 26;
            const int num_output = 5;
            const int num_neurons_hidden = 14;

            // Training variables
            const int max_epochs = 500000;
            const int epochs_betwen_reports = 1000;
            const float desired_error = 0.00001f;

            /** CREATING DATA **/
            {
                List<Dictionary<Language, string>> dicoList = new List<Dictionary<Language, string>>(2);

                string fr_1 = @"On ne connaît que les choses que l'on apprivoise, dit le renard. Les hommes n'ont plus le temps de rien connaître. Ils achètent des choses toutes faites chez les marchands. Mais comme il n'existe point de marchands d'amis, les hommes n'ont plus d'amis. Si tu veux un ami, apprivoise-moi!";
                string de_1 = @"Man kennt die Dinge, die man zähmt, sagte der Fuchs. Die Menschen haben keine Zeit mehr, etwas kennen zu lernen. Sie kaufen die Dinge fix und fertig bei den Händlern. Aber weil niemand mit Freunden handelt, haben die Menschen keine Freunde mehr. Wenn Du einen Freund willst, zähme mich!";
                string it_1 = @"Puoi conoscere solo quello che ti è familiare, che ti è domestico - disse la volpe. - Gli uomini non hanno più tempo di conoscere niente. Comprano nei negozi cose già pronte. Ma siccome non esistono negozianti di amici, gli uomini non hanno più amici. Tu, allora, se vuoi un amico, vedi di addomesticarmi.";
                string sp_1 = @"Sólo conocemos las cosas que domesticamos, dijo el zorro. Los hombres ya no tienen tiempo de conocer nada. Compran en las tiendas cosas hechas. Pero como no hay ninguna tienda donde vendan amigos, los hombres ya no tienen amigos. Si quieres un amigo, !domestícame!";
                string en_1 = @"We only know the things that we tame, said the fox. People no longer have the time to know anything. They buy things already made for peddlers. But since there are no peddlers of friends, they no longer have friends. If you want a friend, tame me!";

                Dictionary<Language, string> dico1 = new Dictionary<Language, string>();
                dico1.Add(Language.French, fr_1);
                dico1.Add(Language.Deutch, de_1);
                dico1.Add(Language.Italian, it_1);
                dico1.Add(Language.Spanish, sp_1);
                dico1.Add(Language.English, en_1);

                dicoList.Add(dico1);



                string fr_2 = @"Adieu, dit le renard. Voici mon secret. Il est très simple: on ne voit bien qu'avec le coeur. L'essentiel est invisible pour les yeux.";
                string de_2 = @"Adieu!, sagte der Fuchs. Dies ist mein Geheimnis. Es ist sehr einfach: man sieht nur mit dem Herzen gut. Das Wesentliche ist für die Augen unsichtbar.";
                string it_2 = @"Addio, disse la volpe. Ed eccolo qui, il mio segreto - semplice semplice. Vedere è cosa del cuore. Agli occhi l'essenziale resta sempre invisibile.";
                string sp_2 = @"Adiós, dijo el zorro. Te diré un secreto. Es muy sencillo: sólo se ve bien con el corazón. Lo esencial les resulta invisible a los ojos.";
                string en_2 = @"Goodbye, said the fox. Here's my secret; it's very simple: we see well only with the heart. The essential is invisible to the eyes.";

                Dictionary<Language, string> dico2 = new Dictionary<Language, string>();
                dico2.Add(Language.French, fr_2);
                dico2.Add(Language.Deutch, de_2);
                dico2.Add(Language.Italian, it_2);
                dico2.Add(Language.Spanish, sp_2);
                dico2.Add(Language.English, en_2);
                
                dicoList.Add(dico2);



                string fr_3 = @"Tous les êtres humains naissent libres et égaux en dignité et en droits. Ils sont doués de raison et de conscience et doivent agir les uns envers les autres dans un esprit de fraternité.";
                string de_3 = @"Alle Menschen sind frei und gleich an Würde und Rechten geboren. Sie sind mit Vernunft und Gewissen begabt und sollen einander im Geist der Brüderlichkeit begegnen.";
                string it_3 = @"Tutti gli esseri umani nascono liberi ed eguali in dignità e diritti. Essi sono dotati di ragione e di coscienza e devono agire gli uni verso gli altri in spirito di fratellanza.";
                string sp_3 = @"Todos los seres humanos nacen libres e iguales en dignidad y derechos y, dotados como están de razón y conciencia, deben comportarse fraternalmente los unos con los otros.";
                string en_3 = @"All human beings are born free and equal in dignity and rights. They are endowed with reason and conscience and should act towards one another in a spirit of brotherhood.";

                Dictionary<Language, string> dico3 = new Dictionary<Language, string>();
                dico3.Add(Language.French, fr_3);
                dico3.Add(Language.Deutch, de_3);
                dico3.Add(Language.Italian, it_3);
                dico3.Add(Language.Spanish, sp_3);
                dico3.Add(Language.English, en_3);

                dicoList.Add(dico3);
                


                string fr_4 = @"Moscou est la capitale de la Russie et compte environ douze millions six cent mille habitants intra muros (2017) sur une superficie de 2 510 km2 ce qui en fait la ville la plus peuplée à la fois du pays et d'Europe. Sur le plan administratif Moscou fait partie du district fédéral central et a le statut de ville d'importance fédérale qui lui donne le même niveau d'autonomie que les autres sujets de la Russie.";
                string de_4 = @"Moskau ist die Hauptstadt der Russischen Föderation. Mit rund 12,4 Millionen Einwohnern (Stand 2017)[1] ist sie die größte Stadt sowie mit 15,1 Millionen Einwohnern (2012)[2] die größte Agglomeration Europas. Moskau ist das politische, wirtschaftliche, wissenschaftliche und kulturelle Zentrum Russlands, mit Universitäten und Instituten sowie zahlreichen Kirchen, Theatern, Museen und Galerien. Im Stadtgebiet befinden sich einige der höchsten europäischen Hochhäuser und die markanten Sieben Schwestern, sowie der 540 Meter hohe Ostankino-Turm, das höchste Bauwerk Europas.";
                string it_4 = @"Mosca è la capitale, la città più popolosa nonché il principale centro economico e finanziario della Russia. Sorge sulle sponde del fiume Moscova e occupa un'area di 2561,50 km², e con più di 12 milioni di abitanti (18 milioni considerando l'area metropolitana), oltre ad essere la città più popolosa del paese, è la seconda città d'Europa per popolazione e superficie dopo Istanbul.";
                string sp_4 = @"Moscú es la capital y la entidad federal más poblada de Rusia. La ciudad es un importante centro político, económico, cultural y científico de Rusia y del continente. Moscú es la megaciudad más septentrional de la Tierra, la segunda ciudad de Europa en población después de Estambul,3​4​5​ y la sexta del mundo.";
                string en_4 = @"Moscow is the capital and largest city of Russia. The city stands on the Moskva River in Central Russia, with a population estimated at 12.4 million residents within the city limits,[12] while over 17 million residents in the urban area,[13] and over 20 million residents in the Moscow Metropolitan Area.";

                Dictionary<Language, string> dico4 = new Dictionary<Language, string>();
                dico4.Add(Language.French, fr_4);
                dico4.Add(Language.Deutch, de_4);
                dico4.Add(Language.Italian, it_4);
                dico4.Add(Language.Spanish, sp_4);
                dico4.Add(Language.English, en_4);

                dicoList.Add(dico4);



                string fr_5 = @"Souvlaki est le deuxième album du groupe anglais Slowdive, sorti le 17 mai 1993 par le label Creation Records. L'album a une bonne réception critique à sa sortie1. Brian Eno collabore sur deux pistes, Sing et Here She Comes. Cet album montre un tournant dans la carrière du groupe et projette aussi les futurs projets musicaux des membres Neil Halstead (chant et guitare) et Rachel Goswell (chant)[réf. nécessaire]. L'album bénéficie d'une réédition en 20052,3. ";
                string de_5 = @"Souvlaki ist das zweite von vier Studioalben der britischen Rockband Slowdive und erschien im Mai 1993 auf dem namhaften Label Creation Records. Das Album zählt zu den Klassikern des Shoegazing. ";
                string it_5 = @"Souvlaki è il secondo album pubblicato dal gruppo shoegaze degli Slowdive. È considerato l'album della maturità del gruppo, i brani sono più strutturati[1] ma meno sperimentali rispetto all'esordio. In due brani del disco è presente Brian Eno. Dall'album è stato tratto il singolo Alison. ";
                string sp_5 = @"Souvlaki es el segundo álbum de estudio de la banda de shoegaze inglesa Slowdive. Grabado en 1992, fue lanzado el 17 de mayo de 1993 en el Reino Unido y el 8 de febrero de 1994 en Estados Unidos. El álbum fue el segundo de sus trabajos en incorporar elementos de shoegaze y dream pop, antes de cambiar de dirección a un estilo más atmosférico y post-rock en su tercer álbum de estudio, Pygmalion.";
                string en_5 = @"Souvlaki is the second studio album by English rock band Slowdive. Recorded in 1992, it was released in the United Kingdom on 1 June 1993 by record label Creation,[5] then on 8 February 1994 in the United States by SBK. ";

                Dictionary<Language, string> dico5 = new Dictionary<Language, string>();
                dico5.Add(Language.French, fr_5);
                dico5.Add(Language.Deutch, de_5);
                dico5.Add(Language.Italian, it_5);
                dico5.Add(Language.Spanish, sp_5);
                dico5.Add(Language.English, en_5);

                dicoList.Add(dico5);



                string fr_6 = @"Flickr, est un site web de partage de photographies et de vidéos gratuit, avec certaines fonctionnalités payantes. En plus d'être un site web populaire auprès des utilisateurs pour partager leurs photos personnelles, il est aussi souvent utilisé par des photographes professionnels. En août 2011, le site a franchi la barre des six milliards de photos hébergées. En février 2017, le site héberge approximativement treize milliards de photos pour cent-vingt deux millions de membres et deux millions de groupes1,2. ";
                string de_6 = @"Flickr ist ein kommerzieller Onlinedienst mit Community-Elementen, der es Benutzern erlaubt, digitale und digitalisierte Bilder sowie kurze Videos von maximal drei Minuten Dauer mit Kommentaren und Notizen auf die Website zu laden und so anderen Nutzern zugänglich zu machen (zu „teilen“). Neben dem herkömmlichen Hochladen über die Website können die Bilder auch per E-Mail oder vom Mobiltelefon aus übertragen und später von anderen Webauftritten aus verlinkt werden. ";
                string it_6 = @"Flickr è stato sviluppato dalla Ludicorp, una compagnia canadese di Vancouver fondata nel 2002 da Stewart Butterfield e Caterina Fake. Il termine viene dall'inglese flicker, che significa tremolare, o sfavillare. Nel marzo del 2005, sia la Ludicorp che Flickr sono stati comprati da Yahoo!: i server, quindi, sono stati trasferiti dal Canada agli Stati Uniti. Il 16 maggio 2006 Flickr conclude la fase di sviluppo beta, definendosi in stato gamma, ossia non più in fase di prova ma in stato di perpetua evoluzione. ";
                string sp_6 = @"Flickr es un sitio web que permite almacenar, ordenar, buscar, vender2​ y compartir fotografías o videos en línea, a través de Internet. Cuenta con una comunidad de usuarios que comparten fotografías y videos creados por ellos mismos. Esta comunidad se rige por normas de comportamiento y condiciones de uso que favorecen la buena gestión de los contenidos. ";
                string en_6 = @"Flickr is an American image hosting and video hosting service, as well as an online community. It was created by Ludicorp in 2004 and has been popular with hosting high resolution photos by amateur and professional photographers.[4][5] It has changed ownership several times and has been owned by SmugMug since April 20, 2018.[6] ";

                Dictionary<Language, string> dico6 = new Dictionary<Language, string>();
                dico6.Add(Language.French, fr_6);
                dico6.Add(Language.Deutch, de_6);
                dico6.Add(Language.Italian, it_6);
                dico6.Add(Language.Spanish, sp_6);
                dico6.Add(Language.English, en_6);

                dicoList.Add(dico6);






                int nb_examples = 0;
                foreach (Dictionary<Language, string> dico in dicoList) {
                    nb_examples += dico.Count;
                }

                using (StreamWriter sw = new StreamWriter(trainingFilename)) {
                    sw.WriteLine(nb_examples.ToString() + " " + num_input.ToString() + " " + num_output.ToString());

                    var format = new NumberFormatInfo();
                    format.NegativeSign = "-";
                    format.NumberDecimalSeparator = ".";
                    format.NumberDecimalDigits = 4;

                    foreach (Dictionary<Language, string> dico in dicoList) {
                        foreach (KeyValuePair<Language, string> entry in dico) {

                            float[] frequencies = GetFrequenciesFromText(entry.Value);

                            string toWrite = "";

                            foreach (float freq in frequencies) {
                                toWrite += freq.ToString(format) + " ";
                            }

                            sw.WriteLine(toWrite);
                            sw.WriteLine(GetExpectedResult(entry.Key));

                        }
                    }
                }




            }

            ///** TRAINING **/
            {
                // Creating NN
                Console.WriteLine("Creating Neural Network");
                NeuralNet net = new NeuralNet(connectivity, num_layers, num_input, num_neurons_hidden, num_output);

                net.ActivationFunctionHidden = ActivationFunction.SIGMOID_SYMMETRIC;
                net.ActivationFunctionOutput = ActivationFunction.SIGMOID_SYMMETRIC;

                Console.WriteLine("Training NN on : " + trainingFilename);
                net.TrainOnFile(trainingFilename, max_epochs, epochs_betwen_reports, desired_error);

                Console.WriteLine("Saving NN on : " + resultFilename);
                net.Save(resultFilename);
            }

            Console.WriteLine("Training complete. Press ENTER to run test.");
            Console.ReadLine();


            ///** RUNNING TEST **/
            {
                NeuralNet nnUse = new NeuralNet(resultFilename);

                string testText = @"L’ autorité repose d’ abord sur la raison. Si tu ordonnes à ton peuple d’ aller se jeter à la mer, il fera la révolution. J’ ai le droit d’ exiger l’ obéissance parce que mes ordres sont raisonnables.";

                Console.WriteLine("Testing text : ");
                Console.WriteLine(" ** " + testText + " ** ");

                float[] input = GetFrequenciesFromText(testText);

                float[] results = nnUse.Run(input);

                Console.WriteLine("French : " + results[0]);
                Console.WriteLine("Deutch : " + results[1]);
                Console.WriteLine("Italian : " + results[2]);
                Console.WriteLine("Spanish : " + results[3]);
                Console.WriteLine("English : " + results[4]);
            }

            Console.ReadLine();

            Console.WriteLine("Press ENTER to run second test.");
            Console.ReadLine();


            ///** RUNNING TEST **/
            {
                NeuralNet nnUse = new NeuralNet(resultFilename);

                string testText = @"Provoquée par le règlement insatisfaisant de la Première Guerre mondiale et par les ambitions expansionnistes et hégémoniques des trois principales nations de l’Axe.";

                Console.WriteLine("Testing text : ");
                Console.WriteLine(" ** " + testText + " ** ");

                float[] input = GetFrequenciesFromText(testText);

                float[] results = nnUse.Run(input);

                Console.WriteLine("French : " + results[0]);
                Console.WriteLine("Deutch : " + results[1]);
                Console.WriteLine("Italian : " + results[2]);
                Console.WriteLine("Spanish : " + results[3]);
                Console.WriteLine("English : " + results[4]);
            }

            Console.ReadLine();
        }

        private static string GetExpectedResult(Language key) {
            switch (key) {
                case Language.French:
                    return "1 0 0 0 0";
                case Language.Deutch:
                    return "0 1 0 0 0";
                case Language.Italian:
                    return "0 0 1 0 0";
                case Language.Spanish:
                    return "0 0 0 1 0";
                case Language.English:
                    return "0 0 0 0 1";
            }
            return "0 0 0 0 0";
        }

        private static float[] GetFrequenciesFromText(string text) {
            float[] frequencies = new float[26];

            int count = 0;

            foreach (char c in text) {
                char tempC = char.ToUpper(c); //ToUpper to test on capital letters only

                if (tempC < 65 || tempC > 90) { //Not a 'common' letter, skipping it
                    continue;
                }

                frequencies[tempC - 65] += 1;
                count++;
            }

            for (int i = 0; i < frequencies.Length; i++) {
                frequencies[i] /= count;
            }

            return frequencies;
        }

        private static void XorExample() {
            string trainingFilename = "xor.data";
            string resultFilename = "xor_float.net";

            /** TRAINING **/
            {
                // Connectivity & layers
                const float connectivity = 1f;
                const int num_layers = 3;
                const int num_input = 2;
                const int num_output = 1;
                const int num_neurons_hidden = 3;

                // Training variables
                const int max_epochs = 500000;
                const int epochs_betwen_reports = 1000;
                const float desired_error = 0.0001f;


                // Creating NN
                Console.WriteLine("Creating Neural Network");
                NeuralNet net = new NeuralNet(connectivity, num_layers, num_input, num_neurons_hidden, num_output);

                net.ActivationFunctionHidden = ActivationFunction.SIGMOID_SYMMETRIC;
                net.ActivationFunctionOutput = ActivationFunction.SIGMOID_SYMMETRIC;

                Console.WriteLine("Training NN on : " + trainingFilename);
                net.TrainOnFile(trainingFilename, max_epochs, epochs_betwen_reports, desired_error);

                Console.WriteLine("Saving NN on : " + resultFilename);
                net.Save(resultFilename);
            }

            Console.WriteLine("Training complete. Press ENTER to run test.");
            Console.ReadLine();


            /** RUNNING TEST **/
            {
                NeuralNet nnUse = new NeuralNet(resultFilename);

                float[] input = new float[2];
                input[0] = -1;
                input[1] = 1;

                float[] results = nnUse.Run(input);

                Console.WriteLine("xor test (" + input[0] + "," + input[1] + ") -> " + results[0]);
            }

            Console.ReadLine();
        }

    }

    enum Language {
        French,
        Deutch,
        Italian,
        Spanish,
        English
    }

}
