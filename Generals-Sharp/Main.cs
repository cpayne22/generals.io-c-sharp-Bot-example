using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

namespace Generals_Sharp
{
    public class Main
    {
        public event EventHandler OnDisconnect;
        public event EventHandler OnLog;

        private Socket socket;
        string user_id = "my_example_bot_id";
        string username = "Example Bot";

        int TILE_EMPTY = -1;
        int TILE_MOUNTAIN = -2;
        int TILE_FOG = -3;
        int TILE_FOG_OBSTACLE = -4; // Cities and Mountains show up as Obstacles in the fog of war.

        // Game data.
        int playerIndex = -1;
        int[] generals = new int[] { }; // The indicies of generals we have vision of.
        int[] cities = new int[0]; // The indicies of cities we have vision of.
        int[] map = new int[] { };

        public Main()
        {
            socket = IO.Socket("http://botws.generals.io");
            socket.On("disconnect", (data) =>
            {
                OnDisconnect?.Invoke(this, null);
            });

            socket.On("game_update", (d) =>
            {
                // Writing the output to file.  Still can't quite figure out WTF is going on...
               // System.IO.File.WriteAllText("output2.txt", d.ToString());
                var data = JsonConvert.DeserializeObject<GameUpdate>(d.ToString());

                cities = patch(cities, data.cities_diff);
                map = patch(map, data.map_diff);
                generals = data.generals;

                // The first two terms in |map| are the dimensions.
                var width = map[0];
                var height = map[1];
                var size = width * height;

                // The next |size| terms are army values.
                // armies[0] is the top-left corner of the map.

                var armies = map.Skip(2).Take(size);

                // The last |size| terms are terrain values.
                // terrain[0] is the top-left corner of the map.
                var terrain = map.Skip(size + 2).Take(size + 2 + size).ToArray();

                var rnd = new Random();
                // Make a random move.
                while (true)
                {
                    // Pick a random tile.
                    var index = (int)Math.Floor(rnd.NextDouble() * size);

                    // If we own this tile, make a random move starting from it.
                    if (terrain[index] == playerIndex)
                    {
                        var row = Math.Floor(Convert.ToDecimal(index / width));
                        var col = index % width;
                        var endIndex = index;

                        var rand = rnd.NextDouble();
                        if (rand < 0.25 && col > 0)
                        { // left
                            endIndex--;
                        }
                        else if (rand < 0.5 && col < width - 1)
                        { // right
                            endIndex++;
                        }
                        else if (rand < 0.75 && row < height - 1)
                        { // down
                            endIndex += width;
                        }
                        else if (row > 0)
                        { //up
                            endIndex -= width;
                        }
                        else
                        {
                            continue;
                        }

                        // Would we be attacking a city? Don't attack cities.
                        if (cities[endIndex] >= 0)
                        {
                            continue;
                        }

                        socket.Emit("attack", index, endIndex);
                        break;
                    }
                }
            });
        }

        public void Initialise()
        {

            // Set the username for the bot.
            socket.Emit("set_username", user_id, username);
            OnLog?.Invoke(this, new Logging { Message = "Set Username" });

            // Join a custom game and force start immediately.
            // Custom games are a great way to test your bot while you develop it because you can play against your bot!
            var custom_game_id = "blister_bot_training";
            socket.Emit("join_private", custom_game_id, user_id);
            socket.Emit("set_force_start", custom_game_id, true);

            OnLog?.Invoke(this, new Logging { Message = "Joined custom game at http://bot.generals.io/games/" + System.Net.WebUtility.UrlEncode(custom_game_id) });

        }

        public int[] patch(int[] old, int[] diff)
        {
            var ret = new List<int>();
            var i = 0;
            while (i < diff.Length)
            {
                if (diff[i] > 0)
                {  // matching        
                    //Array.prototype.push.apply(out, old.slice(out.length, out.length + diff[i]));            
                    ret.AddRange(old.Skip(ret.Count()).Take(diff[i]));
                }
                i++;
                if (i < diff.Length)
                {  // mismatching
                   // Array.prototype.push.apply(out, diff.slice(i + 1, i + 1 + diff[i]));

                    ret.AddRange(diff.Skip(i + 1).Take(diff[i]));
                    i += diff[i];
                }
                i++;
            }
            return ret.ToArray();
        }
    }

    public class Logging : EventArgs
    {
        public string Message { get; set; }
    }
}
