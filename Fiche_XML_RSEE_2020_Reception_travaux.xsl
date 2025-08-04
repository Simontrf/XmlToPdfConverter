<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:import href="RSEE_general.xsl" />
	<xsl:import href="RSEnv_template.xsl" />
	<xsl:variable name="p1" select="/projet/RSET/Entree_Projet/Batiment_Collection/Batiment"/>
	<xsl:variable name="p_bc" select="/projet/Datas_Comp/batiment_collection"/>
	<xsl:variable name="p_env1" select="/projet/RSEnv"/>
	<xsl:variable name="p_env2" select="/projet/Datas_Comp"/>
	<xsl:decimal-format name="fr" decimal-separator="," grouping-separator="&#160;" NaN="--" />
	<xsl:output method="html" media-type="text/html" omit-xml-declaration="yes" indent="yes" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN"/>
	<xsl:param name="depot"/>
	<xsl:param name="pdf"/>	
	<xsl:strip-space elements="*"/>
	<xsl:template match="/">
		<html>
			<head>
				<title>RSEE/RSENV - <xsl:value-of select="/projet/Datas_Comp/donnees_generales/operation/nom"/></title>
				<xsl:choose>
					<xsl:when test="$pdf=1"><link rel="stylesheet" type="text/css" href="css/re2020_pdf.css"/></xsl:when>
					<xsl:otherwise><link rel="stylesheet" type="text/css" href="css/re2020.css"/></xsl:otherwise>
				</xsl:choose>
			</head>
			<body id="top">
				<div id="global">
				<div id="entete">
					<table border="0" class="encadre_rt2012" width="100%">
						<tr>
							<td id="logo"><img src="images/logo.png" alt="Logo RE2020" width="300" height="285"/></td>
							<td>
									<h1>Réglementation Environnementale 2020</h1>
									Récapitulatif Standardisé Energie Environnement<br/><br/>
									<h2>Partie « Etude Environnementale »</h2>
								<ul>
									<li><strong>Opération :</strong>&#160;<xsl:value-of select="/projet/Datas_Comp/donnees_generales/operation/nom"/></li>
									<li><strong>Etude thermique du :</strong>&#160;<xsl:value-of select="format-number(substring(/projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/date_etude, 9, 2), '00')"/><xsl:text>/</xsl:text><xsl:value-of select="format-number(substring(/projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/date_etude, 6, 2), '00')"/><xsl:text>/</xsl:text><xsl:value-of select="format-number(substring(/projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/date_etude, 1, 4), '0000')"/></li>
									<li><strong>Logiciel et version :</strong>&#160;<xsl:value-of select="/projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/editeur"/>,&#160;<xsl:value-of select="projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/nom"/>,&#160;<xsl:value-of select="/projet/Datas_Comp/donnees_generales/logiciel[@ref &gt; 1]/version"/></li>
									<li><strong>Version RSEnv :</strong>&#160;<xsl:value-of select="$p_env1/@version"/></li>
									<p><strong>Date de génération du RSEnv : </strong><xsl:value-of select="$depot"/></p>
								</ul>
							</td>
									
						</tr>
					</table>
				</div>
					<div id="document">		
						<div id="sommaire">
							<h2><span>Sommaire</span></h2>
							<ul>
								<li><strong>Chapitre 1</strong> : <a href="#chap1">Données générales de l'opération</a> (<em>"<xsl:value-of select="$p_env2/donnees_generales/operation/nom"/>"</em>)</li>
							</ul>
							<ul>
								<li><strong>Chapitre 2</strong> : Exigences de performance environnementales<xsl:if test="$pdf != 1"> - <xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap2-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
							</ul>
							<ul>
								<li><strong>Chapitre 3</strong> : Données techniques
								<ul>
									<li>Données techniques générale<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap31-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Données techniques, niveau parcelle<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap32-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Données techniques, niveau bâtiment et zone<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap33-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
								</ul>
							</li>
							<li><strong>Chapitre 4.1</strong> : Quantitatifs saisis, niveau zone, par bâtiment
								<ul>
									<li>Contribution : Composant<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap411-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Contribution : Consommation d'énergie<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap412-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Contribution : Consommation et rejet d'eau<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap413-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Contribution : Chantier<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap414-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
								</ul>
							</li>
							<li><strong>Chapitre 4.2</strong> : Quantitatifs saisis, niveau parcelle
								<xsl:choose>
								<xsl:when test="count(/projet/RSEnv/entree_projet/parcelle/contributeur/*) &gt; 0">
									<ul>
										<xsl:if test="/projet/RSEnv/entree_projet/parcelle/contributeur/composant"><li>Contribution : Composant -&#160;<a href="#chap421">Parcelle</a></li></xsl:if>
										<xsl:if test="/projet/RSEnv/entree_projet/parcelle/contributeur/eau"><li>Contribution : Consommation et rejet d'eau -&#160;<a href="#chap422">Parcelle</a></li></xsl:if>
										<xsl:if test="/projet/RSEnv/entree_projet/parcelle/contributeur/chantier"><li>Contribution : Chantier -&#160;<a href="#chap423">Parcelle</a></li></xsl:if>
									</ul>
								
								</xsl:when>
								<xsl:otherwise><ul><li><i><font color="gray">Pas de données disponibles</font></i></li></ul></xsl:otherwise>
								</xsl:choose>
							</li>
							<li><strong>Chapitre 5</strong> : Sorties de l'analyse de cycle de vie environnementale (ACV), <b>niveau bâtiment</b>
								<ul>
									<li>Indicateurs réglementaires et pédagogiques de performance environnementale du bâtiment<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap51-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Répartition inter et intra-contributeurs de l'indicateur « Stockage Carbone »<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap52-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Indicateur CO<sub>2</sub> dynamique<xsl:if test="$pdf != 1"> -<xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap53-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
									<li>Indicateurs environnementaux statiques<xsl:if test="$pdf != 1"> - <xsl:for-each select="$p_env1/entree_projet/batiment">&#160;<a href="#chap54-{index}">Bât.<xsl:value-of select="index"/></a></xsl:for-each></xsl:if></li>
										
									<li><b>Contributions :</b>
										<ul>
											<xsl:if test="/projet/RSEnv/sortie_projet/batiment/contributeur/composant"><li>Composant<xsl:if test="$pdf != 1"> -&#160;<xsl:for-each select="$p_env1/entree_projet/batiment"><a href="#chap55-{index}">Bât.<xsl:value-of select="index"/></a>&#160;</xsl:for-each></xsl:if></li></xsl:if>
											<xsl:if test="/projet/RSEnv/sortie_projet/batiment/contributeur/composant"><li>Energie<xsl:if test="$pdf != 1"> -&#160;<xsl:for-each select="$p_env1/entree_projet/batiment"><a href="#chap56-{index}">Bât.<xsl:value-of select="index"/></a>&#160;</xsl:for-each></xsl:if></li></xsl:if>
											<xsl:if test="/projet/RSEnv/sortie_projet/batiment/contributeur/eau"><li>Consommation et rejet d'eau<xsl:if test="$pdf != 1"> -&#160;<xsl:for-each select="$p_env1/entree_projet/batiment"><a href="#chap57-{index}">Bât.<xsl:value-of select="index"/></a>&#160;</xsl:for-each></xsl:if></li></xsl:if>
											<xsl:if test="/projet/RSEnv/sortie_projet/batiment/contributeur/chantier"><li>Chantier<xsl:if test="$pdf != 1"> -&#160;<xsl:for-each select="$p_env1/entree_projet/batiment"><a href="#chap58-{index}">Bât.<xsl:value-of select="index"/></a>&#160;</xsl:for-each></xsl:if></li></xsl:if>
										</ul>
									</li>
								</ul>
							</li>
							<li><strong>Chapitre 6</strong> : Sorties de l'analyse de cycle de vie environnementale (ACV), <b>niveau zones de bâtiment</b>
								<ul>
									<xsl:for-each select="$p_env1/entree_projet/batiment">
									<li>
										<b>Bâtiment <xsl:value-of select="index"/></b> :
										<ul>
											<xsl:for-each select="zone">
											<li><b>Indicateurs principaux</b>, à l'échelle de la zone : <a href="#chap6-{../index}{index}">Zone <xsl:value-of select="index"/></a>&#160;</li>
											<li><b>Zone <xsl:value-of select="index"/> / Contributions :</b>
												<ul>
													<xsl:if test="/projet/RSEnv/sortie_projet/batiment/zone/contributeur/composant"><li style="display:inline;"><a href="#chap61-{../index}{index}">Composant</a></li> - </xsl:if>
													<xsl:if test="/projet/RSEnv/sortie_projet/batiment/zone/contributeur/composant"><li style="display:inline;"><a href="#chap62-{../index}{index}">Energie</a></li> - </xsl:if>
													<xsl:if test="/projet/RSEnv/sortie_projet/batiment/zone/contributeur/eau"><li style="display:inline;"><a href="#chap63-{../index}{index}">Consommation et rejet d'eau</a></li> - </xsl:if>
													<xsl:if test="/projet/RSEnv/sortie_projet/batiment/zone/contributeur/chantier"><li style="display:inline;"><a href="#chap64-{../index}{index}">Chantier</a></li></xsl:if>
												</ul>
											</li>
											</xsl:for-each>
										</ul>
									</li>
									</xsl:for-each>
								</ul>
							</li>
							<li><strong>Chapitre 7</strong> : Sorties de l'analyse de cycle de vie environnementale (ACV), <b>niveau parcelle</b>
								<ul>
									<li>Indicateur CO<sub>2</sub> dynamique -&#160;<a href="#chap73">Parcelle</a></li>
									<li>Indicateurs environnementaux statiques -&#160;<a href="#chap74">Parcelle</a></li>
									<li>Indicateurs principaux, à l'échelle de la parcelle, par Contribution :&#160;<a href="#chap75">Composant</a> - <a href="#chap76">Eau</a> - <a href="#chap77">Chantier</a></li>
								</ul>
							</li>
						</ul>						
			</div>
			<div id="b1"></div>
			<xsl:for-each select="/projet">	
				<div id="chap">
					<h2 id="chap1"><span>Chapitre 1 :</span> Données générales de l'opération</h2>
					<xsl:apply-templates select="." mode="donnees_admin_projet"/>
				</div>
			</xsl:for-each>											
			<xsl:for-each select="$p_env1/entree_projet/batiment">
				<xsl:variable name="id_bat" select="index"/>
				<div id="chap">
					<a name="chap2-{index}"/><h2 id="chap2-{entree_projet/batiment/index}"><span>Chapitre 2 :</span> Exigences de performance environnementale</h2>
					<xsl:apply-templates select="." mode="exi_env"/>
					<a name="chap31-{index}"/><h2 id="chap21-{entree_projet/batiment/index}"><span>Chapitre 3 :</span> Données techniques</h2>
					<xsl:apply-templates select="." mode="donnees_generales_ope"/>
					<a name="chap32-{index}"/>
					<xsl:apply-templates select="." mode="donnees_generales_parc"/>			
					<a name="chap33-{index}"/>
					<xsl:apply-templates select="." mode="donnees_generales_bat"/>
				</div>	
		 	
				<br/>
				<div id="chap">
			 		<h2 id="chap41"><span>Chapitre 4.1 :</span> Quantitatifs saisis, par zone, niveau bâtiment (<span class="bat"><xsl:value-of select="nom"/></span>)</h2>
					<b>Période de référence du calcul ACV : 50 ans</b><br/><br/>
					<xsl:apply-templates select="." mode="quantitatifs"/>
				</div>
				<br/>
				<div id="chap">
			 		<h2 id="chap42"><span>Chapitre 4.2 :</span> Quantitatifs saisis, niveau parcelle</h2>
					<xsl:choose>
						<xsl:when test="count(/projet/RSEnv/entree_projet/parcelle/contributeur/*) &gt; 0">
							<b>Période de référence du calcul ACV : 50 ans</b><br/><br/>
							<xsl:apply-templates select="." mode="quantitatifs_parcelle"/>
						</xsl:when>
						<xsl:otherwise><ul><li><i><font color="gray">Pas de données disponibles</font></i></li></ul></xsl:otherwise>
					</xsl:choose>									
				</div>
				
			</xsl:for-each>	
	<xsl:for-each select="$p_env1/entree_projet/batiment">
				<xsl:variable name="id_bat" select="index"/>
				<br/>
				<div id="chap">
					<h2 id="chap51-{index}">Chapitre 5 : Sorties de l'analyse de cycle de vie environnementale (ACV), niveau bâtiment</h2>
					
					<!-- Indicateurs principaux, à l'échelle du bâtiment -->
					<a name="chap51-{index}"/><xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="S_acv_bat"/>
					<!-- Répartition inter et intra-contributeurs de l'indicateur « Stockage Carbone » à l'échelle du bâtiment -->
					<a name="chap52-{index}"/><xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="S_acv_bat_composant"/>
					
					<a name="chap53-{index}"/><xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_dyn_bat_new"/>
					<a name="chap54-{index}"/><xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat"/>
					<a name="chap55-{index}"/><h3>Contribution Bât. <xsl:value-of select="$id_bat"/> : Composant</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="contrib_bat_composant"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_dyn_bat_composant"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_composant"/>
					<a name="chap551-{index}"/><h4>Contribution Bât. <xsl:value-of select="$id_bat"/> : Composant / <b>lots</b></h4>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_composant_lot"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stock_udd_co2"/>
					<a name="chap552-{index}"/><h4>Contribution Bât. <xsl:value-of select="$id_bat"/> : Composant / <b>sous-lots</b></h4>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_composant_sous_lot"/>
					<a name="chap56-{index}"/><h3>Contribution Bât. <xsl:value-of select="$id_bat"/> : Energie</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_dyn_bat_energie"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_energie"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_sous_contrib_energie"/>
					<a name="chap57-{index}"/><h3>Contribution Bât. <xsl:value-of select="$id_bat"/> : Consommation et rejet d'eau</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_dyn_bat_eau"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_eau"/>
 					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_sous_contrib_eau"/> 
					<a name="chap58-{index}"/><h3>Contribution Bât. <xsl:value-of select="$id_bat"/> : Chantier</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_dyn_bat_chantier"/>
					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_stat_bat_chantier"/>
 					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]" mode="ind_sous_contrib_chantier"/> 
				</div>
			</xsl:for-each>
	<xsl:for-each select="$p_env1/entree_projet/batiment">
				<xsl:variable name="id_bat" select="index"/>
				<br/>
				<div id="chap">
					<h2>Chapitre 6 : Sortie de l'analyse de cycle de vie environnementale (ACV), niveau des zones de bâtiments</h2>
					<h4>Bâtiment <xsl:value-of select="index"/> - <xsl:value-of select="nom"/> - <xsl:value-of select="sref"/> m²</h4>
					<xsl:for-each select="zone">
					<xsl:variable name="id_zone" select="index"/>
						<h5 id="chap6-{../index}{index}">Zone : <b><xsl:value-of select="index"/> - <xsl:value-of select="sref"/> m²</b></h5>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="acv_zone"/> 
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_dyn_zone"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone"/>
						<a name="chap61-{../index}{index}"/><h3>Contribution Zone <xsl:value-of select="index"/> (<xsl:value-of select="sref"/> m²) : Composant</h3>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="contrib_zone_composant"/>
	 					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_dyn_zone_composant"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_composant"/>
		
						<a name="chap611-{../index}{index}"/><h4>Contribution Zone <xsl:value-of select="index"/> (<xsl:value-of select="sref"/> m²) : Composant / <b>lots</b></h4>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_composant_lot"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stock_zone_udd_co2"/>
						<a name="chap612-{../index}{index}"/><h4>Contribution Zone <xsl:value-of select="index"/> (<xsl:value-of select="sref"/> m²) : Composant / <b>sous-lots</b></h4>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_composant_sous_lot"/>
						<a name="chap62-{../index}{index}"/><h3>Contribution Zone <xsl:value-of select="$id_bat"/> (<xsl:value-of select="sref"/> m²) : Energie</h3>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_dyn_zone_energie"/>
	 					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_energie"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_sous_contrib_zone_energie"/>
						<a name="chap63-{../index}{index}"/><h3>Contribution Zone <xsl:value-of select="$id_bat"/> (<xsl:value-of select="sref"/> m²) : Consommation et rejet d'eau</h3>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_dyn_zone_eau"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_eau"/>
	 	 				<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_sous_contrib_zone_eau"/>
						<a name="chap64-{../index}{index}"/><h3>Contribution Zone <xsl:value-of select="$id_bat"/> (<xsl:value-of select="sref"/> m²) : Chantier</h3>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_dyn_zone_chantier"/>
						<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_stat_zone_chantier"/>
	 					<xsl:apply-templates select="$p_env1/sortie_projet/batiment[index=$id_bat]/zone[index=$id_zone]" mode="ind_sous_contrib_zone_chantier"/> 
					</xsl:for-each>		
				</div>
	</xsl:for-each>
	<br/>
	<div id="chap">
				<h2 id="chap7"><span>Chapitre 7 :</span> Sorties de l'analyse de cycle de vie environnementale (ACV), niveau parcelle</h2>
				<a name="chap73"/><xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_dyn_parcelle"/>
				<a name="chap74"/><xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_stat_parcelle"/>
			
				<!-- Indicateurs statiques et dynamiques, à l'échelle de la parcelle, contributeur « composant » -->
				<xsl:if test="$p_env1/sortie_projet/parcelle/contributeur/composant">
					<a name="chap75"/><h3>Contribution : Composant</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet" mode="contrib_parcelle_composant"/>
					<xsl:choose>
					<xsl:when test="sum($p_env1/sortie_projet/parcelle/contributeur/composant/indicateurs_acv_collection/valeur_phase_acv) > 0">
						<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_stat_parcelle_composant"/>
					</xsl:when>
					<xsl:otherwise><i>Indicateurs environnementaux statiques, à l'échelle de la parcelle, contribution "Composant" : <b>Pas de données disponibles</b></i><br/><br/></xsl:otherwise>
					</xsl:choose>
					<xsl:choose>
						<xsl:when test="sum($p_env1/sortie_projet/parcelle/contributeur/composant/indicateur_co2_dynamique/valeur_phase_acv) > 0">
							<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_dyn_parcelle_composant"/>
						</xsl:when>
						<xsl:otherwise><i>Indicateur CO<sub>2</sub> dynamique, à l'échelle de la parcelle, contribution "Composant" : <b>Pas de données disponibles</b></i><br/><br/></xsl:otherwise>
					</xsl:choose>
				</xsl:if>
				
				<xsl:if test="$p_env1/sortie_projet/parcelle/contributeur/eau">
					<a name="chap76"/><h3>Contribution : Consommation et rejet d'eau</h3>
					<xsl:if test="sum($p_env1/sortie_projet/parcelle/contributeur/eau/indicateurs_acv_collection/valeur_phase_acv) > 0">
						<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_stat_parcelle_eau"/>
					</xsl:if>
					<xsl:if test="sum($p_env1/sortie_projet/parcelle/contributeur/eau/indicateur_co2_dynamique/valeur_phase_acv) > 0">
						<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_dyn_parcelle_eau"/>
					</xsl:if>
				</xsl:if>
				
				
				<xsl:if test="$p_env1/sortie_projet/parcelle/contributeur/chantier">
					<a name="chap77"/><h3>Contribution : Chantier</h3>
					<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_stat_parcelle_chantier"/>
					<xsl:apply-templates select="$p_env1/sortie_projet" mode="ind_dyn_parcelle_chantier"/>				
				</xsl:if>
	</div>
			

		</div>
	</div>		
