﻿Imports Rhino
Imports Rhino.Geometry
Imports Rhino.Commands
Imports Rhino.Input

Namespace examples_vb
  <System.Runtime.InteropServices.Guid("A7E8558B-5F4E-426C-9352-BEFFA645AC33")> _
  Public Class BrepFromCurveBBoxCommand
    Inherits Command
    Public Overrides ReadOnly Property EnglishName() As String
      Get
        Return "vbBrepFromCurveBBox"
      End Get
    End Property

    Protected Overrides Function RunCommand(doc As RhinoDoc, mode As RunMode) As Result
      Dim objref As Rhino.DocObjects.ObjRef
      Dim rc = RhinoGet.GetOneObject("Select Curve", False, Rhino.DocObjects.ObjectType.Curve, objref)
      If rc <> Result.Success Then
        Return rc
      End If
      Dim curve = objref.Curve()

      Dim view = doc.Views.ActiveView
      Dim plane = view.ActiveViewport.ConstructionPlane()
      ' Create a construction plane aligned bounding box
      Dim bbox = curve.GetBoundingBox(plane)

      If bbox.IsDegenerate(doc.ModelAbsoluteTolerance) > 0 Then
        RhinoApp.WriteLine("the curve's bounding box is degenerate (flat) in at least one direction so a box cannot be created.")
        Return Result.Failure
      End If
      Dim brep__1 = Brep.CreateFromBox(bbox)
      doc.Objects.AddBrep(brep__1)
      doc.Views.Redraw()
      Return Result.Success
    End Function
  End Class
End Namespace