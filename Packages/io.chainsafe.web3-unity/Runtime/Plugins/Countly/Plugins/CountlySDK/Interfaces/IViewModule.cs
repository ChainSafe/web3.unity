using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IViewModule
{
    /// <summary>
    /// Manually starts a view with the given name.
    /// It can be used to open multiple views in parallel.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <returns>ViewId</returns>
    string StartView(string viewName);

    /// <summary>
    /// Manually starts a view with the given name.
    /// It can be used to open multiple views in parallel.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
    /// <returns>ViewId</returns>
    string StartView(string viewName, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Manually starts a view with the given name. Starting any other view or calling this again, 
    /// closes the one that's opened automatically. <br/>
    /// This ensures a 'one view at a time' flow.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <returns>ViewId</returns>
    string StartAutoStoppedView(string viewName);

    /// <summary>
    /// Manually starts a view with the given name. Starting any other view or calling this again, 
    /// closes the one that's opened automatically. <br/>
    /// This ensures a 'one view at a time' flow.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
    /// <returns>ViewId</returns>
    string StartAutoStoppedView(string viewName, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Stops a view with the given name if it is open
    /// If multiple views with same name are open, last opened view will be closed.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    void StopViewWithName(string viewName);

    /// <summary>
    /// Stops a view with the given name if it is open
    /// If multiple views with same name are open, last opened view will be closed.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
    void StopViewWithName(string viewName, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Stops a view with the given ID if it is open
    /// </summary>
    /// <param name="viewID">ID of the view</param>
    void StopViewWithID(string viewID);

    /// <summary>
    /// Stops a view with the given ID if it is open
    /// </summary>
    /// <param name="viewID">ID of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view, set 'null' if none should be added</param>
    void StopViewWithID(string viewID, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Pauses a view with the given ID
    /// </summary>
    /// <param name="viewID">ID of the view</param>
    void PauseViewWithID(string viewID);

    /// <summary>
    /// Resumes a view with the given ID
    /// </summary>
    /// <param name="viewID">ID of the view</param>
    void ResumeViewWithID(string viewID);

    /// <summary>
    /// Stops all views and records a segmentation if set
    /// </summary>
    /// <param name="viewSegmentation">Segmentation that will be added, set 'null' if none should be added</param>
    void StopAllViews(Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Set a segmentation to be recorded with all views
    /// </summary>
    /// <param name="viewSegmentation">Global View Segmentation</param>
    void SetGlobalViewSegmentation(Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Updates the segmentation of a view with view id.
    /// </summary>
    /// <param name="viewID">ID of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
    void AddSegmentationToViewWithID(string viewID, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Updates the segmentation of a view with view name.
    /// If multiple views with same name are open, last opened view will be updated.
    /// </summary>
    /// <param name="viewName">Name of the view</param>
    /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
    void AddSegmentationToViewWithName(string viewName, Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Updates the global segmentation
    /// </summary>
    /// <param name="viewSegmentation">Segmentation that will be added to the view</param>
    void UpdateGlobalViewSegmentation(Dictionary<string, object> viewSegmentation);

    /// <summary>
    /// Records the opening of a view. This method is deprecated.
    /// </summary>
    /// <remarks>
    /// This method must be used in conjunction with <see cref="RecordCloseViewAsync"/>.
    /// Do not use with <see cref="StopView"/> as it will not function correctly.
    /// Please use <see cref="StartView"/> and <see cref="StopView"/> for new implementations.
    /// </remarks>
    /// <param name="viewName">The name of the view to open.</param>
    /// <param name="viewSegmentation">Optional segmentation data for the view.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Obsolete("RecordOpenViewAsync(string name, IDictionary<string, object> segmentation = null) is deprecated and will be removed in the future. Please use StartView instead.")]
    Task RecordOpenViewAsync(string name, IDictionary<string, object> segmentation = null);

    /// <summary>
    /// Records the closing of a view. This method is deprecated.
    /// </summary>
    /// <remarks>
    /// This method should only be used to close views that were opened using <see cref="RecordOpenViewAsync"/>.
    /// Do not use to close views started with <see cref="StartView"/>.
    /// </remarks>
    /// <param name="viewName">The name of the view to close.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Obsolete("RecordCloseViewAsync(string name) is deprecated and will be removed in the future. Please use StopView instead.")]
    Task RecordCloseViewAsync(string name);

    /// <summary>
    /// Reports a particular action with the specified details.
    /// </summary>
    /// <remarks>
    /// <para>This method is deprecated and will be removed in a future release. There is no direct replacement for this method.</para>
    /// <para>Consider re-evaluating the need for this functionality or implementing a custom solution as needed.</para>
    /// </remarks>
    /// <param name="type">The type of action.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="width">The width of the screen.</param>
    /// <param name="height">The height of the screen.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Obsolete("ReportActionAsync(string type, int x, int y, int width, int height) is deprecated and will be removed in the future.")]
    Task ReportActionAsync(string type, int x, int y, int width, int height);
}