</body>
</html>
</xsl:template>
<!-- ******* Chapitre 1 : Données administratives de l'opération ****** -->
<!-- Exigences de performance énergétique -->
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="exi_env">
	<xsl:variable name="id_bat" select="index"/>	
		<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h4" width="100%">
			<thead>
				<tr>
					<th>Respect des exigences de l'arrêté pour le bâtiment</th>
					<th>Conformité à la RE2020</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td>La valeur de l'indicateur Ic<sub>énergie</sub> du bâtiment est inférieure ou égale à la valeur maximale Ic<sub>énergie_max</sub></td>
					<td align="center">
						<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/indicateur_perf_env">
							<xsl:choose>
								<xsl:when test="ic_energie &gt; ic_energie_max">
									<span class="ko">Non conforme</span>
								</xsl:when>
								<xsl:otherwise>
									<span class="ok">Conforme</span>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</td>
				</tr>
				<tr>
					<td>La valeur de l'indicateur Ic<sub>construction</sub> du bâtiment est inférieure ou égale respectivement à la valeur maximale Ic<sub>construction_max</sub> </td>
					<td align="center">
						<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/indicateur_perf_env">
							<xsl:choose>
								<xsl:when test="ic_construction &gt; ic_construction_max">
									<span class="ko">Non conforme</span>
								</xsl:when>
								<xsl:otherwise>
									<span class="ok">Conforme</span>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</td>
				</tr>				
			</tbody>
		</table>
	</xsl:template>

<!-- donnees_generales du BATIMENT -->
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="donnees_generales_ope">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<th class="h7" align="left" colspan="2">Données techniques générales</th>
			</tr>	
		</thead>
		<tbody>
			<tr>
				<th width="30%">Version du RSEnv</th>
				<td align="center" width="70%"><xsl:value-of select="/projet/RSEnv/@version"/></td>
			</tr>				
			<tr>
				<th>Phase de cycle de vie de l'étude</th>
				<td align="center">
						<xsl:choose>
							<xsl:when test="/projet/RSEnv/@phase=1">Programmation</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=2">Esquisse</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=3">APS</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=4">APD</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=5">PRO DCE</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=6">EXE</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=7">Construction</xsl:when>
							<xsl:when test="/projet/RSEnv/@phase=8">Exploitation</xsl:when>
						</xsl:choose>
				</td>
			</tr>				
			<tr>
				<th>Nombre de bâtiments</th><td align="center"><xsl:value-of select="count(/projet/RSEnv/entree_projet/batiment)"/></td>
			</tr>
			<xsl:if test="/projet/RSEnv/entree_projet/reseau/type=1">
				<tr>
					<th>Nom du réseau de chaleur urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=1]/nom"/></td>
				</tr>
				<tr>
					<th>Identifiant du réseau de chaleur urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=1]/identifiant"/></td>
				</tr>
				<tr>
					<th>Localisation du réseau de chaleur urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=1]/localisation"/></td>
				</tr>
				<tr>
					<th>Contenu CO<sub>2</sub> du réseau de chaleur urbain [kgeq.CO<sub>2</sub>/kWhEF]</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=1]/contenu_co2"/></td>
				</tr>				
			</xsl:if>
			<xsl:if test="/projet/RSEnv/entree_projet/reseau/type=2">
				<tr>
					<th>Nom du réseau de froid urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=2]/nom"/></td>
				</tr>
				<tr>
					<th>Identifiant du réseau de froid urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=2]/identifiant"/></td>
				</tr>				
				<tr>
					<th>Localisation du réseau de froid urbain</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=2]/localisation"/></td>
				</tr>
				<tr>
					<th>Contenu CO<sub>2</sub> du réseau de froid urbain [kgeq.CO<sub>2</sub>/kWhEF]</th><td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/reseau[type=2]/contenu_co2"/></td>
				</tr>				
			</xsl:if>			
	</tbody>
	</table>
</xsl:template>

<!-- données techniques niveau parcelle -->
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="donnees_generales_parc">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<th class="h7" align="left" colspan="2">Données techniques, niveau parcelle</th>
			</tr>	
		</thead>
		<tbody>
				
			<tr>
				<th width="30%">Surface parcelle [m²]</th>
				<td align="center" width="70%"><xsl:value-of select="/projet/RSEnv/entree_projet/parcelle/surface_parcelle"/></td>
			</tr>				
			<tr>
				<th>Surface arrosée [m²]</th>
				<td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/parcelle/surface_arrosee"/></td>
			</tr>	
			<xsl:if test="/projet/RSEnv/entree_projet/parcelle/surface_veg">
			<tr>
				<th>Surface végétalisée [m²]</th>
				<td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/parcelle/surface_veg"/></td>
			</tr>	
			</xsl:if>
			<xsl:if test="/projet/RSEnv/entree_projet/parcelle/surface_imper">
			<tr>
				<th>Surface imperméabilisée [m²]</th>
				<td align="center"><xsl:value-of select="/projet/RSEnv/entree_projet/parcelle/surface_imper"/></td>
			</tr>	
			</xsl:if>									
	</tbody>
	</table>
