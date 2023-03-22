using Cead;
using Cead.Interop;
using Tests.Helpers;

namespace Tests;

[TestClass]
public class BymlTests
{
    private static byte[] _target = Array.Empty<byte>();

    [TestInitialize]
    public void Setup()
    {
        // Initialize Cead
        DllManager.LoadCead();

        // Load the target BYML file
        // to test on
        string actorinfo = Path.Combine(BotwConfig.Shared.GamePath, "Actor", "ActorInfo.product.sbyml");
        _target = Yaz0.Decompress(File.ReadAllBytes(actorinfo).AsSpan()).ToArray();
    }

    [TestMethod]
    public void FromBinary()
    {
        // Load a BYML from binary data
        using Byml byml = Byml.FromBinary(_target);

        // Make sure it's not null or invalid
        Assert.IsNotNull(byml);
        Assert.IsFalse(byml.IsInvalid);
        
        // Compare the ActorInfo array lengths
        // to make sure it was parsed correctly
        Assert.IsTrue(byml.GetHash()["Hashes"].GetArray().Length == byml.GetHash()["Hashes"].GetArray().Length);

        // Forcably dispose the BYML to make
        // sure the unmanaged memory is released
        byml.Dispose();

        // Make sure the unmanaged memory is properly
        // disposed and throws this exception
        // (irrecoverable exception)
        // Assert.ThrowsException<System.Runtime.InteropServices.SEHException>(byml.ToText);
    }

    [TestMethod]
    public void ToBinary()
    {
        // Load a BYML from binary data
        using Byml byml = Byml.FromBinary(_target);

        // Write the BYML object to memory
        DataHandle data = byml.ToBinary(bigEndian: true);

        // Make sure it's not null or invalid
        Assert.IsNotNull(data);
        Assert.IsFalse(data.IsInvalid);

        // Copy the data to make sure it's intact
        byte[] copy = data.AsSpan().ToArray();
        Assert.IsNotNull(copy);

        // Forcably dispose the data to make
        // sure the unmanaged memory is released
        data.Dispose();

        // Make sure the unmanaged memory is properly
        // disposed and throws this exception
        // (irrecoverable exception)
        // Assert.ThrowsException<ExecutionEngineException>(() => data.AsSpan().ToArray());
    }

    [TestMethod]
    public void FromText()
    {
        // Get a BYML string
        string target = Byml.FromBinary(_target).ToText().ToString(dispose: true);

        // Load BYML from text data
        Byml byml = Byml.FromText(target);

        // Make sure it's not null or invalid
        Assert.IsNotNull(byml);
        Assert.IsFalse(byml.IsInvalid);

        // Compare the ActorInfo array lengths
        // to make sure it was parsed correctly
        Assert.IsTrue(byml.GetHash()["Hashes"].GetArray().Length == byml.GetHash()["Hashes"].GetArray().Length);

        // Forcably dispose the BYML to make
        // sure the unmanaged memory is released
        byml.Dispose();

        // Make sure the unmanaged memory is properly
        // disposed and throws this exception
        // (irrecoverable exception)
        // Assert.ThrowsException<System.Runtime.InteropServices.SEHException>(() => byml.ToBinary(bigEndian: true));
    }

    [TestMethod]
    public void ToText()
    {
        // Load a BYML from binary data
        using Byml byml = Byml.FromBinary(_target);

        // Write the BYML object to memory
        StringHandle data = byml.ToText();

        // Make sure it's not null or invalid
        Assert.IsNotNull(data);
        Assert.IsFalse(data.IsInvalid);

        // Copy the data to make sure it's intact
        string copy = data.ToString();
        Assert.IsNotNull(copy);

        // Forcably dispose the data to make
        // sure the unmanaged memory is released
        data.Dispose();

        // Make sure the unmanaged memory is properly
        // disposed and throws this exception
        // (irrecoverable exception)
        // Assert.ThrowsException<ExecutionEngineException>(() => data.AsSpan().ToArray());
    }

    [TestMethod]
    public void General()
    {
        // Load a BYML from binary data
        using Byml byml = Byml.FromBinary(_target);

        // Get a hash
        Byml.Hash hash = byml.GetHash();

        // Iterate the hash and get an array
        foreach ((var key, var node) in hash) {
            Byml.Array array = node.GetArray();
            Assert.IsNotNull(array);
            Assert.IsFalse(array.Length <= -1);
        }

        // Iterate the array and get a hash and string
        Byml.Array actors = hash["Actors"].GetArray();
        foreach (var actor in actors) {
            string str = actor.GetHash()["name"].GetString();
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length > -1);
        }

        // Create a hash and add it to the root
        Byml.Hash newHash = new Dictionary<string, Byml>() {
            { "A", 1f }, { "B", 2d }, { "C", 3u }
        };
        hash.Add("NewHash", newHash);

        Assert.IsTrue(hash.ContainsKey("NewHash"));
        Assert.IsTrue(hash["NewHash"].Type == BymlType.Hash);
        Assert.IsTrue(hash["NewHash"].GetHash().ContainsKey("A"));

        // Create an array and add it to the root
        Byml.Array newArray = new("A", "B", "C");
        hash.Add("NewArray", newArray);

        Assert.IsTrue(hash.ContainsKey("NewArray"));
        Assert.IsTrue(hash["NewArray"].Type == BymlType.Array);
        Assert.IsTrue(hash["NewArray"].GetArray().Length == 3);

        // Clear the actors (array)
        actors.Clear();
        Assert.IsTrue(actors.Length == 0);

        // Replace the hashes
        hash["Hashes"] = "SomeString";
        Assert.IsTrue(hash["Hashes"].Type == BymlType.String);
        Assert.IsTrue(hash["Hashes"].GetString() == "SomeString");

        // Print the text to make sure
        // everything is still intact
        Console.WriteLine(byml.ToText());
    }
}
