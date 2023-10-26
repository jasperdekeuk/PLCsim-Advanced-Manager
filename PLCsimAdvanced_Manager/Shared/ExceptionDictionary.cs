using MudBlazor;
using Siemens.Simatic.Simulation.Runtime;

namespace PLCsimAdvanced_Manager.Shared;

public class ExceptionDictionary
{
    ISnackbar Snackbar { get; }
    public ExceptionDictionary(ISnackbar snackbar)
    {
        Snackbar = snackbar;
    }
    
    private readonly Dictionary<ERuntimeErrorCode, string> exceptionMessages = new Dictionary<ERuntimeErrorCode, string>()
    {
        {ERuntimeErrorCode.InterfaceRemoved, "The interface is disconnected from the remote Runtime Manager"},
        {ERuntimeErrorCode.ConnectionError, "Unable to establish a connection to the Runtime Manager OR The port cannot be opened OR Connection to the remote Runtime Manager cannot be established."},
        {ERuntimeErrorCode.Timeout, "The function does not return on time."},
        {ERuntimeErrorCode.MessageCorrupt, ""},
        {ERuntimeErrorCode.WrongVersion, "The version of the API is not compatible with Runtime."},
        {ERuntimeErrorCode.InstanceNotRunning, "The process of the virtual controller is not running."},
        {ERuntimeErrorCode.SharedMemoryNotInitialized, ""},
        {ERuntimeErrorCode.ApiNotInitialized, ""},
        {ERuntimeErrorCode.NotSupported, "Access to entire structures or arrays is not supported"},
        {ERuntimeErrorCode.ErrorLoadingDll, ""},
        {ERuntimeErrorCode.SignalNameDoesNotExist, ""},
        {ERuntimeErrorCode.SignalTypeMismatch, ""},
        {ERuntimeErrorCode.SignalConfigurationError, ""},
        {ERuntimeErrorCode.NoSignalConfigurationLoaded, ""},
        {ERuntimeErrorCode.ConfiguredConnectionNotFound, ""},
        {ERuntimeErrorCode.ConfiguredDeviceNotFound, ""},
        {ERuntimeErrorCode.InvalidConfiguration, ""},
        {ERuntimeErrorCode.TypeMismatch, ""},
        {ERuntimeErrorCode.LicenseNotFound, "Test mode has expired"},
        {ERuntimeErrorCode.NoLicenseAvailable, ""},
        {ERuntimeErrorCode.WrongCommunicationInterface, ""},
        {ERuntimeErrorCode.LimitReached, "There are already 16 instances registered in Runtime Manager"},
        {ERuntimeErrorCode.NoStoragePathSet, "The path could not be created. The length of the DSTORAGE_PATH_MAX_LENGTH characters might be exceeded."},
        {ERuntimeErrorCode.StoragePathAlreadyInUse, "The selected path for this instance is already being used by another instance."},
        {ERuntimeErrorCode.MessageIncomplete, ""},
        {ERuntimeErrorCode.ArchiveStorageNotCreated, "The ZIP file could not be created."},
        {ERuntimeErrorCode.RetrieveStorageFailure, "The ZIP file cannot be unzipped."},
        {ERuntimeErrorCode.InvalidOperatingState, "The instance is not in OFF operating state."},
        {ERuntimeErrorCode.InvalidArchivePath, "The archive path is invalid"},
        {ERuntimeErrorCode.DeleteExistingStorageFailed, "The old storage cannot be deleted."},
        {ERuntimeErrorCode.CreateDirectoriesFailed, ""},
        {ERuntimeErrorCode.NotEnoughMemory, ""},
        {ERuntimeErrorCode.NotRunning, ""},
        {ERuntimeErrorCode.NotEmpty, "Warning: No valid license for PLCSIM Advanced is available, but a \"TIA Portal Test Suite\" license. If this is the case, power-up from the Virtual SIMATIC Memory Card is not supported"},
        {ERuntimeErrorCode.NotUpToDate, "The stored tag list must be updated."},
        {ERuntimeErrorCode.CommunicationInterfaceNotAvailable, "A problem has occurred with the selected communication interface. Check your settings."},
        {ERuntimeErrorCode.VirtualSwitchMisconfigured, " The S7-PLCSIM Advanced Virtual Switch is configured incorrectly. OR  The binding of an S7-PLCSIM Advanced Virtual Switch on a PC network interface is not correct, or the driver of an S7-PLCSIM Advanced Virtual Switch is not functioning properly. If the binding is not correct, call the SetNetInterfaceBindings() function or set the binding manually before starting the instance."},
        {ERuntimeErrorCode.RuntimeNotAvailable, "No Runtime Manager runs in this Windows user session"},
        {ERuntimeErrorCode.IsEmpty, ""},
        {ERuntimeErrorCode.WrongModuleState, "The module is currently unplugged"},
        {ERuntimeErrorCode.WrongModuleType, ""},
        {ERuntimeErrorCode.NotSupportedByModule, "The module is not supported by this user action."},
        {ERuntimeErrorCode.InternalError, "The system call has caused an internal error. Enable the traces to obtain detailed information."},
        {ERuntimeErrorCode.StorageTransferError, "Error during network data transfer. Memory data between client and server computers do not match."},
        {ERuntimeErrorCode.AnotherVariantOfPlcsimRunning, ""},
        {ERuntimeErrorCode.AccessDenied, "No authorized"},
        {ERuntimeErrorCode.NotAllowedDuringDownload, ""},
        {ERuntimeErrorCode.PCAPDriverNotRunning, "he Nmap Packet Capture Driver (Npcap) is not active on the system. Remedy: 1. Start the command line in administrator mode. 2. Execute the command \"net start npcap\"."},
        {ERuntimeErrorCode.UnsupportedPcapDriver, "The PCAP driver used is not supported. PLCSIM Advanced supports Npcap as of V0.9995. TCP/IP communication is not possible."},
        {ERuntimeErrorCode.ConfigFileError, ""},
        {ERuntimeErrorCode.BufferOverflow, ""},
        {ERuntimeErrorCode.NotAllowed, "In Single Adapter Network Mode (promiscuous mode), it is not permitted to change the binding while an instance is running."}
    };
    
    private string GetExceptionMessage(ERuntimeErrorCode errorCode)
    {
        return exceptionMessages.TryGetValue(errorCode, out var message)
            ? message
            : "An unknown error occurred.";
    }
    
    public void Execute(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            var errorCode = (ERuntimeErrorCode) e.HResult;
            var message = GetExceptionMessage(errorCode);
            Snackbar.Add(message, Severity.Error, config => { config.HideIcon = true; });
        }
    }
}