</xsl:template>
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="donnees_generales_bat">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<th class="h7" align="left" colspan="3">Données techniques, niveau bâtiment et zone</th>
			</tr>	
		</thead>
		<tbody>
		<xsl:for-each select="/projet/RSEnv/entree_projet/batiment">
		<xsl:variable name="id_bat" select="index"/>	
			<tr>
				<th width="30%">Nom du bâtiment</th><td width="70%" align="center"  colspan="2"><b><xsl:value-of select="nom"/></b></td>
			</tr>
			<tr>
				<th>Commentaires libres</th>
				<td align="center" colspan="2">
					<xsl:choose>
						<xsl:when test="commentaires">
							<xsl:value-of select="commentaires"/>
						</xsl:when>
						<xsl:otherwise>-</xsl:otherwise>
					</xsl:choose>		
				</td>
			</tr>
			<tr>
				<th>Surface de Référence [m<sup>2</sup>]</th><td align="center" colspan="2"><xsl:value-of select="format-number(sref, '#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<th>Emprise au sol [m<sup>2</sup>]</th><td align="center" colspan="2"><xsl:value-of select="format-number(emprise_au_sol, '#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<th>Période de référence [an]</th><td align="center" colspan="2"><xsl:value-of select="periode_reference"/></td>
			</tr>
			<tr>
				<th>Durée de chantier [mois]</th><td align="center" colspan="2"><xsl:value-of select="duree_chantier"/></td>
			</tr>
			<tr>
				<th>Nombre de place de parking (en infrastructure)</th><td align="center" colspan="2"><xsl:value-of select="/projet/Datas_Comp/batiment_collection/batiment[Index=$id_bat]/nb_place_parking_infra"/></td>
			</tr>
			<tr>
				<th>Nombre de place de parking (en superstructure)</th><td align="center" colspan="2"><xsl:value-of select="/projet/Datas_Comp/batiment_collection/batiment[Index=$id_bat]/nb_place_parking_supra"/></td>
			</tr>
			<tr>
				<th>Nombre de place de parking (en extérieur)</th><td align="center" colspan="2"><xsl:value-of select="/projet/Datas_Comp/batiment_collection/batiment[Index=$id_bat]/nb_place_parking_ext"/></td>
			</tr>
			<xsl:for-each select="zone">
				<tr>
					<th align="right" rowspan="7"><font size="4em">ZONE&#160;<xsl:value-of select="index"/></font></th>
				</tr>
				<tr>
					<th class="h8" align="left">Usage</th>
					<td align="center"><b><xsl:call-template name="usage_re2020"/></b></td>
				</tr>
				<tr>	
					<th class="h8" align="left">Surface de référence [m<sup>2</sup>]</th>
					<td align="center"><xsl:value-of select="format-number(sref, '#&#160;##0,##', 'fr')"/></td>
				</tr>		
				<tr>	
					<th class="h8" align="left">Surface de plancher des combles aménagés dont la hauteur sous plafond est inférieure à 1.8 [m<sup>2</sup>]</th>
					<td align="center"><xsl:value-of select="format-number(scombles, '#&#160;##0,##', 'fr')"/></td>
				</tr>
				<tr>	
					<th class="h8" align="left">Nombre d'occupants</th>
					<td align="center">
						<xsl:value-of select="n_occ"/>&#160;
					</td>
				</tr>	
				<tr>	
					<th class="h8" align="left">Nombre de logement</th>
					<td align="center">
						<xsl:value-of select="nb_logement"/>
					</td>	
				</tr>				
</xsl:for-each>
		</xsl:for-each>
</tbody>
</table>
</xsl:template>
<!-- Quantitatifs par parcelle -->
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="quantitatifs_parcelle">
	<xsl:variable name="id_bat" select="index"/>
<!-- si composant -->
<xsl:if test="../parcelle/contributeur/composant">
	<a name="chap421"/><h3>Contribution : Composant</h3>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
			<thead>
				<tr class="b">
					<th width="5%">Base</th>
					<th width="3%" class="center">Identifiant<br/>fiche</th>
					<th width="10%" class="center">Type de données</th>
					<th width="25%" class="left">Nom</th>
					<th width="24%" class="left">Commentaire</th>
					<th width="10%" class="center">Unité de l'UF</th>
					<th width="5%" class="center">Quantité</th>
					<th width="5%" class="center">DVE<br/>[an]</th>
					<th width="5%" class="center">Perf. UF Fille</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="/projet/RSEnv/entree_projet/parcelle/contributeur/composant/donnees_composant">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center">
							<xsl:choose>
								<xsl:when test="type_donnees=7"><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="center"><xsl:call-template name="type_donnees"/></td>
						<td><xsl:value-of select="translate(substring(nom, 1, 300),'_',' ')"/><xsl:if test="string-length(nom) &gt; 300"> [...]</xsl:if></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<td align="center"><xsl:value-of select="dve"/></td>
						<td align="right"><xsl:value-of select="perf_uf_fille"/></td>
					</tr>
				</xsl:for-each>
			</tbody>
		</table>
		<!-- seulement si le type_donnees=7 -->
		<xsl:if test="/projet/RSEnv/entree_projet/parcelle/contributeur/composant/donnees_composant[type_donnees=7]">
			<h5>Données saisies (complément d'information sur les données issues de configurateurs de fiches environnementales)</h5>
			<xsl:for-each select="/projet/RSEnv/entree_projet/parcelle/contributeur/composant/donnees_composant[type_donnees=7]">
				<xsl:if test="donnees_configurateur">
					<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h8" width="100%">
						<thead>
							<tr>
								<th colspan="4" class="h8" align="center">
									<xsl:call-template name="base"/>&#160;<xsl:if test="version_configurateur">(<xsl:value-of select="version_configurateur"/>)</xsl:if> : <xsl:value-of select="nom"/>
								</th>
							</tr>	
							<tr class="b">
								<th width="20%">Identifiant fiche configurée</th>
								<td width="30%" align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
								<th width="25%">Identifiant fiche mère</th>
								<td width="25%" align="center"><xsl:choose><xsl:when test="id_fiche_mere !=''"><xsl:value-of select="translate(id_fiche_mere,'_',' ')"/></xsl:when><xsl:otherwise>-</xsl:otherwise></xsl:choose></td>
							</tr>
						</thead>
						<tbody>
							<tr class="b">
								<td width="30%">Quantité</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="quantite"/></td>
							</tr>
							<tr class="b">
								<td width="30%">Unité</td>
								<td width="70%" colspan="3" align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
							</tr>
							<tr class="b">
								<td width="30%">DVE (années)</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="dve"/></td>
							</tr>
							<tr class="b">
								<td width="30%">Commentaire</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
							</tr>
							<xsl:if test="donnees_configurateur">
								<tr class="b">
									<th>N° Paramètre</th>
									<th>Nom</th>
									<th>Valeur</th>
									<th>Unité</th>
								</tr>
								<xsl:for-each select="donnees_configurateur/parametre">
									<tr class="b">
										<th><xsl:value-of select="@numero"/></th>
										<td><xsl:value-of select="nom"/></td>
										<td align="center"><xsl:value-of select="valeur"/></td>
										<td align="center">
											<xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template>
										</td>
									</tr>
								</xsl:for-each>
							</xsl:if>
						</tbody>
					</table>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
</xsl:if>
<!-- pas de composant energie -->
<xsl:if test="../parcelle/contributeur/eau">
<a name="chap422"/><h3>Contribution : Consommation et rejet d'eau</h3>
<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
			<thead>
				<tr class="b">
					<th width="5%">Base</th>
					<th width="3%" class="center">Identifiant<br/>fiche</th>
					<th width="15%" class="left">Nom</th>
					<th width="18%" class="left">Commentaire</th>
					<th width="10%" class="center">Unité de l'UF</th>
					<th width="5%" class="center">Quantité</th>
					<th width="5%" class="center">DVE<br/>[an]</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="/projet/RSEnv/entree_projet/parcelle/contributeur/eau/donnees_service">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center">
							<xsl:choose>
								<xsl:when test="type_donnees=7"><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:otherwise>
							</xsl:choose>
						</td>
						<td><xsl:value-of select="translate(substring(nom, 1, 300),'_',' ')"/><xsl:if test="string-length(nom) &gt; 300"> [...]</xsl:if></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<td align="center"><xsl:value-of select="dve"/></td>
					</tr>
				</xsl:for-each>
			</tbody>
		</table>
</xsl:if>
<xsl:if test="../parcelle/contributeur/chantier">
<a name="chap423"/><h3>Contribution : Chantier</h3>
<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
			<thead>
				<tr class="b">
					<th width="5%">Base</th>
					<th width="3%">Identifiant<br/>fiche</th>
					<th width="15%" class="left">Nom</th>
					<th width="29%" class="left">Commentaire</th>
					<th width="10%" class="center">Unité de l'UF</th>
					<th width="5%" class="center">Quantité</th>
					<th width="5%">DVE<br/>[an]</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="/projet/RSEnv/entree_projet/parcelle/contributeur/chantier/donnees_service">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center">
							<xsl:choose>
								<xsl:when test="type_donnees=7"><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:otherwise>
							</xsl:choose>
						</td>
						<td><xsl:value-of select="translate(substring(nom, 1, 300),'_',' ')"/><xsl:if test="string-length(nom) &gt; 300"> [...]</xsl:if></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<td align="center"><xsl:value-of select="dve"/></td>
					</tr>
				</xsl:for-each>
			</tbody>
		</table>
</xsl:if>
</xsl:template>
<!-- quantitatifs par bâtiment -->
<xsl:template match="/projet/RSEnv/entree_projet/batiment" mode="quantitatifs">
	<xsl:variable name="id_bat" select="index"/>
<xsl:if test="*/contributeur/composant/lot/sous_lot or */contributeur/composant/lot/donnees_composant">
	<a name="chap411-{$id_bat}"/><h3>Contribution : Composant</h3>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">1</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">2</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">3</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">4</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">5</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">6</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">7</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">8</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">9</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">10</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">11</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">12</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">13</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
	<xsl:call-template name="contributeur_1">
		<xsl:with-param name="v_lot">14</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
</xsl:if>
<xsl:if test="*/contributeur/energie/sous_contributeur/donnees_service">
	<a name="chap412-{$id_bat}"/><h3>Contribution : Consommation d'énergie</h3>
	<xsl:call-template name="contributeur_2_3">
		<xsl:with-param name="v_contrib">energie</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
</xsl:if>
<xsl:if test="*/contributeur/eau/sous_contributeur/donnees_service">
	<a name="chap413-{$id_bat}"/><h3>Contribution : Consommation et rejet d'eau</h3>
	<xsl:call-template name="contributeur_2_3">
		<xsl:with-param name="v_contrib">eau</xsl:with-param>
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
</xsl:if>
<xsl:if test="*/contributeur/chantier/sous_contributeur/donnees_service or */contributeur/chantier/sous_contributeur/donnees_composant">
	<a name="chap414-{$id_bat}"/><h3>Contribution : Chantier</h3>
	<xsl:call-template name="contributeur_4">
		<xsl:with-param name="id_bat" select="$id_bat"/>
	</xsl:call-template>
</xsl:if>
</xsl:template>


<!-- Indicateurs environnementaux dynamiques, à l'échelle de la parcelle -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_dyn_parcelle">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur dynamique CO<sub>2</sub>, à l'échelle de la parcelle</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>			
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/indicateur_co2_dynamique">
			<tr>			
				<td>Indicateur CO<sub>2</sub> dynamique</td>
				<td align="center">kg éq.CO<sub>2</sub></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A1-A3']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A4-A5']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='B']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='C']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='D']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center" class="bexp"><xsl:value-of select="format-number((valeur_phase_acv[@ref='Bexp']),'#&#160;##0,###', 'fr')"/></td>
			</tr>
			</xsl:for-each>
		</tbody>
	</table>
	
	<xsl:call-template name="phase_acv"/>

</xsl:template>


<!-- Indicateurs environnementaux dynamiques, à l'échelle du batiment -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_dyn_bat_new">
	<div class="spacer"></div>
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur dynamique CO<sub>2</sub>, à l'échelle du bâtiment <xsl:value-of select="index"/></th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>			
		</thead>
 		<tbody>
		<xsl:for-each select="indicateur_co2_dynamique">
			<tr>			
				<td>Indicateur CO<sub>2</sub> dynamique</td>
				<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A1-A3']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A4-A5']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='B']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='C']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='D']),'#&#160;##0,###', 'fr')"/></td>
				<td align="center" class="bexp"><xsl:value-of select="format-number((valeur_phase_acv[@ref='Bexp']),'#&#160;##0,###', 'fr')"/></td>
			</tr>
			</xsl:for-each>
		</tbody>
	</table>
	
</xsl:template>



