using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TableBorderCharacter
	{
		/// <summary>
		/// SingleUp_SingleDown
		/// </summary>
		SU_SD,
		/// <summary>
		/// SingleUp_SingleLeft
		/// </summary>
		SU_SL,
		/// <summary>
		/// SingleLeft_SingleRight
		/// </summary>
		SL_SR,
		/// <summary>
		/// DoubleLeft_SingleDown
		/// </summary>
		DL_SD,
		/// <summary>
		/// DoubleRight_DoubleDown
		/// </summary>
		DR_DD,
		/// <summary>
		/// DoubleLeft_DoubleRight
		/// </summary>
		DL_DR,
		/// <summary>
		/// DoubleLeft_DoubleRight_DoubleDown
		/// </summary>
		DL_DR_DD,
		/// <summary>
		/// DoubleLeft_DoubleRight_SingleDown
		/// </summary>
		DL_DR_SD,
		/// <summary>
		/// DoubleUp_DoubleDown
		/// </summary>
		DU_DD,
		/// <summary>
		/// DoubleUp_DoubleRight_DoubleDown
		/// </summary>
		DU_DR_DD,
		/// <summary>
		/// DoubleUp_DoubleLeft_DoubleRight_DoubleDown
		/// </summary>
		DU_DL_DR_DD,
		/// <summary>
		/// SingleUp_DoubleLeft_DoubleRight_SingleDown
		/// </summary>
		SU_DL_DR_SD,
		/// <summary>
		/// SingleUp_DoubleLeft_SingleDown
		/// </summary>
		SU_DL_SD,
		/// <summary>
		/// DoubleUp_SingleRight_DoubleDown
		/// </summary>
		DU_SR_DD,
		/// <summary>
		/// DoubleUp_SingleLeft_SingleRight_DoubleDown
		/// </summary>
		DU_SL_SR_DD,
		/// <summary>
		/// SingleUp_SingleLeft_SingleRight_SingleDown
		/// </summary>
		SU_SL_SR_SD,
		/// <summary>
		/// SingleUp_SingleLeft_SingleDown
		/// </summary>
		SU_SL_SD,
		/// <summary>
		/// DoubleUp_SingleRight
		/// </summary>
		DU_SR,
		/// <summary>
		/// DoubleUp_SingleLeft_SingleRight
		/// </summary>
		DU_SL_SR,
		/// <summary>
		/// SingleUp_SingleLeft_SingleRight
		/// </summary>
		SU_SL_SR
	}
}