<!-- Indicateurs environnementaux statiques, à l'échelle de la parcelle -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_stat_parcelle">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la parcelle</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>			
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle du batiment -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment <xsl:value-of select="index"/></th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>			
		</thead>
 		<tbody>
 		<!-- precision pour @ref pour les anciens xml avec 25 indicateurs à sup en version finale -->	
		<xsl:for-each select="indicateurs_acv_collection/indicateur[@ref &lt; 25]">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- Indicateurs principaux, à l'échelle de la parcelle, contributeur COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="contrib_parcelle_composant">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="2" class="h9" align="left">Indicateurs principaux, à l'échelle de la parcelle, contribution "Composant"</th>
			</tr>			
		</thead>
		<tbody>
			<tr>
				<td width="70%">Indicateur de stockage Carbone de la parcelle [kgC]</td><td width="30%" align="center"><xsl:value-of select="format-number(/projet/RSEnv/sortie_projet/parcelle/contributeur/composant/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td>Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</td><td align="center"><xsl:value-of select="format-number(/projet/RSEnv/sortie_projet/parcelle/contributeur/composant/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs principaux, à l'échelle du batiment, contributeur COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="contrib_bat_composant">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="2" class="h9" align="left">Indicateurs principaux, à l'échelle du bâtiment <xsl:value-of select="index"/>, contribution "Composant"</th>
			</tr>			
		</thead>
		<tbody>
			<tr>
				<td width="70%">Indicateur de stockage Carbone du bâtiment [kgC]</td><td width="30%" align="center"><xsl:value-of select="format-number(contributeur/composant/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td>Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</td><td align="center"><xsl:value-of select="format-number(contributeur/composant/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs principaux, à l'échelle de la zone, contributeur COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="contrib_zone_composant">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="2" class="h9" align="left">Indicateurs principaux, à l'échelle de la zone, contribution "Composant"</th>
			</tr>						
		</thead>
		<tbody>
			<tr>
				<td width="70%">Indicateur de stockage Carbone du bâtiment [kgC]</td><td width="30%" align="center"><xsl:value-of select="format-number(contributeur/composant/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td>Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</td><td align="center"><xsl:value-of select="format-number(contributeur/composant/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la parcelle, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_stat_parcelle_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la parcelle, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/composant/indicateurs_acv_collection/indicateur[@ref=1]">
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic">1</xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment <xsl:value-of select="index"/>, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/composant/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/composant/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle de la zone -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>

<!-- les indicateurs pour  COMPOSANT par LOT -->
<xsl:template name="template_recursif">
  <xsl:param name="iteration" select="1"/>
 
  	<tr>
  		<xsl:call-template name="boucle1">
  			<xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param>
  		</xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 

<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, COMPOSANT par LOT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_composant_lot">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contribution "Composant", par lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="13" width="36%" align="center">Numéro de lot <i>(Somme des phases A + B + C + D)</i></th> 
			</tr>
			<tr>
				<xsl:for-each select="contributeur/composant/lot">
						<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif"/>						
		</tbody>
	</table>
</xsl:template>
<!-- le template pour les indicateurs COMPOSANT par LOT -->
<xsl:template name="boucle1">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	<xsl:for-each select="contributeur/composant/lot">
		<td align="center">
   			<xsl:choose>
				<xsl:when test="string(number(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='Bexp'])) = 'NaN'">
					<xsl:value-of select="format-number(sum(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv),'#&#160;##0,##', 'fr')"/>			
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number(sum(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv) - indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/>				
				</xsl:otherwise>
			</xsl:choose>
		</td>
	</xsl:for-each>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, COMPOSANT par LOT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_composant_lot">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contribution "Composant", par lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="13" width="36%" align="center">Numéro de lot <i>(Somme des phases A + B + C + D)</i></th> 
			</tr>
			<tr>
				<xsl:for-each select="contributeur/composant/lot">
						<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif"/>						
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, COMPOSANT par LOT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stock_udd_co2">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs principaux, à l'échelle du bâtiment, contribution "Composant", par lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="13" width="36%" align="center">Numéro de lot</th> 
			</tr>
			<tr>
				<xsl:for-each select="contributeur/composant/lot">
				<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<tr>
					<td><b>Stock C</b></td>
					<td align="center">kgC/m²</td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>
			<tr>
					<td><b>UDD (Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</b></td>
					<td align="center">-</td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(udd,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>
			<tr>
					<td><b>Indicateur CO<sub>2</sub> Dynamique</b></td>
					<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(indicateur_co2_dynamique/valeur_phase_acv,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>			
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, COMPOSANT par LOT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stock_zone_udd_co2">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contribution "Composant", par lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="13" width="36%" align="center">Numéro de lot</th> 
			</tr>
			<tr>
				<xsl:for-each select="contributeur/composant/lot">
				<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<tr>
					<td><b>Stock C</b></td>
					<td align="center">kgC/m²</td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>
			<tr>
					<td><b>UDD (Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</b></td>
					<td align="center">-</td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(udd,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>
			<tr>
					<td><b>Indicateur CO<sub>2</sub> Dynamique</b></td>
					<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
					<xsl:for-each select="contributeur/composant/lot">
						<td align="center"><xsl:value-of select="format-number(indicateur_co2_dynamique/valeur_phase_acv,'#&#160;##0,##', 'fr')"/></td>
					</xsl:for-each>					
			</tr>			
		</tbody>
	</table>
</xsl:template>
<!-- les indicateurs pour  COMPOSANT par LOT  / SOUS LOTS -->
<xsl:template name="template_recursif_sslot">
  <xsl:param name="iteration" select="1"/>
 
  	<tr><xsl:call-template name="boucle_sslot"><xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param></xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif_sslot">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, COMPOSANT par LOT / SOUS LOT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_composant_sous_lot">
	<br/>
	<br/>
	<xsl:for-each select="contributeur/composant/lot[sous_lot]">
	<xsl:variable name="count_ss_lot" select="count(sous_lot)"/>
		<b>LOT : 
		<xsl:call-template name="lot">
			<xsl:with-param name="v_lot" select="@ref"/>
		</xsl:call-template>
		</b>	
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contribution "Composant", par sous-lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="{$count_ss_lot}" width="36%" align="center">Numéro(s) de sous-lot <i>(Somme des phases A + B + C + D)</i></th> 
			</tr>
			<tr>
				<xsl:for-each select="sous_lot">
						<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif_sslot"/>						
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
			<thead>
				<tr>
					<th colspan="16" class="h9" align="left">Indicateurs principaux, à l'échelle du bâtiment, contribution "Composant", par sous-lot</th>
				</tr>
				<tr>
					<th rowspan="2" width="39%">Indicateur</th> 
					<th rowspan="2" width="10%" align="center">Unité</th> 
					<th colspan="13" width="36%" align="center">Numéro(s) de sous-lot</th> 
				</tr>
				<tr>
					<xsl:for-each select="sous_lot">
					<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
					</xsl:for-each>			
				</tr>									
			</thead>
	 		<tbody>
				<tr>
						<td><b>Stock C</b></td>
						<td align="center">kgC/m²</td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>
				<tr>
						<td><b>UDD (Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</b></td>
						<td align="center">-</td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(udd,'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>
				<tr>
						<td><b>Indicateur CO<sub>2</sub> Dynamique</b></td>
						<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(sum(indicateur_co2_dynamique/valeur_phase_acv),'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>			
			</tbody>
		</table>
</xsl:for-each>
</xsl:template>
<!-- le template pour les indicateurs COMPOSANT par SOUS LOT -->
<xsl:template name="boucle_sslot">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	
		<xsl:for-each select="sous_lot">
		<td align="center">
   			<xsl:choose>
				<xsl:when test="string(number(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='Bexp'])) = 'NaN'">
					<xsl:value-of select="format-number(sum(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv),'#&#160;##0,##', 'fr')"/>			
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number(sum(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv) - indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/>				
				</xsl:otherwise>
			</xsl:choose>
		</td>
	</xsl:for-each>


</xsl:template>

<!-- les indicateurs pour ZONE/COMPOSANT par LOT / SOUS LOTS -->
<xsl:template name="template_recursif_zone_sslot">
  <xsl:param name="iteration" select="1"/>
 
  	<tr><xsl:call-template name="boucle_zone_sslot"><xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param></xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif_zone_sslot">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_composant_sous_lot">
<br/>
	<br/>
	<xsl:for-each select="contributeur/composant/lot[sous_lot]">
	<xsl:variable name="count_ss_lot" select="count(sous_lot)"/>
		<b>LOT : 
		<xsl:call-template name="lot">
			<xsl:with-param name="v_lot" select="@ref"/>
		</xsl:call-template>
		</b>	
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contribution "Composant", par sous-lot</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="{$count_ss_lot}" width="36%" align="center">Numéro(s) de sous-lot <i>(Somme des phases A + B + C + D)</i></th> 
			</tr>
			<tr>
				<xsl:for-each select="sous_lot">
						<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
				</xsl:for-each>			
			</tr>									
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif_sslot"/>						
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
			<thead>
				<tr>
					<th colspan="16" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contribution "Composant", par sous-lot</th>
				</tr>
				<tr>
					<th rowspan="2" width="39%">Indicateur</th> 
					<th rowspan="2" width="10%" align="center">Unité</th> 
					<th colspan="13" width="36%" align="center">Numéro(s) de sous-lot</th> 
				</tr>
				<tr>
					<xsl:for-each select="sous_lot">
					<td align="center" width="6%"><b><xsl:value-of select="@ref"/></b></td>
					</xsl:for-each>			
				</tr>									
			</thead>
	 		<tbody>
				<tr>
						<td><b>Stock C</b></td>
						<td align="center">kgC/m²</td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>
				<tr>
						<td><b>UDD (Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</b></td>
						<td align="center">-</td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(udd,'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>
				<tr>
						<td><b>Indicateur CO<sub>2</sub> Dynamique</b></td>
						<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
						<xsl:for-each select="sous_lot">
							<td align="center"><xsl:value-of select="format-number(sum(indicateur_co2_dynamique/valeur_phase_acv),'#&#160;##0,##', 'fr')"/></td>
						</xsl:for-each>					
				</tr>			
			</tbody>
		</table>
</xsl:for-each>
</xsl:template>
<!-- le template pour les indicateurs COMPOSANT par SOUS LOT -->
<xsl:template name="boucle_zone_sslot">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	<xsl:for-each select="sous_lot">
		<td align="center"><xsl:value-of select="format-number(sum(indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv[../@ref=$v_indic]) - indicateurs_acv_collection/indicateur/valeur_phase_acv[@ref='Bexp'][../@ref=$v_indic],'#&#160;##0,##', 'fr')"/></td>
	</xsl:for-each>
</xsl:template>
<!-- Indicateurs environnementaux dynamique, à l'échelle de la parcelle, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_dyn_parcelle_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la parcelle, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/composant/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux dynamique, à l'échelle du batiment, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_dyn_bat_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle du bâtiment, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/composant/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux dynamique, à l'échelle de la zone, COMPOSANT -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_dyn_zone_composant">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la zone, contribution "Composant"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/composant/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux dynamique, à l'échelle de la zone, -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_dyn_zone">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la zone</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="indicateur_co2_dynamique">
			 <xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Contribution : ENERGIE -->
<!-- Indicateurs environnementaux dynamique, à l'échelle du batiment, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_dyn_bat_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle du bâtiment, contributeur "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/energie/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux dynamique, à l'échelle de la zone, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_dyn_zone_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la zone, contributeur "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/energie/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contributeur "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/energie/indicateurs_acv_collection/indicateur">
			<!-- <xsl:sort select="@ref" data-type="number"/> -->
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contributeur "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/energie/indicateurs_acv_collection/indicateur">
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- le template pour les sous-contributions ENERGIE -->
<xsl:template name="template_recursif2">
  <xsl:param name="iteration" select="1"/>
 
  	<tr><xsl:call-template name="boucle2"><xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param></xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif2">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 
<!-- la boucle -->
<xsl:template name="boucle2">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	<xsl:for-each select="contributeur/energie/sous_contributeur">
		<xsl:variable name="id_ref" select="@ref"/>
		<td align="center">
			<xsl:value-of select="format-number(sum(../sous_contributeur[@ref=$id_ref]/indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv),'#&#160;##0,##', 'fr')"/>
		</td>
	</xsl:for-each>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, pour les sous-contributions ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_sous_contrib_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="10" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, sous-contributions "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="46%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%"><b>Chauffage</b></td>
				<td align="center" width="6%"><b>Refroidissement</b></td>
				<td align="center" width="6%"><b>ECS</b></td>
				<td align="center" width="6%"><b>Eclairage</b></td>
				<td align="center" width="6%"><b>Auxiliaires ventilation</b></td>
				<td align="center" width="6%"><b>Auxiliaires distribution</b></td>
				<td align="center" width="6%"><b>Déplacements (ascenseurs, escalators, parkings)</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif2"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle du bâtiment, sous-contributions "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="51%" align="center">Sous-contributions</th> 
			</tr>	
			<tr>
				<td align="center" width="6%"><b>Chauffage</b></td>
				<td align="center" width="6%"><b>Refroidissement</b></td>
				<td align="center" width="6%"><b>ECS</b></td>
				<td align="center" width="6%"><b>Eclairage</b></td>
				<td align="center" width="6%"><b>Auxiliaires ventilation</b></td>
				<td align="center" width="6%"><b>Auxiliaires distribution</b></td>
				<td align="center" width="6%"><b>Déplacements (ascenseurs, escalators, parkings)</b></td>
			</tr>				
		</thead>
 		<tbody>
	
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=4]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=5]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=6]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=7]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, pour les sous-contributions ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_sous_contrib_zone_energie">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="10" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, sous-contributions "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="46%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%"><b>Chauffage</b></td>
				<td align="center" width="6%"><b>Refroidissement</b></td>
				<td align="center" width="6%"><b>ECS</b></td>
				<td align="center" width="6%"><b>Eclairage</b></td>
				<td align="center" width="6%"><b>Auxiliaires ventilation</b></td>
				<td align="center" width="6%"><b>Auxiliaires distribution</b></td>
				<td align="center" width="6%"><b>Déplacements (ascenseurs, escalators, parkings)</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif2"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle de la zone, sous-contributions "Energie"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="51%" align="center">Sous-contributions</th> 
			</tr>	
			<tr>
				<td align="center" width="6%"><b>Chauffage</b></td>
				<td align="center" width="6%"><b>Refroidissement</b></td>
				<td align="center" width="6%"><b>ECS</b></td>
				<td align="center" width="6%"><b>Eclairage</b></td>
				<td align="center" width="6%"><b>Auxiliaires ventilation</b></td>
				<td align="center" width="6%"><b>Auxiliaires distribution</b></td>
				<td align="center" width="6%"><b>Déplacements (ascenseurs, escalators, parkings)</b></td>
			</tr>				
		</thead>
 		<tbody>
	
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=4]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=5]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=6]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/energie/sous_contributeur[@ref=7]/indicateur_co2_dynamique/valeur_phase_acv) - contributeur/energie/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>


<!-- Contribution : EAU -->
<!-- Indicateurs environnementaux dynamique, à l'échelle du batiment, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_dyn_bat_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle du bâtiment, contribution "Consommation et rejet d'eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/eau/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux dynamique, à l'échelle de la zone, ENERGIE -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_dyn_zone_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la zone, contributeur "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/eau/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contributeur "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/eau/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contributeur "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/eau/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>


<!-- sous contributeur EAU -->
<!-- le template pour les sous-contributions EAU -->
<xsl:template name="template_recursif3">
  <xsl:param name="iteration" select="1"/>
 
  	<tr><xsl:call-template name="boucle3"><xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param></xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif3">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 

<xsl:template name="boucle3">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	<xsl:for-each select="contributeur/eau/sous_contributeur">
		<xsl:variable name="id_ref" select="@ref"/>
		<td align="center">
			<xsl:value-of select="format-number(sum(../sous_contributeur[@ref=$id_ref]/indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv),'#&#160;##0,##', 'fr')"/>
		</td>
	</xsl:for-each>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, pour les sous-contributions EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_sous_contrib_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, sous-contributions "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="3" width="46%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="15%"><b>Eau potable</b></td>
				<td align="center" width="15%"><b>Eau usée</b></td>
				<td align="center" width="15%"><b>Eau pluviale</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif3"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle du bâtiment, sous-contributions "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="51%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="15%"><b>Eau potable</b></td>
				<td align="center" width="15%"><b>Eau usée</b></td>
				<td align="center" width="15%"><b>Eau pluviale</b></td>
			</tr>			
		</thead>
 		<tbody>
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, pour les sous-contributions EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_sous_contrib_zone_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, sous-contributions "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="3" width="46%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="15%"><b>Eau potable</b></td>
				<td align="center" width="15%"><b>Eau usée</b></td>
				<td align="center" width="15%"><b>Eau pluviale</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif3"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle de la zone, sous-contributions "Eau"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="7" width="51%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="15%"><b>Eau potable</b></td>
				<td align="center" width="15%"><b>Eau usée</b></td>
				<td align="center" width="15%"><b>Eau pluviale</b></td>
			</tr>			
		</thead>
 		<tbody>
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/eau/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/eau/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>

<!-- Contribution : CHANTIER -->
<!-- Indicateurs environnementaux dynamique, à l'échelle du batiment, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_dyn_bat_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle du bâtiment, contributeur "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="54%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/chantier/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux dynamique, à l'échelle de la zone, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_dyn_zone_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la zone, contributeur "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="54%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>					
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/chantier/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_stat_bat_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, contributeur "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="49%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/chantier/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_stat_zone_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, contributeur "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="49%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
				
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="contributeur/chantier/indicateurs_acv_collection/indicateur">
			<xsl:sort select="@ref" data-type="number"/>
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic"><xsl:value-of select="$id_ref"/></xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
	<xsl:call-template name="phase_acv"/>
</xsl:template>
<!-- sous contributeur CHANTIER -->
<!-- le template pour les sous-contributions CHANTIER -->
<xsl:template name="template_recursif4">
  <xsl:param name="iteration" select="1"/>
 
  	<tr><xsl:call-template name="boucle4"><xsl:with-param name="v_indic"><xsl:value-of select="$iteration"/></xsl:with-param></xsl:call-template></tr>
 
  <xsl:if test="23 >= $iteration">
    <xsl:call-template name="template_recursif4">
      <xsl:with-param name="iteration" select="$iteration + 1"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template> 

<xsl:template name="boucle4">
<xsl:param name="v_indic"/>
	<td><xsl:value-of select="$v_indic"/></td>
	<xsl:call-template name="indicateurs_libelle">
		<xsl:with-param name="v_indic" select="$v_indic"/>
	</xsl:call-template>
	<xsl:for-each select="contributeur/chantier/sous_contributeur">
		<xsl:variable name="id_ref" select="@ref"/>
		<td align="center">
			<xsl:value-of select="format-number(sum(../sous_contributeur[@ref=$id_ref]/indicateurs_acv_collection/indicateur[@ref=$v_indic]/valeur_phase_acv),'#&#160;##0,##', 'fr')"/>
		</td>
	</xsl:for-each>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle du batiment, pour les sous-contributions CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="ind_sous_contrib_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="7" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle du bâtiment, sous-contributions "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="45%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="4" width="40%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="10%" class="L3"><b>Energie</b></td>
				<td align="center" width="10%" class="L3"><b>Eau</b></td>
				<td align="center" width="10%" class="L3"><b>Terre</b></td>
				<td align="center" width="10%" class="L3"><b>Composant</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif4"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle du bâtiment, sous-contributions "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="50%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="4" width="40%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="10%" class="L3"><b>Energie</b></td>
				<td align="center" width="10%" class="L3"><b>Eau</b></td>
				<td align="center" width="10%" class="L3"><b>Terre</b></td>
				<td align="center" width="10%" class="L3"><b>Composant</b></td>
			</tr>			
		</thead>
 		<tbody>
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=4]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>

<!-- Indicateurs environnementaux statiques, à l'échelle de la zone, pour les sous-contributions CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="ind_sous_contrib_zone_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="7" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la zone, sous-contributions "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="45%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="4" width="40%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="10%" class="L3"><b>Energie</b></td>
				<td align="center" width="10%" class="L3"><b>Eau</b></td>
				<td align="center" width="10%" class="L3"><b>Terre</b></td>
				<td align="center" width="10%" class="L3"><b>Composant</b></td>
			</tr>				
		</thead>
 		<tbody>
			<xsl:call-template name="template_recursif4"/>
		</tbody>
	</table>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="6" class="h9" align="left">Indicateurs environnementaux dynamiques, à l'échelle de la zone, sous-contributions "Chantier"</th>
			</tr>
			<tr>
				<th rowspan="2" width="50%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="4" width="40%" align="center">Sous-contributions</th> 
				
			</tr>	
			<tr>
				<td align="center" width="10%" class="L3"><b>Energie</b></td>
				<td align="center" width="10%" class="L3"><b>Eau</b></td>
				<td align="center" width="10%" class="L3"><b>Terre</b></td>
				<td align="center" width="10%" class="L3"><b>Composant</b></td>
			</tr>			
		</thead>
 		<tbody>
		<tr>
			<td>Indicateur CO<sub>2</sub> dynamique</td><td>kg<sub>éq.</sub> CO<sub>2</sub></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=2]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=3]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
			<td align="center"><xsl:value-of select="format-number(sum(contributeur/chantier/sous_contributeur[@ref=4]/indicateur_co2_dynamique/valeur_phase_acv)-contributeur/chantier/sous_contributeur[@ref=1]/indicateur_co2_dynamique/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,##', 'fr')"/></td>
		</tr>
	
		</tbody>
	</table>
</xsl:template>


<!-- Chapitre 6 -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment/zone" mode="acv_zone">
	
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<th class="h7" align="left" colspan="4">Indicateurs principaux, à l'échelle de la zone</th>
			</tr>	
		</thead>
		<tbody>
			<tr>
				<td width="55%" align="left">Indicateur de stockage Carbone de la zone</td>
				<td align="center" width="15%">[kgC]</td>
				<td align="center" width="30%" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>				
			<tr>
				<td align="left">Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement</td>
				<td align="center">(valeur entre 0 et 1)</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>				
			<tr>
				<td align="left">Indicateur Carbone (total contributions)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_zone,'#&#160;##0,##', 'fr')"/></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution construction)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" width="15%" >
					<xsl:if test="indicateur_perf_env/ic_construction &lt; indicateur_perf_env/ic_construction_max">
						<font size="+1em" color="green"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
					<xsl:if test="indicateur_perf_env/ic_construction >= indicateur_perf_env/ic_construction_max">
						<font size="+1em" color="red"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
				</td>
				<td align="center"  width="15%"><i>max</i><br/><i><b><xsl:value-of select="format-number(indicateur_perf_env/ic_construction_max,'#&#160;##0,##', 'fr')"/></b></i></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution énergie)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center">
					<xsl:if test="indicateur_perf_env/ic_energie &lt; indicateur_perf_env/ic_energie_max">
						<font size="+1em" color="green"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
					<xsl:if test="indicateur_perf_env/ic_energie >= indicateur_perf_env/ic_energie_max">
						<font size="+1em" color="red"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
				</td>
				<td align="center"><i>max</i><br/><b><i><xsl:value-of select="format-number(indicateur_perf_env/ic_energie_max,'#&#160;##0,##', 'fr')"/></i></b></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution eau)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_eau,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Contribution Composant</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_composant,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Contribution Chantier</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_chantier,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td colspan="4" align="center" bgcolor="#F6A637"><b>Données Complémentaires</b></td>
			</tr>
			
			<tr>
				<td align="left">Indicateur Carbone (Zone + quote-part de la parcelle)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_projet,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone (Zone + quote-part de la parcelle) par occupant</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/occ]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_projet_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant sur toute la zone </td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/occ]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_zone_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant pour le contribution "Composant"</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant pour la contribution "Energie"</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Quote-part des impacts env. de la parcelle attribuée au bâtiment et ramenée à la surface de référence de la zone</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_parcelle,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Impacts environnementaux (CO<sub>2</sub> dynamique) associée à des DED et des valeurs forfaitaires (Lots 3 à 13)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_ded,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td colspan="4" align="center" bgcolor="#F6A637"><b>Coefficients modulateurs de la variable "ic_construction"</b></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la zone géographique (zone climatique et altitude)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/migeo,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la présence de combles aménagées</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/micombles,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la surface de référence de l'objet traité</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/misurf,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée aux impacts de l'infrastructure</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/miinfra,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée aux impacts de la VRD</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/mivrd,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée aux impacts des DED (données environnementales par défaut)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/mided,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée aux impacts du lot 13 pour les usages de type bureau</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/mipv,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Valeur pivot (dépendant de l'usage et de l'année du PC)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icconstruction/ic_construction_maxmoyen,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td colspan="4" align="center" bgcolor="#F6A637"><b>Coefficients modulateurs de la variable "ic_energie"</b></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la zone géographique (zone climatique et altitude)</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/mcgeo,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la présence de combles aménagés</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/mccombles,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la surface moyenne des logements (surf_moy = Sref/nb_logement)</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/mcsurf_moy,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la surface de référence de l'objet traité</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/mcsurf_tot,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Modulation liée à la catégorie de contraintes extérieures</td>
				<td align="center">-</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/mccat,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Valeur pivot (dépendant de l'usage et de l'année du PC)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/coef_mod_icenergie/ic_energie_maxmoyen,'#&#160;##0,##', 'fr')"/></td>
			</tr>
	</tbody>
	</table>
</xsl:template>
<!-- Indicateurs principaux, à l'échelle de la parcelle, contributeur EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="contrib_parcelle_eau">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="2" class="h9">Indicateurs principaux, à l'échelle de la parcelle, contribution "EAU"</th>
			</tr>			
		</thead>
		<tbody>
			<tr>
				<td width="70%">Indicateur de stockage Carbone de la parcelle [kgC]</td><td width="30%" align="center"><xsl:value-of select="format-number(/projet/RSEnv/sortie_projet/parcelle/contributeur/eau/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td>Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement (valeur comprise entre 0 et 1)</td><td align="center"><xsl:value-of select="format-number(/projet/RSEnv/sortie_projet/parcelle/contributeur/eau/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux statiques, à l'échelle de la parcelle, EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_stat_parcelle_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9">Indicateurs environnementaux statiques, à l'échelle de la parcelle, contribution "EAU"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/eau/indicateurs_acv_collection/indicateur[@ref=1]">
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic">1</xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux dynamique, à l'échelle de la parcelle, EAU -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_dyn_parcelle_eau">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la parcelle, contribution "EAU"</th>
			</tr>
			<tr> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>			
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/eau/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- CHANTIER -->
<!-- Indicateurs environnementaux statiques, à l'échelle de la parcelle, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_stat_parcelle_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="9" class="h9" align="left">Indicateurs environnementaux statiques, à l'échelle de la parcelle, contribution "CHANTIER"</th>
			</tr>
			<tr>
				<th rowspan="2" width="5%" align="center">N°</th> 
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/chantier/indicateurs_acv_collection/indicateur[@ref=1]">
			<xsl:variable name="id_ref" select="@ref"/>
			<tr>
				<xsl:call-template name="indic_env_stat">
					<xsl:with-param name="v_indic">1</xsl:with-param>
				</xsl:call-template>
			</tr>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- Indicateurs environnementaux dynamique, à l'échelle de la parcelle, CHANTIER -->
<xsl:template match="/projet/RSEnv/sortie_projet" mode="ind_dyn_parcelle_chantier">
	<br/>
	<br/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="8" class="h9" align="left">Indicateur CO<sub>2</sub> dynamique, à l'échelle de la parcelle, contribution "CHANTIER"</th>
			</tr>
			<tr>
				<th rowspan="2" width="39%">Indicateur</th> 
				<th rowspan="2" width="10%" align="center">Unité</th> 
				<th colspan="6" width="36%" align="center">Phases du Cycle de Vie*</th> 
			</tr>	
			<tr>
				<td align="center" width="6%" class="L3"><b>A1-A3</b></td>
				<td align="center" width="6%" class="L3"><b>A4-A5</b></td>
				<td align="center" width="6%" class="L3"><b>B</b></td>
				<td align="center" width="6%" class="L3"><b>C</b></td>
				<td align="center" width="6%" class="L3"><b>D</b></td>
				<td align="center" width="6%" class="bexp"><b>Bexp</b></td>
			</tr>				
		</thead>
 		<tbody>
		<xsl:for-each select="parcelle/contributeur/chantier/indicateur_co2_dynamique">
			<xsl:call-template name="indic_co2_phases"/>
		</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<!-- chapitre 5 -->
<!-- Indicateurs principaux, à l'échelle du bâtiment -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="S_acv_bat">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<th class="h7" align="left" colspan="4">Indicateurs principaux, à l'échelle du bâtiment <xsl:value-of select="index"/></th>
			</tr>			
		</thead>
		<tbody>
			<tr>
				<td width="55%" align="left">Indicateur de stockage Carbone</td><td width="15%" align="center">[kgC]</td>
				<td align="center" width="30%" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/stock_c_batiment,'#&#160;##0,##', 'fr')"/></td>
			</tr>				
			<tr>
				<td align="left">Part des impacts environnementaux des données génériques sur l'indicateur Réchauffement Climatique uniquement</td>
				<td align="center">(valeur entre 0 et 1)</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/udd,'#&#160;##0,##', 'fr')"/></td>
			</tr>				
			<tr>
				<td align="left">Indicateur Carbone <b>(total contributions)</b></td><td>&#160;</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_batiment,'#&#160;##0,##', 'fr')"/></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution <b>construction</b>)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" width="15%" >
					<xsl:if test="indicateur_perf_env/ic_construction &lt; indicateur_perf_env/ic_construction_max">
						<font size="+1em" color="green"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
					<xsl:if test="indicateur_perf_env/ic_construction >= indicateur_perf_env/ic_construction_max">
						<font size="+1em" color="red"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
				</td>
				<td align="center"  width="15%"><i>max</i><br/><i><b><xsl:value-of select="format-number(indicateur_perf_env/ic_construction_max,'#&#160;##0,##', 'fr')"/></b></i></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution <b>énergie</b>)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center">
					<xsl:if test="indicateur_perf_env/ic_energie &lt; indicateur_perf_env/ic_energie_max">
						<font size="+1em" color="green"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
					<xsl:if test="indicateur_perf_env/ic_energie >= indicateur_perf_env/ic_energie_max">
						<font size="+1em" color="red"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie,'#&#160;##0,##', 'fr')"/></font>
					</xsl:if>
				</td>
				<td align="center"><i>max</i><br/><b><i><xsl:value-of select="format-number(indicateur_perf_env/ic_energie_max,'#&#160;##0,##', 'fr')"/></i></b></td>
			</tr>	
			<tr>
				<td align="left">Indicateur Carbone (contribution <b>eau</b>)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_eau,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Contribution <b>Composant</b></td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_composant,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Contribution <b>Chantier</b></td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_chantier,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td colspan="4" align="center" bgcolor="#F6A637"><b>Données Complémentaires</b></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant sur toute la zone </td><td>&#160;</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_batiment_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant pour la contribution "Composant"</td><td>&#160;</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_construction_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone par occupant pour la contribution "Energie"</td><td>&#160;</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie_occ,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Indicateur Carbone annualisé pour la contribution "Energie"</td><td>&#160;</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_energie,'#&#160;##0,##', 'fr')"/></td>
			</tr>			
			<tr>
				<td align="left">Quote-part des impacts env. de la parcelle attribuée au bâtiment et ramenée à la surface de référence de la zone</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_parcelle,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<tr>
				<td align="left">Impacts environnementaux (CO<sub>2</sub> dynamique) associée à des DED et des valeurs forfaitaires (Lots 3 à 13)</td>
				<td align="center">[kg<sub>éq.</sub> CO<sub>2</sub>/m²]</td>
				<td align="center" colspan="2"><xsl:value-of select="format-number(indicateur_perf_env/ic_ded,'#&#160;##0,##', 'fr')"/></td>
			</tr>
		</tbody>
	</table>
</xsl:template>

<!-- Répartition inter et intra-contributeurs de l'indicateur « Stockage Carbone » à l'échelle du bâtiment  -->
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="S_acv_bat_composant">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th colspan="2" class="h9" align="left">Répartition inter et intra-contributions de l'indicateur « Stockage Carbone » à l'échelle du bâtiment <xsl:value-of select="index"/>*</th>
			</tr>			
		</thead>
		<tbody>
			<tr><td><b>Total</b></td><td align="center"><b><xsl:value-of select="format-number(indicateur_perf_env/stock_c_batiment,'#&#160;##0,##', 'fr')"/></b></td></tr>
			<tr>
				<td width="70%"><i>Contribution Composant</i></td><td width="30%" align="center"><xsl:value-of select="format-number(contributeur/composant/stock_c,'#&#160;##0,##', 'fr')"/></td>
			</tr>
			<xsl:for-each select="contributeur/composant/lot[stock_c &gt; 0]">
				<xsl:variable name="la_ref" select="@ref"/>
				<tr>
					<td class="lot"><b>Lot&#160;<xsl:value-of select="$la_ref"/></b></td><td align="center" class="lot"><b><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></b></td>
				</tr>
				<xsl:for-each select="sous_lot[stock_c &gt; 0]">
					<tr><td><i>Sous-Lot <xsl:value-of select="@ref"/> - <xsl:call-template name="ss_lot"><xsl:with-param name="v_ss_lot" select="@ref"/></xsl:call-template></i></td><td align="center"><xsl:value-of select="format-number(stock_c,'#&#160;##0,##', 'fr')"/></td></tr>
				</xsl:for-each>
			</xsl:for-each>
		</tbody>
	</table>
<p class="note">*Lots non présents = valeur 0</p>
</xsl:template>
<xsl:template name="indicateurs_libelle">
<xsl:param name="v_indic"/>
	<xsl:choose>
		<xsl:when test="$v_indic=1"><td>Emission de gaz à effet de serre (GES)</td><td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td></xsl:when>
		<xsl:when test="$v_indic=2"><td>Potentiel de destruction de la couche d'ozone stratosphérique (ODP)</td><td align="center">kg<sub>éq.</sub> CFC-11</td></xsl:when>
		<xsl:when test="$v_indic=3"><td>Potentiel d'acidification du sol et de l'eau (AP)</td><td align="center">kg<sub>éq.</sub> SO<sub>2</sub><sup>-</sup></td></xsl:when>
		<xsl:when test="$v_indic=4"><td>Potentiel d'eutrophisation (EP)</td><td align="center">kg<sub>éq.</sub> (PO<sub>4</sub>)<sup>3-</sup></td></xsl:when>
		<xsl:when test="$v_indic=5"><td>Potentiel de formation d'oxydants photochimiques de l'ozone troposphérique (POCP)</td><td align="center">kg<sub>éq.</sub> éthylène</td></xsl:when>
		<xsl:when test="$v_indic=6"><td>Potentiel de dégradation abiotique des ressources pour les éléments (ADP_éléments)</td><td align="center">kg<sub>éq.</sub> Sb</td></xsl:when>
		<xsl:when test="$v_indic=7"><td>Potentiel de dégradation abiotique des combustibles fossiles (ADP_combustibles fossiles)</td><td align="center">MJ, valeur calorifique nette</td></xsl:when>
		<xsl:when test="$v_indic=8"><td>Utilisation de l'énergie primaire renouvelable à l'exclusion des ressources d'énergie employées en tant que matière première (UEP_pro,ren)</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=9"><td>Utilisation de ressources énergétiques primaires renouvelables employées en tant que matière première</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=10"><td>Utilisation totale des ressources d'énergie primaire renouvelables (énergie primaire et ressources d'énergie primaire employées en tant que matières premières) (UEP_ren)</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=11"><td>Utilisation de l’énergie primaire non renouvelable à l’exclusion des ressources d’énergie primaire employées en tant que matière première (UEP_pro,nren)</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=12"><td>Utilisation de ressources énergétiques primaires non renouvelables employées en tant que matière première (UEP_mat,nren)</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=13"><td>Utilisation totale des ressources d'énergie primaire non renouvelables (énergie primaire et ressources d'énergie primaire employées en tant que matières premières) (UEP_nren)</td><td align="center">MJ, pouvoir calorifique inférieur</td></xsl:when>
		<xsl:when test="$v_indic=14"><td>Utilisation de matières secondaires(C_MS)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=15"><td>Utilisation de combustibles secondaires renouvelables (C_CSRen)</td><td align="center">MJ</td></xsl:when>
		<xsl:when test="$v_indic=16"><td>Utilisation de combustibles secondaires non renouvelables (C_CSNRen)</td><td align="center">MJ</td></xsl:when>
		<xsl:when test="$v_indic=17"><td>Utilisation nette d'eau douce (C_eau)</td><td align="center">m<sup>3</sup></td></xsl:when>
		<xsl:when test="$v_indic=18"><td>Déchets dangereux éliminés (DD)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=19"><td>Déchets non dangereux éliminés (DND)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=20"><td>Déchets radioactifs (RD)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=21"><td>Composants destinés à la réutilisation (M_Réu)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=22"><td>Matières pour le recyclage (M_Recy)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=23"><td>Matières pour la récupération d'énergie (à l'exception de l'incinération) (M_VE)</td><td align="center">kg</td></xsl:when>
		<xsl:when test="$v_indic=24"><td>Énergie fournie à l'extérieur (E_ex)</td><td align="center">MJ pour chaque vecteur énergétique</td></xsl:when>
	</xsl:choose>					
</xsl:template>
<!-- indicateurs statique -->
<xsl:template name="indic_env_stat">
	<xsl:param name="v_indic"/>
		<td align="center"><b><xsl:value-of select="$v_indic"/></b></td>
		<xsl:call-template name="indicateurs_libelle">
			<xsl:with-param name="v_indic" select="$v_indic"/>
		</xsl:call-template>
		<td align="center"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='A1-A3'],'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='A4-A5'],'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='B'],'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='C'],'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='D'],'#&#160;##0,###', 'fr')"/></td>
		<td align="center" class="bexp"><xsl:value-of select="format-number(../indicateur[@ref=$v_indic]/valeur_phase_acv[@ref='Bexp'],'#&#160;##0,###', 'fr')"/></td>
</xsl:template>
<xsl:template match="/projet/RSEnv/sortie_projet/batiment" mode="indic_dyn_bat">
	<xsl:variable name="id_bat" select="index"/>
	
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h9" width="100%">
		<thead>
			<tr>
				<th width="5%" align="center">Contributions</th> 
				<th width="37%">Nom</th> 
				<th width="10%" align="center">Valeur</th> 
				<th class="h9">Unité</th>
				<th class="h9">Phases</th>
			</tr>				
		</thead>
		<tbody>
			<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/contributeur/composant">
				<tr>
					<td><b>Composant</b></td>
					<xsl:call-template name="indic_dyn"/>
				</tr>
			</xsl:for-each>
			<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/contributeur/energie">
				<tr>
					<td><b>Energie</b></td>
					<xsl:call-template name="indic_dyn"/>
				</tr>
			</xsl:for-each>
			<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/contributeur/eau">
				<tr>
					<td><b>Eau</b></td>
					<xsl:call-template name="indic_dyn"/>
				</tr>
			</xsl:for-each>
			<xsl:for-each select="/projet/RSEnv/sortie_projet/batiment[index=$id_bat]/contributeur/chantier">
				<tr>
					<td><b>Chantier</b></td>
					<xsl:call-template name="indic_dyn"/>
				</tr>						
			</xsl:for-each>
		</tbody>
	</table>
</xsl:template>
<xsl:template name="indic_dyn">
	<td><xsl:value-of select="indicateur_co2_dynamique/nom"/></td>
	<td><xsl:value-of select="indicateur_co2_dynamique/valeur"/></td>
	<td><xsl:value-of select="indicateur_co2_dynamique/unite"/></td>
	<td><xsl:for-each select="indicateur_co2_dynamique/valeur_phase_acv"><xsl:value-of select="./@ref"/> / <xsl:value-of select="."/><br/></xsl:for-each></td>
</xsl:template>
<!-- ******************************************************************************* CALL-TEMPLATES ********************************************************* -->
<xsl:template name="indic_co2_phases">
	<tr>			
		<td>Indicateur CO<sub>2</sub> dynamique</td>
		<td align="center">kg<sub>éq.</sub> CO<sub>2</sub></td>
		<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A1-A3']),'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='A4-A5']),'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='B']),'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='C']),'#&#160;##0,###', 'fr')"/></td>
		<td align="center"><xsl:value-of select="format-number((valeur_phase_acv[@ref='D']),'#&#160;##0,###', 'fr')"/></td>
		<td align="center" class="bexp"><xsl:value-of select="format-number((valeur_phase_acv[@ref='Bexp']),'#&#160;##0,###', 'fr')"/></td>
	</tr>
</xsl:template>

<xsl:template name="phase_acv">
	<p class="note"><b>*Phases du Cycle de Vie</b> : 
	<br/>Production (A1-A2-A3),<br/>
Edification (A4-A5),<br/>
Exploitation (B),<br/>
Fin de vie (C),<br/>
Bénéfices et charges liés à la valorisation en fin de vie (D),<br/>
Bénéfices liés à l'export d'énergie (Bexp)</p>
</xsl:template>

<!-- tous contributeurs confondus -->
<xsl:template name="indic_det">
	<xsl:param name="v_indic"/>
	<xsl:param name="id_bat"/>
	<xsl:param name="sum_indic"/>
	<xsl:param name="sdp_bat"/>
	<xsl:if test="$sum_indic &gt; 0">
		<tr>
			<td align="center"><xsl:value-of select="$v_indic"/></td>
			<xsl:call-template name="indic_unite">
				<xsl:with-param name="v_indic" select="$v_indic"/>
			</xsl:call-template>
			<td align="center">
			<xsl:choose>
				<xsl:when test="$v_indic=2">
					<xsl:value-of select="format-number($sum_indic, '#&#160;##0,0#####','fr')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($sum_indic, '#&#160;##0,##','fr')"/>
				</xsl:otherwise>
			</xsl:choose>
		</td>
		<td align="center"><xsl:value-of select="format-number($sum_indic div $sdp_bat, '#&#160;##0,0####','fr')"/></td>
		<td align="center"><xsl:value-of select="format-number(($sum_indic div $sdp_bat) div $p_env1/entree_projet/batiment[index=$id_bat]/periode_reference, '#&#160;##0,0####','fr')"/></td>
	</tr>
</xsl:if>
</xsl:template>

<xsl:template name="contributeur_1">
	<xsl:param name="v_lot"/>
	<xsl:param name="id_bat"/>
	<xsl:variable name="c_zone" select="count(zone)"/>
	<xsl:if test="*/contributeur/composant/lot[@ref=$v_lot]/sous_lot or */contributeur/composant/lot[@ref=$v_lot]/donnees_composant">
		<xsl:variable name="nb_col">
		<xsl:for-each select="*/contributeur/composant/lot[@ref=$v_lot]//donnees_composant">
			<xsl:choose>
				<xsl:when test="commentaire">
					<xsl:choose>
						<xsl:when test="$c_zone &gt; 1">12</xsl:when>
						<xsl:otherwise>11</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$c_zone &gt; 1">11</xsl:when>
					<xsl:otherwise>10</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		</xsl:variable>
		<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
			<thead>
				<tr>
					<th colspan="{$nb_col}" class="h7" align="left">LOT 
						<xsl:call-template name="lot">
							<xsl:with-param name="v_lot" select="$v_lot"/>
						</xsl:call-template>
					</th>		
				</tr>	
				<tr class="b">
					<th width="5%">Sous-lot</th>
					<th width="5%">Base</th>
					<th width="3%">Identifiant<br/>fiche</th>
					<th width="10%">Type de données</th>
					<th width="18%" class="left">Nom</th>
					<xsl:for-each select="*/contributeur/composant/lot[@ref=$v_lot]//donnees_composant">
						<xsl:if test="commentaire">
							<th width="29%" class="left">Commentaire</th>
						</xsl:if>
					</xsl:for-each>
					<th width="7%" class="center">Unité de l'UF</th>
					<th width="5%" class="center">Quantité</th>
					<th width="5%">DVE<br/>[an]</th>
					<th width="5%" class="center">Perf. UF Fille</th>
					<xsl:if test="$c_zone &gt; 1">
						<th width="10%">Fiche liée à (zone)</th>
					</xsl:if>	
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="*/contributeur/composant/lot[@ref=$v_lot]//donnees_composant">
					<!-- <xsl:sort select="id_produit" data-type="text" order="ascending" case-order="upper-first"/> -->
					<tr>
						<td align="center"><xsl:choose><xsl:when test="parent::sous_lot"><xsl:value-of select="../@ref"/></xsl:when><xsl:otherwise>-</xsl:otherwise></xsl:choose></td>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center">
							<xsl:choose>
								<xsl:when test="type_donnees=7"><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(id_fiche,'_',' ')"/></xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="center"><xsl:call-template name="type_donnees"/></td>
						<td><xsl:value-of select="translate(substring(nom, 1, 300),'_',' ')"/><xsl:if test="string-length(nom) &gt; 300"> [...]</xsl:if></td>
						<xsl:if test="commentaire">
							<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						</xsl:if>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<td align="center"><xsl:value-of select="dve"/></td>
						<td align="right"><xsl:value-of select="perf_uf_fille"/></td>
						<xsl:if test="$c_zone &gt; 1">
							<td align="center"><xsl:value-of select="ancestor::zone/index"/></td>
						</xsl:if>
					</tr>
				</xsl:for-each>
			</tbody>
		</table>
		<!-- seulement si le type_donnees=7 -->
		<xsl:if test="*/contributeur/composant/lot[@ref=$v_lot]/sous_lot/donnees_composant[type_donnees=7] or */contributeur/composant/lot[@ref=$v_lot]/donnees_composant[type_donnees=7]">
			<h5>Données saisies (complément d'information sur les données issues de configurateurs de fiches environnementales)</h5>
			<xsl:for-each select="*/contributeur/composant/lot[@ref=$v_lot]//donnees_composant[type_donnees=7]">
				<xsl:if test="donnees_configurateur">
					<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h8" width="100%">
						<thead>
							<tr>
								<th colspan="4" class="h8" align="left">
									<xsl:call-template name="base"/>&#160;<xsl:if test="version_configurateur">(<xsl:value-of select="version_configurateur"/>)</xsl:if> : <xsl:value-of select="nom"/>
								</th>
							</tr>	
							<tr class="b">
								<th width="20%">Identifiant fiche configurée</th>
								<td width="30%" align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
								<th width="25%">Identifiant fiche mère</th>
								<td width="25%" align="center"><xsl:choose><xsl:when test="id_fiche_mere !=''"><xsl:value-of select="translate(id_fiche_mere,'_',' ')"/></xsl:when><xsl:otherwise>-</xsl:otherwise></xsl:choose></td>
							</tr>
						</thead>
						<tbody>
							<tr class="b">
								<td width="30%">Quantité</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="quantite"/></td>
							</tr>
							<tr class="b">
								<td width="30%">Unité</td>
								<td width="70%" colspan="3" align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
							</tr>
							<tr class="b">
								<td width="30%">DVE (années)</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="dve"/></td>
							</tr>
							<tr class="b">
								<td width="30%">Commentaire</td>
								<td width="70%" colspan="3" align="center"><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
							</tr>
							<!-- on regarde si les données ont été completées -->	
							<xsl:if test="donnees_configurateur">
								<tr class="b">
									<th>N° Paramètre</th>
									<th>Nom</th>
									<th>Valeur</th>
									<th>Unité</th>
								</tr>
								<xsl:for-each select="donnees_configurateur/parametre">
									<tr class="b">
										<th><xsl:value-of select="@numero"/></th>
										<td><xsl:value-of select="nom"/></td>
										<td align="center"><xsl:value-of select="valeur"/></td>
										<td align="center">
											<xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite"/></xsl:call-template>
										</td>
									</tr>
								</xsl:for-each>
							</xsl:if>
						</tbody>
					</table>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<br/>
	</xsl:if>
</xsl:template>
<!-- données d'entrée EAU et ENERGIE -->
<xsl:template name="contributeur_2_3">
	<xsl:param name="v_contrib"/>
	<xsl:param name="id_bat"/>
	<xsl:variable name="c_zone" select="count(zone)"/>
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<xsl:choose>
					<xsl:when test="$c_zone &gt; 1"><th colspan="9" class="h7" align="left">Données</th></xsl:when>
					<xsl:otherwise>
						<th colspan="8" class="h7" align="left">Données</th>
					</xsl:otherwise>
				</xsl:choose>						
			</tr>	
			<tr class="b">
				<th width="5%">Base</th>
				<th width="5%" class="center">Identifiant fiche</th>
				<th width="5%" class="center">Sous-contribution</th>
				<th width="10%" class="center">Nom</th>
				<xsl:if test="$v_contrib='eau'"><th width="15%" class="left">Commentaire</th></xsl:if>
				<th width="7%" class="center">Unité de l'UF</th>
				<th width="10%" class="center">Quantité</th>
				<xsl:if test="$c_zone &gt; 1">
					<th width="8%" class="center">Fiche liée à<br/>(zone)</th>	
				</xsl:if>
			</tr>
		</thead>
		<tbody>
			<xsl:if test="$v_contrib='energie'">
				<xsl:for-each select="*/contributeur/energie/sous_contributeur/donnees_service">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
						<td align="center"><xsl:call-template name="poste"/></td>
						<td align="center" class="mod"><xsl:value-of select="translate(nom,'_',' ')"/></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<xsl:if test="$c_zone &gt; 1">
							<td align="center"><xsl:value-of select="../../../../index"/></td>
						</xsl:if>
					</tr>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="$v_contrib='eau'">
				<xsl:for-each select="*/contributeur/eau/sous_contributeur/donnees_service">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
						<td align="center"><xsl:call-template name="eau_type"/></td>
						<td align="center" class="mod"><xsl:value-of select="translate(id_produit,'_',' ')"/></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<xsl:if test="$c_zone &gt; 1">
							<td align="center"><xsl:value-of select="../../../../index"/></td>
						</xsl:if>
					</tr>
				</xsl:for-each>
			</xsl:if>
		</tbody>
	</table>
	<br/>
	<!-- tableau contributeur 3 - EAU -->
	<xsl:if test="$v_contrib='eau'">
<!-- calculette eau -->
<xsl:for-each select="/projet/RSEnv/entree_projet/batiment[index=$id_bat]/zone">
	<xsl:variable name="id_zone" select="index"/>
	<xsl:if test="calculette_eau">
	    <span class="zone">ZONE <xsl:value-of select="index"/></span>	
		<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h4" width="93%">
			<thead>
				<tr>
					<th colspan="3" class="h4" align="left">Calculette des paramètres d'entrée du contributeur « EAU » de la zone <xsl:value-of select="$id_zone"/></th>
				</tr>	
			</thead>
			<tbody>
				<xsl:for-each select="calculette_eau">
					<tr>
						<th width="70%" align="center" colspan="2">Réseau de collecte des eaux usées et pluviales</th>
						<td width="30%" align="center">
							<xsl:choose>
								<xsl:when test="reseau_collecte=1">Réseau séparatif</xsl:when>
								<xsl:when test="reseau_collecte=2">Réseau unitaire</xsl:when>
								<xsl:otherwise>-</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					<tr>
						<th align="center" colspan="2">Réseau d'assainissement des eaux usées et pluviales</th>
						<td align="center">
							<xsl:choose>
								<xsl:when test="reseau_assainissement=1">Assainissement collectif</xsl:when>
								<xsl:when test="reseau_assainissement=2">Assainissement non collectif</xsl:when>
								<xsl:otherwise>-</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					<tr>
						<th align="center" colspan="2">Consommation d'eau de pluie pour les usages intérieurs [m3/an]</th>
						<td align="center">
							<xsl:value-of select="conso_ep_usage_int"/> m<sup>3</sup>/an
						</td>
					</tr>
					<xsl:if test="fg_equipement">
						<tr>
							<th align="center" colspan="2">Facteur de correction de la consommation conventionnelle en fonction des équipements disponibles dans la zone</th>
							<td align="center">
								<xsl:value-of select="f_equipement"/>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="toilettes">
						<xsl:variable name="nb" select="count(toilettes/child::*)"/>
						<xsl:for-each select="toilettes">
							
							<xsl:if test="taux_seches">
								<tr>
									<th width="20%" align="center" rowspan="{$nb}">Toilettes</th>
									<th width="40%" align="center">Taux d'équipement en toilettes sèches</th>
									<td align="center">
										<xsl:value-of select="taux_seches"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="taux_3l_6l">
								<tr>
									<th width="40%" align="center">Taux d'équipement en toilettes double flux 3L / 6L</th>
									<td align="center">
										<xsl:value-of select="taux_3l_6l"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="taux_2l_4l">
								<tr>
									<th width="40%" align="center">Taux d'équipement en toilettes double flux 2L / 4L</th>
									<td align="center">
										<xsl:value-of select="taux_2l_4l"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="taux_urinoirs">
							<tr>
								<th width="40%" align="center">Taux d'équipement en urinoirs (hors résidentiel)</th>
								<td align="center">
									<xsl:value-of select="taux_urinoirs"/>
								</td>
							</tr>
							</xsl:if>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="regulateur_debit">
						<xsl:variable name="nb2" select="count(regulateur_debit/child::*)"/>
						<xsl:for-each select="regulateur_debit">
							
							<xsl:if test="taux_lavabo">
								<tr>
									<th width="20%" align="center" rowspan="{$nb2}">Réducteur de pression</th>
									<th width="40%" align="center">Taux d'équipement en robinets de lavabos avec régulateur</th>
									<td align="center">
										<xsl:value-of select="taux_lavabo"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="taux_evier">
								<tr>
									<th width="40%" align="center">Taux d'équipement en robinets d'éviers avec régulateur </th>
									<td align="center">
										<xsl:value-of select="taux_evier"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="taux_douche">
								<tr>
									<th width="40%" align="center">Taux d'équipement en douches économes</th>
									<td align="center">
										<xsl:value-of select="taux_douche"/>
									</td>
								</tr>
							</xsl:if>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="taux_puis_sup_8m">
						<tr>
							<th width="60%" align="center" colspan="2">Part des points de puisage de la zone dont la distance à la production est supérieure à 8m</th>
							<td align="center">
								<xsl:value-of select="taux_puis_sup_8m"/>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="surf_bat_veg">
						<tr>
							<th width="60%" align="center" colspan="2">Fraction de surface de murs et de toitures végétalisés attribuée à la zone [m²]</th>
							<td align="center">
								<xsl:value-of select="surf_bat_veg"/>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="q_pluviometrie">
						<tr>
							<th width="60%" align="center" colspan="2">Pluviométrie annuelle [mm/m²]</th>
							<td align="center">
								<xsl:value-of select="q_pluviometrie"/>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="surf_toiture">
						<tr>
							<th width="60%" align="center" colspan="2">Fraction de surface de toitures et stationnement couvert attribué à la zone [m²]</th>
							<td align="center">
								<xsl:value-of select="surf_toiture"/>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="surf_park_imper_nc">
						<tr>
							<th width="60%" align="center" colspan="2">Fraction de surface de parking imperméabilisé non couvert attribué à la zone [m²]</th>
							<td align="center">
								<xsl:value-of select="surf_park_imper_nc"/>
							</td>
						</tr>
					</xsl:if>
				</xsl:for-each>
			</tbody>
		</table>
	</xsl:if>
</xsl:for-each>
</xsl:if>

</xsl:template>
<!-- contributeur chantier -->
<xsl:template name="contributeur_4">
	<xsl:param name="id_bat"/>
	<xsl:variable name="c_zone" select="count(zone)"/>
	
<xsl:if test="*/contributeur/chantier/sous_contributeur/donnees_composant">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<xsl:choose>
					<xsl:when test="$c_zone &gt; 1"><th colspan="10" class="h7" align="left">Données Composant</th></xsl:when>
					<xsl:otherwise>
						<th colspan="9" class="h7" align="left">Données Composant</th>
					</xsl:otherwise>
				</xsl:choose>												
			</tr>	
				<tr class="b">
					<th width="5%">Base</th>
					<th width="5%" class="center">Identifiant fiche</th>
					<th width="5%" class="center">Type de données</th>
					<th width="15%" class="left">Nom</th>
					<th width="15%" class="left">Commentaire</th>
					<th width="5%" class="center">Unité de l'UF</th>
					<th width="5%" class="center">DVE (années)</th>
					<th width="10%" class="center">Quantité</th>
					<th width="10%" class="center">Perf. UF Fille</th>
					<xsl:if test="$c_zone &gt; 1">
						<th width="10%" class="center">Fiche liée à<br/>(zone)</th>	
					</xsl:if>
				</tr>
		
		</thead>
		<tbody>
				<xsl:for-each select="*/contributeur/chantier/sous_contributeur/donnees_composant">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
						<td align="center" class="mod"><xsl:call-template name="type_donnees"/></td>
						<td align="left" class="mod"><xsl:value-of select="nom"/></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="dve"/></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<td align="center"><xsl:value-of select="perf_uf_fille"/></td>
						<xsl:if test="$c_zone &gt; 1">
							<td align="center"><xsl:value-of select="../../../../index"/></td>
						</xsl:if>
					</tr>
				</xsl:for-each>

		</tbody>
	</table>
	</xsl:if>
<!-- </xsl:for-each> -->
<xsl:if test="*/contributeur/chantier/sous_contributeur/donnees_service">
	<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h7" width="100%">
		<thead>
			<tr>
				<xsl:choose>
					<xsl:when test="$c_zone &gt; 1"><th colspan="7" class="h7" align="left">Données Service</th></xsl:when>
					<xsl:otherwise>
						<th colspan="6" class="h7" align="left">Données Service</th>
					</xsl:otherwise>
				</xsl:choose>												
			</tr>	
		<tr class="b">
					<th width="5%">Base</th>
					<th width="5%" class="center">Identifiant fiche</th>
					<th width="15%">Sous-contribution</th>
					<th width="15%">Commentaire</th>
					<th width="10%" class="center">Unité de l'UF</th>
					<th width="10%" class="center">Quantité</th>
					<xsl:if test="$c_zone &gt; 1">
						<th width="10%" class="center">Fiche liée à<br/>(zone)</th>	
					</xsl:if>
				</tr>
			
		</thead>
		<tbody>
				<xsl:for-each select="*/contributeur/chantier/sous_contributeur/donnees_service">
					<tr>
						<td align="center"><xsl:call-template name="base"/></td>
						<td align="center"><xsl:value-of select="translate(id_fiche,'_',' ')"/></td>
						<td><xsl:call-template name="chantier_conso"/></td>
						<td><xsl:value-of select="translate(substring(commentaire, 1, 300),'_',' ')"/><xsl:if test="string-length(commentaire) &gt; 300"> [...]</xsl:if></td>
						<td align="center"><xsl:call-template name="unites"><xsl:with-param name="v_unites" select="unite_uf"/></xsl:call-template></td>
						<td align="center"><xsl:value-of select="format-number(quantite, '#&#160;##0,##','fr')"/></td>
						<xsl:if test="$c_zone &gt; 1">
							<td align="center"><xsl:value-of select="../../../../index"/></td>
						</xsl:if>
					</tr>
				</xsl:for-each>
		</tbody>
	</table>
	</xsl:if>

	<br/>
	
<!-- calculette chantier -->
<xsl:for-each select="/projet/RSEnv/entree_projet/batiment[index=$id_bat]/zone">
	<xsl:variable name="id_zone" select="index"/>
	
	<xsl:if test="calculette_chantier">
	<span class="zone">ZONE <xsl:value-of select="index"/></span>
		<table border="1" cellpadding="0" cellspacing="0" class="rt2012 h4" width="83%">
			<thead>
				<tr>
					<th colspan="3" class="h4" align="left">Calculette des paramètres d'entrée du contributeur « Chantier » de la zone <xsl:value-of select="$id_zone"/></th>
				</tr>	
			</thead>
			<tbody>
				<xsl:for-each select="calculette_chantier">
					<xsl:if test="terres_importees">
						<tr>
							<th width="60%" align="center" colspan="2">Quantité de terres importées</th>
							<td align="center">
								<xsl:value-of select="terres_importees"/> tonne(s)
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="terres_evacuees">
						<tr>
							<th width="60%" align="center" colspan="2">Quantité de terres évacuées lors du terrassement du chantier</th>
							<td align="center">
								<xsl:value-of select="terres_evacuees"/> tonne(s)
							</td>
						</tr>
					</xsl:if>
					<tr>
						<th width="60%" align="center" colspan="2">Quantité de terres excavées lors du terrassement du chantier</th>
						<td align="center">
							<xsl:value-of select="terres_excavees"/> m<sup>3</sup>
						</td>
					</tr>
					<tr>
						<th width="60%" align="center" colspan="2">Distance entre le chantier et le lieu d'import des terres</th>
						<td align="center">
							<xsl:value-of select="dist_import_terres"/> km
						</td>
					</tr>
					<xsl:if test="dist_traitement_terres">
						<tr>
							<th width="60%" align="center" colspan="2">Distance entre le chantier et le lieu de traitement des terres</th>
							<td align="center">
								<xsl:value-of select="dist_traitement_terres"/> m<sup>2</sup>
							</td>
						</tr>
					</xsl:if>
					<xsl:if test="grue">
						<xsl:variable name="nb" select="count(grue/child::*)"/>
						<xsl:for-each select="grue">
							
							<xsl:if test="duree_ete_avec">
								<tr>
									<th width="10%" align="center" rowspan="{$nb}">Grue</th>
									<th width="30%" align="center">Nombre de mois d'été (d'avril à septembre) avec grue</th>
									<td align="center">
										<xsl:value-of select="duree_ete_avec"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="duree_ete_sans">
								<tr>
									<th width="30%" align="center">Nombre de mois d'été (d'avril à septembre) sans grue</th>
									<td align="center">
										<xsl:value-of select="duree_ete_sans"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="duree_hiv_avec">
								<tr>
									<th width="30%" align="center">Nombre de mois d'hiver (d'octobre à mars) avec grue</th>
									<td align="center">
										<xsl:value-of select="duree_hiv_avec"/>
									</td>
								</tr>
							</xsl:if>
							<xsl:if test="duree_hiv_sans">
							<tr>
								<th width="30%" align="center">Nombre de mois d'hiver (d'octobre à mars) sans grue</th>
								<td align="center">
									<xsl:value-of select="duree_hiv_sans"/>
								</td>
							</tr>
							</xsl:if>
						</xsl:for-each> 
					</xsl:if>
				</xsl:for-each>
			</tbody>
		</table>
	</xsl:if>
</xsl:for-each>
</xsl:template>


<!-- ********** RSEnv_template.xsl ********** -->
</xsl:stylesheet>
